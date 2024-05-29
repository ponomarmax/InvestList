using Core.Entities;

namespace Core.Interfaces
{
    public interface IImageService
    {
        Task LoadOnFileSystem();
        void RefreshImages(Post post, IEnumerable<Guid> oldImagePaths);
    }
}