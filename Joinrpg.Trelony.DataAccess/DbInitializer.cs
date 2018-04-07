using System;
using System.Collections.Generic;
using System.Linq;
using Joinrpg.Trelony.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Joinrpg.Trelony.DataAccess
{
    public static class DbInitializer
    {
        public static void EnsureInitialized(this TrelonyContext context)
        {
            context.Database.Migrate();

            if (!context.MacroRegions.Any())
            {
                var northWest = new MacroRegion
                {
                    MacroRegionName = "Северо-Запад",
                    SubRegions = new HashSet<SubRegion>()
                    {
                        new SubRegion()
                        {
                            SubRegionName = "Санкт-Петербург"
                        },
                        new SubRegion()
                        {
                            SubRegionName = "Ленинградская область"
                        }
                    }
                };

                var  center = new MacroRegion
                {
                    MacroRegionName = "Москва и центральный регион",
                    SubRegions = new HashSet<SubRegion>()
                    {
                        new SubRegion()
                        {
                            SubRegionName = "Москва"
                        },
                        new SubRegion()
                        {
                            SubRegionName = "Московская область"
                        }
                    }
                };

                context.MacroRegions.AddRange(northWest, center);

                context.SaveChanges();
            }

            if (!context.Games.Any())
            {
                var games = new[]
                {
                    new Game()
                    {
                        Dates = new HashSet<GameDate>()
                        {
                            new GameDate()
                            {
                                Actual = true,
                                GameDurationDays = 4,
                                GameStartDate = new DateTime(year: 2018, month: 07, day: 25)
                            }
                        },
                        Email = "klepa@example.com",
                        GameName = "Железный трон",
                        SubRegionId = context.SubRegions.Single(sr => sr.SubRegionName == "Московская область")
                            .SubRegionId,
                        GameStatus = GameStatus.ProbablyHappen,
                        PlayersCount = 1500,
                        VkontakteLink = "vesteros2018",
                        Organizers = "МГ «Наррентурм»",
                        GameType = GameType.Forest,
                    },
                    new Game()
                    {
                        Dates = new HashSet<GameDate>()
                        {
                            new GameDate()
                            {
                                Actual = true,
                                GameDurationDays = 4,
                                GameStartDate = new DateTime(year: 2018, month: 08, day: 15)
                            }
                        },
                        Email = "freexee@example.com",
                        GameName = "Магеллан: Время для звезд",
                        SubRegionId = context.SubRegions.Single(sr => sr.SubRegionName == "Ленинградская область")
                            .SubRegionId,
                        GameStatus = GameStatus.ProbablyHappen,
                        PlayersCount = 300,
                        VkontakteLink = "magellan2018",
                        Organizers = "МГ «Питерский формат»",
                        GameUrl = "http://magellan2018.ru",
                        GameType = GameType.OnRentedPlace,
                    },
                    new Game()
                    {
                        Dates = new HashSet<GameDate>()
                        {
                            new GameDate()
                            {
                                Actual = true,
                                GameDurationDays = 4,
                                GameStartDate = new DateTime(year: 2016, month: 07, day: 6)
                            }
                        },
                        Email = "leo@example.com",
                        GameName = "Стимпанк: Век разума",
                        SubRegionId = context.SubRegions.Single(sr => sr.SubRegionName == "Ленинградская область")
                            .SubRegionId,
                        GameStatus = GameStatus.DefinitelyPassed,
                        PlayersCount = 400,
                        VkontakteLink = "steampunk2016",
                        Organizers = "МГ «Питерский формат»",
                        GameUrl = "http://steam2016.ru",
                        GameType = GameType.Forest,
                    },
                };

                context.Games.AddRange(games);
                context.SaveChanges();
            }
        }
    }
}
