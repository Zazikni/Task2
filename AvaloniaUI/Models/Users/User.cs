using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaUI.Models.Users
{
    internal class User : NewUser
    {
        private int _id;

        int Id { get { return _id; } }
        public User(string name, string password, string login, int id) : base(name, password, login)
        {
            _id = id;

        }
    }
}
