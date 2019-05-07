using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HomeCorner2.Models;
using HomeCorner2.ViewModels;
using HomeCorner2.Services;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNet.Identity;

namespace HomeCorner2.Controllers
{
    public class ReservationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Book(int? HouseId)
        {
            var currentUserId = User.Identity.GetUserId();
            //var passinfo = db.Reservations.FirstOrDefault(d => d.IUser.Id == currentUserId);

            if (HouseId == null || currentUserId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ReservationsViewModel = new ReservationsViewModel();
            {
                ReservationsViewModel.House = db.Houses.Include(i => i.Features).First(i => i.Id == HouseId);
            }

            if (ReservationsViewModel.Reservation == null)
            {
                return HttpNotFound();
            }
            //var allFeaturesList = HousesViewModel.House.Features.ToList();


            return View(ReservationsViewModel);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Book(HousesViewModel housesViewModel)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var houseToReserve = housesViewModel;

        //        if (TryUpdateModel(houseToReserve, "reservation", new string[] { "Id", "StartDate", "EndDate" }))
        //        {
        //            var newStartDate = new HashSet<byte>(housesViewModel.SelectedFeatures);

        //        }

        //        db.Reservations.Add(houseToReserve.Reservation);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //        //return RedirectToAction("UploadImages");
        //    }

        //    return View(housesViewModel);
        //}
    }
}