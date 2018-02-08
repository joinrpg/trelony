using System.Collections.Generic;
using JetBrains.Annotations;

namespace Joinrpg.Trelony.DataModel
{
    public class Game
    {
        public int GameId { get; set; }

        public string GameName { get; set; }
        public string GameUrl { get; set; }

        public ISet<GameDate> Dates { get; set; }

        public GameType GameType { get; set; }

        [CanBeNull]
        public Polygon Polygon { get; set; }
        public int? PolygonId { get; set; }

        [NotNull]
        public SubRegion SubRegion { get; set; }
        public int SubRegionId { get; set; }

        public string Organizers { get; set; }
        public int? PlayersCount { get; set; }

        public string Email { get; set; }
        public string VkontakteLink { get; set; }
        public string LivejournalLink { get; set; }
        public string FacebookLink { get; set; }
        public string TelegramLink { get; set; }

        public GameStatus GameStatus { get; set; }
    }
}