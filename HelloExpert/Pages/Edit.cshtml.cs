using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace HelloExpert.Pages
{
    public class EditModel : PageModel
    {
        public ExpertInfo expertInfo = new ExpertInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            string id = Request.Query["id"];

            try
            {
                string connectionString = "Data Source=vminthecloud\\sqlexpress;Initial Catalog=helloexpert;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM experts WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                expertInfo.id = "" + reader.GetInt32(0);
                                expertInfo.name = reader.GetString(1);
                                expertInfo.email = reader.GetString(2);
                                expertInfo.phone = reader.GetString(3);
                                expertInfo.tag = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            expertInfo.id = Request.Form["id"];
            expertInfo.name = Request.Form["name"];
            expertInfo.email = Request.Form["email"];
            expertInfo.phone = Request.Form["phone"];
            expertInfo.tag = Request.Form["tag"];

            if (expertInfo.id.Length == 0 || expertInfo.name.Length == 0 ||
                expertInfo.email.Length == 0 || expertInfo.phone.Length == 0 || expertInfo.tag.Length == 0)
            {
                errorMessage = "Alle velden zijn verplicht!";
                return;
            }

            try
            {
                string connectionString = "Data Source=vminthecloud\\sqlexpress;Initial Catalog=helloexpert;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE experts " +
                        "SET name=@name, email=@email, phone=@phone, tag=@tag" +
                        " WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", expertInfo.name);
                        command.Parameters.AddWithValue("@email", expertInfo.email);
                        command.Parameters.AddWithValue("@phone", expertInfo.phone);
                        command.Parameters.AddWithValue("@tag", expertInfo.tag);
                        command.Parameters.AddWithValue("@id", expertInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/");
        }
    }
}
