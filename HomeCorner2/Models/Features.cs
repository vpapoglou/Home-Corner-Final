using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HomeCorner2.Models
{
    public class Features
    {
        public Features()
        {

            this.Houses = new HashSet<House>();
        }
        public byte Id { get; set; }
        public string Feature { get; set; }

        public virtual ICollection<House> Houses { get; set; }

    }
}