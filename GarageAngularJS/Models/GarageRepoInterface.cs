using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageAngularJS.Models
{
    public interface GarageRepoInterface
    {
        List<Vehicle> GetFilteredList(string type);
        List<Vehicle> SortParking(bool descend);
        List<Vehicle> SortOwner(bool descend);
        List<Vehicle> SortDate(bool descend);
        List<Vehicle> SortReg(bool descend);
        List<Vehicle> SortType(bool descend);
        List<Vehicle> GetAll();
        Models.Vehicle GetVehicle(int id);
        Models.Vehicle GetVehicle(string id);
        void Edit(Models.Vehicle vehicle);
        void UpdateParkPrice();
        void UpdateVehiclePrice(int id);
        bool Add(Models.Vehicle vehicle);
        List<Models.Vehicle> Search(string searchTerm);
        Models.Vehicle Remove(int id);
    }
}
