using System;
using Joinrpg.Trelony.DataModel;

namespace Joinrpg.Trelony.Repositories.Interfaces
{
    public class CalendarRow
    {
        public int GameId { get; set; }
        public GameStatus Status { get; set; }
        public string Name { get; set; }

        public string VkontakteLink { get; set; }

        public DateTime StartDate { get; set; }
        public int GameDurationDays { get; set; }

        public string SubRegionShortName { get; set; }
        public int MacroRegionId { get; set; }

        public GameType GameType { get; set; }

        public string PolygonName { get; set; }

        public int? PlayersCount { get; set; }

        public string Organizers { get; set; }
        public string Email { get; set; }
        public string FacebookLink { get; set; }
        public string TelegramLink { get; set; }
        public string LivejournalLink { get; set; }
    }
}