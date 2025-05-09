using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Cartoon_Coursework
{
    internal static class Program
    {
        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Prompt for username
            string currentUser = PromptForUsername();

            // Start the application with the Start form
            Application.Run(new Start(currentUser));
        }
        private static string PromptForUsername()
        {
            // Create a simple input dialog to get the username
            string username = "";
            return username;
        }
    }
}
