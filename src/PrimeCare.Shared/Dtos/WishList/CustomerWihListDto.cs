using System.ComponentModel.DataAnnotations;
using PrimeCare.Shared.Dtos.WishList;

public class CustomerWihListDto
{
    [Required]
    public string Id { get; set; }

    public List<WishListItemDto> Items { get; set; }

    public int TotalItems => Items?.Count ?? 0;
}