namespace Core.Entities;

public class UserRequestInfo
{
    public int Id { get; set; }
    public string UserId { get; set; } // Прив'язка до користувача

    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public string Cookies { get; set; }
    public string Headers { get; set; }
    public int TimeSpent { get; set; }
    public bool MouseMoved { get; set; }
    public bool NavigatorWebdriver { get; set; }
    public bool HasChrome { get; set; }
    public int ScreenHeight { get; set; }
    public int ScreenWidth { get; set; }
    public DateTime RequestTime { get; set; } = DateTime.UtcNow;
}
