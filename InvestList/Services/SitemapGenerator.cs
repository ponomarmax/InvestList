using System.Text;
using System.Xml;
using DataAccess;
using Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace InvestList.Services
{
    public class SitemapGenerator(ApplicationDbContext context, IWebHostEnvironment env, IConfiguration configuration)
        : ISitemapGenerator
    {
        public string GenerateSitemap(string host)
        {
            if (host == null)
                throw new NullReferenceException("host is null");
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings { Indent = true };

            using (var writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                var posts = context.Posts
                    .Where(p => p.IsActive)
                    .ToList();

                var baseUrl = host;

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
