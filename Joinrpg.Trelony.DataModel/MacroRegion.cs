using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Joinrpg.Trelony.DataModel
{
    public class MacroRegion
    {
        public int MacroRegionId { get; set; }
        [Required]
        public string MacroRegionName { get; set; }

        public ISet<SubRegion> SubRegions { get; set; }
    }
}
