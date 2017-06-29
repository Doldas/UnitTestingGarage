using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using GarageAngularJS;
using GarageMVC.Controllers;
using GarageAngularJS.Models;
using System.Collections.Generic;
using Telerik.JustMock;
using GarageAngularJS.Repository;

namespace GarageUnitTests
{
    [TestClass]
    public class GarageControllerTests
    {
        //Check In Page Tests
        [TestMethod]
        public void Add_Vehicle_Given_TestData_Results_One_Vehicle_Added()
        {
            //Arrange
            GarageRepoTests testRepo = new GarageRepoTests();
            Vehicle a = new Vehicle { Owner = "Teddy", RegNumber = "BAS521", SSN = "1999/08/14-XXXX"};

            GarageController controller = new GarageController(testRepo);
            //Act
            controller.Create(a,"Car");
            ViewResult result = controller.Index("") as ViewResult;
            List<Vehicle> vehicles = result.ViewBag.OutputListTestingPurpose;
            //Assert
            Assert.AreEqual(1, vehicles.Count);
            Assert.AreEqual("Teddy", vehicles[0].Owner);
            Assert.AreEqual(1, vehicles[0].ParkingPlace);
        }
        [TestMethod]
        public void Add_Vehicle_Given_Duplicated_TestData_Results_Not_Added()
        {
            //Arrange
            GarageRepoTests testRepo = new GarageRepoTests();
            GarageController controller = new GarageController(testRepo);
            Vehicle a = new Vehicle { Owner = "Teddy", RegNumber = "BAS521", SSN = "1999/08/14-XXXX" };
            Vehicle b = new Vehicle { Owner = "Teddy", RegNumber = "BAS521", SSN = "1999/08/14-XXXX" };
            Vehicle c = new Vehicle { Owner = "Teddy", RegNumber = "BAS521", SSN = "1999/08/14-XXXX" };
            controller.Create(a, "Car");
            //Act
            controller.Create(b, "Car");
            controller.Create(c, "Car");
            ViewResult result = controller.Index("") as ViewResult;
            List<Vehicle> vehicles = result.ViewBag.OutputListTestingPurpose;
            //Assert
            Assert.AreEqual(1, vehicles.Count);
        }
        [TestMethod]
        public void Add_Vehicle_Given_Multiple_TestData_Results_Added_And_Each_Vehicle_Have_A_Unique_ParkingPlace()
        {
            //Arrange
            GarageRepoTests testRepo = new GarageRepoTests();
            GarageController controller = new GarageController(testRepo);
            Vehicle a = new Vehicle { Owner = "Rafiki", RegNumber = "BAS521", SSN = "1927/01/22-XXXX" };
            Vehicle b = new Vehicle { Owner = "Mustafa", RegNumber = "SRE113", SSN = "1958/07/04-XXXX" };
            Vehicle c = new Vehicle { Owner = "Simba", RegNumber = "JIO208", SSN = "1998/08/14-XXXX" };
            //Act
            controller.Create(a, "MC");
            controller.Create(b, "MC");
            controller.Create(c, "MC");
            ViewResult result = controller.Index("") as ViewResult;
            List<Vehicle> vehicles = result.ViewBag.OutputListTestingPurpose;
            //Assert
            Assert.AreEqual(3, vehicles.Count);
            Assert.AreEqual(1,vehicles[0].ParkingPlace);
            Assert.AreEqual(2, vehicles[1].ParkingPlace);
            Assert.AreEqual(3, vehicles[2].ParkingPlace);
        }
        [TestMethod]
        public void Simulated_Add_And_Remove_Vehicle_Given_Multiple_TestData_Results_Added_And_Each_Vehicle_Have_A_Unique_ParkingPlace()
        {
            //Arrange
            GarageRepoTests testRepo = new GarageRepoTests();
            GarageController controller = new GarageController(testRepo);
            Vehicle a = new Vehicle { ID=1, Owner = "Rafiki", RegNumber = "BAS521", SSN = "1927/01/22-XXXX" };
            Vehicle b = new Vehicle { ID=2, Owner = "Mustafa", RegNumber = "SRE113", SSN = "1958/07/04-XXXX" };
            Vehicle c = new Vehicle { ID=3, Owner = "Simba", RegNumber = "JIO208", SSN = "1998/08/14-XXXX" };
            
            Vehicle d = new Vehicle { ID=4, Owner = "Teor", RegNumber = "IOS134", SSN = "1927/01/22-XXXX" };
            Vehicle e = new Vehicle { ID=5, Owner = "Teddy", RegNumber = "MVG211", SSN = "1958/07/04-XXXX" };
            Vehicle f = new Vehicle { ID=6, Owner = "Deric", RegNumber = "ASK538", SSN = "1998/08/14-XXXX" };

            Vehicle g = new Vehicle { ID = 7, Owner = "Garrosh", RegNumber = "KKA238", SSN = "1996/08/14-XXXX" };
           
            //Act
            controller.Create(a, "MC"); //ParkingPlace 1
            controller.Create(b, "Car"); //ParkingPlace 2
            controller.Create(c, "MC"); //ParkingPlace 3 - Simba

            controller.Create(d, "Horse"); //ParkingPlace 4
            controller.Create(e, "Car"); //ParkingPlace 5 - Teddy
            controller.Create(f, "Truck"); //ParkingPlace 6

            controller.Delete(3,""); //ParkingPlace 3 will be empty - Simba is Gone - Next vehicle can take this place
            controller.Delete(5,""); //ParkingPlace 5 Will be empty - Teddy is Gone - Than every parkingplace before this one is taken, Next vehicle can park here.

            controller.Create(e,"Car"); //Parkingplace 3
            controller.Create(c, "MC"); //parkingplace 5
            controller.Create(g, "Truck"); //parkingplace 7 - Garrosh

            ViewResult result = controller.Index("") as ViewResult;
            List<Vehicle> vehicles = result.ViewBag.OutputListTestingPurpose; //Vehicle List

            //Assert
            Assert.AreEqual(7, vehicles.Count);
            
            Assert.AreEqual(3, vehicles[4].ParkingPlace);
            Assert.AreEqual("Teddy", vehicles[4].Owner);
            
            Assert.AreEqual(5, vehicles[5].ParkingPlace);
            Assert.AreEqual("Simba", vehicles[5].Owner);
            
            Assert.AreEqual(7, vehicles[6].ParkingPlace);
            Assert.AreEqual("Garrosh", vehicles[6].Owner);
        }
        
        //Index Page Tests
        [TestMethod]
        public void With_Interface_Index_GetAll_Results_All_Vehicles()
        {
            //Arrange
            var repo = Mock.Create<GarageRepoInterface>();
            Mock.Arrange(()=>repo.GetAll()).
                Returns(new List<Vehicle>(){
                    new Vehicle{ ID=1, ParkingPlace=1, ParkingDate=DateTime.Now, RegNumber="SDD627", Type= VehicleType.Horse, Owner="Ted", SSN="1997/09/06-XXXX"},
                    new Vehicle{ ID=2, ParkingPlace=2, ParkingDate=DateTime.Now, RegNumber="ACF101", Type= VehicleType.Car, Owner="Murphy", SSN="1999/08/14-XXXX"}     
                }).MustBeCalled();

            GarageController controller = new GarageController(repo);
            //Act
            ViewResult result = controller.Index("") as ViewResult;
            List<Vehicle> vehicles = result.ViewBag.OutputListTestingPurpose;
            //Assert
            Assert.AreEqual(2, vehicles.Count);
            Assert.AreEqual("Murphy", vehicles[1].Owner);
        }
        [TestMethod]
        public void Index_Search_Given_Ted_Results_Ted()
        {
            //Arrange
            GarageRepoTests testRepo = new GarageRepoTests();
            Vehicle a = new Vehicle { ID = 1, Owner = "Ted", RegNumber = "AAS321", SSN = "1999/08/14-XXXX", Type = VehicleType.Horse };
            Vehicle b = new Vehicle { ID = 2, Owner = "Telia", RegNumber = "BBE456", SSN = "2017/01/12-XXXX", Type = VehicleType.Car };
            Vehicle c = new Vehicle { ID = 3, Owner = "Dennis", RegNumber = "ERG651", SSN = "1944/6/26-XXXX", Type = VehicleType.Mc };
            Vehicle d = new Vehicle { ID = 4, Owner = "Sandra", RegNumber = "dsf477", SSN = "1925/01/02-XXXX", Type = VehicleType.Truck };

            testRepo.Add(a);
            testRepo.Add(b);
            testRepo.Add(c);
            testRepo.Add(d);

            GarageController controller = new GarageController(testRepo);
            //Act
            ViewResult result = controller.Index("Ted") as ViewResult;
            string test = result.ViewBag.Test;
            List<Vehicle> vehicles = result.ViewBag.OutputListTestingPurpose;
            //Assert
            Assert.AreEqual(1, vehicles.Count);
            Assert.AreEqual("Ted", vehicles[0].Owner);
        }
        [TestMethod]
        public void Index_Search_Given_3_Results_Dennis()
        {
            //Arrange
            GarageRepoTests testRepo = new GarageRepoTests();
            Vehicle a = new Vehicle { ID = 1, Owner = "Ted", RegNumber = "AAS321", SSN = "1999/08/14-XXXX", Type = VehicleType.Horse };
            Vehicle b = new Vehicle { ID = 2, Owner = "Telia", RegNumber = "BBE456", SSN = "2017/01/12-XXXX", Type = VehicleType.Car };
            Vehicle c = new Vehicle { ID = 3, Owner = "Dennis", RegNumber = "ERG651", SSN = "1944/6/26-XXXX", Type = VehicleType.Mc };
            Vehicle d = new Vehicle { ID = 4, Owner = "Sandra", RegNumber = "dsf477", SSN = "1925/01/02-XXXX", Type = VehicleType.Truck };

            testRepo.Add(a);
            testRepo.Add(b);
            testRepo.Add(c);
            testRepo.Add(d);
            GarageController controller = new GarageController(testRepo);
            
            //Act
            ViewResult result = controller.Index("3") as ViewResult;
            string test = result.ViewBag.Test;
            List<Vehicle> vehicles = result.ViewBag.OutputListTestingPurpose;
            
            //Assert
            Assert.AreEqual(1, vehicles.Count);
            Assert.AreEqual("Dennis", vehicles[0].Owner);
        }
        [TestMethod]
        public void Index_Search_Given_DSF477_Results_VehichleType_Truck()
        {
            //Arrange
            GarageRepoTests testRepo = new GarageRepoTests();
            Vehicle a = new Vehicle { Owner = "Ted", RegNumber = "AAS321", SSN = "1999/08/14-XXXX", Type = VehicleType.Horse };
            Vehicle b = new Vehicle { Owner = "Telia", RegNumber = "BBE456", SSN = "2017/01/12-XXXX", Type = VehicleType.Car };
            Vehicle c = new Vehicle { Owner = "Dennis", RegNumber = "ERG651", SSN = "1944/6/26-XXXX", Type = VehicleType.Mc };
            Vehicle d = new Vehicle { Owner = "Sandra", RegNumber = "dSf477", SSN = "1925/01/02-XXXX", Type = VehicleType.Truck };

            testRepo.Add(a);
            testRepo.Add(b);
            testRepo.Add(c);
            testRepo.Add(d);

            GarageController controller = new GarageController(testRepo);
            //Act
            ViewResult result = controller.Index("DSF477") as ViewResult;
            string test = result.ViewBag.Test;
            List<Vehicle> vehicles = result.ViewBag.OutputListTestingPurpose;
            //Assert
            Assert.AreEqual(1, vehicles.Count);
            Assert.AreEqual(VehicleType.Truck, vehicles[0].Type);
        }
    }
}
