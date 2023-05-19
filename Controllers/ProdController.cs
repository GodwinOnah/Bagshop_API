using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.ErrorsHandlers;
using AutoMapper;
using core.Controllers;
using core.Entities.DTOs;
using core.Interfaces;
using core.Specifications;
using infrastructure.data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

   
    public class ProductsController: ApiControllerBase
    {
        private readonly IgenericInterfaceRepository<Products> _products;
        private readonly IgenericInterfaceRepository<ProductBrand> _productBrands;

         private readonly IgenericInterfaceRepository<ProductType> _productTypes;
          private readonly productContext  _context;
        //  private readonly IProductDetails _productDetails;

         private readonly IMapper _imapper;
        
        public ProductsController(
            IgenericInterfaceRepository<Products> products,
            IgenericInterfaceRepository<ProductBrand> productBrands,
            IgenericInterfaceRepository<ProductType> productTypes,
            productContext context,
            // IProductDetails productDetails,
            IMapper imapper)
                            {
                                _productTypes = productTypes;
                                _productBrands = productBrands;
                                _products = products;
                                // _productDetails = productDetails;
                                _context=context;
                            _imapper = imapper;
                            }

        [HttpPost] //No curly braces
        public async Task<string> UploadProducts(ProductDetails productsDetails)
        {

            // Console.WriteLine("\n\n\\n\n\n\n\n"+productsDetails.prodName+"\n\n\n\n");
            var productDetails = new Products{
               
                prodName = productsDetails.prodName,
                prodPicture = productsDetails.prodPicture,
                prodDescription = productsDetails.prodDescription,
                prodPrice = productsDetails.prodPrice,
                productBrand = new ProductBrand {Name = productsDetails.productBrand},
                productType =  new ProductType {Name = productsDetails.productType}
            };

            // Console.WriteLine("\n\n\\n\n\n\n\n"+productsDetails.prodPrice+"\n\n\n\n");

        //     //  var productDetails =   _productDetails.UploadProductAsync(
        //     //     productsDetails.prodName,
        //     //    productsDetails.prodPicture,
        //     //    productsDetails.prodDescription,
        //     //     productsDetails.prodPrice,
        //     //     productsDetails.productBrand,
        //     //     productsDetails.productType
        //     // );

           
               
            // return await  _imapper.Map<ProductDetails,Products>(productDetails);
                        
                 
                            //   Console.WriteLine("\n\n\\n\n\n\n\n"+_imapper.Map<ProductDetails,Products>(productDetails)+334455+"\n\n\n\n");
                        _context.Products.Add(productDetails);
                        
                   await  _context.SaveChangesAsync();

                return "Uploaded succecssfully";
                    
                   
        }          

        
        // [Cashing(600)]
        [HttpGet]
        public async Task<ActionResult<ProductsPagination<ProductsShapedObject>>> GetProducts(
            [FromQuery]ProductParameters parameters)//The [FromQuery] help the controller trace the parameter from the object passed
        {
             var countPageSpecification = new ProductSpecificationWithFilter(parameters);
             var totalProducts = await _products.CountPage(countPageSpecification);
             var specification = new GetProductsWithBrandAndType(parameters);
             var productsList = await _products.ListAllAsync(specification);
             var data = _imapper.Map<IReadOnlyList<Products>,
                             IReadOnlyList<ProductsShapedObject>>(productsList);
             return Ok(new ProductsPagination<ProductsShapedObject>
                (parameters.pageIndex,parameters.PageSize,totalProducts,data));
        }

        // [Cashing(600)]
        [HttpGet("{id}")]// Notice the curly braces
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Responses),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductsShapedObject>> GetProducts(int id)
        {
            var specification = new GetProductsWithBrandAndType(id);
            var product = await _products.GetProductsWithSpecification(specification);
            if(product==null) return NotFound(new Responses(400));
            return _imapper.Map<Products,ProductsShapedObject>(product);
        }

        // [Cashing(600)]
        [HttpGet("brands")] //No curly braces
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrand()
        {
            var productBrandList=await _productBrands.ListAllAsync();          
            return Ok(productBrandList);
        }

        // [Cashing(600)]
        [HttpGet("types")] //No curly braces
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductType()
        {
            var productTypeList = await _productTypes.ListAllAsync();
             return Ok(productTypeList);
        }   
}
}