using BOMService.Application.Common.Interfaces;
using BOMService.Application.Models;
using BOMService.Infrastructure.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BOMService.Infrastructure.Services
{
    public class BOMReportService : IBOMReportService
    {
        private readonly string _connectionString;

        public BOMReportService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BOMDatabase");
        }

        //Old name: WipeOutOldReportsAndMakeNewOnes
        public async Task<List<HouseOptionReportResultModel>> ClearAndGenerateReportsAsync(string SQLCommandName, string typeName, DataTable dtInputReports)
        {
            var result = new List<HouseOptionReportResultModel>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(SQLCommandName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 300; //TODO: move to settings

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@TableInputs",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = typeName,
                    Value = dtInputReports
                });

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var model = new HouseOptionReportResultModel();

                        if (reader.TryGetValue<int>("GeneratedReportId", out var reportId))
                            model.GeneratedReportId = reportId;

                        if (reader.TryGetValue<string>("ReportParams", out var reportParams))
                            model.ReportParams = reportParams;

                        if (reader.TryGetValue<int>("OptionId", out var optionId))
                            model.OptionId = optionId;

                        if (reader.TryGetValue<string>("DependentCondition", out var condition))
                            model.DependentCondition = condition;

                        result.Add(model);
                    }
                }
            }

            return result;
        }
    }
}
