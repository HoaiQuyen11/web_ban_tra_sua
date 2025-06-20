using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace ShopManager.Helper
{
    public class ImageHelper
    {
        public static string UpLoadImage(IFormFile Hinh, string folder)
        {
            try
            {
                // tao ten file theo thoi gian upload -> tranh trung ten file
                var FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Hinh.FileName;
                // upload image vao wwwroot, folder image
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", folder, FileName);
                using(var myFile = new FileStream(fullPath,FileMode.CreateNew))
                {
                    Hinh.CopyTo(myFile);
                }
                return FileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }
    }
}
