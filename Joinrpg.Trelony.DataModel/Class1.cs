using System;
using System.Collections.Generic;
using Joinrpg.Common.Helpers;

namespace Joinrpg.Trelony.DataModel
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public GameType GameType { get; set; }
        public GamePolygon GamePolygon { get; set; }
        public int? GamePolygonId { get; set; }
        public string Organizers { get; set; }
        public string OrganizerEmail { get; set; }
        public GameStatus GameStatus { get; set; }
        public MarkdownString Comment { get; set; }
        public SubRegion SubRegion { get; set; }
        public int SubRegionId { get; set; }
        public bool IsActive { get; set; }
        public bool HideEmail { get; set; }
        public int? PlayersCount { get; set; }
        public int? AllrpgInfoId { get; set; }
        public int? GameRedirectId { get; set; }
        public string VKontakte { get; set; }
        public string Livejournal { get; set; }
        public string Facebook { get; set; }
        public string Telegram { get; set; }

        public virtual ICollection<GameDate> Dates { get; set; } = new HashSet<GameDate>();
        public virtual ICollection<GamePhoto> Photos { get; set; } = new HashSet<GamePhoto>();
        public virtual ICollection<GameReview> Reviews { get; set; } = new HashSet<GameReview>();

        public virtual ICollection<ApplicationSystemsAssociations> Applications { get; set; } = new HashSet<ApplicationSystemsAssociations>();
    }

    public class ApplicationSystemsAssociations
    {
        public int ApplicationSystemsAssociationsId { get; set; }
        public int? AllrpgProjectId { get; set; }
        public int? JoinrpgProjectId { get; set; }
        public int? GameId { get; set; }
        public string ExternalName { get; set; }
        public bool IsActive { get; set; }
    }

    public class GameDate
    {
        public int GameDateId { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public DateTime BeginDay { get; set; }
        public int LengthDays { get; set; }
        public int Order { get; set; }
    }

    public class GamePhoto
    {
        public int GamePhotoId { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public string LegacyAuthor { get; set; }
        public string PhotoUri { get; set; }
        public LegacyTrelonyUser LegacyTrelonyUser { get; set; }
        public int? LegacyTrelonyUserId { get; set; }
        public MarkdownString PhotoComment { get; set; }
        public bool LegacyGoodFlag { get; set; }
    }

    public class GameReview
    {
        public int GameReviewId { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public string LegacyAuthor { get; set; }
        public LegacyTrelonyUser LegacyTrelonyUser { get; set; }
        public int? LegacyTrelonyUserId { get; set; }
        public string PhotoUri { get; set; }
        public bool IsActive { get; set; }
    }

    public class LegacyTrelonyUser
    {

    }

    public class SubRegion
    {
        public int SubRegionId { get; set; }
        public string SubRegionName { get; set; }
        public string SubRegionShortName { get; set; }
        public int MacroRegionId { get; set; }
        public MacroRegion MacroRegion { get; set; }
    }

    public class MacroRegion
    {
        public int MacroRegionId { get; set; }
        public string MacroRegionName { get; set; }
        public string MacroRegionCode { get; set; }
        public virtual ICollection<SubRegion> SubRegions { get; set; }= new HashSet<SubRegion>();
    }

    public class GamePolygon {
        public int GamePolygonId { get; set; }
        public string PolygonName { get; set; }
        public SubRegion SubRegion { get; set; }
        public int SubRegionId { get; set; }
        public bool IsActive { get; set; }
    }



    public enum GameType
    {
        Forest = 1,
        City = 2,
        Hotel = 3,
        Room = 4,
        Convention = 5,
        Ball = 6,
        Maneuvers = 7,
        CityAndForest = 8,
        CityAndHotel = 9,
        Tournament = 10,
        Underground = 11,
        Airsoft = 12,
        Fest = 13,
    }

    public enum GameStatus
    {
        Ok = 1,
        Passed = 1,
        Unknown = 2,
        Postponed = 3,
        DateUnknown = 4,
        Cancelled = 5,
    }
}
