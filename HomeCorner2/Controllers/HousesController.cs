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

        [Authorize(Roles = RoleName.CanManageHouses)]
        public ActionResult Reservations()
        {
            var reservations = db.Reservations.Include(i => i.House).Include(j => j.User).ToList();
            return View(reservations);
        }

        public ActionResult HouseReservations(string id)
        {
            var reservations = db.Reservations.Include(i => i.House).Include(j => j.User).Where(x => x.House.Id.ToString() == id).ToList();
            return View(reservations);
        }

        public ActionResult MyHouses()
        {
            var currentUserId = User.Identity.GetUserId();
            var myHouses = db.Houses.Where(i => i.OwnerId.ToString() == currentUserId).OrderByDescending(house => house.Id);
            return View(myHouses);
        }

        public ActionResult MyReservations()
        {
            var currentUserId = User.Identity.GetUserId();
            var myReservations = db.Reservations.Include(i => i.House).Where(i => i.User.Id == currentUserId).OrderByDescending(reservation => reservation.Id);
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


                if ((givenStartDate < houseStartDate) || (givenEndDate > houseEndDate))
                //if ((givenStartDate.Date> houseStartDate.Date) || (givenEndDate.Date < houseEndDate.Date))
                {
                    return RedirectToAction("Search", new { message = "Error" });
                }
                //else
                //{

                foreach (Reservation reservations in db.Reservations.Where(n => n.House.Id == id))
                {
                    if (((givenStartDate == reservations.StartDate) && (givenEndDate == reservations.StartDate)) || ((givenStartDate < reservations.StartDate) && (givenEndDate < reservations.StartDate)) || ((givenStartDate < reservations.StartDate) && (givenEndDate > reservations.StartDate)) || ((givenStartDate > reservations.StartDate) && (givenEndDate < reservations.StartDate)) || ((givenStartDate > reservations.StartDate) && (givenStartDate < reservations.EndDate)))
                    //if ((givenStartDate > reservations.StartDate) || (givenEndDate < reservations.EndDate))
                    {
                        return RedirectToAction("Search", new { message = "Error" });
                    }
                }
                //}

                reservationsViewModel.Reservation.House = house;
                reservationsViewModel.Reservation.User = user;
                db.Reservations.Add(reservationToAdd.Reservation);
                db.SaveChanges();

                return RedirectToAction("MyReservations", new { message = "Success" });
            }
            else
            {
                return View(reservationsViewModel);
            }
        }

        public ActionResult Search(string message, string option, string search)
        {
            if (option == "Region")
            {
                //Index action method will return a view with a student records based on what a user specify the value in textbox  
                return View(db.Houses.Where(x => x.Region.RegionName == search || search == null).OrderByDescending(house => house.Id));
            }
            else if (option == "PostalCode")
            {
                return View(db.Houses.Where(x => x.PostalCode.ToString() == search || search == null).OrderByDescending(house => house.Id));
            }
            else if (option == "Occupancy")
            {
                return View(db.Houses.Where(x => x.Occupancy.ToString() == search || search == null).OrderByDescending(house => house.Id));
            }
            else if (option == "Price")
            {
                return View(db.Houses.Where(x => x.Price.ToString() == search || search == null).OrderByDescending(house => house.Id));
            }
            else if (option == "StartDate")
            {
                return View(db.Houses.Where(x => x.StartDate.ToString() == search || search == null).OrderByDescending(house => house.Id));
            }
            else if (option == "EndDate")
            {
                return View(db.Houses.Where(x => x.EndDate.ToString() == search || search == null).OrderByDescending(house => house.Id));
            }
            else if (option == "AllHouses")
            {
                return View(db.Houses.OrderByDescending(house => house.Id));
            }
            else
            {
                if (!string.IsNullOrEmpty(message))
                {
                    ViewBag.message = message;
                }
                return View(db.Houses.Include(i => i.Owner).OrderByDescending(house => house.Id));
            }
        }

        // GET: Houses
        [Authorize(Roles = RoleName.CanManageHouses)]
        public ActionResult Index(string message, string option, string search)
        {
            var AllHouses = db.Houses.Include(i => i.Owner).ToList();
            //if a user choose the radio button option as Subject  
            if (option == "Region")
            {
                //Index action method will return a view with a student records based on what a user specify the value in textbox  
                return View(db.Houses.Where(x => x.Region.RegionName == search || search == null).ToList());
            }
            else if (option == "PostalCode")
            {
                return View(db.Houses.Where(x => x.PostalCode.ToString() == search || search == null).ToList());
            }
            else if (option == "Occupancy")
            {
                return View(db.Houses.Where(x => x.Occupancy.ToString() == search || search == null).ToList());
            }
            else if (option == "Price")
            {
                return View(db.Houses.Where(x => x.Price.ToString() == search || search == null).ToList());
            }
            else if (option == "StartDate")
            {
                return View(db.Houses.Where(x => x.StartDate.ToString() == search || search == null).ToList());
            }
            else if (option == "EndDate")
            {
                return View(db.Houses.Where(x => x.EndDate.ToString() == search || search == null).ToList());
            }
            else if (option == "AllHouses")
            {
                return View(db.Houses.OrderByDescending(house => house.Id));
            }
            else
            {
                if (!string.IsNullOrEmpty(message))
                {
                    ViewBag.message = message;
                }
                return View(AllHouses);
            }


            //if (option == "Region")
            //{
            //    //Index action method will return a view with a student records based on what a user specify the value in textbox  
            //    return View(db.Houses.Where(x => x.Region.RegionName == search || search == null).ToList());
            //}
            //else if (option == "Occupancy")
            //{
            //    return View(db.Houses.Where(x => x.Occupancy.ToString() == search || search == null).ToList());
            //}
            //else if (option == "StartDate")
            //{
            //    return View(db.Houses.Where(x => x.StartDate.ToString() == search || search == null).ToList());
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(message))
            //    {
            //        ViewBag.message = message;
            //    }
            //    return View(db.Houses.Include(i => i.Owner).ToList());
            //}
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
                ".png", ".jpg", ".jpeg"
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
                    string myfile = Guid.NewGuid() + ext;
                    var path = Path.Combine(Server.MapPath("/HouseImages") + "/", myfile);
                    houseToAdd.House.ImageName = myfile;
                    houseToAdd.House.ImageData.SaveAs(path);
                }
                houseToAdd.House.OwnerId = userId;

                db.Houses.Add(houseToAdd.House);
                db.SaveChanges();
                return RedirectToAction("MyHouses", new { message = "Success" });
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
                HousesViewModel.House = db.Houses.Include(i => i.Features).Include(j => j.Owner).First(i => i.Id == id);
            }

            if (HousesViewModel.House == null)
            {
                return HttpNotFound();
            }
            //var allFeaturesList = HousesViewModel.House.Features.ToList();


            return View(HousesViewModel);
        }

        // GET: Houses/Edit/5
        //[Authorize(Roles = RoleName.CanManageHouses)]
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
        //[Authorize(Roles = RoleName.CanManageHouses)]
        public ActionResult Edit(HousesViewModel housesViewModel)
        {
            var allowedExtensions = new[]{
                ".png", ".jpg", ".jpeg"
                };

            if (ModelState.IsValid)
            {
                var houseToAdd = housesViewModel;

                if (TryUpdateModel(houseToAdd, "house", new string[] { "Features", "RegionId", "StartDate", "EndDate" }))
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
                    var fileName = Path.GetFileName(houseToAdd.House.ImageData.FileName);
                    var ext = Path.GetExtension(houseToAdd.House.ImageData.FileName);
                    if (allowedExtensions.Contains(ext))
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName);
                        string myfile = Guid.NewGuid() + ext;
                        var path = Path.Combine(Server.MapPath("/HouseImages") + "/", myfile);
                        houseToAdd.House.ImageName = myfile;
                        houseToAdd.House.ImageData.SaveAs(path);
                    }
                    db.Entry(houseToAdd.House).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("MyHouses", new { message = "Success" });
                }

            }
            //ViewBag.Id = new SelectList(db.Houses, "Id", "Title", housesViewModel.House.Id);
            ViewBag.RegionId = new SelectList(db.Regions, "RegionId", "RegionName");

            return View(housesViewModel);
        }

        // GET: Houses/Delete/5
        //[Authorize(Roles = RoleName.CanManageHouses)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            House house = db.Houses.Find(id);
            if (house == null)
            {
                return Json(new { Status = "error", Error = "House not found" }, JsonRequestBehavior.AllowGet);
            }
            //return View(house);
            return Json(new { Status = "ok" }, JsonRequestBehavior.AllowGet);
        }

        // POST: Houses/Delete/5 
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = RoleName.CanManageHouses)]
        public JsonResult DeleteConfirmed(int id)
        {
            House house = db.Houses.Find(id);
            if (house == null)
            {
                return Json(new { Status = "error", Error = "House not found" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var HouseReservations = db.Reservations.Count(i => i.House.Id == id);
                if (HouseReservations > 0)
                {
                    return Json(new { Status = "error", Error = "House can't be deleted" }, JsonRequestBehavior.AllowGet);
                }
            }
            db.Houses.Remove(house);
            db.SaveChanges();
            return Json(new { Status = "ok" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Reservations/Cancel/5
        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return Json(new { Status = "error", Error = "Reservation not found" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Status = "ok" }, JsonRequestBehavior.AllowGet);
        }

        // POST: Reservations/Cancel/5 
        [HttpPost, ActionName("Cancel")]
        //[ValidateAntiForgeryToken]
        public JsonResult CancelConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return Json(new { Status = "error", Error = "Reservation not found" }, JsonRequestBehavior.AllowGet);
            }
            db.Reservations.Remove(reservation);
            db.SaveChanges();
            return Json(new { Status = "ok" }, JsonRequestBehavior.AllowGet);
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

