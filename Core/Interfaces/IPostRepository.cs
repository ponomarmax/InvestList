using Core.Entities;

namespace Core.Interfaces
{
    public interface IPostRepository
    {
        Task<(int count, IEnumerable<Post> list)> Filter(int page,
            int offset,
            IEnumerable<Guid>? tagIds);

        Task<Post?> Get(string id);
        Task<IEnumerable<Post>> GetSimilarPosts(Guid id, List<Guid> tagIds);
        Task Put(Guid id, Post post);
        Task Create(Post post);
        Task<bool> Exists(string slug);
        Task<bool> Exists(Guid id);
        Task<bool> IsOwnerOfPost(string userId, string postId);
        Task<int> Count(PostType? postType);
    }
}