using PrimeCare.Core.Entities;

namespace PrimeCare.Core.Specifications
{
    public class ProductWithPhotosSpecification : BaseSpecification<Product>
    {
        public ProductWithPhotosSpecification(int id)
            : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductPhotos);
        }
    }
}
