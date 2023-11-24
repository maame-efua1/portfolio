
using EFolio1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


namespace EFolio1.Controllers
{
    public class StartController : Controller
    {
        private const string firstname = "";

        public IActionResult SignIn(SignUp User)
        {
            {
                string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=EFolio;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM Registration WHERE username = @username AND password = @password";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", User.username);
                    command.Parameters.AddWithValue("@password", User.password);

                    try
                    {
                        connection.Open();
                        int userCount = (int)command.ExecuteScalar();

                        if (userCount > 0)
                        {
                            // Login successful, redirect to another view
                            return RedirectToAction("Index","Home");
                        }
                        else
                        {
                            // Login failed, display a message or redirect to login page with an error
                            TempData["ErrorMessage"] = "Invalid username or password.";
                            return RedirectToAction("SignIn", "Start");
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

        

        public IActionResult SignUp(SignUp User)
        {
            HttpContext.Session.SetString(firstname, "");

            string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=EFolio;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";
             
            if (User.password == User.confirmpassword)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Registration (firstname, lastname, username, password, datecreated) VALUES (@firstname, @lastname, @username, @password, @datecreated)";

                    SqlCommand command = new SqlCommand(query, connection);

                    // Replace parameters with actual values from the User object
                    command.Parameters.AddWithValue("@firstname", User.firstname);
                    command.Parameters.AddWithValue("@lastname", User.lastname);
                    command.Parameters.AddWithValue("@username", User.username);
                    command.Parameters.AddWithValue("@password", User.password);
                    command.Parameters.AddWithValue("@datecreated", User.datecreated);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        
                        return RedirectToAction("SignIn","Start");
                        
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error: " + User.errormessage);
                        // Handle the exception or provide feedback to the user
                    }
                    finally
                    {
                        connection.Close();
                    }
                    
                }
            }
            else
            {
                // Handle case where passwords don't match
                Console.WriteLine("Passwords do not match");
            }
            Console.WriteLine(User.successmessage);
            return View();
        }

    }
}
           
 
    

