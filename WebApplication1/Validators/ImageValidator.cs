using SixLabors.ImageSharp;

namespace WebApplication1.Validators
{
    public class ImageValidator
    {
        public static bool IsBase64StringAnImage(string base64String)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64String);

                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (Image image = Image.Load(ms))
                    {
                        // You can perform additional image validation or processing here
                    }
                }

                return true;
            }
            catch (Exception)
            {
                // An exception occurred, indicating that the base64 string is not a valid image.
                return false;
            }
        }
    }
}
