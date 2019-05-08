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

namespace HomeCorner.Controllers
{
    public class HousesController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Reservations()
        {

            return View(db.Reservations.ToList());
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
                ReservationsViewModel.House = db.Houses.Include(i => i.Features).First(i => i.Id == id);
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
        public ActionResult Book(ReservationsViewModel reservationsViewModel)
        {
            if (ModelState.IsValid)
            {
                var reservationToAdd = reservationsViewModel;

                db.Reservations.Add(reservationToAdd.Reservation);
                db.SaveChanges();
                return RedirectToAction("ReservationsIndex");
            }

            return View(reservationsViewModel);
        }

        public int UploadImageInDataBase(HttpPostedFileBase image, HousesViewModel housesViewModel)
        {
            housesViewModel.Image = ConvertToBytes(image);
            var imageToAdd = housesViewModel.Images;

            db.Images.Add(imageToAdd);

            int i = db.SaveChanges();
            if (i == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        public ActionResult RetrieveImage(Guid id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        public byte[] GetImageFromDataBase(Guid Id)
        {
            var q = from temp in db.Images where temp.Id == Id select temp.byteImage;
            byte[] cover = q.First();
            return cover;
        }

        // GET: Images  
        public ActionResult UploadImages()
        {
            return View();
        }
        // POST: Images
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadImages(HousesViewModel housesViewModel)
        {

            HttpPostedFileBase file = Request.Files["ImageData"];

            UploadImageInDataBase(file, housesViewModel);

            return RedirectToAction("Index"); ;
        }

        // GET: Houses
        public ActionResult Index()
        {

            return View(db.Houses.ToList());
        }

        public ActionResult Create()
        {
            var housesViewModel = new HousesViewModel();
            var allFeaturesList = db.Features.ToList();
            ViewBag.AllFeatures = allFeaturesList.Select(o => new SelectListItem
            {
                Text = o.Feature.ToString(),
                Value = o.Id.ToString()
            });
            ViewBag.RegionId = new SelectList(db.Regions, "RegionId", "RegionName");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HousesViewModel housesViewModel)
        {
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

                db.Houses.Add(houseToAdd.House);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            return RedirectToAction("Index");
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