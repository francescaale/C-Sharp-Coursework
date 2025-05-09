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
    public partial class Form3 : Form
    {
        private string currentUser;
        public Form3(string username)
        {
            InitializeComponent();
            currentUser = username; //store the current user
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Pass currentUser to Form4 when opening it
            Form4 form4 = new Form4(currentUser);
            form4.Show(); //show the login page
            Visible = false; //hides the form
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Pass currentUser to Form2 when opening it
            Form2 form2 = new Form2(currentUser);
            form2.Show(); //show login/signup page
            Visible = false; //hides the form
        }

        private void tableBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate(); //vaidate any changes made in the form before saving
            this.tableBindingSource.EndEdit();//end the current edit operation on the binding source
            this.tableAdapterManager.UpdateAll(this.database1DataSet);//update the dataset and save the changes to the database

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // This loads data into the 'database1DataSet.Table' table. I can move, or remove it, as needed.
            this.tableTableAdapter.Fill(this.database1DataSet.Table);
            textBox2.PasswordChar = '*'; // Hide password on form load
        }

        private void register_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;  // Retrieves the text entered in the textBox1 control for the username
            string password = HashPassword(textBox2.Text);  // Hashes the password entered in textBox2 using the HashPassword method

            // Insert the hashed password and username into the database
            this.tableTableAdapter.InsertQuery(username, password);  // Executes an SQL INSERT query to store the username and hashed password in the database
            this.tableTableAdapter.Fill(this.database1DataSet.Table);  // Reloads the table data into the dataset after insertion to ensure it's up-to-date
            this.tableTableAdapter.Update(this.database1DataSet.Table);  // Updates the database with any changes made to the dataset


            // Proceed to login form
            Form4 form4 = new Form4(currentUser); //create new form
            form4.Show();//show the login form
            this.Hide();//hide the form
        }

        // Hashing function
        private string HashPassword(string password) // code from to hash the password https://stackoverflow.com/questions/16999361/obtain-sha-256-string-of-a-string
        {
            using (SHA256 pass = SHA256.Create())  // Creates an instance of the SHA256 hashing algorithm
            {
                byte[] bytes = pass.ComputeHash(Encoding.UTF8.GetBytes(password));  // Converts the password string into bytes and computes its hash

                StringBuilder builder = new StringBuilder();  // Initializes a StringBuilder to construct the resulting hash as a string
                foreach (byte b in bytes)  // Loops through each byte in the computed hash
                {
                    builder.Append(b.ToString("x2"));  // Converts the byte to its hexadecimal representation and appends it to the StringBuilder
                }

                return builder.ToString();  // Returns the resulting hexadecimal string as the hashed password
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle password visibility / show password check
            textBox2.PasswordChar = checkBox1.Checked ? '\0' : '*'; //show *
        }
    }
}
