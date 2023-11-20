using Efolio.Models;
using EFolio1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;


namespace EFolio1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(Contacts User)
        {

            string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=EFolio;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Contact (name, email, phone, subject, message, datecreated) VALUES (@name, @email, @phone, @subject, @message, @datecreated)";

                    SqlCommand command = new SqlCommand(query, connection);

                    // Replace parameters with actual values from the User object
                    command.Parameters.AddWithValue("@name", User.name);
                    command.Parameters.AddWithValue("@email", User.email);
                    command.Parameters.AddWithValue("@phone", User.phone);
                    command.Parameters.AddWithValue("@subject", User.subject);
                    command.Parameters.AddWithValue("@message", User.message);
                command.Parameters.AddWithValue("@datecreated", User.datecreated);

                try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();

                     return View();
;                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        // Handle the exception or provide feedback to the user
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            return View();
        }

        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync();
            
            return RedirectToAction("SignIn", "Start");
        }

        public IActionResult Contact()
        {
            string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=EFolio;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

            SqlConnection connection = new SqlConnection(connectionString);
            
                string query = " select userid ,name,email,phone,subject,message from Contact;";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                List<Contacts> contacts = new List<Contacts>();

                while (reader.Read())
            {
                Contacts contact = new Contacts();
                contact.userid = reader["userid"].ToString();
                contact.name = reader["name"].ToString();
                contact.email = reader["email"].ToString();
                contact.phone = reader["phone"].ToString();
                contact.subject = reader["subject"].ToString();
                contact.message = reader["message"].ToString();
                //contact.datecreated = reader["datecreated"].ToString("yyyy-MM-dd HH:mm:ss");

                contacts.Add(contact);
            }
            connection.Close();
                
            return View(contacts);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
