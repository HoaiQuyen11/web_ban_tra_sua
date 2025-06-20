using Microsoft.AspNetCore.Mvc;
using ShopManager.DAL;
using ShopManager.Models;

namespace ShopManager.ViewComponents
{
    public class RelatedProductViewComponent : ViewComponent
    {
        ProductDAL productDAL = new ProductDAL();

        public IViewComponentResult Invoke(int nonDisplayId)
        {
            List<Product> relatedProducts = new List<Product>();

            relatedProducts = productDAL.GetRelatedProduct(nonDisplayId);

            return View("RelatedProduct", relatedProducts);
        }
    }
}
