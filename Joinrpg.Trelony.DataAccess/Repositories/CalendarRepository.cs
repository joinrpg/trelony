using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Joinrpg.Trelony.DataModel;
using Joinrpg.Trelony.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Joinrpg.Trelony.DataAccess.Repositories
{
    [UsedImplicitly]
    public class CalendarRepository : ICalendarRepository
    {
        private TrelonyContext Context { get; }

        public CalendarRepository(TrelonyContext context)
        {
            Context = context;
        }

        public async Task<IReadOnlyList<CalendarRow>> GetCalendar(int year, int? gameRegionId)
        {
            return await Context
                .Games
                .AsNoTracking()
                .SelectMany(g => g.Dates)
                .Where(GetYearPredicate(year))
                .Where(GetRegionPredicate(gameRegionId))
                .Select(row => new CalendarRow()
                {
                    GameType = row.Game.GameType,
                    GameDurationDays = row.GameDurationDays,
                    Status = row.Game.GameStatus,
                    PlayersCount = row.Game.PlayersCount,
                    Organizers = row.Game.Organizers,
                    
                    GameId = row.GameId,
                    Name = row.Game.GameName,
                    PolygonName = row.Game.Polygon.PolygonName,
                    StartDate = row.GameStartDate,

                    SubRegionShortName = row.Game.SubRegion.SubRegionName,
                    MacroRegionId = row.Game.SubRegion.MacroRegionId,

                    VkontakteLink = row.Game.VkontakteLink,
                    FacebookLink = row.Game.FacebookLink,
                    TelegramLink = row.Game.TelegramLink,
                    LivejournalLink = row.Game.LivejournalLink,
                    
                    Email = row.Game.Email,
                    
                })
                .ToListAsync();
        }

        private Expression<Func<GameDate, bool>> GetRegionPredicate(int? gameRegionId)
        {
            if (gameRegionId == null)
            {
                return game => true;
            }

            return game => game.Game.SubRegion.MacroRegionId == gameRegionId;
        }

        private static Expression<Func<GameDate, bool>> GetYearPredicate(int year)
        {
            return row => row.GameStartDate.Year == year || row.GameStartDate.AddDays(row.GameDurationDays).Year  == year;
        }
    }
}
