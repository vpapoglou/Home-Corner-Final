using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HomeCorner2.Models;

namespace HomeCorner2.Services
{
    public class HouseDb
    {
        public static IEnumerable<House> GetAll()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Houses.Include("House").ToList();
            }
        }
        public static House GetById(int Id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Houses.Include("House").SingleOrDefault(m => m.Id == Id);
            }
        }

        public static void Add(House house)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Houses.Add(house);
                context.SaveChanges();
            }
        }

        public static void Update(House house)
        {
            using (var context = new ApplicationDbContext())
            {
                House houseToUpdate = context.Houses.Find(house.Id);
                house.Id = house.Id;
                houseToUpdate.Title = house.Title;

                context.SaveChanges();
            }
        }

        public static void Delete(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                House house = context.Houses.Find(id);
                context.Houses.Remove(house);
                context.SaveChanges();
            }
        }
    }
}