using Core.Entities;
using Core.Interfaces;
using DataAccess;
using InvestList.Models.V2;
using Microsoft.EntityFrameworkCore;

namespace InvestList.Services
{
    public class ImageService(ApplicationDbContext context): IImageService
    {
        private static string _root = "wwwroot";
        private static string _investF = "/images/investad/";
        private static string _newsF = "/images/news/";

        public async Task LoadOnFileSystem()
        {
            Directory.CreateDirectory(_root + _investF);
            Directory.CreateDirectory(_root + _newsF);

            await foreach (var post in context.Posts.Include(x => x.ImagesV2).ThenInclude(x => x.ImageObject)
                               .AsAsyncEnumerable())
            {
                if (post.ImagesV2?.Any() == true)
                    foreach (var image in post.ImagesV2)
                    {
                        SaveImage(image.ImageObject.Image,
                            post.PostType == PostType.News ? _root + _newsF : _root + _investF,
                            $"{image.Id}.jpg");
                    }
            }
        }

        public void RefreshImages(Post post, IEnumerable<Guid> oldImagePaths)
        {
            if (oldImagePaths != null)
                foreach (var oldId in oldImagePaths)
                {
                    var path = $"{_root}{GetImagePath(oldId, post)}";
                    if(File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }

            if (post.ImagesV2?.Any() == true)
                foreach (var image in post.ImagesV2)
                {
                    SaveImage(image.ImageObject.Image,
                        post.PostType == PostType.News ? _root + _newsF : _root + _investF,
                        $"{image.Id}.jpg");
                }
        }

        public static ImageView GetImageView(Guid imageId, Post post)
        {
            return new ImageView()
            {
                AltText = post.Title,
                FilePath = GetImagePath(post.ImagesV2.First().Id, post)
            };
        }

        public static string GetImagePath(Guid imageId, Post post)
        {
            var filePath = string.Empty;
            if (post.PostType == PostType.News)
                filePath = _newsF;
            else filePath = _investF;
            return $"{filePath}{imageId}.jpg";
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