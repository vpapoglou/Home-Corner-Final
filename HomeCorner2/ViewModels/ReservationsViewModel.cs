using HomeCorner2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeCorner2.ViewModels
{
    public class ReservationsViewModel
    {

        public House House { get; set; }
        public Reservation Reservation { get; set; }
        public ApplicationUser User { get; set; }
    }
}