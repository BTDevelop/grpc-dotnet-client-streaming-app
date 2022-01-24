using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace Product.GRPC
{
    public class ProductService : ProductGRPCService.ProductGRPCServiceBase
    {
        public override async Task<InsertRangeProductsResponse> InsertRangeProductsStream(IAsyncStreamReader<InsertRangeProductsRequest> requestStream, ServerCallContext context)
        {
            var InsertRangeResponse = new InsertRangeProductsResponse();

            await foreach (var insertedProductItem in requestStream.ReadAllAsync())
            {
                // product insert operations...

                InsertRangeResponse.Count += 1;
                Console.WriteLine($"1 product has been inserted. SKU: {insertedProductItem.Sku} Brand: {insertedProductItem.Brand}");
            }

            Console.WriteLine("Insert products stream has been ended.");

            return InsertRangeResponse;
        }
    }
}
