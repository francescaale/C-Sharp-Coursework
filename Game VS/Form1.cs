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
    public partial class Start : Form
    {
        private string currentUser;
        public Start(string username)
        {
            InitializeComponent();
            currentUser = username; //store the currentuser
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Pass currentUser to Form2 when opening it
            Form2 form2 = new Form2(currentUser);
            form2.Show(); //show signup/login page
            Visible = false; //hides the form
        }
    }
}
