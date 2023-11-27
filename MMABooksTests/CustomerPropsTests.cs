using NUnit.Framework;

using MMABooksProps;
using System;
using System.Linq;

namespace MMABooksTests
{
    [TestFixture]
    public class CustomerPropsTests
    {
        CustomerProps props;
        [SetUp]
        public void Setup()
        {
            props = new CustomerProps();
            props.CustomerID = 37;
            props.Name = "Mr Test";
            props.Address = "1010 Test St";
            props.City = "Testburg";
            props.State = "TE";
            props.ZipCode = "11111";
        }

        [Test]
        public void TestGetState()
        {
            string jsonString = props.GetState();
            Console.WriteLine(jsonString);
			Assert.IsTrue(jsonString.Contains(props.CustomerID.ToString()));
			Assert.IsTrue(jsonString.Contains(props.Name));
            Assert.IsTrue(jsonString.Contains(props.Address));
            Assert.IsTrue(jsonString.Contains(props.City));
            Assert.IsTrue(jsonString.Contains(props.State));
            Assert.IsTrue(jsonString.Contains(props.ZipCode));
        }

        [Test]
        public void TestSetState()
        {
            string jsonString = props.GetState();
            CustomerProps newProps = new CustomerProps();
            newProps.SetState(jsonString);
            Assert.AreEqual(props.CustomerID, newProps.CustomerID);
            Assert.AreEqual(props.Name, newProps.Name);
            Assert.AreEqual(props.Address, newProps.Address);
            Assert.AreEqual(props.City, newProps.City);
            Assert.AreEqual(props.State, newProps.State);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }

        [Test]
        public void TestClone()
        {
            CustomerProps newProps = (CustomerProps)props.Clone();
			Assert.AreEqual(props.CustomerID, newProps.CustomerID);
			Assert.AreEqual(props.Name, newProps.Name);
			Assert.AreEqual(props.Address, newProps.Address);
			Assert.AreEqual(props.City, newProps.City);
			Assert.AreEqual(props.State, newProps.State);
			Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
		}
    }
}