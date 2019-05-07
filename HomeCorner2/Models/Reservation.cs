using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HomeCorner2.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public House House { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}