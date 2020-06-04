using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAzureSQL.Model
{
    public class DriverModel
    {
        string ConnectionString = "Server=tcp:azuredatabasesqlserver.database.windows.net,1433;Initial Catalog=AzureSqlDatabase;Persist Security Info=False;User ID={ your userID };Password={ your password };MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public int IDDriver { get; set; }

        public string Name { get; set; }

        public string Picture { get; set; }

        public string Status { get; set; }

        public PositionModel ActualPosition { get; set; }

        public List<DriverModel> GetAll()
        {
            List<DriverModel> list = new List<DriverModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "SELECT * FROM Driver INNER JOIN Position ON Driver.IDActualPosition = Position.IDPosition";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new DriverModel
                                {
                                    IDDriver = (int)reader["IDDriver"],
                                    Name = reader["Name"].ToString(),
                                    Status = reader["Status"].ToString(),
                                    Picture = reader["Picture"].ToString(),
                                    ActualPosition = new PositionModel
                                    {
                                        Latitude = reader["Latitude"].ToString(),
                                        Longitude = reader["Longitude"].ToString()
                                    }
                                });
                            }
                        }
                    }
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
