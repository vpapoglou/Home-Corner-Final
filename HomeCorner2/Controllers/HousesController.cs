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
using System.Collections;

namespace HomeCorner.Controllers
{
    public class HousesController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Reservations()
        {
            var reservations = db.Reservations.Include(i => i.House).ToList();
            return View(reservations);
        }

        public ActionResult MyHouses()
        {
            var currentUserId = User.Identity.GetUserId();
            var myHouses = db.Houses.Where(i => i.OwnerId.ToString() == currentUserId).ToList();
            return View(myHouses);
        }

        public ActionResult MyReservations()
        {
            var currentUserId = User.Identity.GetUserId();
            var myReservations = db.Reservations.Where(i => i.User.Id == currentUserId).ToList();
            //var myReservations = db.Reservations.Where(i => i.House.Id.ToString() == currentUserId).ToList();
            return View(myReservations);
        }

        public ActionResult Book(int? id)
        {
            var currentUserId = User.Identity.GetUserId();
            //var passinfo = db.Reservations.FirstOrDefault(d => d.IUser.Id == currentUserId);

            if (id == null || currentUserId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ReservationsViewModel = new ReservationsViewModel();
            {
                ReservationsViewModel.House = db.Houses.First(i => i.Id == id);
                ReservationsViewModel.User = db.Users.First(i => i.Id == currentUserId);
            }

            if (ReservationsViewModel.House == null)
            {
                return HttpNotFound();
            }
            


            return View(ReservationsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Book(ReservationsViewModel reservationsViewModel, int id)
        {
            if (ModelState.IsValid)
            {
                var reservationToAdd = reservationsViewModel;

                var house = db.Houses.FirstOrDefault(i => i.Id == id);
                var userId = User.Identity.GetUserId();
                var user = db.Users.FirstOrDefault(i => i.Id == userId);

                DateTime houseStartDate = house.StartDate;
                DateTime houseEndDate = house.EndDate;
                DateTime givenStartDate = reservationToAdd.Reservation.StartDate;
                DateTime givenEndDate = reservationToAdd.Reservation.EndDate;


                if ((givenStartDate.Date > houseStartDate.Date) || (givenEndDate.Date < houseEndDate.Date))
                {
                    return RedirectToAction("Index", new { message = "Error" });
                }
                else
                {

                    foreach (Reservation reservations in db.Reservations.Where(n => n.House.Id == id))
                    {
                        if ((givenStartDate.Date > reservations.StartDate.Date) || (givenEndDate.Date < reservations.EndDate.Date))
                        {
                            return RedirectToAction("Index", new { message = "Error" });
                        }
                    }
                }

                reservationsViewModel.Reservation.House = house;
                reservationsViewModel.Reservation.User = user;
                db.Reservations.Add(reservationToAdd.Reservation);
                db.SaveChanges();
                
                return RedirectToAction("Index", new { message = "Success" });
            }
            else{
                return View(reservationsViewModel);
            }
        }

        // GET: Houses
        public ActionResult Index(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }
            return View(db.Houses.ToList());
        }

        public ActionResult Create()
        {
            var housesViewModel = new HousesViewModel();
            var allFeaturesList = db.Features.ToList();
            var currentUserId = User.Identity.GetUserId();
            ViewBag.AllFeatures = allFeaturesList.Select(o => new SelectListItem
            {
                Text = o.Feature.ToString(),
                Value = o.Id.ToString()
            });
            ViewBag.RegionId = new SelectList(db.Regions, "RegionId", "RegionName");
            ViewBag.OwnerId = currentUserId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HousesViewModel housesViewModel)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Where(i => i.Id == userId).FirstOrDefault();
            housesViewModel.Owner = user;
            var allowedExtensions = new[]
            {
                ".spng", ".jpg", ".jpeg"
            };


            if (ModelState.IsValid)
            {
                var houseToAdd = housesViewModel;

                if (TryUpdateModel(houseToAdd, "house", new string[] { "Id", "Features", "RegionId" }))
                {
                    var updatedFeatures = new HashSet<byte>(housesViewModel.SelectedFeatures);

                    foreach (Features features in db.Features)
                    {
                        if (!updatedFeatures.Contains(features.Id))
                        {
                            houseToAdd.House.Features.Remove(features);
                        }
                        else
                        {
                            houseToAdd.House.Features.Add((features));
                        }
                    }
                }

                //houseToAdd.House.ImageName = file.ToString();
                var fileName = Path.GetFileName(houseToAdd.House.ImageData.FileName);
                var ext = Path.GetExtension(houseToAdd.House.ImageData.FileName);
                if (allowedExtensions.Contains(ext))
                {
                    string name = Path.GetFileNameWithoutExtension(fileName);
                    string myfile = Guid.NewGuid()+ ext;
                    var path = Path.Combine(Server.MapPath("/HouseImages") + "/", myfile);
                    houseToAdd.House.ImageName = myfile;
                    houseToAdd.House.ImageData.SaveAs(path);
                }
                houseToAdd.House.OwnerId = userId;

                db.Houses.Add(houseToAdd.House);
                db.SaveChanges();
                return RedirectToAction("Index", new { message = "Success" });
                //return RedirectToAction("UploadImages");
            }

            ViewBag.RegionId = new SelectList(db.Regions, "RegionId", "RegionName");
            return View(housesViewModel);
        }


        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        // GET: Houses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var HousesViewModel = new HousesViewModel();
            {
                HousesViewModel.House = db.Houses.Include(i => i.Features).First(i => i.Id == id);
            }

            if (HousesViewModel.House == null)
            {
                return HttpNotFound();
            }
            //var allFeaturesList = HousesViewModel.House.Features.ToList();


            return View(HousesViewModel);
        }

        // GET: Houses/Edit/5
        [Authorize(Roles = RoleName.CanManageHouses)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //House house = db.Houses.Find(id);
            //if (house == null)
            //{
            //    return HttpNotFound();
            //}
            //var allFeatures = db.Features.ToList();
            var housesViewModel = new HousesViewModel();
            {
                housesViewModel.House = db.Houses.Include(i => i.Features).SingleOrDefault(i => i.Id == id);
                //HousesViewModel.Features = allFeatures.ToList();
            }
            if (housesViewModel.House == null)
                return HttpNotFound();

            var allFeaturesList = db.Features.ToList();
            ViewBag.AllFeatures = allFeaturesList.Select(o => new SelectListItem
            {
                Text = o.Feature.ToString(),
                Value = o.Id.ToString()
            });

            ViewBag.RegionId = new SelectList(db.Regions, "RegionId", "RegionName");

            return View(housesViewModel);
        }

        // POST: Houses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageHouses)]
        public ActionResult Edit(HousesViewModel housesViewModel)
        {
            if (ModelState.IsValid)
            {
                var houseToAdd = housesViewModel;
                if (TryUpdateModel(houseToAdd, "house", new string[] { "Features", "RegionId" }))
                {
                    var updatedFeatures = new HashSet<byte>(housesViewModel.SelectedFeatures);
                    //var updatedRegion = housesViewModel.SelectedRegion;

                    foreach (Features features in db.Features)
                    {
                        if (!updatedFeatures.Contains(features.Id))
                        {
                            houseToAdd.House.Features.Remove(features);
                        }
                        else
                        {
                            houseToAdd.House.Features.Add((features));
                        }
                    }
                }
                db.Entry(houseToAdd.House).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.Id = new SelectList(db.Houses, "Id", "Title", housesViewModel.House.Id);
            ViewBag.RegionId = new SelectList(db.Regions, "RegionId", "RegionName");

            return View(housesViewModel);
        }

        // GET: Houses/Delete/5
        [Authorize(Roles = RoleName.CanManageHouses)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            House house = db.Houses.Find(id);
            if (house == null)
            {
                return HttpNotFound();
            }
            return View(house);
        }

        // POST: Houses/Delete/5 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageHouses)]
        public ActionResult DeleteConfirmed(int id)
        {
            House house = db.Houses.Find(id);
            db.Houses.Remove(house);
            db.SaveChanges();
            return RedirectToAction("Index", new { message = "Success" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

