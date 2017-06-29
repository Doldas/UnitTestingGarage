using GarageAngularJS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GarageAngularJS.Repository
{
    public class GarageRepoTests : GarageRepoInterface
    {
        private List<Vehicle> db = new List<Vehicle>();

        public List<Vehicle> GetFilteredList(string type)
        {
            throw new NotImplementedException();
        }

        public List<Vehicle> SortParking(bool descend)
        {
            throw new NotImplementedException();
        }

        public List<Vehicle> SortOwner(bool descend)
        {
            throw new NotImplementedException();
        }

        public List<Vehicle> SortDate(bool descend)
        {
            throw new NotImplementedException();
        }

        public List<Vehicle> SortReg(bool descend)
        {
            throw new NotImplementedException();
        }

        public List<Vehicle> SortType(bool descend)
        {
            throw new NotImplementedException();
        }

        public List<Vehicle> GetAll()
        {
            return db;
        }

        public Vehicle GetVehicle(int id)
        {
            throw new NotImplementedException();
        }

        public Vehicle GetVehicle(string id)
        {
            throw new NotImplementedException();
        }

        public void Edit(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public void UpdateParkPrice()
        {
            throw new NotImplementedException();
        }

        public void UpdateVehiclePrice(int id)
        {
            throw new NotImplementedException();
        }
        // Set default price of vehicle
        private Vehicle SetDefaultPrice(Vehicle vehicle)
        {
            //reset the parkingPrice to it's default values
            if (vehicle.Type == VehicleType.Car) { vehicle.ParkingPrice = 1; }
            else if (vehicle.Type == VehicleType.Mc) { vehicle.ParkingPrice = 0.45M; }
            else if (vehicle.Type == VehicleType.Bus) { vehicle.ParkingPrice = 2; }
            else { vehicle.ParkingPrice = 3.50M; }

            return vehicle;
        }
        public bool Add(Vehicle vehicle)
        {
            bool exists = false;
                vehicle.RegNumber = vehicle.RegNumber.ToUpper();
                int index = 1;

                foreach (var v in db.OrderBy(v => v.ParkingPlace))
                {
                    //If Vehicle Exists
                    if (v.RegNumber == vehicle.RegNumber)
                    {
                        exists = true;
                        break;
                    }
                    //Set the parking place for the vehicle to the empty parking slot
                    if (index != v.ParkingPlace)
                    {
                        vehicle.ParkingPlace = index;
                        break;
                    }
                    index++;
                }
                //If the Vehicle doesn't exist in the database, add it to the db
                if (exists == false)
                {
                    if (vehicle.ParkingPlace == 0) { vehicle.ParkingPlace = index; }

                    vehicle = SetDefaultPrice(vehicle);

                    db.Add(vehicle);
                }
            return exists;
        }

        public List<Vehicle> Search(string searchTerm)
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
                return db.Where(vehicle => vehicle.Owner.Contains(searchTerm) || vehicle.RegNumber == searchTerm).ToList();
            }
            return db.Where(vehicle => vehicle.ParkingPlace == pSlot).ToList();    
        }

        public Vehicle Remove(int id)
        {
            Models.Vehicle vehicle;
            vehicle = db.Where(v => v.ID == id).FirstOrDefault();

            if (vehicle != null)
            {
                db.Remove(vehicle);
            }

            return vehicle;
        }
    }
}