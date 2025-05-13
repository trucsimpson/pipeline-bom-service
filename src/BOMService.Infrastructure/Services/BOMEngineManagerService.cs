using BOMService.Application.Common.Interfaces;
using BOMService.Application.Models;
using BOMService.Infrastructure.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BOMService.Infrastructure.Services
{
    public class BOMEngineManagerService : IBOMEngineManagerService
    {
        private readonly string _connectionString;
        private readonly ILogger<BOMEngineManagerService> _logger;
        private readonly int _timeout; //TODO: move to settings

        public BOMEngineManagerService(IConfiguration configuration, ILogger<BOMEngineManagerService> logger)
        {
            _connectionString = configuration.GetConnectionString("BOMDatabase");
            _logger = logger;
            _timeout = 300;
        }

        public async Task<List<HouseOptionReportResultModel>> ClearAndGenerateReportsAsync(string SQLCommandName, string typeName, DataTable inputReportTable)
        {
            var result = new List<HouseOptionReportResultModel>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(SQLCommandName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = _timeout;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@TableInputs",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = typeName,
                    Value = inputReportTable
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

        public async Task<List<JobFlipStatusModel>> GetJobFlipStatusesAsync(string jobIds)
        {
            var result = new List<JobFlipStatusModel>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("BOM.GetJobFlipStatuses", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = _timeout;
                cmd.Parameters.AddWithValue("@Job_Ids", jobIds);

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var model = new JobFlipStatusModel();

                        if (reader.TryGetValue<int>("Jobs_Id", out var jobId))
                            model.JobId = jobId;

                        if (reader.TryGetValue<short>("Jobs_Reversed", out var jobReversed))
                            model.JobReversed = jobReversed;

                        result.Add(model);
                    }
                }
            }

            return result;
        }
    }
}
