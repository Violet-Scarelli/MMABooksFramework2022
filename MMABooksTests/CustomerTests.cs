using NUnit.Framework;

using MMABooksBusiness;
using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace MMABooksTests
{
    [TestFixture]
    public class CustomerTests
    {

        [SetUp]
        public void TestResetDatabase()
        {
            CustomerDB db = new CustomerDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewCustomerConstructor()
        {
            // not in Data Store - no code
            Customer c = new Customer();
            Assert.AreEqual(0, c.CustomerID);
            Assert.AreEqual(string.Empty, c.Name);
            Assert.AreEqual(string.Empty, c.Address);
            Assert.AreEqual(string.Empty, c.City);
            Assert.AreEqual(string.Empty, c.State);
            Assert.AreEqual(string.Empty, c.ZipCode);
            Assert.IsTrue(c.IsNew);
            Assert.IsFalse(c.IsValid);
        }


        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Customer c = new Customer(3);
            Assert.AreEqual(3, c.CustomerID);
            Assert.IsTrue(c.CustomerID != 0);
            Assert.IsFalse(c.IsNew);
            Assert.IsTrue(c.IsValid);
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Customer c = new Customer();
            c.CustomerID = 700;
            c.Name = "Who am I";
            c.Address = "121 West Drive";
            c.City = "Flatsburg";
            c.State = "OH";
            c.ZipCode = "99301";
            c.Save();
            Customer c2 = new Customer(700);
            Assert.AreEqual(c2.CustomerID, c.CustomerID);
            Assert.AreEqual(c2.Name, c.Name);
        }

        [Test]
        public void TestUpdate()
        {
            Customer c = new Customer(43);
            c.Name = "Edited Name";
            c.Save();

            Customer c2 = new Customer(43);
            Assert.AreEqual(c2.CustomerID, c.CustomerID);
            Assert.AreEqual(c2.Name, c.Name);
        }

        [Test]
        public void TestDelete()
        {
            Customer c = new Customer(43);
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(43));
        }

        [Test]
        public void TestGetList()
        {
            Customer s = new Customer();
            List<Customer> customers = (List<Customer>)s.GetList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual("Molunguri, A", customers[0].Name);
            Assert.AreEqual("AL", customers[0].State);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Customer c = new Customer();
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Customer c = new Customer();
            Assert.Throws<Exception>(() => c.Save());
            c.Name = "??";
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            Customer c = new Customer();
            Assert.Throws<ArgumentOutOfRangeException>(() => c.ZipCode = "99999-999999");
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Customer c1 = new Customer(1);
            Customer c2 = new Customer(1);

            c1.Name = "Updated first";
            c1.Save();

            c2.Name = "Updated second";
            Assert.Throws<Exception>(() => c2.Save());
        }
    }
}