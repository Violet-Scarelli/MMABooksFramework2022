using MMABooksDB;
using MMABooksProps;
using MMABooksTools;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MMABooksBusiness
{
	public class Customer : BaseBusiness
	{
		public int CustomerID
		{
			get
			{
				return ((CustomerProps)mProps).CustomerID;
			}
			set
			{
				if(!(value == ((CustomerProps)mProps).CustomerID))
				{
					mRules.RuleBroken("CustomerID", false);
					((CustomerProps)mProps).CustomerID = value;	
					mIsDirty = true;
				}
				else
				{
					throw new Exception("New ID is the same as old ID.");
				}
			}
		}
		public String Name
		{
			get
			{
				return ((CustomerProps)mProps).Name;
			}
			set
			{
				if (!(value == ((CustomerProps)mProps).Name))
				{
					mRules.RuleBroken("Name", false);
					((CustomerProps)mProps).Name = value;
					mIsDirty = true;
				}
				else
				{
					throw new Exception("New name is the same as old name.");
				}
			}
		}
		public String Address
		{
			get
			{
				return ((CustomerProps)mProps).Address;
			}
			set
			{
				if(!(value == ((CustomerProps)mProps).Address))
				{
					mRules.RuleBroken("Address", false);
					((CustomerProps)mProps).Address = value;
					mIsDirty = true;
				}
				else
				{
					throw new Exception("New address is the same as old address.");
				}
			}
		}
		public String City
		{
			get
			{
				return ((CustomerProps)mProps).City;
			}
			set
			{
				if(!(value == ((CustomerProps)mProps).City))
				{
					mRules.RuleBroken("City", false);
					((CustomerProps)mProps).City = value;
					mIsDirty = true;
				}
				else
				{
					throw new Exception("New city is the same as old city."); ;
				}
			}
		}
		public String State
		{
			get
			{
				return ((CustomerProps)mProps).State;
			}
			set
			{
				if(!(value == ((CustomerProps)mProps).State))
				{
					if(value.Trim().Length >= 1 && value.Trim().Length <= 2) 
					{
						mRules.RuleBroken("State", false);
						((CustomerProps)mProps).State = value;
						mIsDirty = true;
					}
					else
					{
						throw new ArgumentOutOfRangeException("State code must be no more than 2 characters long.");
					}
				}
				
			}
		}
		public String ZipCode
		{
			get
			{
				return ((CustomerProps)mProps).ZipCode;	
			}
			set
			{
				if(!(value == ((CustomerProps)mProps).ZipCode))
				{
					if (value.Trim().Length >= 5 && value.Trim().Length <= 11)
					{
						mRules.RuleBroken("ZipCode", false);
						((CustomerProps)mProps).ZipCode = value;
						mIsDirty = true;
					}
					else
					{
						throw new ArgumentOutOfRangeException("Zipcode must be between 5 and 11 digits.");
					}
				}
				
			}
		}
		public override object GetList()
		{
			List<Customer> customers = new List<Customer>();
			List<CustomerProps> props = new List<CustomerProps>();

			props = (List<CustomerProps>)mdbReadable.RetrieveAll();
			foreach (CustomerProps prop in props)
			{
				Customer c = new Customer(prop);
				customers.Add(c);
			}
			return customers;
		}

		protected override void SetDefaultProperties()
		{
		}

		protected override void SetRequiredRules()
		{
			mRules.RuleBroken("CustomerID", true);
			mRules.RuleBroken("Name", true);
			mRules.RuleBroken("Address", true);
			mRules.RuleBroken("City", true);
			mRules.RuleBroken("State", true);
			mRules.RuleBroken("ZipCode", true);
		}

		protected override void SetUp()
		{
			mProps = new CustomerProps();
			mOldProps = new CustomerProps();

			mdbReadable = new CustomerDB();
			mdbWriteable = new CustomerDB();
		}
		public Customer() : base()
		{
		}
		public Customer(string key) : base(key)
		{
		}
		private Customer(CustomerProps props) : base(props)
		{
		}

		public Customer(int key) : base(key)
		{
		}

		public Customer(object key) : base(key)
		{
		}
	}
}
