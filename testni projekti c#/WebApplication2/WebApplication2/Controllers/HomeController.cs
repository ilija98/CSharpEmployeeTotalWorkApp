using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication2.Models;


public class HomeController : Controller
{
    public async Task<IActionResult> Index()
    {
        List<EmployeeEntry> entries = await GetEmployeeEntries();

        var groupedEntries = entries.GroupBy(e => e.EmployeeName)
                                    .Select(g => new EmployeeWorkSummary
                                    {
                                        EmployeeName = g.Key,
                                        TotalWorkHours = g.Sum(e => (e.EndTimeUtc - e.StarTimeUtc).TotalHours)
                                    })
                                    .ToList();

        return View(groupedEntries);
    }

    private async Task<List<EmployeeEntry>> GetEmployeeEntries()
    {
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync("https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==");
            string apiResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<EmployeeEntry>>(apiResponse);
        }
    }
}
