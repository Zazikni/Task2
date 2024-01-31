using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
  
    internal class Person : SocietyElement
    {

        private int age;

        public int Age { get; }
        public override string Name { get; }
        public string CurrentLocation { get; set; }
        public Person(string name, int age, string location)
        { 
            Age = age;
            CurrentLocation = location;
            Name = name;
        }

    }
    
}
