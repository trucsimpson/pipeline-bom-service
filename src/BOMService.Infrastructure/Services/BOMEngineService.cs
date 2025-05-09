using BOMService.Application.Common.Interfaces;
using BOMService.Application.Models;
using BOMService.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection.PortableExecutable;

namespace BOMService.Infrastructure.Services
{
    public class BOMEngineService : IBOMEngineService
    {
        private readonly IBOMReportService _BOMReportService;
        private readonly ILogger<BOMEngineService> _logger;

        public BOMEngineService(
            IBOMReportService BOMReportService,
            ILogger<BOMEngineService> logger)
        {
            _BOMReportService = BOMReportService;
            _logger = logger;
        }

        public async Task RunAsync()
        {
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

            var buildHeader = await BuildBOMHeadersFromReportsAsync(BOMType, runningType, BOMHeaders);
            if (!buildHeader.Headers.Any()) return;
        }

        //Old name: TimestampHeadersStart
        public async Task<BOMHeaderMappingResultModel> BuildBOMHeadersFromReportsAsync(BOMTypes BOMType, BOMRunningType BOMRunningType, List<BOMHeaderModel> BOMHeaders)
        {
            try
            {
                var nodeGeneratorIds = new Dictionary<int, int>();
                var headerOptionsAndConditions = new Dictionary<int, Dictionary<HeaderOptionConditionModel, bool>>();

                var dtInputReports = new DataTable();
                var reportResults = new List<HouseOptionReportResultModel>();
                var houseReportMap = new Dictionary<string, LinkedList<HouseReportDetailModel>>();
                var simpleReportMap = new Dictionary<string, int>();

                switch (BOMType)
                {
                    case BOMTypes.GlobalOption:
                        dtInputReports.Columns.Add("CommunityId", typeof(int));
                        dtInputReports.Columns.Add("OptionId", typeof(int));
                        dtInputReports.Columns.Add("DependentCondition", typeof(string));

                        foreach (var header in BOMHeaders)
                        {
                            dtInputReports.Rows.Add(header.HeaderArgs.CommunityId, header.HeaderArgs.OptionId, header.HeaderArgs.DependentCondition);
                        }

                        reportResults = await _BOMReportService.ClearAndGenerateReportsAsync("BOM.WipeAndMakeNewGlobalOptionBOMReports", "BOM.GlobalOptionReportsToGenerateData", dtInputReports);

                        if (BOMRunningType == BOMRunningType.All) BOMRunningType = BOMRunningType.Multi;
                        break;
                    case BOMTypes.Worksheet:
                        dtInputReports.Columns.Add("WorksheetId", typeof(int));
                        dtInputReports.Columns.Add("DependentCondition", typeof(string));
                        dtInputReports.Columns.Add("CommunityId", typeof(int));

                        foreach (var header in BOMHeaders)
                        {
                            dtInputReports.Rows.Add(header.HeaderArgs.WorksheetId, header.HeaderArgs.DependentCondition, header.HeaderArgs.CommunityId);
                        }

                        reportResults = await _BOMReportService.ClearAndGenerateReportsAsync("BOM.WipeAndMakeNewWorksheetBOMReports", "BOM.WorksheetReportsToGenerateData", dtInputReports);

                        break;
                    case BOMTypes.CustomOption:
                        dtInputReports.Columns.Add("CustomOptionId", typeof(int));
                        dtInputReports.Columns.Add("DependentCondition", typeof(string));
                        dtInputReports.Columns.Add("CommunityId", typeof(int));

                        foreach (var header in BOMHeaders)
                        {
                            dtInputReports.Rows.Add(header.HeaderArgs.CustomOptionId, header.HeaderArgs.DependentCondition, header.HeaderArgs.CommunityId);
                        }

                        reportResults = await _BOMReportService.ClearAndGenerateReportsAsync("BOM.WipeAndMakeNewCustomOptionBOMReports", "BOM.CustomOptionReportsToGenerateData", dtInputReports);
                        break;
                    case BOMTypes.Job:
                        dtInputReports.Columns.Add("JobId", typeof(int));
                        dtInputReports.Columns.Add("LastConfigurationNumber", typeof(int));
                        dtInputReports.Columns.Add("RunNumber", typeof(int));

                        foreach (var header in BOMHeaders)
                        {
                            dtInputReports.Rows.Add(header.HeaderArgs.JobId, header.HeaderArgs.LastConfigurationNumber, header.HeaderArgs.RunNumber);
                        }

                        reportResults = await _BOMReportService.ClearAndGenerateReportsAsync("BOM.WipeAndMakeNewJobBOMReports", "BOM.JobReportsToGenerateData", dtInputReports);
                        break;
                    case BOMTypes.House:
                        dtInputReports.Columns.Add("CommunityId", typeof(int));
                        dtInputReports.Columns.Add("HouseId", typeof(int));
                        dtInputReports.Columns.Add("OptionId", typeof(int));
                        dtInputReports.Columns.Add("DependentCondition", typeof(string));

                        foreach (var header in BOMHeaders)
                        {
                            dtInputReports.Rows.Add(header.HeaderArgs.CommunityId, header.HeaderArgs.HouseId, header.HeaderArgs.OptionId, header.HeaderArgs.DependentCondition);
                        }

                        reportResults = await _BOMReportService.ClearAndGenerateReportsAsync("BOM.WipeAndMakeNewHouseOptionBOMReports", "BOM.HouseOptionReportsToGenerateData", dtInputReports);

                        //TODO: Why we need to set BOMRunningType to Multi here?
                        if (BOMRunningType == BOMRunningType.All) BOMRunningType = BOMRunningType.Multi;
                        break;
                    case BOMTypes.LBMJob:
                        dtInputReports.Columns.Add("JobId", typeof(int));
                        dtInputReports.Columns.Add("RunNumber", typeof(int));

                        foreach (var header in BOMHeaders)
                        {
                            dtInputReports.Rows.Add(header.HeaderArgs.JobId, header.HeaderArgs.RunNumber);
                        }

                        reportResults = await _BOMReportService.ClearAndGenerateReportsAsync("BOM.WipeAndMakeNewLBMJobBOMReports", "BOM.LBMJobBOMReportsToGenerateData", dtInputReports);
                        break;
                    default:
                        break;
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
                        BOMTypes.Job => $"job={header.HeaderArgs.JobId}&lcfg={header.HeaderArgs.lcfg}&run={header.HeaderArgs.run}",
                        BOMTypes.House => $"community={header.HeaderArgs.CommunityId}&house={header.HeaderArgs.HouseId}",
                        BOMTypes.LBMJob => $"job={header.HeaderArgs.JobId}&run={header.HeaderArgs.run}",
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

                return new BOMHeaderMappingResultModel
                {
                    Headers = BOMHeaders,
                    NodeGeneratorIds = nodeGeneratorIds,
                    HeaderOptionsAndConditions = headerOptionsAndConditions
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {nameof(BuildBOMHeadersFromReportsAsync)}");
                return new BOMHeaderMappingResultModel();
            }
        }
    }
}
