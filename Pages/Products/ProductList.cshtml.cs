using KimTaiPhongThuy.Extension;
using KimTaiPhongThuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KimTaiPhongThuy.Pages.Products
{
    public class ProductListModel : PageModel
    {
        private JewelryStoreContext _context;
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public int? SelectedCategoryId { get; set; }
        public string? SearchProductQuery { get; set; }

        public ProductListModel(JewelryStoreContext context)
        {
            _context = context;
        }

        // get all product
        public IActionResult OnGet(int? categoryId, string? querySearch)
        {
            SelectedCategoryId = categoryId;
            SearchProductQuery = querySearch;
            Categories = _context.Categories.ToList();
            // tạo sẵn 1 query trước
            IQueryable<Product> query = _context.Products.AsQueryable();
            if (SelectedCategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == SelectedCategoryId.Value);
            }
            if (!String.IsNullOrEmpty(SearchProductQuery))
            {
                query = query.Where(p => p.ProductName.Contains(SearchProductQuery));
            }

            Products = query.ToList();
            return Page();
        }
        public IActionResult OnPostAddToCart(int productId, string productName, decimal price, string? imageUrl)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var item = cart.FirstOrDefault(p => p.ProductID == productId);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductID = productId,
                    ProductName = productName,
                    Price = price,
                    Quantity = 1,
                    ImageUrl = imageUrl
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToPage("./ProductList");
        }
    }
}
