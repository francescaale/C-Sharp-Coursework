using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection;


namespace Cartoon_Coursework
{
    public partial class Form6 : Form
    {

        private List<Cartoon> cartoons = new List<Cartoon>(); // List to store cartoon objects loaded from the database
        private int currentIndex = 0; // Index to track the current cartoon being displayed
        private int guessCount = 0; // Counter for incorrect guesses before revealing the correct answer
        private string currentUser; // Stores the username of the currently logged-in user
        private string csvFilePath; // Path to the CSV file where user answers are saved

        public Form6(string username)
        {
            InitializeComponent();
            currentUser = username; // Store the logged-in user
            // Define the path to the debug folder where the CSV file is stored
            string debugFolderPath = AppDomain.CurrentDomain.BaseDirectory; //"Peter Blanchfield code"
            csvFilePath = Path.Combine(debugFolderPath, "results.csv"); // Set the full path to the results CSV file "Peter Blanchfield code"
            LoadCartoons(); // Load the cartoons from the database
            DisplayCartoon(); // Display the first cartoon in the list
        }


        private void LoadCartoons()
        {
            cartoons.Clear(); // Clear the existing list to avoid duplicates when reloading

            // Define the connection string for the local database
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database2.mdf;Integrated Security=True;";

            using (SqlConnection conn = new SqlConnection(connectionString)) // Create a new database connection https://stackoverflow.com/questions/4717789/in-a-using-block-is-a-sqlconnection-closed-on-return-or-exception
            {
                conn.Open(); // Open the connection to the database

                string query = "SELECT * FROM [Table]"; // SQL query to select all records from the table

                using (SqlCommand cmd = new SqlCommand(query, conn)) // Create a command with the SQL query
                using (SqlDataReader reader = cmd.ExecuteReader()) // Execute the query and retrieve the results
                {
                    while (reader.Read()) // Loop through each row in the result set
                    {
                        cartoons.Add(new Cartoon( // Create a new Cartoon object and add it to the list
                            reader.GetInt32(0),   // Read the first column (ID)
                            reader.GetString(1),  // Read the second column (Cartoon Name)
                            reader.GetString(2),  // Read the third column (Hint 1)
                            reader.GetString(3),  // Read the fourth column (Hint 2)
                            reader.GetString(4),  // Read the fifth column (Hint 3)
                            reader.GetString(5)   // Read the sixth column (Image Path)
                        ));
                    }
                }
            }
        }

        private void DisplayCartoon()
        {
            if (currentIndex < cartoons.Count) // Check if there are more cartoons to display
            {
                Cartoon cartoon = cartoons[currentIndex]; // Get the current cartoon
                labelHint.Text = cartoon.Hint1; // Display the first hint
                textBoxAnswer.Text = ""; // Clear the answer input field
                labelFeedback.Text = ""; // Clear feedback label
                button1.Visible = true; // Show the submit button
                pictureBox1.Visible = false; // Hide the skip button initially

                // Extract the image file name from the cartoon object
                string imageFileName = Path.GetFileName(cartoon.Image);

                // Get the project directory and construct the image path
                string projectDirectory = AppDomain.CurrentDomain.BaseDirectory; //https://stackoverflow.com/questions/14549766/how-to-get-my-project-path
                string imagePath = Path.Combine(projectDirectory, @"..\..\images", imageFileName);

                // Check if the image file exists before displaying
                if (File.Exists(imagePath))
                {
                    pictureBox.Image = Image.FromFile(imagePath); // Load the image into the PictureBox
                }
                else
                {
                    // Show an error message if the image is not found
                    MessageBox.Show("Image not found: " + imagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // If no more cartoons are left
            {
                labelFeedback.Text = "You've completed all \n the cartoons!"; // Display completion message
                labelFeedback.ForeColor = Color.Blue; // Change text color to blue
                button1.Visible = false; // Hide the submit button
                pictureBox1.Visible = false; // Hide the skip button
                ShowUserAnswers(); // Show the user's answers at the end
            }
        }


        private void ShowUserAnswers()
        {
            // Create a StringBuilder to store user answers  
            StringBuilder userAnswers = new StringBuilder(); //https://stackoverflow.com/questions/15520744/how-to-create-array-of-stringbuilder-initialized-with
            userAnswers.AppendLine("These are your answers:"); // Add a title to the answers list https://stackoverflow.com/questions/11155801/why-does-stringbuilder-appendline-not-add-a-new-line-with-some-strings

            int correctCount = 0;  // Initialize counter for correct answers
            int wrongCount = 0;    // Initialize counter for wrong answers

            try
            {
                // Check if the CSV file exists before attempting to read it  
                if (!File.Exists(csvFilePath))
                {
                    MessageBox.Show("The answers file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if the file does not exist  
                }

                // Open the CSV file for reading  
                using (StreamReader sr = new StreamReader(csvFilePath))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null) // Read each line from the file  
                    {
                        if (line.StartsWith("Username")) // Skip the header line  
                            continue;

                        string[] parts = line.Split(','); // Split the line by commas  

                        // Check if the line has at least three parts and matches the current user  
                        if (parts.Length >= 3 && parts[0].Trim() == currentUser)
                        {
                            // Append each answer and its result
                            userAnswers.AppendLine($"Answer: {parts[1]}, Result: {parts[2]}");

                            // Increment the correct or wrong counter based on the result
                            if (parts[2].Trim() == "Correct")
                            {
                                correctCount++;
                            }
                            else if (parts[2].Trim() == "Wrong")
                            {
                                wrongCount++;
                            }
                        }
                    }
                }

                // Check if any answers were found for the user  
                if (userAnswers.Length > "These are your answers:".Length)
                {
                    // Append the correct and wrong answer counts to the message
                    userAnswers.AppendLine($"Correct Answers: {correctCount}");
                    userAnswers.AppendLine($"Wrong Answers: {wrongCount}");

                    // Display answers with counts of correct and wrong answers  
                    MessageBox.Show(userAnswers.ToString(), "Your Answers", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No answers found for the current user.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); // Inform user if no answers were found  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading answers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Display an error if reading fails  
            }
        }



        private async void button1_Click(object sender, EventArgs e)
        {
            if (currentIndex < cartoons.Count)  // Check if there are cartoons left to display
            {
                Cartoon cartoon = cartoons[currentIndex];  // Get the current cartoon object

                string userAnswer = textBoxAnswer.Text.Trim();  // Get the user's answer and trim any extra spaces from the input
                string correctAnswer = cartoon.CartoonName.Trim();  // Get the correct cartoon name and trim any extra spaces

                bool isCorrect = userAnswer.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase);  // Check if the user's answer matches the correct answer (case-insensitive) https://learn.microsoft.com/en-us/dotnet/api/system.stringcomparer.ordinalignorecase?view=net-9.0s

                if (isCorrect)  // If the answer is correct
                {
                    labelFeedback.Text = "Correct! Yay! 🎉";  // Display correct message
                    labelFeedback.ForeColor = Color.Green;  // Change the label text color to green
                    labelFeedback.Visible = true;  // Make sure the label is visible
                    await Task.Delay(1000);  // Wait for 1 second before proceeding https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.delay?view=net-9.0s

                    SaveAnswerToCsv(currentUser, userAnswer, true);  // Save the user's answer to the CSV file as correct

                    currentIndex++;  // Move to the next cartoon
                    guessCount = 0;  // Reset the guess count
                    DisplayCartoon();  // Show the next cartoon
                }
                else
                {
                    guessCount++;  // Increase the guess count after a wrong answer
                    if (guessCount == 1)  // If it's the first incorrect guess
                    {
                        labelHint.Text = cartoon.Hint2;  // Show the second hint
                    }
                    else if (guessCount == 2)  // If it's the second incorrect guess
                    {
                        labelHint.Text = cartoon.Hint3;  // Show the third hint
                    }
                    else  // If there have been three wrong guesses
                    {
                        labelFeedback.Text = $"Wrong! \n The correct answer was:\n {cartoon.CartoonName}";  // Display the correct answer
                        labelFeedback.ForeColor = Color.Red;  // Change the label text color to red
                        labelFeedback.Visible = true;  // Make sure the label is visible
                        SaveAnswerToCsv(currentUser, userAnswer, false);  // Save the user's answer to the CSV file as incorrect
                        button1.Visible = false;  // Hide the submit button
                        pictureBox1.Visible = true;  // Show the skip button
                    }
                }
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            currentIndex++;  // Skip to the next cartoon
            guessCount = 0;  // Reset the guess count
            pictureBox1.Visible = false;  // Hide skip button
            button1.Visible = true;  // Show submit button again
            DisplayCartoon();  // Show the next cartoon
        }


        private void SaveAnswerToCsv(string username, string userAnswer, bool isCorrect) //ChatGbt Prompt - how to make the answers of the users save to the result.csv file into the debug folder? :
        {
            string debugFolderPath = AppDomain.CurrentDomain.BaseDirectory;  // Get the base directory of the application
            string csvFilePath = Path.Combine(debugFolderPath, "results.csv");  // Combine the base directory with the "results.csv" filename to get the full file path

            try
            {
                // Ensure the directory exists
                string directoryPath = Path.GetDirectoryName(csvFilePath);  // Get the directory part of the CSV file path
                if (!Directory.Exists(directoryPath))  // Check if the directory does not exist
                {
                    Directory.CreateDirectory(directoryPath);  // Create the directory if it doesn't exist
                }

                using (StreamWriter sw = new StreamWriter(csvFilePath, true))  // Open the CSV file for writing (append mode)
                {
                    if (new FileInfo(csvFilePath).Length == 0)  // Check if the file is empty (size == 0)
                    {
                        sw.WriteLine("Username,Answer,Result");  // Write the header line if the file is empty
                    }

                    string result = isCorrect ? "Correct" : "Wrong";  // Determine if the answer is correct or wrong
                    sw.WriteLine($"{username},{userAnswer},{result}");  // Write the username, answer, and result to the file
                }
            }
            catch (Exception ex)  // If an error occurs during file operations
            {
                MessageBox.Show("Error saving to CSV: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);  // Show an error message
            }
        }
    }
}
