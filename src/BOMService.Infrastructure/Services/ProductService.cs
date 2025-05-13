using BOMService.Application.Common.Interfaces;
using BOMService.Application.Models;
using BOMService.Domain.Entities;
using BOMService.Domain.Enums;
using BOMService.Domain.Repositories;
using BOMService.Infrastructure.Extensions;
using BOMService.Infrastructure.Persistence.EFModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Threading;

namespace BOMService.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly string _connectionString;
        private readonly IBaseRepository<ProductsProduct, ProductModel> _productRepo;
        private readonly IBaseRepository<ProductsProductsToBuildingPhase, ProductToBuildingPhaseModel> _productToBuildingPhaseRepo;
        private readonly IBaseRepository<ProductsProductsToStyle, ProductToStyleModel> _productToStyleRepo;
        private readonly IBaseRepository<ProductsProductsToCategory, ProductToCategoryModel> _productToCategoryRepo;
        private readonly IBaseRepository<ProductsWithPtoBidAndPtoSid, ProductToBuildingPhaseAndStyleModel> _productToBuildingPhaseAndStyleRepo;
        private readonly IBaseRepository<ProductsToProductPairing, ProductToProductPairingModel> _pairingProductRepo;
        private readonly IBaseRepository<ProductOrientation, ProductOrientationModel> _productOrientationRepo;
        private readonly ILogger<ProductService> _logger;
        private readonly int _timeout; //TODO: move to settings

        public ProductService(
            IConfiguration configuration,
            IBaseRepository<ProductsProduct, ProductModel> productRepo,
            IBaseRepository<ProductsProductsToBuildingPhase, ProductToBuildingPhaseModel> productToBuildingPhaseRepo,
            IBaseRepository<ProductsProductsToStyle, ProductToStyleModel> productToStyleRepo,
            IBaseRepository<ProductsProductsToCategory, ProductToCategoryModel> productToCategoryRepo,
            IBaseRepository<ProductsWithPtoBidAndPtoSid, ProductToBuildingPhaseAndStyleModel> productToBuildingPhaseAndStyleRepo,
            IBaseRepository<ProductsToProductPairing, ProductToProductPairingModel> pairingProductRepo,
            IBaseRepository<ProductOrientation, ProductOrientationModel> productOrientationRepo,
            ILogger<ProductService> logger)
        {
            _connectionString = configuration.GetConnectionString("BOMDatabase");
            _productRepo = productRepo;
            _productToBuildingPhaseRepo = productToBuildingPhaseRepo;
            _productToStyleRepo = productToStyleRepo;
            _productToCategoryRepo = productToCategoryRepo;
            _productToBuildingPhaseAndStyleRepo = productToBuildingPhaseAndStyleRepo;
            _pairingProductRepo = pairingProductRepo;
            _productOrientationRepo = productOrientationRepo;
            _logger = logger;
            _timeout = 300;
        }

        public async Task<Dictionary<int, string>> GetProductDictAsync()
        {
            var products = await _productRepo.GetAsync(t => new ProductModel
            {
                Id = t.ProductsId,
                Name = t.ProductsName
            });

            return products.ToDictionary(p => p.Id, p => p.Name);
        }

        public async Task<List<ProductToBuildingPhaseModel>> GetProductsToBuildingPhaseAsync()
        {
            return await _productToBuildingPhaseRepo.GetAsync(t => new ProductToBuildingPhaseModel
            {
                Id = t.ProductsToBuildingPhasesId,
                ProductId = t.ProductsId,
                BuildingPhaseId = t.BuildingPhasesId
            });
        }

        public async Task<List<ProductToBuildingPhaseAndStyleModel>> GetProductsToBuildingPhaseAndStyleAsync()
        {
            return await _productToBuildingPhaseAndStyleRepo.GetAsync(
                selector: t => new ProductToBuildingPhaseAndStyleModel
                {
                    ProductToBuildingPhaseId = t.ProductsToBuildingPhasesId,
                    ProductToStyleId = t.ProductsToStylesId,
                    ProductStyleIsDefault = t.ProductsToStylesIsDefault
                },
                predicate: t => t.ProductsToStylesIsDefault);
        }

        public async Task<List<ProductToStyleModel>> GetProductsToStyleAsync()
        {
            return await _productToStyleRepo.GetAllAsync();
        }

        public async Task<List<ProductToCategoryModel>> GetProductsToCategoryAsync()
        {
            return await _productToCategoryRepo.GetAllAsync();
        }

        public async Task<Dictionary<int, int>> GetReverseProductDictAsync()
        {
            var pairingProducts = await _pairingProductRepo.GetAllAsync();

            return pairingProducts
                .OrderBy(t => t.ProductId)
                .ToDictionary(t => t.ProductId, t => t.PairedProductId);
        }

        public async Task<Dictionary<string, Tuple<int, string>>> GetProductOrientationDictAsync()
        {
            var productOrientations = await _productOrientationRepo.GetAllAsync();
            return productOrientations.ToDictionary(t => t.Name, t => new Tuple<int, string>(t.Id, t.ShortDisplay));
        }

        public async Task<Dictionary<int, Tuple<string, string>>> GetProductOrientationIdKeyDictAsync()
        {
            var productOrientations = await _productOrientationRepo.GetAllAsync();
            return productOrientations.ToDictionary(t => (int)t.Id, t => new Tuple<string, string>(t.Name, t.ShortDisplay));
        }

        public async Task<List<BOMGeneratingProductModel>> GetProductsForBOMGenerationAsync(string SQLCommandName, string typeName, DataTable inputReportTable)
        {
            var result = new List<BOMGeneratingProductModel>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(SQLCommandName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = _timeout;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@TableInputReports",
                    Direction = ParameterDirection.Input,
                    SqlDbType = SqlDbType.Structured,
                    TypeName = typeName,
                    Value = inputReportTable
                });

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var model = new BOMGeneratingProductModel();

                        if (reader.TryGetValue<int>("AssetsId", out var assetId))
                            model.AssetId = assetId;

                        if (reader.TryGetValue<int>("OptionId", out var optionId))
                            model.OptionId = optionId;

                        if (reader.TryGetValue<int>("CustomOptionId", out var customOptionId))
                            model.CustomOptionId = customOptionId;

                        if (reader.TryGetValue<string>("DependentCondition", out var dependentCondition))
                            model.DependentCondition = dependentCondition;

                        if (reader.TryGetValue<int>("ProductToBuildingPhaseId", out var productToBuildingPhaseId))
                            model.ProductToBuildingPhaseId = productToBuildingPhaseId;

                        if (reader.TryGetValue<int>("ProductToStyleId", out var productToStyleId))
                            model.ProductToStyleId = productToStyleId;

                        if (reader.TryGetValue<decimal>("ProductQuantity", out var productQuantity))
                        {
                            model.ProductQuantity = productQuantity;
                            model.ProductQuantityTotal = productQuantity;
                        }

                        if (reader.TryGetValue<short>("KMSourceId", out var KMSourceId))
                            model.KMSourceId = KMSourceId;

                        if (reader.TryGetValue<int>("KMTypeId", out var KMTypeId))
                            model.KMTypeId = KMTypeId;

                        if (reader.TryGetValue<int>("BuildingPhaseId", out var buildingPhaseId))
                            model.BuildingPhaseId = buildingPhaseId;

                        if (reader.TryGetValue<int>("ProductId", out var productId))
                            model.ProductId = productId;

                        if (reader.TryGetValue<string>("ProductName", out var productName))
                            model.ProductName = productName;

                        if (reader.TryGetValue<int>("UseId", out var useId))
                            model.UseId = useId;

                        if (reader.TryGetValue<int>("GeneratedReportId", out var generatedReportId))
                            model.BOMGeneratedReportId = generatedReportId;

                        if (reader.TryGetValue<string>("Parameters", out var parameters))
                            model.Parameters = parameters;

                        result.Add(model);
                    }
                }
            }

            return result;
        }
    }
}
