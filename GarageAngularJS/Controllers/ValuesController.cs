using GarageAngularJS.Models;
using GarageAngularJS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;

namespace GarageAngularJS.Controllers
{
    public class ValuesController : ApiController
    {
        GarageRepository garage = new GarageRepository();
        // GET api/values

        public IEnumerable<Vehicle> Get()
        {
            return garage.GetAll();
        }

        // GET api/values/5
        public Vehicle Get(string regNr)
        {
            return garage.GetVehicle(regNr);
        }
        /// <summary>
        /// Get Price info for specific Vehicle
        /// </summary>
        /// <param name="regNr"></param>
        /// <returns></returns>
        public decimal PriceInfo(string regNr)
        {
            return garage.GetVehicle(regNr).ParkingPrice;
        }
        /// <summary>
        /// Get all Empty ParkingSpots between all vehicles
        /// </summary>
        /// <returns></returns>
        public int[] FreeParkingSpotsBetweenAllCars()
        {
            List<int> spots = new List<int>();
            int index = 1;
            foreach (var v in garage.SortParking(false))
            {
                if (v.ParkingPlace != index)
                {
                    spots.Add(index);
                }
                index++;
            }
            return spots.ToArray();
        }
        /*
        [HttpGet]
        public IEnumerable<Vehicle> TypeSearch(string sTerm="")
        {
            return garage.Search(sTerm);
        }
        [HttpPost]
        public IEnumerable<Vehicle> Filter(string filter = "None")
        {
            return garage.GetFilteredList(filter);
        }
        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
         * */
    }
}
