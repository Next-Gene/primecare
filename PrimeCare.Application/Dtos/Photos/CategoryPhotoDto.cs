
using PrimeCare.Core.Entities;

namespace PrimeCare.Application.Dtos.Photos
{
    public class CategoryPhotoDto
    {



        public int id { get; set; } 


        /// <summary>
        /// Gets or sets the URL where the photo is stored.
        /// </summary>

        public string Url { get; set; } = null!;



    }
}
