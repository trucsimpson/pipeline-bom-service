using BOMService.Application.Models;
using System.Data;

namespace BOMService.Application.Common.Interfaces
{
    public interface IBOMReportService
    {
        /// <summary>
        /// Deletes old reports and generates new ones based on current requirements.
        /// <para>
        /// Old name: WipeOutOldReportsAndMakeNewOnes
        /// </para>
        /// </summary>
        /// <param name="SQLCommandName"></param>
        /// <param name="typeName"></param>
        /// <param name="dtInputReports"></param>
        /// <returns></returns>
        Task<List<HouseOptionReportResultModel>> ClearAndGenerateReportsAsync(string SQLCommandName, string typeName, DataTable dtInputReports);
    }
}
