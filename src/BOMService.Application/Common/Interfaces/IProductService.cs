using BOMService.Application.DTOs;
using BOMService.Domain.Entities;
using System.Data;

namespace BOMService.Application.Common.Interfaces
{
    public interface IProductService
    {
        Task<Dictionary<int, string>> GetProductDictAsync();

        Task<List<ProductToBuildingPhaseModel>> GetProductsToBuildingPhaseAsync();

        Task<List<ProductToBuildingPhaseAndStyleModel>> GetProductsToBuildingPhaseAndStyleAsync();

        Task<List<ProductToStyleModel>> GetProductsToStyleAsync();

        Task<List<ProductToCategoryModel>> GetProductsToCategoryAsync();

        Task<Dictionary<int, int>> GetReverseProductDictAsync();

        Task<Dictionary<string, Tuple<int, string>>> GetProductOrientationDictAsync();

        Task<Dictionary<int, Tuple<string, string>>> GetProductOrientationIdKeyDictAsync();
        
        Task<List<BOMGeneratingProductDto>> GetProductsForBOMGenerationAsync(string SQLCommandName, string typeName, DataTable inputReportTable);
    }
}
