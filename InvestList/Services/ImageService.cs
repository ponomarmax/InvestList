using Core.Entities;
using Core.Interfaces;
using DataAccess;
using InvestList.Models.V2;
using Microsoft.EntityFrameworkCore;
using Radar.Domain.Entities;
using Radar.Domain.Interfaces;

namespace InvestList.Services
{
    public class ImageService(ApplicationDbContext context): IImageService
    {
        private static string _baseImagePath = "wwwroot/images/";

        public async Task LoadOnFileSystem()
        {
            foreach (var type in Enum.GetNames(typeof(PostType)))
            {
                Directory.CreateDirectory(Path.Combine(_baseImagePath, type.ToLower()));
            }

            await foreach (var post in context.Posts.Include(x => x.Images).ThenInclude(x => x.ImageObject)
                               .AsAsyncEnumerable())
            {
                if (post.Images?.Any() == true)
                    foreach (var image in post.Images)
                    {
                        SaveImage(image.ImageObject.Image,
                            Path.Combine(_baseImagePath, post.PostType.ToLower()),
                            $"{image.Id}.jpg");
                    }
            }
        }

        public void RefreshImages(Post post, IEnumerable<Guid> oldImagePaths)
        {
            if (oldImagePaths != null)
                foreach (var oldId in oldImagePaths)
                {
                    var path = Path.Combine(_baseImagePath, post.PostType.ToLower(), $"{oldId}.jpg");
                    if(File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }

            if (post.Images?.Any() == true)
                foreach (var image in post.Images)
                {
                    SaveImage(image.ImageObject.Image,
                        Path.Combine(_baseImagePath, post.PostType.ToLower()),
                        $"{image.Id}.jpg");
                }
        }
        
        public static Radar.Application.Models.ImageView GetImageView2(Guid imageId, Post post)
        {
            return new Radar.Application.Models.ImageView()
            {
                AltText = post.Title,
                FilePath = GetImagePath(post.Images.First().Id, post)
            };
        }

        public static string GetImagePath(Guid imageId, Post post)
        {
            return $"/images/{post.PostType.ToLower()}/{imageId}.jpg";
        }

        public static void SaveImage(byte[] base64Image, string folderPath, string fileName)
        {
            // Convert base64 string to byte array
            // byte[] imageBytes = Convert.FromBase64String(base64Image);

            // Combine the folder path and file name
            string filePath = Path.Combine(folderPath, fileName);

            // Save byte array to file
            File.WriteAllBytes(filePath, base64Image);
        }
    }
}