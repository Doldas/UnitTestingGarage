using System.Web.Mvc;
using GarageAngularJS.Repository;
using GarageAngularJS.Models;
using System.Collections.Generic;
using System;


namespace GarageMVC.Controllers
{
    public class GarageController : Controller
    {
        //Interface for testing purpose
        GarageRepoInterface garage;

        #region Constructor
        public GarageController()
        {
            this.garage = new GarageRepository();
        }
        //Constructor for Test purpose Only
        public GarageController(GarageRepoInterface garageRepoTestInterface)
        {
            this.garage = garageRepoTestInterface;
        }
        public GarageController(GarageRepoTests testRepo)
        {
            this.garage = testRepo;
        }
        #endregion;
        //GET: Garage
        [HttpGet]
        public ActionResult Index(string Search = "")
        {
            if(Search == "")
            {
                //Test purpose
                ViewBag.OutputListTestingPurpose = garage.GetAll();

                return View(garage.GetAll());
            }
            //Test purpose
            ViewBag.Test = "True";
            ViewBag.OutputListTestingPurpose = garage.Search(Search);
            return View(garage.Search(Search));
        }

        [HttpPost]
        public ActionResult Index(string Sort = "", string Filter = "")
        {

            if (Filter == "Car" || Filter == "Bus" || Filter == "Truck" || Filter == "Mc")
            {
                return View(garage.GetFilteredList(Filter));
            }

            Sort = Sort.ToLower();
            if (Sort == "regnumber")
            {
                return View(garage.SortReg(false));
            }
            else if (Sort == "owner")
            {
                return View(garage.SortOwner(false));
            }
            else if (Sort == "type")
            {
                return View(garage.SortType(false));
            }
            else if (Sort == "parkingplace")
            {
                return View(garage.SortParking(false));
            }
            return RedirectToAction("Index");
        }


        // GET: Garage/Details/5
        public ActionResult Details(int id=0)
        {
            if (id != 0)
            {
                //Update the ParkPrice to current price
                garage.UpdateParkPrice();
                return View(garage.GetVehicle(id));
            }
            return RedirectToAction("Index");
        }

        // GET: Garage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Garage/Create
        [HttpPost]
        public ActionResult Create(Vehicle vehicle, string vehicleType)
        {
            switch(vehicleType)
            { 
                case "Car":
                    vehicle.Type = VehicleType.Car;
                    break;
                case "MC":
                    vehicle.Type = VehicleType.Mc;
                    break;
                case "Bus":
                    vehicle.Type = VehicleType.Bus;
                    break;
                case "Truck":
                    vehicle.Type = VehicleType.Truck;
                    break;
                case "Horse":
                    vehicle.Type = VehicleType.Horse;
                    break;
                default :
                    return RedirectToAction("Index");
            }

            this.garage.Add(vehicle);
            
            return RedirectToAction("Create");
        }

        // GET: Garage/Edit/5
        public ActionResult Edit(int id)
        {
            return View(garage.GetVehicle(id));
        }

        // POST: Garage/Edit/5
        [HttpPost]
        public ActionResult Edit(Vehicle vehicle)
        {
            try
            {
                garage.Edit(vehicle);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Garage/Delete/5
        public ActionResult Delete(int id=0)
        {
            if (id != 0)
            {
                garage.UpdateVehiclePrice(id);
                return View(garage.GetVehicle(id));
            }
            return RedirectToAction("Index");
        }


        // POST: Garage/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, string value = "")
        {
            garage.Remove(id);

            return RedirectToAction("Index");
        }
    }
}
