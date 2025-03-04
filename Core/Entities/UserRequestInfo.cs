namespace Core.Entities;

public class UserRequestInfo
{
    public int Id { get; set; }
    public string UserId { get; set; } // Прив'язка до користувача

    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public string Cookies { get; set; }
    public string Headers { get; set; }
    public DateTime RequestTime { get; set; } = DateTime.UtcNow;
}
