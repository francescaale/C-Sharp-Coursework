using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cartoon_Coursework
{
    public partial class Form2 : Form
    {
        private string currentUser; //make currentuser a string
        public Form2(string username)
        {
            InitializeComponent();
            currentUser = username; //store the currentuser
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Pass currentUser to Form3 when opening it
            Form3 form3 = new Form3(currentUser); 
            form3.Show(); //show the register form
            Visible = false;//hides the form
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            // Pass currentUser to Form4 when opening it
            Form4 form4 = new Form4(currentUser);
            form4.Show(); //show the login form
            Visible = false; //hides the form
        }
    }
}
