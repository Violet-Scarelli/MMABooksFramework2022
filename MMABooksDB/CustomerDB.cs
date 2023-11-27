using System;
using System.Collections.Generic;
using System.Text;

using MMABooksTools;
using MMABooksProps;

using System.Data;

// *** I use an "alias" for the ado.net classes throughout my code
// When I switch to an oracle database, I ONLY have to change the actual classes here
using DBBase = MMABooksTools.BaseSQLDB;
using DBConnection = MySql.Data.MySqlClient.MySqlConnection;
using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using DBParameter = MySql.Data.MySqlClient.MySqlParameter;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;
using DBDataAdapter = MySql.Data.MySqlClient.MySqlDataAdapter;
using DBDbType = MySql.Data.MySqlClient.MySqlDbType;

namespace MMABooksDB
{
	public class CustomerDB : DBBase, IReadDB, IWriteDB
	{
		public CustomerDB() : base() { }
		public CustomerDB(DBConnection cn) : base(cn) { }

		public IBaseProps Create(IBaseProps p)
		{
			int rowsAffected = 0;
			CustomerProps props = (CustomerProps)p;
			DBCommand command = new DBCommand();
			command.CommandText = "usp_CustomerCreate";
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.AddWithValue("customerId", props.CustomerID);
			command.Parameters.AddWithValue("name_p", props.Name);
			command.Parameters.AddWithValue("address_p", props.Address);
			command.Parameters.AddWithValue("city_p", props.City);
			command.Parameters.AddWithValue("state_p", props.State);
			command.Parameters.AddWithValue("zipcode_p", props.ZipCode);
			command.Parameters[0].Direction = ParameterDirection.Output;

			try
			{
				rowsAffected = RunNonQueryProcedure(command);
				if (rowsAffected == 1)
				{
					props.CustomerID = (int)command.Parameters[0].Value;
					props.ConcurrencyID = 1;
					return props;
				}
				else
					throw new Exception("Unable to insert record. " + props.GetState());
			}
			catch (Exception e)
			{
				// log this error
				throw;
			}
			finally
			{
				if (mConnection.State == ConnectionState.Open)
					mConnection.Close();
			}
		}

		public bool Delete(IBaseProps p)
		{
			CustomerProps props = (CustomerProps)p;
			int rowsAffected = 0;

			DBCommand command = new DBCommand();
			command.CommandText = "usp_CustomerDelete";
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("custId", DBDbType.Int32);
			command.Parameters.Add("conCurrId", DBDbType.Int32);
			command.Parameters["custId"].Value = props.CustomerID;
			command.Parameters["conCurrId"].Value = props.ConcurrencyID;

			try
			{
				rowsAffected = RunNonQueryProcedure(command);
				if (rowsAffected == 1)
				{
					return true;
				}
				else
				{
					string message = "Record cannot be deleted. It has been edited by another user.";
					throw new Exception(message);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}
			finally
			{
				if (mConnection.State == ConnectionState.Open)
				{
					mConnection.Close();
				}
			}
		}

		public IBaseProps Retrieve(object key)
		{
			DBDataReader data = null;
			CustomerProps props = new CustomerProps();
			DBCommand command = new DBCommand();

			command.CommandText = "usp_CustomerSelect";
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("custId", DBDbType.Int32);
			command.Parameters["custId"].Value = key;
			try
			{
				data = RunProcedure(command);
				if (!data.IsClosed)
				{
					if (data.Read())
					{
						props.SetState(data);
					}
					else throw new Exception("Record does not exist in the database.");

				}
				return props;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}
			finally
			{
				if (data != null)
				{
					if (!data.IsClosed)
						data.Close();
				}
			}
		}

		public object RetrieveAll()
		{
			List<CustomerProps> list = new List<CustomerProps>();
			DBDataReader reader = null;
			CustomerProps props;

			try
			{
				reader = RunProcedure("usp_CustomerSelectAll");
				if (!reader.IsClosed)
				{
					while (reader.Read())
					{
						props = new CustomerProps();
						props.SetState(reader);
						list.Add(props);
					}
				}
				return list;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}
			finally
			{
				if (!reader.IsClosed)
				{
					reader.Close();
				}
			}
		}

		public bool Update(IBaseProps p)
		{
			int rowsAffected = 0;
			CustomerProps props = (CustomerProps)p;

			DBCommand command = new DBCommand();
			command.CommandText = "usp_CustomerUpdate";
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.AddWithValue("custId", props.CustomerID);
			command.Parameters.AddWithValue("name_p", props.Name);
			command.Parameters.AddWithValue("address_p", props.Address);
			command.Parameters.AddWithValue("city_p", props.City);
			command.Parameters.AddWithValue("state_p", props.State);
			command.Parameters.AddWithValue("zipcode_p", props.ZipCode);
			command.Parameters.AddWithValue("conCurrId", props.ConcurrencyID);

			try
			{
				rowsAffected = RunNonQueryProcedure(command);
				Console.WriteLine(rowsAffected);
				if (rowsAffected == 1)
				{
					props.ConcurrencyID++;
					return true;
				}
				else
				{
					string message = "Record cannot be updated. It has been edited by another user.";
					throw new Exception(message);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}
			finally
			{
				if (mConnection.State == ConnectionState.Open)
				{
					mConnection.Close();
				}
			}
		}
	}
}
