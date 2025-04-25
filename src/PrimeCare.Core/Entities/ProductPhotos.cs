namespace PrimeCare.Core.Entities;

public class ProductPhotos : BaseEntity
{
    public string? Url { get; set; }
    public bool? IsMain { get; set; }
    public string PublicId { get; set; } = null!;
    public int ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
}
