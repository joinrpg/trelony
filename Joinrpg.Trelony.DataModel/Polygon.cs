using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace Joinrpg.Trelony.DataModel
{
    public class Polygon
    {
        public int PolygonId { get; set; }
        [Required, NotNull]
        public string PolygonName { get; set; }

        public int SubRegionId { get; set; }
        [Required, NotNull]
        public SubRegion Region { get; set; }
    }
}