using Core.Entities;

namespace InvestList.Models.V2;

public class AdminPostView
{
    public Guid Id { get; set; }

    public PostType PostType { get; set; }

    public string Title { get; set; }

    public int Priority { get; set; }
}
    
public class PutAdminPost
{
    public Guid Id { get; set; }

    public int Priority { get; set; }
}
