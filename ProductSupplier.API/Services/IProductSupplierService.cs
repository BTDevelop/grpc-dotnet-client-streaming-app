using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProductSupplier.API.Services
{
    public interface IProductSupplierService
    {
        Task<int> InsertRangeProductsAsync(IFormFile formFile);
    }
}