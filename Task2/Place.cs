using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    internal record class Place: INameable
    {
        public string Name { get; }
        public Place(string name)
        {
            Name = name;

        }
    }
}
