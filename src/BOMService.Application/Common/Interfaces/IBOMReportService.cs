using BOMService.Application.Models;
using System.Data;

namespace BOMService.Application.Common.Interfaces
{
    public interface IBOMReportService
    {
        //Old name: WipeOutOldReportsAndMakeNewOnes
        Task<List<HouseOptionReportResultModel>> ClearAndGenerateReportsAsync(string SQLCommandName, string typeName, DataTable dtInputReports);
    }
}
