using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrimeCare.Core.Entities
{
    /// <summary>
    /// Represents a photo associated with a category in the system.
    /// </summary>
    [Table("CategoryPhotos")] // Plural table name convention
    public class CategoryPhoto : BaseEntity
    {
        /// <summary>
        /// Gets or sets the URL where the photo is stored.
        /// </summary>
        [Required]
        [MaxLength(500)]
        [Url] // Validates that the string is a valid URL
        public string Url { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether this is the main photo for the category.
        /// </summary>
        public bool IsMain { get; set; } 

        /// <summary>
        /// Gets or sets the public identifier from the cloud storage provider.
        /// </summary>
        [MaxLength(100)]
        public string PublicId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the foreign key of the associated category.
        /// </summary>
        [Required]
        public int CategoryId { get; set; } 

        /// <summary>
        /// Gets or sets the navigation property to the associated category.
        /// </summary>
        public virtual Category Category { get; set; }=null!;
    }
}