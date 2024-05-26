using DataAccess;
using DataAccess.Models;
using InvestList.Models.V2;
using Microsoft.EntityFrameworkCore;

namespace InvestList.Services
{
    public class ImageService(ApplicationDbContext context)
    {
        private static string _investF = "/images/investad/";
        private static string _newsF = "/images/news/";

        public async Task LoadOnFileSystem()
        {
            var root = "wwwroot";
            Directory.CreateDirectory(root + _investF);
            Directory.CreateDirectory(root + _newsF);

            await foreach (var post in context.Posts.Include(x => x.Images).AsAsyncEnumerable())
            {
                if (post.Images?.Any() == true)
                    foreach (var image in post.Images)
                    {
                        SaveBase64Image(image.ImageBase64,
                            post.PostType == PostType.News ? root + _newsF : root + _investF,
                            $"{image.Id}.jpg");
                    }
            }
        }

        public static ImageView GetImagePath(Guid imageId, Post post)
        {
            var filePath = string.Empty;
            if (post.PostType == PostType.News)
                filePath = _newsF;
            else filePath = _investF;
            filePath += post.Images.First().Id;
            return new ImageView()
            {
                AltText = post.Title,
                FilePath = $"{filePath}.jpg"
            };
        }

        public static void SaveBase64Image(string base64Image, string folderPath, string fileName)
        {
            // Convert base64 string to byte array
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            // Combine the folder path and file name
            string filePath = Path.Combine(folderPath, fileName);

            // Save byte array to file
            File.WriteAllBytes(filePath, imageBytes);
        }
    }
}