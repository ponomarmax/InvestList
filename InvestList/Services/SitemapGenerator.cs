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
            var settings = new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 };

            using var ms = new MemoryStream();
            using (var writer = XmlWriter.Create(ms, settings))
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

                    WriteUrl(writer, postUrl, post.UpdatedAt);
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            ms.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(ms))
            {
                return reader.ReadToEnd();
            }
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
