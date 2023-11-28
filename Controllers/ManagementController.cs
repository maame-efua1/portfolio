using Efolio.Models;
using EFolio1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EFolio1.Controllers
{
    public class ManagementController : Controller
    {
        public IActionResult Index(Admin Admin)
        {
            
            {
                string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=EFolio;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM Admins WHERE username = @username AND password = @password";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", Admin.username);
                    command.Parameters.AddWithValue("@password", Admin.password);

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
        }


        public IActionResult Dashboard()
        {
            HttpContext.Session.SetString("name", "Eugenia");

            return View();
        }

        public IActionResult Contact()
        {
            var name = HttpContext.Session.GetString("name");

            if (name == null) return RedirectToAction("Index");

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

            HttpContext.Session.GetString("name");

            return RedirectToAction("Index", "Management");
        }

        public IActionResult Edit(int userId)
        {
            string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=EFolio;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                try
                {
                    string query = $"Select * from Registration where userid={userId}";


                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();


                    reader.Read();
                    var userid = reader["userid"].ToString();
                    var firstname = reader["firstname"].ToString();
                    var lastname = reader["lastname"].ToString();
                    var username = reader["username"].ToString();

                    var user = new SignUp()
                    {
                        userid = userid,
                        firstname = firstname,
                        lastname = lastname,
                        username = username

                    };

                    return View(user);
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


        [HttpPost]
        public IActionResult Edit(int userId, SignUp User)
        {
            
            string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=EFolio;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"UPDATE Registration SET firstname=@firstname,lastname=@lastname,username=@username WHERE UserId = {userId};";

                
                SqlCommand command = new SqlCommand(query, connection);


                command.Parameters.AddWithValue("@firstname", User.firstname);
                command.Parameters.AddWithValue("@lastname", User.lastname);
                command.Parameters.AddWithValue("@username", User.username);
                command.Parameters.AddWithValue("@userid", userId);
                command.Parameters.AddWithValue("@datecreated", User.datecreated);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    return RedirectToAction("Users", "Management");
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

        

        public IActionResult Delete(string userId, SignUp User)
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