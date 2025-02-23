using KimTaiPhongThuy.Models;

namespace KimTaiPhongThuy.DataAccess
{
    public class ProductDAO
    {
        private readonly JewelryStoreContext _context;

        public ProductDAO(JewelryStoreContext context)
        {
            _context = context;
        }

        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
    }
}
