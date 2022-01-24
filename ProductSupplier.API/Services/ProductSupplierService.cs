using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using ProductSupplier.API.Models;

namespace ProductSupplier.API.Services
{
    public class ProductSupplierService : IProductSupplierService
    {
        private readonly ProductGRPCService.ProductGRPCServiceClient _productGRPCServiceClient;

        public ProductSupplierService(ProductGRPCService.ProductGRPCServiceClient productGRPCServiceClient)
        {
            _productGRPCServiceClient = productGRPCServiceClient;
        }

        public async Task<int> InsertRangeProductsAsync(IFormFile formFile)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };

            using var insertRangeProductsStream = _productGRPCServiceClient.InsertRangeProductsStream();
            using (var reader = new StreamReader(formFile.OpenReadStream()))
            using (var csv = new CsvReader(reader, config))
            {
                IAsyncEnumerable<ProductSupplierDTO> products = csv.GetRecordsAsync<ProductSupplierDTO>();

                await foreach (ProductSupplierDTO product in products)
                {
                    InsertRangeProductsRequest insertRangeProductsRequest = new()
                    {
                        SupplierId = product.SupplierID,
                        Sku = product.SKU,
                        Name = product.Name,
                        Description = product.Description,
                        Brand = product.Brand
                    };

                    await insertRangeProductsStream.RequestStream.WriteAsync(insertRangeProductsRequest);
                }
            }
            await insertRangeProductsStream.RequestStream.CompleteAsync();

            InsertRangeProductsResponse response = await insertRangeProductsStream;

            return response.Count;
        }
    }
}
