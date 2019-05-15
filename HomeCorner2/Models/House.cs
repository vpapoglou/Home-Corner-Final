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

        [Required(ErrorMessage = "Please, provide a Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please, provide a Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please, provide an Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please, provide an Address Number")]
        public string AddressNumber { get; set; }

        [Required(ErrorMessage = "Please, provide a Postal Code")]
        [StringLength(5, ErrorMessage = "The Postal Code must be 5 characters long.", MinimumLength = 5)]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Please, provide a Occupancy")]
        public int Occupancy { get; set; }

        [Required(ErrorMessage = "Please, provide a Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please, provide Availability")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please, provide Availability")]
        public DateTime EndDate { get; set; }


        public string ImageName { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageData { get; set; }

        public ApplicationUser Owner { get; set; }

        //public int OwnerId { get; set; }
        public string OwnerId { get; set; }

        public byte RegionId { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<Features> Features { get; set; }

        public virtual ICollection<Images> Images { get; set; }
    }
}