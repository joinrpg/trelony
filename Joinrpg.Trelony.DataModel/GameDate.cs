using System;
using JetBrains.Annotations;

namespace Joinrpg.Trelony.DataModel
{
    public class GameDate
    {
        public int GameDateId { get; set; }

        public int GameId { get; set; }
        [NotNull]
        public Game Game { get; set; }

        public DateTime GameStartDate { get; set; }
        public int GameDurationDays { get; set; }
        public bool Actual { get; set; }
    }
}