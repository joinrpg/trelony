using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Joinrpg.Trelony.DataModel
{
    public class SubRegion
    {
        public int SubRegionId { get; set; }

        [Required]
        public string SubRegionName { get; set; }

        public int MacroRegionId { get; set; }
        public MacroRegion MacroRegion { get; set; }
        public ISet<Polygon> Polygons { get; set; }
    }
}