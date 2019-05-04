using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HomeCorner2.Models
{
    public class Images
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public int RecordId { get; set; }
        public Guid Id { get; set; }

        public string ImageName { get; set; }
        public string Extension { get; set; }

        public int HouseId { get; set; }
        public virtual House House { get; set; }
        public byte[] byteImage { get; set; }
    }
}