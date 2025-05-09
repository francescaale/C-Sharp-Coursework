using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Cartoon_Coursework
{
    internal class CartoonDatabase //define the class cartoondatabase
    {
        // Define the connection string to the database
        private string connectionString = @"C:\Users\franc\OneDrive\Documents\GitHub\Coursework-C\Cartoon Coursework\Database2.mdf";

        public List<Cartoon> LoadCartoons()  //  load cartoons from the database
        {
            List<Cartoon> cartoons = new List<Cartoon>();  // Create a new list to store the cartoons

            // Establish a connection to the database using the connection string
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();  // Open the connection to the database

                // Define the SQL query to select all records from the "Table" in the database
                string query = "SELECT * FROM Table";

                // Create a new SQL command using the connection and the query
                using (SqlCommand cmd = new SqlCommand(query, conn))
                // Execute the query and get a data reader to read the results
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Loop through each row in the result set
                    while (reader.Read())
                    {
                        // For each row, create a new Cartoon object with data from the database
                        cartoons.Add(new Cartoon(
                            reader.GetInt32(0),  // Get the ID from the first column (index 0)
                            reader.GetString(1), // Get the Cartoon Name from the second column (index 1)
                            reader.GetString(2), // Get Hint 1 from the third column (index 2)
                            reader.GetString(3), // Get Hint 2 from the fourth column (index 3)
                            reader.GetString(4), // Get Hint 3 from the fifth column (index 4)
                            reader.GetString(5)  // Get the Image Path from the sixth column (index 5)
                        ));
                    }
                }
            }

            // Return the list of cartoons that were loaded
            return cartoons;
        }

    }
}
