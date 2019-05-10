using HomeCorner2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomeCorner2.ViewModels
{
    public class HousesViewModel
    {
        public House House { get; set; }  //oxi lista giati mas endiaferei sto details kai sta ypoloipa views na emfanizetai mono ena spiti kai ola ta features tou kai oxi ola ta spitia mazi
        /// Gets or sets Image file.
        public ApplicationUser Owner { get; set; }
        public Images Images { get; set; }
        public IEnumerable<SelectListItem> AllFeatures { get; set; }
        /// <summary>  
        /// Gets or sets Image file list.  
        /// </summary>  
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
        public HousesViewModel()
        {
            AllFeatures = new List<SelectListItem>();

        }
    }
}