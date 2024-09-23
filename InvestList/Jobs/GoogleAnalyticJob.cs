using Core;
using Core.Entities;
using DataAccess;
using Google.Analytics.Data.V1Beta;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;

namespace InvestList.Jobs;

public class GoogleAnalyticJob(IServiceProvider serviceProvider, ILogger<GoogleAnalyticJob> logger, Timer timer)
    : IHostedService, IDisposable
{
    private Timer _timer = timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Google Analytic Service is starting.");
        ScheduleTaskAtMidnight();
        ExecuteTask(null);
        return Task.CompletedTask;
    }

    private void ScheduleTaskAtMidnight()
    {
        var now = DateTime.Now;
        var midnight = DateTime.Today.AddDays(1); // Next midnight
        var firstRun = midnight - now; // Time until next midnight

        // Schedule the first run
        _timer = new Timer(ExecuteTask, null, firstRun, TimeSpan.FromDays(1));
    }

    private void ExecuteTask(object state)
    {
        Task.Run(async () =>
        {
            logger.LogInformation("Google Analytic is being updated");
            try
            {
                await GetPageViewsAsync();
            }
            catch (Exception ex)
            {
                // Handle exceptions (log them, for example)
                Console.WriteLine($"Error during task execution: {ex.Message}");
            }
            logger.LogInformation("Google Analytic is updated");
        });
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public async Task GetPageViewsAsync()
    {
        var credential = GoogleCredential.FromFile("keys/invest-radar-auth-7fe230c91b25.json")
            .CreateScoped("https://www.googleapis.com/auth/analytics.readonly");
        // Create the AnalyticsDataClient
        var cli = new BetaAnalyticsDataClientBuilder();
        cli.GoogleCredential = credential;
        var client = await cli.BuildAsync();

        // Define the request
        var request = new RunReportRequest
        {
            Property = $"properties/{437562217}",
            Dimensions = { new Dimension { Name = "pagePath" } },
            Metrics = { new Metric { Name = "screenPageViews" } },
            DateRanges = { new DateRange { StartDate = "60daysAgo", EndDate = "today" } }
        };

        // Execute the request
        var response = await client.RunReportAsync(request);

        // Process the response
        using var scope = serviceProvider.CreateScope();
        var context  = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        foreach (var row in response.Rows)
        {
            var pagePath = row.DimensionValues[0].Value; // Example: /invest/real-partner-nvestits-v-orendu-avtomoblv
            var pageViews = int.Parse(row.MetricValues[0].Value);

            // Parse the pagePath
            var segments = pagePath.Split('/');
            if (segments.Length < 3) continue; // Ensure it's a valid path with at least 2 segments

            var postType = segments[1]; // invest
            var slug = segments[2]; // real-partner-nvestits-v-orendu-avtomoblv

            // Find the corresponding Post based on PostType and Slug
            var postT = SlugGenerator.GetPostType(postType);
            var post = await context.Posts.FirstOrDefaultAsync(p => p.PostType == postT && p.Slug == slug);

            if (post != null)
            {
                // Check if a PostViews entry already exists
                var postView = context.GoogleAnalyticPostViews.FirstOrDefault(pv => pv.PostId == post.Id);

                if (postView == null)
                {
                    // Create new PostView entry
                    postView = new GoogleAnalyticPostView
                    {
                        PostId = post.Id,
                        PageViews = pageViews,
                        LastUpdated = DateTime.UtcNow
                    };
                    context.GoogleAnalyticPostViews.Add(postView);
                }
                else
                {
                    // Update existing PostView entry
                    postView.PageViews = pageViews;
                    postView.LastUpdated = DateTime.UtcNow;
                    context.GoogleAnalyticPostViews.Update(postView);
                }

                // Save changes to the database
            }
        }
        await context.SaveChangesAsync();
    }
}