using System;

namespace Joinrpg.Trelony.DataModel
{
    public class AddUri
    {
        public int AddUriId { get; set; }
        public string Uri { get; set; }
        public int? AllrpgInfoId { get; set; }
        public bool Resolved { get; set; }
    }
}
