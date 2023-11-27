using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Configuration;

namespace MMABooksTools
{
    class ConfigDB
    {
        public static string GetMySqlConnectionString()
        {
            string folder = System.AppContext.BaseDirectory;
            var builder = new ConfigurationBuilder()
                    .SetBasePath(folder)
                    .AddJsonFile("mySqlSettings.json", optional: false, reloadOnChange: true);

            string connectionString = builder.Build().GetConnectionString("mySql");

            return connectionString;
        }
    }
}
