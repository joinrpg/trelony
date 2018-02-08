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
        }
    }
}
