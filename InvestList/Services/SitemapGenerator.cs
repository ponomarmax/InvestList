using System.Text;
using System.Xml;
using DataAccess;
using Core.Entities;

namespace InvestList.Services
{
    public class SitemapGenerator
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public SitemapGenerator(ApplicationDbContext context, IWebHostEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _env = env;
            _configuration = configuration;
        }

        public string GenerateSitemap()
        {
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings { Indent = true };

            using (var writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                var posts = _context.Post
                    .Where(p => p.IsActive)
                    .ToList();

                var baseUrl = _env.IsDevelopment() ? _configuration["BaseUrl:Development"] : _configuration["BaseUrl:Production"];

                foreach (var post in posts)
                {
                    var postUrl = post.PostType == PostType.InvestAd
                        ? $"{baseUrl}invest/{post.Slug}"
                        : $"{baseUrl}news/{post.Slug}";

                    WriteUrl(writer, postUrl, post.CreatedAt);
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            return sb.ToString();
        }

        private void WriteUrl(XmlWriter writer, string url, DateTime lastModified)
        {
            writer.WriteStartElement("url");
            writer.WriteElementString("loc", url);
            writer.WriteElementString("lastmod", lastModified.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            writer.WriteEndElement();
        }
    }
}
