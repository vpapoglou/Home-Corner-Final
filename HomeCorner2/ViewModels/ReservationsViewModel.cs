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

        public Images Images { get; set; }
        public IEnumerable<SelectListItem> AllFeatures { get; set; }
        public byte[] Image { get; set; }
        private List<byte> _selectedFeatures;
        public List<byte> SelectedFeatures
        {
            get
            {
                if (_selectedFeatures == null)
                {
                    _selectedFeatures = House.Features.Select(m => m.Id).ToList();
                }
                return _selectedFeatures;
            }
            set { _selectedFeatures = value; }

        }
        public ReservationsViewModel()
        {
            AllFeatures = new List<SelectListItem>();

        }
    }
}