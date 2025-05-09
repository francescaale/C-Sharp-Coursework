using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Cartoon_Coursework
{
    public partial class Form4 : Form
    {

        private string currentUser;
        public Form4(string username)
        {
            InitializeComponent();
            currentUser = username;  // Store the username
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Pass currentUser to Form2 when opening it
            Form2 form2 = new Form2(currentUser); 
            form2.Show(); //show the login/sign up page
            Visible = false; //hides the form
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Pass currentUser to Form5 when opening it
            Form5 form5 = new Form5(currentUser); 
            form5.Show(); // //show the welcome message form
            Visible = false; // hides the form
        }

        private void tableBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate(); //vaidate any changes made in the form before saving
            this.tableBindingSource.EndEdit(); //end the current edit operation on the binding source
            this.tableAdapterManager.UpdateAll(this.database1DataSet); //update the dataset and save the changes to the database
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            this.tableTableAdapter.Fill(this.database1DataSet.Table); //fill the database
            textBox2.PasswordChar = '*'; // Hide password on form load
            textBox1.PasswordChar = '\0'; // Ensure username is visible (default)

        }

        public static string LoggedInUser; // Store the logged-in user 


        private void logIn_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;  // Retrieve the username entered in the textbox
            string hashedPassword = HashPassword(textBox2.Text);  // Hash the password entered in the textbox

            // Perform a database query to check if the username and hashed password exist in the database
            int count = (int)this.tableTableAdapter.ScalarQuery(username, hashedPassword);

            // If the query returns a count greater than 0, login is successful
            if (count > 0)
            {
                Form6 form6 = new Form6(username);  // Create a new instance of Form6 and pass the username
                form6.Show();  // Show the Form6
                this.Hide();  // Hide the current form (login form)
            }
            else
            {
                // Show an error message if the username or password is invalid
                MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // Hashing function
        private string HashPassword(string password) //https://stackoverflow.com/questions/16999361/obtain-sha-256-string-of-a-string
        {
            using (SHA256 sha256 = SHA256.Create())  // Create an instance of the SHA256 algorithm to hash the password
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));  // Convert the password to bytes and compute the hash

                StringBuilder builder = new StringBuilder();  // Create a StringBuilder to build the final hashed password string
                foreach (byte b in bytes)  // go through each byte in the hash result
                {
                    builder.Append(b.ToString("x2"));  // Convert each byte to a 2-digit hexadecimal representation and append it to the builder
                }

                return builder.ToString();  // Return the hexadecimal string of the hashed password
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle password visibility
            textBox2.PasswordChar = checkBox1.Checked ? '\0' : '*'; //should show *
        }
    }
}
