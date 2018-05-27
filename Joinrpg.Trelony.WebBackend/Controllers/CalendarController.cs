using System.Collections.Generic;
using System.Threading.Tasks;
using Joinrpg.Trelony.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Joinrpg.Trelony.WebBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class CalendarController : Controller
    {
        private ICalendarRepository CalendarRepository { get; }

        public CalendarController(ICalendarRepository calendarRepository)
        {
            CalendarRepository = calendarRepository;
        }

        [HttpGet]
        public Task<IReadOnlyList<CalendarRow>> Get([FromQuery] int year, [FromQuery] int? macroRegionId)
        {
            return CalendarRepository.GetCalendar(year, macroRegionId);
        }

        [HttpGet]
        public Task<IReadOnlyList<int>> GetYears() => CalendarRepository.GetYears();

        [HttpGet]
        public Task<IReadOnlyList<MacroRegionRow>> GetMacroRegions() => CalendarRepository.GetMacroRegionsAsync();
    }
}