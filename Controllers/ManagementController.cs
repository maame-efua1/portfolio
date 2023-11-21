using Efolio.Models;
using EFolio1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EFolio1.Controllers
{
    public class ManagementController : Controller
    {
        public IActionResult Index(Admin Admins)
        {
            string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=EFolio;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Admins WHERE username = @username AND password = @password;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", Admins.username);
                command.Parameters.AddWithValue("@password", Admins.password);

                try
                {
                    connection.Open();
                    int userCount = (int)command.ExecuteScalar();

                    if (userCount > 0)
                    {
                        // Login successful, redirect to another view
                        return RedirectToAction("Dashboard", "Management");
                    }
                    else
                    {
                        // Login failed, display a message or redirect to login page with an error
                        TempData["ErrorMessage"] = "Invalid username or password.";
                        return RedirectToAction("Index", "Management");
                    }
                }
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

        public IActionResult UserCount()
        {
            string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=EFolio;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Registration;";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                int userCount = (int)command.ExecuteScalar();
                connection.Close();

                ViewBag.UserCount = userCount; 

                return View();
            }
        }


        public IActionResult Dashboard()
        {
            return View();
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

        public IActionResult Users()
        {
            string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=EFolio;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

            SqlConnection connection = new SqlConnection(connectionString);

            string query = " select userid ,firstname,lastname,username from Registration;";

            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            List<SignUp> signups = new List<SignUp>();

            while (reader.Read())
            {
                SignUp signup = new SignUp();
                signup.userid = reader["userid"].ToString();
                signup.firstname = reader["firstname"].ToString();
                signup.lastname = reader["lastname"].ToString();
                signup.username = reader["username"].ToString();
                //signup.datecreated = reader["datecreated"].ToString("yyyy-MM-dd HH:mm:ss");

                signups.Add(signup);
            }
            connection.Close();

            return View(signups);
        }

        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Management");
        }

        public IActionResult Update()
        {
            
            return View();
        }
        public IActionResult Edit(int userId, SignUp User)
        {
            string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=EFolio;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"UPDATE Registration SET firstname=@firstname,lastname=@lastname,username=@username WHERE UserId = {userId};";

                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);


                command.Parameters.AddWithValue("@firstname", User.firstname);
                command.Parameters.AddWithValue("@lastname", User.lastname);
                command.Parameters.AddWithValue("@username", User.username);
                command.Parameters.AddWithValue("@userid", userId);
                command.Parameters.AddWithValue("@datecreated", User.datecreated);
                

                command.ExecuteNonQuery();
                connection.Close();

                return RedirectToAction("Users");
        }
        }

        public IActionResult CEdit()
        {
            return View();
        }

        public IActionResult Delete(string userId)
        {
            string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=EFolio;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

            SqlConnection connection = new SqlConnection(connectionString);

            string query = $"delete from registration where userid={userId};";

            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();

            return RedirectToAction("Users");
        }

        public IActionResult CDelete(string userId)
        {
            string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=EFolio;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

            SqlConnection connection = new SqlConnection(connectionString);

            string query = $"delete from Contact where userid={userId};";

            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();

            return RedirectToAction("Contact");
        }
    }
}