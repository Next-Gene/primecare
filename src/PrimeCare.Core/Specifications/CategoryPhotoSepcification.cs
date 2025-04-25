using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeCare.Core.Entities;

namespace PrimeCare.Core.Specifications
{



    /// <summary>
    /// Specification for products with their Categories and brands included.
    /// </summary>
    public class CategoryPhotoSepcification : BaseSpecification<Category>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryPhotoSepcification"/> class.
        /// </summary>
        public CategoryPhotoSepcification() : base(null!)
        {
            AddInclude(x => x.CategoryPhoto);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryPhotoSepcification"/> class with a specific product ID.
        /// </summary>
        /// <param name="id">The product identifier.</param>
        public CategoryPhotoSepcification(int id)
            : base(x => x.Id == id)
        {

            AddInclude(x => x.CategoryPhoto);

        }

    }
}

