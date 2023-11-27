using NUnit.Framework;

using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace MMABooksTests
{
    public class CustomerDBTests
    {
        CustomerDB db;

        [SetUp]
        public void ResetData()
        {
            db = new CustomerDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(43);
            Assert.AreEqual("Haldorai, Brent", p.Name);
            Assert.AreEqual("1427 Valley", p.Address);
            Assert.AreEqual("Huntsville", p.City);
            Assert.AreEqual("AL", p.State);
            Assert.AreEqual("35806", p.ZipCode);
            Console.WriteLine(p.CustomerID);
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<CustomerProps> list = (List<CustomerProps>)db.RetrieveAll();
            Assert.AreEqual(696, list.Count);
        }

        [Test]
        public void TestDelete()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(43);
            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve(43));
        }

        [Test]
        public void TestUpdate()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(43);
            p.Name = "Haldorai, Michael";
            Assert.True(db.Update(p));
            p = (CustomerProps)db.Retrieve(43);
            Assert.AreEqual("Haldorai, Michael", p.Name);
        }

        [Test]
        public void TestUpdateFieldTooLong()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(43);
            p.State = "Oregon";
            Assert.Throws<MySqlException>(() => db.Update(p));
        }

        [Test]
        public void TestCreate()
        {
            CustomerProps p = new CustomerProps();
            p.Name = "Michael Hansen";
            p.Address = "103 Prairie Drive";
            p.City = "Flatsburg";
            p.State = "OH";
            p.ZipCode = "99999";
			db.Create(p);
            CustomerProps p2 = (CustomerProps)db.Retrieve(p.CustomerID);
            Assert.AreEqual(p.GetState(), p2.GetState());
            Console.WriteLine(p.GetState());
        }

        [Test]
        public void TestCreatePrimaryKeyViolation()
        {
            CustomerProps p = new CustomerProps();
			p.Name = "Michael Vanderburg";
			p.Address = "103 Prairie Drive";
			p.City = "Flatsburg";
			p.State = "Ohio";
			p.ZipCode = "99999";
			Assert.Throws<MySqlException>(() => db.Create(p));
        }
    }
}