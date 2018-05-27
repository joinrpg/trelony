using System.Collections.Generic;
using System.Threading.Tasks;

namespace Joinrpg.Trelony.Repositories.Interfaces
{
    public interface ICalendarRepository
    {
        Task<IReadOnlyList<CalendarRow>> GetCalendar(int year, int? gameRegionId);
        Task<IReadOnlyList<int>> GetYears();
        Task<IReadOnlyList<MacroRegionRow>> GetMacroRegionsAsync();
    }
}
