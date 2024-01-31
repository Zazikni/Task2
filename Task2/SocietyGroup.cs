using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    internal class SocietyGroup: SocietyElement
    {
        public override string Name { get;}
        public SocietyGroup(params Person[] members ) 
        {
            foreach( Person member in members )
            {
                if (Name == null)
                { 
                    Name = member.Name;
                }
                else
                {
                    Name += $" и {member.Name}";
                }
                
            }
            

        }
    }
}
