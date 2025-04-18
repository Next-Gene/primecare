
using AutoMapper;
using Microsoft.Extensions.Configuration;
using PrimeCare.Application.Dtos.Category;
using PrimeCare.Core.Entities;

namespace PrimeCare.Application.Helpers
{
      internal class CategoryUrlResolver : IValueResolver<Category, CategoryDto, string>
    {

        /// <summary>
        /// Resolves the full URL for a category's image.
        /// </summary>
        private readonly IConfiguration _configuration;



        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryUrlResolver"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use for resolving the URL.</param>
        public CategoryUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }




        /// <summary>
        /// Resolves the full URL for a category's image.
        /// </summary>
        /// <param name="source">The source category entity.</param>
        /// <param name="destination">The destination category DTO.</param>
        /// <param name="destMember">The destination member being resolved.</param>
        /// <param name="context">The resolution context.</param>
        /// <returns>The full URL for the category's image.</returns>


        public string Resolve(Category source, CategoryDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageUrl))
                return _configuration["ApiUrl"] + source.ImageUrl;
            return null!;
        }
    }
    }

