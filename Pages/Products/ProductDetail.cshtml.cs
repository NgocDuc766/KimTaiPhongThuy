using KimTaiPhongThuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KimTaiPhongThuy.Pages.Products
{
    public class ProductDetailModel : PageModel
    {
        private JewelryStoreContext _context;

        public Product Product { get; set; }
        public List<Review> Reviews { get; set; }
        public Dictionary<string, string> DescriptionSections { get; set; }

        public ProductDetailModel(JewelryStoreContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            Product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if(Product == null)
            {
                return NotFound();
            }

            Reviews = _context.Reviews
                        .Where(r => r.ProductId == id)
                        .ToList();
            DescriptionSections = SplitDescription(Product.Description);

            return Page();
        }

        private Dictionary<string, string> SplitDescription(string description)
        {
            var sections = new Dictionary<string, string>();
            var lines = description.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            string currentSection = "Mô tả chung";
            string currentContent = "";

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine)) continue;

                // Nhận diện các tiêu đề section dựa trên ký tự đặc biệt hoặc từ khóa
                if (trimmedLine.StartsWith("🔹 Đặc Điểm Nổi Bật:"))
                {
                    if (!string.IsNullOrEmpty(currentContent))
                    {
                        sections[currentSection] = currentContent.Trim();
                        currentContent = "";
                    }
                    currentSection = "Đặc Điểm Nổi Bật";
                }
                else if (trimmedLine.StartsWith("🔮 Công Dụng & Ý Nghĩa Phong Thủy:"))
                {
                    if (!string.IsNullOrEmpty(currentContent))
                    {
                        sections[currentSection] = currentContent.Trim();
                        currentContent = "";
                    }
                    currentSection = "Công Dụng & Ý Nghĩa Phong Thủy";
                }
                else if (trimmedLine.StartsWith("📌 Cam kết sản phẩm:"))
                {
                    if (!string.IsNullOrEmpty(currentContent))
                    {
                        sections[currentSection] = currentContent.Trim();
                        currentContent = "";
                    }
                    currentSection = "Cam kết sản phẩm";
                }
                else
                {
                    currentContent += trimmedLine + "\n";
                }
            }

            if (!string.IsNullOrEmpty(currentContent))
            {
                sections[currentSection] = currentContent.Trim();
            }

            return sections;
        }
    }
}
