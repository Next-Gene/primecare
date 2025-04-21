
using PrimeCare.Core.Entities;

namespace PrimeCare.Application.Dtos.Photos
{
    internal class ProductPhotosDto
    {


        public int Id { get; set; } 

        /// <summary>
        /// Gets or sets the URL where the photo is stored.
        /// </summary>

        public string Url { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether this is the main photo for the Product.
        /// </summary>
        public bool IsMain { get; set; }

        /// <summary>
        /// Gets or sets the public identifier from the cloud storage provider.
        /// </summary>

        ///// <summary>
        ///// Gets or sets the foreign key of the associated Product.
        ///// </summary>
        //public int ProductId { get; set; }

        ///// <summary>
        ///// Gets or sets the navigation property to the associated Product.
        ///// </summary>
        //public virtual Product Product { get; set; } = null!;
    }
}
