using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace HelloExpert.Pages.Experts
{
    public class IndexModel : PageModel
    {
        public List<ExpertInfo> listExperts = new List<ExpertInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=vminthecloud\\sqlexpress;Initial Catalog=helloexpert;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM experts";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ExpertInfo expertInfo = new ExpertInfo();

                                expertInfo.id = "" + reader.GetInt32(0);
                                expertInfo.name = reader.GetString(1);
                                expertInfo.email = reader.GetString(2);
                                expertInfo.phone = reader.GetString(3);
                                expertInfo.tag = reader.GetString(4);
                                expertInfo.created_at = reader.GetDateTime(5).ToString();

                                listExperts.Add(expertInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class ExpertInfo
    {
        public String id;
        public String name;
        public String email;
        public String phone;
        public String tag;
        public String created_at;
    }
}
