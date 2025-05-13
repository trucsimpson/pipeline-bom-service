using BOMService.Application.Common.Interfaces;
using BOMService.Application.Models;
using BOMService.Application.Models.BOMStepOutputs;
using BOMService.Domain.Entities;
using BOMService.Domain.Enums;
using BOMService.Domain.Repositories;
using BOMService.Infrastructure.Persistence.EFModels;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

namespace BOMService.Infrastructure.Services
{
    public class BOMEngineService : IBOMEngineService
    {
        private readonly IBOMEngineManagerService _BOMEngineManagerService;
        private readonly IProductService _productService;
        private readonly ILogger<BOMEngineService> _logger;

        protected string _getsettingCommunitySpecificHouseQuantities = "Additive";
        protected bool _groupByParameter = false;
        protected string _groupByParameterList = "";

        public BOMEngineService(
            IBOMEngineManagerService BOMEngineManagerService,
            IProductService productService,
            ILogger<BOMEngineService> logger)
        {
            _BOMEngineManagerService = BOMEngineManagerService;
            _productService = productService;
            _logger = logger;
        }

        public async Task RunAsync()
        {
            var operationTimer = new Stopwatch();

            //TODO: Fake data for testing
            var BOMType = BOMTypes.House;
            var runningType = BOMRunningType.All;
            var BOMHeaders = new List<BOMHeaderModel>
            {
                new BOMHeaderModel
                {
                    HeaderArgs = new HouseOptionReportInputModel
                    {
                        CommunityId = 6122,
                        HouseId = 1645,
                        OptionId = 9799
                    }
                },
                new BOMHeaderModel
                {
                    HeaderArgs = new HouseOptionReportInputModel
                    {
                        CommunityId = 6122,
                        HouseId = 1645,
                        OptionId = 9800
                    }
                }
            };

            operationTimer.Start();
            var buildHeader = await TimestampHeadersStart(BOMType, runningType, BOMHeaders);
            if (!buildHeader.IsSuccess) return;
            operationTimer.Stop();
            _logger.LogInformation($"{nameof(TimestampHeadersStart)} took {operationTimer.ElapsedMilliseconds} ms");

            operationTimer.Start();
            var preparationData = await GetGeneratingProducts(BOMType, BOMHeaders, buildHeader.Data);
            if (!preparationData.IsSuccess) return;
            operationTimer.Stop();
            _logger.LogInformation($"{nameof(GetGeneratingProducts)} took {operationTimer.ElapsedMilliseconds} ms");
        }

        /// <summary>
        /// Generates fresh reports for each BOM header.
        /// <para>
        /// Links them with their corresponding GeneratedReportId, and prepares condition data for the next steps in BOM processing.
        /// </para>
        /// <para>
        /// Suggested name: BuildBOMHeadersFromReportsAsync
        /// </para>
        /// </summary>
        /// <param name="BOMType"></param>
        /// <param name="BOMRunningType"></param>
        /// <param name="BOMHeaders"></param>
        /// <returns></returns>
        public async Task<BaseResultModel<BOMHeaderMappingModel>> TimestampHeadersStart(BOMTypes BOMType, BOMRunningType BOMRunningType, List<BOMHeaderModel> BOMHeaders)
        {
            try
            {
                var nodeGeneratorIds = new Dictionary<int, int>();
                var headerOptionsAndConditions = new Dictionary<int, Dictionary<HeaderOptionConditionModel, bool>>();

                var inputReportTable = new DataTable();
                var reportResults = new List<HouseOptionReportResultModel>();
                var houseReportMap = new Dictionary<string, LinkedList<HouseReportDetailModel>>();
                var simpleReportMap = new Dictionary<string, int>();

                switch (BOMType)
                {
                    case BOMTypes.GlobalOption:
                        inputReportTable.Columns.AddRange(new[]
                        {
                            new DataColumn("CommunityId", typeof(int)),
                            new DataColumn("OptionId", typeof(int)),
                            new DataColumn("DependentCondition", typeof(string))
                        });

                        foreach (var header in BOMHeaders)
                        {
                            inputReportTable.Rows.Add(header.HeaderArgs.CommunityId, header.HeaderArgs.OptionId, header.HeaderArgs.DependentCondition);
                        }

                        reportResults = await _BOMEngineManagerService.ClearAndGenerateReportsAsync("BOM.WipeAndMakeNewGlobalOptionBOMReports", "BOM.GlobalOptionReportsToGenerateData", inputReportTable);

                        if (BOMRunningType == BOMRunningType.All) BOMRunningType = BOMRunningType.Multi;
                        break;
                    case BOMTypes.Worksheet:
                        inputReportTable.Columns.AddRange(new[]
                        {
                            new DataColumn("WorksheetId", typeof(int)),
                            new DataColumn("DependentCondition", typeof(string)),
                            new DataColumn("CommunityId", typeof(int))
                        });

                        foreach (var header in BOMHeaders)
                        {
                            inputReportTable.Rows.Add(header.HeaderArgs.WorksheetId, header.HeaderArgs.DependentCondition, header.HeaderArgs.CommunityId);
                        }

                        reportResults = await _BOMEngineManagerService.ClearAndGenerateReportsAsync("BOM.WipeAndMakeNewWorksheetBOMReports", "BOM.WorksheetReportsToGenerateData", inputReportTable);

                        break;
                    case BOMTypes.CustomOption:
                        inputReportTable.Columns.AddRange(new[]
                        {
                            new DataColumn("CustomOptionId", typeof(int)),
                            new DataColumn("DependentCondition", typeof(string)),
                            new DataColumn("CommunityId", typeof(int))
                        });

                        foreach (var header in BOMHeaders)
                        {
                            inputReportTable.Rows.Add(header.HeaderArgs.CustomOptionId, header.HeaderArgs.DependentCondition, header.HeaderArgs.CommunityId);
                        }

                        reportResults = await _BOMEngineManagerService.ClearAndGenerateReportsAsync("BOM.WipeAndMakeNewCustomOptionBOMReports", "BOM.CustomOptionReportsToGenerateData", inputReportTable);
                        break;
                    case BOMTypes.Job:
                        inputReportTable.Columns.AddRange(new[]
                        {
                            new DataColumn("JobId", typeof(int)),
                            new DataColumn("LastConfigurationNumber", typeof(int)),
                            new DataColumn("RunNumber", typeof(int))
                        });

                        foreach (var header in BOMHeaders)
                        {
                            inputReportTable.Rows.Add(header.HeaderArgs.JobId, header.HeaderArgs.LastConfigurationNumber, header.HeaderArgs.RunNumber);
                        }

                        reportResults = await _BOMEngineManagerService.ClearAndGenerateReportsAsync("BOM.WipeAndMakeNewJobBOMReports", "BOM.JobReportsToGenerateData", inputReportTable);
                        break;
                    case BOMTypes.House:
                        inputReportTable.Columns.AddRange(new[]
                        {
                            new DataColumn("CommunityId", typeof(int)),
                            new DataColumn("HouseId", typeof(int)),
                            new DataColumn("OptionId", typeof(int)),
                            new DataColumn("DependentCondition", typeof(string))
                        });

                        foreach (var header in BOMHeaders)
                        {
                            inputReportTable.Rows.Add(header.HeaderArgs.CommunityId, header.HeaderArgs.HouseId, header.HeaderArgs.OptionId, header.HeaderArgs.DependentCondition);
                        }

                        reportResults = await _BOMEngineManagerService.ClearAndGenerateReportsAsync("BOM.WipeAndMakeNewHouseOptionBOMReports", "BOM.HouseOptionReportsToGenerateData", inputReportTable);

                        //TODO: Why we need to set BOMRunningType to Multi here?
                        if (BOMRunningType == BOMRunningType.All) BOMRunningType = BOMRunningType.Multi;
                        break;
                    case BOMTypes.LBMJob:
                        inputReportTable.Columns.AddRange(new[]
                        {
                            new DataColumn("JobId", typeof(int)),
                            new DataColumn("RunNumber", typeof(int))
                        });

                        foreach (var header in BOMHeaders)
                        {
                            inputReportTable.Rows.Add(header.HeaderArgs.JobId, header.HeaderArgs.RunNumber);
                        }

                        reportResults = await _BOMEngineManagerService.ClearAndGenerateReportsAsync("BOM.WipeAndMakeNewLBMJobBOMReports", "BOM.LBMJobBOMReportsToGenerateData", inputReportTable);
                        break;
                    default:
                        return BaseResultModel<BOMHeaderMappingModel>.Failure("BOM Type not set.");
                }

                foreach (var report in reportResults)
                {
                    var reportParams = report.ReportParams;
                    var generatedReportId = report.GeneratedReportId;

                    if (BOMType == BOMTypes.House)
                    {
                        var detail = new HouseReportDetailModel
                        {
                            GeneratedReportId = generatedReportId,
                            OptionId = report.OptionId,
                            DependentCondition = report.DependentCondition
                        };

                        if (!houseReportMap.ContainsKey(reportParams))
                        {
                            houseReportMap[reportParams] = new LinkedList<HouseReportDetailModel>();
                        }
                        houseReportMap[reportParams].AddLast(detail);
                    }
                    else
                    {
                        simpleReportMap[reportParams] = generatedReportId;
                    }
                }

                foreach (var header in BOMHeaders)
                {
                    string reportParams = BOMType switch
                    {
                        BOMTypes.GlobalOption => $"goid={header.HeaderArgs.OptionId}&condition={header.HeaderArgs.DependentCondition}&community={header.HeaderArgs.CommunityId}",
                        BOMTypes.Worksheet => $"ws={header.HeaderArgs.WorksheetId}&condition={header.HeaderArgs.DependentCondition}&community={header.HeaderArgs.CommunityId}",
                        BOMTypes.CustomOption => $"coid={header.HeaderArgs.CustomOptionId}&condition={header.HeaderArgs.DependentCondition}&community={header.HeaderArgs.CommunityId}",
                        BOMTypes.Job => $"job={header.HeaderArgs.JobId}&lcfg={header.HeaderArgs.LastConfigurationNumber}&run={header.HeaderArgs.RunNumber}",
                        BOMTypes.House => $"community={header.HeaderArgs.CommunityId}&house={header.HeaderArgs.HouseId}",
                        BOMTypes.LBMJob => $"job={header.HeaderArgs.JobId}&run={header.HeaderArgs.RunNumber}",
                        _ => string.Empty
                    };

                    if (BOMType == BOMTypes.House && houseReportMap.TryGetValue(reportParams, out var details))
                    {
                        header.GeneratedReportId = details.First.Value.GeneratedReportId;
                        nodeGeneratorIds[header.GeneratedReportId] = 0;
                    }
                    else if (BOMType != BOMTypes.House && simpleReportMap.TryGetValue(reportParams, out var reportId))
                    {
                        header.GeneratedReportId = reportId;
                        nodeGeneratorIds[reportId] = 0;

                        //Generate the data for HeaderOptionsAndConditions, to avoid affecting another bom type
                        var key = new HeaderOptionConditionModel
                        {
                            OptionId = header.HeaderArgs.OptionId,
                            DependentCondition = header.HeaderArgs.DependentCondition
                        };
                        if (!headerOptionsAndConditions.ContainsKey(reportId))
                            headerOptionsAndConditions[reportId] = new Dictionary<HeaderOptionConditionModel, bool>();

                        headerOptionsAndConditions[reportId][key] = false;
                    }
                }

                if (BOMType == BOMTypes.House)
                {
                    BOMHeaders = BOMHeaders
                        .GroupBy(h => h.GeneratedReportId)
                        .Select(g => g.First())
                        .ToList();

                    foreach (var reportDetailList in houseReportMap.Values)
                    {
                        foreach (var detail in reportDetailList)
                        {
                            var key = new HeaderOptionConditionModel { OptionId = detail.OptionId, DependentCondition = detail.DependentCondition };

                            if (!headerOptionsAndConditions.ContainsKey(detail.GeneratedReportId))
                                headerOptionsAndConditions[detail.GeneratedReportId] = new Dictionary<HeaderOptionConditionModel, bool>();

                            headerOptionsAndConditions[detail.GeneratedReportId][key] = false;
                        }
                    }
                }

                return BaseResultModel<BOMHeaderMappingModel>.Success(new BOMHeaderMappingModel
                {
                    Headers = BOMHeaders,
                    DictionaryNodeGeneratorIds = nodeGeneratorIds,
                    HeaderOptionsAndConditions = headerOptionsAndConditions
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(TimestampHeadersStart)}");
                return BaseResultModel<BOMHeaderMappingModel>.Failure($"Error in {nameof(TimestampHeadersStart)}");
            }
        }

        public async Task<BaseResultModel<BOMPreparationDataModel>> GetGeneratingProducts(BOMTypes BOMType, List<BOMHeaderModel> BOMHeaders, BOMHeaderMappingModel headerResult)
        {
            var productDict = new Dictionary<int, string>();

            var productToBuildingPhaseConversionDict = new Dictionary<int, ProductToBuildingPhaseConversionInformationNode>();
            var productToBuildingPhaseDict = new Dictionary<ProductPhaseKey, int>();

            var productToStyleDict = new Dictionary<int, ProductToStyleLookupNode>();
            var defaultProductToStyleDict = new Dictionary<int, Tuple<int, int>>();
            var productLookupForStyleDict = new Dictionary<int, ProductInverseStyleLookupNode>();
            var productToStyleIdDict = new Dictionary<ProductStyleKey, int>();

            var productToCategoryDict = new Dictionary<Tuple<int, int>, int>();
            var productLookupCategoryIdsDict = new Dictionary<int, List<int>>();

            var reverseProductDict = new Dictionary<int, int>();
            var systemOrientationDict = new Dictionary<string, Tuple<int, string>>();
            var systemOrientationIdKeyDict = new Dictionary<int, Tuple<string, string>>();

            var jobOrientation = new JobFlipStatusModel();

            var listProductsWithOutStyle = new List<string>();

            var generatingProducts = new LinkedList<BOMGeneratingProductModel>();

            if (!productDict.Any())
            {
                productDict = await _productService.GetProductDictAsync();
            }

            // Get product => building phase
            if (!productToBuildingPhaseConversionDict.Any())
            {
                var productsToBuildingPhase = await _productService.GetProductsToBuildingPhaseAsync();
                foreach (var p in productsToBuildingPhase)
                {
                    productToBuildingPhaseConversionDict[p.Id] = new ProductToBuildingPhaseConversionInformationNode
                    {
                        BuildingPhaseId = p.BuildingPhaseId,
                        ProductId = p.ProductId
                    };
                    productToBuildingPhaseDict[new ProductPhaseKey(p.ProductId, p.BuildingPhaseId)] = p.Id;
                }
            }

            // Get product => style
            if (!productToStyleDict.Any())
            {
                var productToStyles = await _productService.GetProductsToStyleAsync();
                foreach (var item in productToStyles)
                {
                    var keyStyle = new ProductStyleKey(item.ProductId, item.StyleId);
                    var chainNode = new ProductInverseStyleLookupNode
                    {
                        StyleId = item.StyleId,
                        ProductToStyleId = item.Id,
                        Link = productLookupForStyleDict.ContainsKey(item.ProductId) ? productLookupForStyleDict[item.ProductId] : null
                    };

                    productToStyleDict[item.Id] = new ProductToStyleLookupNode
                    {
                        ProductId = item.ProductId,
                        StyleId = item.StyleId
                    };

                    if (item.IsDefault)
                    {
                        defaultProductToStyleDict[item.ProductId] = new Tuple<int, int>(item.StyleId, item.Id);
                    }

                    productLookupForStyleDict[item.ProductId] = chainNode;

                    if (productToStyleIdDict.ContainsKey(keyStyle))
                        productToStyleIdDict[keyStyle] = item.Id;
                    else
                        productToStyleIdDict.Add(keyStyle, item.Id);
                }
            }

            // Get product => category
            if (!productToCategoryDict.Any())
            {
                var productToCategories = await _productService.GetProductsToCategoryAsync();
                foreach (var item in productToCategories)
                {
                    productToCategoryDict[new Tuple<int, int>(item.ProductId, item.CategoryId)] = item.Id;

                    if (!productLookupCategoryIdsDict.TryGetValue(item.ProductId, out var categories))
                    {
                        categories = new List<int>();
                        productLookupCategoryIdsDict[item.ProductId] = categories;
                    }

                    categories.Add(item.CategoryId);
                }
            }

            // Get default style mapping
            var defaultProductToStyles = await _productService.GetProductsToBuildingPhaseAndStyleAsync();
            var productDefaultStyleLookup = defaultProductToStyles
                .ToDictionary(
                    x => x.ProductToBuildingPhaseId,
                    x => x.ProductToStyleId
                );
            if (!productDefaultStyleLookup.Any())
            {
                return BaseResultModel<BOMPreparationDataModel>.Failure("No products / styles found for GetProductsForBOMGeneration!");
            }

            if (!reverseProductDict.Any())
            {
                reverseProductDict = await _productService.GetReverseProductDictAsync();
            }

            if (!systemOrientationDict.Any())
            {
                systemOrientationDict = await _productService.GetProductOrientationDictAsync();
            }

            if (!systemOrientationIdKeyDict.Any())
            {
                systemOrientationIdKeyDict = await _productService.GetProductOrientationIdKeyDictAsync();
            }

            // Prepare input report table
            var productNameNeededForThings = false;
            var SQLCommandName = string.Empty;
            var typeName = string.Empty;
            var inputReportTable = new DataTable();

            switch (BOMType)
            {
                case BOMTypes.GlobalOption:
                    productNameNeededForThings = true;
                    SQLCommandName = "BOM.GetGlobalOptionBOMGeneratingProducts";
                    typeName = "BOM.GlobalOptionInputReports";

                    inputReportTable.Columns.AddRange(new[]
                    {
                        new DataColumn("GlobalOptionId", typeof(int)),
                        new DataColumn("DependentCondition", typeof(string)),
                        new DataColumn("CommunityId", typeof(int)),
                        new DataColumn("GeneratedReportId", typeof(int))
                    });

                    foreach (var header in BOMHeaders)
                    {
                        inputReportTable.Rows.Add(header.HeaderArgs.OptionId, header.HeaderArgs.DependentCondition, header.HeaderArgs.CommunityId, header.GeneratedReportId);
                    }
                    break;
                case BOMTypes.Worksheet:
                    SQLCommandName = "BOM.GetWorksheetBOMGeneratingProducts";
                    typeName = "BOM.WorksheetInputReports";

                    inputReportTable.Columns.AddRange(new[]
                    {
                        new DataColumn("WorksheetId", typeof(int)),
                        new DataColumn("DependentCondition", typeof(string)),
                        new DataColumn("GeneratedReportId", typeof(int))
                    });

                    foreach (var header in BOMHeaders)
                    {
                        inputReportTable.Rows.Add(header.HeaderArgs.WorksheetId, string.Empty, header.GeneratedReportId);
                    }
                    break;
                case BOMTypes.CustomOption:
                    productNameNeededForThings = true;
                    SQLCommandName = "BOM.GetCustomOptionBOMGeneratingProducts";
                    typeName = "BOM.CustomOptionInputReports";

                    inputReportTable.Columns.AddRange(new[]
                    {
                        new DataColumn("CustomOptionId", typeof(int)),
                        new DataColumn("DependentCondition", typeof(string)),
                        new DataColumn("GeneratedReportId", typeof(int))
                    });

                    foreach (var header in BOMHeaders)
                    {
                        inputReportTable.Rows.Add(header.HeaderArgs.CustomOptionId, string.Empty, header.GeneratedReportId);
                    }
                    break;
                case BOMTypes.Job:
                    productNameNeededForThings = true;
                    SQLCommandName = "BOM.GetJobBOMGeneratingProducts";
                    typeName = "BOM.JobInputReports";

                    inputReportTable.Columns.AddRange(new[]
                    {
                        new DataColumn("JobId", typeof(int)),
                        new DataColumn("GeneratedReportId", typeof(int))
                    });

                    foreach (var header in BOMHeaders)
                    {
                        inputReportTable.Rows.Add(header.HeaderArgs.JobId, header.GeneratedReportId);
                    }

                    var jobIds = BOMHeaders.First().HeaderArgs.JobId.ToString() + ",";
                    var jobFlipStatuses = await _BOMEngineManagerService.GetJobFlipStatusesAsync(jobIds);
                    foreach (var item in jobFlipStatuses)
                    {
                        jobOrientation = item;
                    }
                    break;
                case BOMTypes.House:
                    SQLCommandName = _getsettingCommunitySpecificHouseQuantities == "Additive"
                        ? "BOM.GetALLHouseOptionBOMGeneratingProducts"
                        : "BOM.GetHouseOptionBOMGeneratingProducts";
                    typeName = "BOM.HouseOptionInputReports";

                    inputReportTable.Columns.AddRange(new[]
                    {
                        new DataColumn("HouseId", typeof(int)),
                        new DataColumn("OptionId", typeof(int)),
                        new DataColumn("DependentCondition", typeof(string)),
                        new DataColumn("CommunityId", typeof(int)),
                        new DataColumn("GeneratedReportId", typeof(int))
                    });

                    foreach (var header in BOMHeaders)
                    {
                        if (headerResult.HeaderOptionsAndConditions.ContainsKey(header.GeneratedReportId))
                        {
                            foreach (var optConItem in headerResult.HeaderOptionsAndConditions[header.GeneratedReportId].Keys)
                            {
                                inputReportTable.Rows.Add(header.HeaderArgs.HouseId, optConItem.OptionId, optConItem.DependentCondition, header.HeaderArgs.CommunityId, header.GeneratedReportId);
                            }
                        }
                    }
                    break;
                case BOMTypes.LBMJob:
                    productNameNeededForThings = true;
                    SQLCommandName = "BOM.GetLBMJobBOMGeneratingProducts";
                    typeName = "BOM.LBMJobInputReports";

                    inputReportTable.Columns.AddRange(new[]
                    {
                        new DataColumn("JobId", typeof(int)),
                        new DataColumn("GeneratedReportId", typeof(int))
                    });

                    foreach (var header in BOMHeaders)
                    {
                        inputReportTable.Rows.Add(header.HeaderArgs.JobId, header.GeneratedReportId);
                    }
                    break;
                default:
                    return BaseResultModel<BOMPreparationDataModel>.Failure("Unable to retrieve products for BOM generation at this time in place #02. Error: Selected AtomicBOMTypes is not handled");
            }

            // Get actual product list
            var groupParams = _groupByParameterList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var productsForBOMGeneration = await _productService.GetProductsForBOMGenerationAsync(SQLCommandName, typeName, inputReportTable);
            foreach (var product in productsForBOMGeneration)
            {
                // Preserve raw info
                var tempBuildingPhaseId = product.BuildingPhaseId;
                var tempProductId = product.ProductId;
                var tempProductName = product.ProductName;

                // Reset
                product.BuildingPhaseId = 0;
                product.ProductId = 0;
                product.ProductName = string.Empty;

                if (BOMType == BOMTypes.House)
                {
                    if (!_groupByParameter)
                    {
                        product.Parameters = string.Empty;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(product.Parameters) && groupParams.Length > 0)
                        {
                            var validParams = product.Parameters
                                .Split('|', StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => p.Split('='))
                                .Where(p => p.Length > 1 && groupParams.Contains(p[0]))
                                .Select(p => $"{p[0]}={p[1]}");

                            product.Parameters = string.Join(";", validParams);
                        }
                    }
                }

                if (product.ProductToStyleId == 0 && productDefaultStyleLookup.ContainsKey(product.ProductToBuildingPhaseId))
                {
                    product.ProductToStyleId = productDefaultStyleLookup[product.ProductToBuildingPhaseId];
                }

                if (productNameNeededForThings)
                {
                    product.BuildingPhaseId = tempBuildingPhaseId;
                    product.ProductId = tempProductId;
                    product.ProductName = tempProductName;

                    if (product.ProductToStyleId <= 0)
                    {
                        listProductsWithOutStyle.Add(product.ProductName);
                    }
                    else if (productToStyleDict.TryGetValue(product.ProductToStyleId, out var node))
                    {
                        product.StyleId = node.StyleId;
                    }
                }
                else
                {
                    var key = productToBuildingPhaseDict
                        .Where(t => t.Value == product.ProductToBuildingPhaseId)
                        .Select(t => t.Key)
                        .FirstOrDefault();

                    if (key != null)
                    {
                        product.BuildingPhaseId = key.BuildingPhaseId;
                        product.ProductId = key.ProductId;
                        product.ProductName = productDict[product.ProductId];

                        if (product.ProductToStyleId == 0 && defaultProductToStyleDict.ContainsKey(product.ProductId))
                        {
                            product.StyleId = defaultProductToStyleDict[product.ProductId].Item1;
                            product.ProductToStyleId = defaultProductToStyleDict[product.ProductId].Item2;
                        }
                        else if (productLookupForStyleDict.TryGetValue(product.ProductId, out var lookupNode))
                        {
                            do
                            {
                                product.StyleId = lookupNode.StyleId;
                                lookupNode = lookupNode.Link;
                            }
                            while (lookupNode != null && lookupNode.ProductToStyleId != product.ProductToStyleId);
                        }
                    }
                }

                if (product.BOMGeneratedReportId > 0)
                {
                    product.NodeId = GetNextTraceNodeId(headerResult.DictionaryNodeGeneratorIds, product.BOMGeneratedReportId);
                    generatingProducts.AddLast(product);
                }
            }

            if (listProductsWithOutStyle.Any())
            {
                var productsOutStyle = String.Join(", ", listProductsWithOutStyle.Distinct());
                var errorMsg = string.Empty;
                switch (BOMType)
                {
                    case BOMTypes.CustomOption:
                        errorMsg = $"Custom Option BOM may not contain all Products, the following {productsOutStyle} are missing Styles.";
                        break;
                    case BOMTypes.Job:
                        errorMsg = $"Job BOM may not contain all Products, the following {productsOutStyle} are missing Styles.";
                        break;
                    case BOMTypes.GlobalOption:
                        errorMsg = $"Global Option BOM may not contain all Products, the following {productsOutStyle} are missing Styles.";
                        break;
                }
                return BaseResultModel<BOMPreparationDataModel>.Failure(errorMsg);
            }

            return BaseResultModel<BOMPreparationDataModel>.Success(new BOMPreparationDataModel
            {
                ProductDict = productDict
            });
        }

        protected int GetNextTraceNodeId(Dictionary<int, int> nodeGeneratorIds, int generatedReportId)
        {
            if (!nodeGeneratorIds.ContainsKey(generatedReportId))
            {
                nodeGeneratorIds[generatedReportId] = 0;
            }

            return ++nodeGeneratorIds[generatedReportId];
        }
    }

    public record ProductPhaseKey(int ProductId, int BuildingPhaseId);

    public record ProductStyleKey(int ProductId, int StyleId);

    /// <summary>
    /// Suggested name: ProductToBuildingPhaseNode
    /// </summary>
    public class ProductToBuildingPhaseConversionInformationNode
    {
        public int BuildingPhaseId { get; set; }
        public int ProductId { get; set; }
    }

    /// <summary>
    /// Suggested name: ProductStyleNode
    /// </summary>
    public class ProductToStyleLookupNode
    {
        public int ProductId { get; set; }
        public int StyleId { get; set; }
    }

    /// <summary>
    /// Suggested name: StyleProductChainNode
    /// </summary>
    public class ProductInverseStyleLookupNode
    {
        public int StyleId { get; set; }
        public int ProductToStyleId { get; set; }
        public ProductInverseStyleLookupNode? Link { get; set; }
    }
}
