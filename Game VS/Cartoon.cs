using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartoon_Coursework
{
    internal class Cartoon //define the class cartoon https://www.w3schools.com/cs/cs_properties.php
    {
        public int Id { get; set; }  // hold the ID of the cartoon
        public string CartoonName { get; set; }  //  hold the name of the cartoon
        public string Hint1 { get; set; }  //  for the first hint of the cartoon
        public string Hint2 { get; set; }  // for the second hint of the cartoon
        public string Hint3 { get; set; }  //  for the third hint of the cartoon
        public string Image { get; set; }  // hold the image path of the cartoon

        // initialize the properties with values
        public Cartoon(int id, string name, string hint1, string hint2, string hint3, string image)
        {
            Id = id;  // Set the cartoon's ID
            CartoonName = name;  // Set the cartoon's name
            Hint1 = hint1;  // Set the first hint
            Hint2 = hint2;  // Set the second hint
            Hint3 = hint3;  // Set the third hint
            Image = image;  // Set the image file path or name
        }
    }
}
