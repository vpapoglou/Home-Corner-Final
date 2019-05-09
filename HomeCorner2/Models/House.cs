using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace HomeCorner2.Models
{
    public class House
    {
        public House()
        {
            this.Features = new HashSet<Features>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        //[Required(ErrorMessage = "Please, provide a description")]
        public string Description { get; set; }

        //[Required(ErrorMessage = "Please, provide a Region")]
        //public string Region { get; set; }

        //[Required(ErrorMessage = "Please, provide an Address")]
        public string Address { get; set; }

        public string AddressNumber { get; set; }

        public int PostalCode { get; set; }

        public int Occupancy { get; set; }

        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        public string ImageName { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageData { get; set; }

        public ApplicationUser Owner { get; set; }

        public int OwnerId { get; set; }

        public byte RegionId { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<Features> Features { get; set; }

        public virtual ICollection<Images> Images { get; set; }
    }
}