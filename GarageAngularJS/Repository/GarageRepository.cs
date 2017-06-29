using GarageAngularJS.DataAccess;
using GarageAngularJS.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
namespace GarageAngularJS.Repository
{
    public class GarageRepository : GarageRepoInterface
    {
        private GarageContext db;

        #region Constructors
        public GarageRepository()
        {
            db = new GarageContext();
        }
        #endregion

        //Add a vehicle to database
        public bool Add(Models.Vehicle vehicle)
        {
            bool exists = false;
            if (vehicle != null)
            {
                vehicle.RegNumber = vehicle.RegNumber.ToUpper();
                bool Once = true;
                int index = 1;

                foreach (var v in db.Vehicles.OrderBy(v=>v.ParkingPlace))
                {
                    //If Vehicle Exists
                    if (v.RegNumber == vehicle.RegNumber)
                    {
                        exists = true;
                        break;
                    }
                    //Set the parking place for the vehicle to the empty parking slot
                    if (index != v.ParkingPlace && Once == true)
                    {
                        vehicle.ParkingPlace = index;
                        Once = false;
                        break;
                    }
                    index++;
                }
                //If the Vehicle doesn't exist in the database, add it to the db
                if (exists == false)
                {
                    if (vehicle.ParkingPlace == 0) { vehicle.ParkingPlace = index; }
                    
                    vehicle = SetDefaultPrice(vehicle);

                    db.Entry(vehicle).State = EntityState.Added;
                    db.SaveChanges();
                }
            }
            return exists;
        }
        #region Get Vehicle(s)

        //GET all vehicles from database
        public List<Vehicle> GetAll()
        {
            return db.Vehicles.ToList();
        }

        public Models.Vehicle GetVehicle(int id)
        {
            return db.Vehicles.Where(v => v.ID == id).FirstOrDefault();
        }
        public Models.Vehicle GetVehicle(string reg)
        {
            return db.Vehicles.Where(v => v.RegNumber == reg).FirstOrDefault();
        }
        //GET filtered vehicle list
        public List<Vehicle> GetFilteredList(string type)
        {
            if(type=="Car")
            {
                return db.Vehicles.Where(vehicle => vehicle.Type == VehicleType.Car).ToList();
            }
            else if(type=="Bus")
            {
                return db.Vehicles.Where(vehicle => vehicle.Type == VehicleType.Bus).ToList();
            }
            else if(type=="Mc")
            {
                return db.Vehicles.Where(vehicle => vehicle.Type == VehicleType.Mc).ToList();
            }
            else if (type == "Horse")
            {
                return db.Vehicles.Where(vehicle => vehicle.Type == VehicleType.Horse).ToList();
            }
            else
                return db.Vehicles.Where(vehicle => vehicle.Type == VehicleType.Truck).ToList();
        }

        //GET Sorted Lists
        public List<Vehicle> SortParking(bool descend)
        {
            if(descend)
            {
                return db.Vehicles.OrderByDescending(v => v.ParkingPlace).ToList();
            }
            return db.Vehicles.OrderBy(v => v.ParkingPlace).ToList();
        }
        public List<Vehicle> SortOwner(bool descend)
        {
            if (descend)
            {
                return db.Vehicles.OrderByDescending(v => v.Owner).ToList();
            }
            return db.Vehicles.OrderBy(v => v.Owner).ToList();
        }
        public List<Vehicle> SortDate(bool descend)
        {
            if (descend)
            {
                return db.Vehicles.OrderByDescending(v => v.ParkingDate).ToList();
            }
            return db.Vehicles.OrderBy(v => v.ParkingDate).ToList();
        }
        public List<Vehicle> SortReg(bool descend)
        {
            if (descend)
            {
                return db.Vehicles.OrderByDescending(v => v.RegNumber).ToList();
            }
            return db.Vehicles.OrderBy(v => v.RegNumber).ToList();
        }
        public List<Vehicle> SortType(bool descend)
        {
            if (descend)
            {
                return db.Vehicles.OrderByDescending(v => v.Type).ToList();
            }
            return db.Vehicles.OrderBy(v => v.Type).ToList();
        }
        #endregion

        //Edit a vehicle
        public void Edit(Models.Vehicle vehicle)
        {
            //Edits the element without removing and inserting it
            db.Entry(vehicle).State = EntityState.Modified; 
            //Saves the new Data in the Database
            db.SaveChanges();
        }

        //Updates the parking prices for each vehicle
        public void UpdateParkPrice()
        {
            foreach(var v in db.Vehicles)
            {
                //reset the parkingPrice to it's default values
                Vehicle vehicle = SetDefaultPrice(v);

                //Calculate the timespan and than update the cost
                System.TimeSpan tspan = System.DateTime.Now - vehicle.ParkingDate;
                vehicle.ParkingPrice = vehicle.ParkingPrice * (System.Convert.ToDecimal(tspan.TotalMinutes));
            }
            db.SaveChanges();
        }

        // Updates price for a specific vehicle
        public void UpdateVehiclePrice(int id)
        {
            Vehicle veh = db.Vehicles.Where(v=>v.ID==id).FirstOrDefault();
            veh = SetDefaultPrice(veh);
                System.TimeSpan tspan = System.DateTime.Now - veh.ParkingDate;
                veh.ParkingPrice = veh.ParkingPrice * (System.Convert.ToDecimal(tspan.TotalMinutes));
                Edit(veh);
        }

        // Set default price of vehicle
        private Vehicle SetDefaultPrice(Vehicle vehicle)
        {
            //reset the parkingPrice to it's default values
            if(vehicle.Type == VehicleType.Car) { vehicle.ParkingPrice = 1; }
            else if(vehicle.Type == VehicleType.Mc) { vehicle.ParkingPrice = 0.45M; }
            else if(vehicle.Type == VehicleType.Bus) { vehicle.ParkingPrice = 2; }
            else { vehicle.ParkingPrice = 3.50M; }

            return vehicle;
        }
        //Remove vehicle from database and return vehicle information
        public Models.Vehicle Remove(int id)
        {
            Models.Vehicle vehicle;
            vehicle = db.Vehicles.Where(v => v.ID == id).FirstOrDefault();

            if (vehicle != null)
            {
                db.Entry(vehicle).State = EntityState.Deleted;
                db.SaveChanges();
            }

            return vehicle;
        }

        //Search vehicle(s)
        public List<Models.Vehicle> Search(string searchTerm)
        {
            int pSlot = -1;

            // Try to parse the input string to double,
            // if it works, the user want to search by parking slot
            try
            {
                pSlot = int.Parse(searchTerm);
            }
            catch
            {
                return db.Vehicles.Where(vehicle => vehicle.Owner.Contains(searchTerm) || vehicle.RegNumber == searchTerm).ToList();
            }
            return db.Vehicles.Where(vehicle => vehicle.ParkingPlace == pSlot).ToList();
        }
    }
}