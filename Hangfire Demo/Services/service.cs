using System.Data.SqlClient;
using System.Data;
using Hangfire_Demo.Models;

namespace Hangfire_Demo.Services
{
    public class service : Iservice
    {
        private readonly IConfiguration _configuration;
        public service(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendEmail()
        {
            Console.WriteLine($"SendEmail :Sending email is in process..{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public DataTable SyncData()
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
            SqlConnection con = new SqlConnection(connection);
            var query = "SELECT * FROM PRODUCT";
            con.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            adapter.Fill(ds);

            Console.WriteLine($"SyncData :sync is going on..{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            return ds.Tables[0];
        }

        public void InsertRecords(product product)
        {
            try
            {
                string connection = _configuration.GetValue<string>("ConnectionStrings:DefaultConnection");
                var query = "INSERT INTO PILOT (id,name,price) VALUES(@id,@name,@price)";
                SqlConnection con = new SqlConnection(connection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("id", product.id);
                    cmd.Parameters.AddWithValue("name", product.name);
                    cmd.Parameters.AddWithValue("price", product.price);
                   
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
            }
            Console.WriteLine($"UpdatedDatabase :Updating the database is in process..{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        public List<product> GetAllRecords()
        {
            DataTable pilots = SyncData();
            return (from DataRow dr in pilots.Rows
                    select new product()
                    {
                        id = Convert.ToInt32(dr["Id"]),
                        name = Convert.ToString (dr["Name"]),
                        price = Convert.ToDouble (dr["price"]),
                        
                    }).ToList();


        }
    }
}
