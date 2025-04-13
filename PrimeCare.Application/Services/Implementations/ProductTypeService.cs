using AutoMapper;
using PrimeCare.Application.Dtos;
using PrimeCare.Application.Dtos.ProductType;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities;
using PrimeCare.Core.Interfaces;

namespace PrimeCare.Application.Services.Implementations
{
    /// <summary>
    /// Service for managing product types, providing CRUD operations for product type entities.
    /// </summary>
    public class ProductTypeService : IProductTypeService
    {
        private readonly IGenericRepository<ProductType> _productTypeInterface;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductTypeService"/> class.
        /// </summary>
        /// <param name="productTypeInterface">The product type repository interface used for CRUD operations.</param>
        /// <param name="mapper">The AutoMapper instance for mapping between DTOs and entities.</param>
        public ProductTypeService(IGenericRepository<ProductType> productTypeInterface, IMapper mapper)
        {
            _productTypeInterface = productTypeInterface;
            _mapper = mapper;
        }

        #region Methods

        /// <summary>
        /// Retrieves a product type by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The product type identifier.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the product type data transfer object, or null if not found.</returns>
        public async Task<ProductTypeDto> GetByIdAsync(int id)
        {
            var product = await _productTypeInterface.GetByIdAsync(id);
            return product == null ? null! : _mapper.Map<ProductType, ProductTypeDto>(product);
        }

        /// <summary>
        /// Retrieves all product types asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of product type data transfer objects.</returns>
        public async Task<IReadOnlyList<ProductTypeDto>> GetAllAsync()
        {
            var products = await _productTypeInterface.GetAllAsync();
            return _mapper.Map<IReadOnlyList<ProductType>, IReadOnlyList<ProductTypeDto>>(products);
        }

        /// <summary>
        /// Adds a new product type asynchronously.
        /// </summary>
        /// <param name="entity">The product type data transfer object (DTO) to be added.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the service response indicating success or failure.</returns>
        public async Task<ServiceResponse> AddAsync(CreateProductTypeDto entity)
        {
            var mappedData = _mapper.Map<ProductType>(entity);
            int result = await _productTypeInterface.AddAsync(mappedData);

            return result > 0
                ? new ServiceResponse(true, "Product Type Added")
                : new ServiceResponse(false, "Product Type failed to be Added");
        }

        /// <summary>
        /// Updates an existing product type asynchronously.
        /// </summary>
        /// <param name="entity">The product type data transfer object (DTO) to be updated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the service response indicating success or failure.</returns>
        public async Task<ServiceResponse> UpdateAsync(ProductTypeDto entity)
        {
            var productType = await _productTypeInterface.GetByIdAsync(entity.Id);
            if (productType == null)
            {
                return new ServiceResponse(false, "Product Type Not Found");
            }

            var mappedData = _mapper.Map(entity, productType);
            int result = await _productTypeInterface.UpdateAsync(mappedData!);

            return result > 0
                ? new ServiceResponse(true, "Product Type Updated")
                : new ServiceResponse(false, "Product Type failed to be Updated");
        }

        /// <summary>
        /// Deletes a product type by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The product type identifier.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the service response indicating success or failure.</returns>
        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            int result = await _productTypeInterface.DeleteAsync(id);

            return result > 0
                ? new ServiceResponse(true, "Product Type Deleted")
                : new ServiceResponse(false, "Product Type Not Found or failed to be Deleted");
        }

        #endregion
    }
}