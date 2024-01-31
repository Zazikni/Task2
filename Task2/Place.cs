using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    internal class Place: INameable
    {
        private string name;
        public string Name { get { return name; } }
        public Place(string name)
        {
            this.name = name;

        }
    }
}
