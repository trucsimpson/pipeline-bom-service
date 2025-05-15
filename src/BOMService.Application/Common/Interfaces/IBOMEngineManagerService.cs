using BOMService.Application.DTOs;
using System.Data;

namespace BOMService.Application.Common.Interfaces
{
    /// <summary>
    /// This interface is temporarily used to contain the functions in the BOMEngineManager class. Will refactor later.
    /// Old name: BOMEngineManager
    /// </summary>
    public interface IBOMEngineManagerService
    {
        /// <summary>
        /// Deletes old reports and generates new ones based on current requirements.
        /// <para>
        /// Old name: WipeOutOldReportsAndMakeNewOnes
        /// </para>
        /// </summary>
        /// <param name="SQLCommandName"></param>
        /// <param name="typeName"></param>
        /// <param name="inputReportTable"></param>
        /// <returns></returns>
        Task<List<HouseOptionReportResultDto>> ClearAndGenerateReportsAsync(string SQLCommandName, string typeName, DataTable inputReportTable);

        /// <summary>
        /// Get the status of the job flip.
        /// <para>
        /// Old name: GetJobFlipStatuses_DataCall001
        /// </para>
        /// </summary>
        /// <param name="jobIds"></param>
        /// <returns></returns>
        Task<List<JobFlipStatusDto>> GetJobFlipStatusesAsync(string jobIds);
    }
}
