namespace InvestList.Models;

public class UserDetectionInfo
{
    public int TimeSpent { get; set; }
    public bool MouseMoved { get; set; }
    public bool NavigatorWebdriver { get; set; }
    public bool HasChrome { get; set; }
    public int ScreenHeight { get; set; }
    public int ScreenWidth { get; set; }
}