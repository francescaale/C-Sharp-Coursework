using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Cartoon_Coursework
{
    public partial class Form5 : Form
    {
        private string currentUser; //make a string for the currentuser of the game
        public Form5(string username)
        {
            InitializeComponent();
            currentUser = username;  // Store the username
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Pass currentUser to Form6 when opening it
            Form6 form6 = new Form6(currentUser); 
            form6.Show(); //show the playing game
            this.Hide(); // Hide Form5
        }
    }
}
