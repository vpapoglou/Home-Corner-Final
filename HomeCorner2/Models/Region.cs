using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeCorner2.Models
{
    public class Region
    {
        public byte RegionId { get; set; }
        public string RegionName { get; set; }
        public ICollection<House> Houses { get; set; }

    }
}