using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace HelloExpert.Pages
{
    public class CreateModel : PageModel
    {
        public ExpertInfo expertInfo = new();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            expertInfo.name = Request.Form["name"];
            expertInfo.email = Request.Form["email"];
            expertInfo.phone = Request.Form["phone"];
            expertInfo.tag = Request.Form["tag"];

            if (expertInfo.name.Length == 0 || expertInfo.email.Length == 0 ||
                expertInfo.phone.Length == 0 || expertInfo.tag.Length == 0)
            {
                errorMessage = "All velden zijn verplicht!";
                return;
            }

            // Bewaar nieuw expert in database
            try
            {
                string connectionString = "Data Source=vminthecloud\\sqlexpress;Initial Catalog=helloexpert;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO experts " +
                        "(name, email, phone, tag) VALUES " +
                        "(@name, @email, @phone, @tag);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", expertInfo.name);
                        command.Parameters.AddWithValue("@email", expertInfo.email);
                        command.Parameters.AddWithValue("@phone", expertInfo.phone);
                        command.Parameters.AddWithValue("@tag", expertInfo.tag);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            // clear alle velden van expert object en toon succesbericht
            expertInfo.name = ""; expertInfo.email = ""; expertInfo.phone = ""; expertInfo.tag = "";
            successMessage = "Nieuw Expert toegevoegd!";

            Response.Redirect("/Index");
        }
    }
}
