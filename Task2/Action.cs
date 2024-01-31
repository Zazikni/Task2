using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Task2

{
    enum Actions
    {
        Wait,
        Upload,
        Receive,
        Purchase,
        Exchange,
        Fold
    }
    internal class Action
    {
        public SocietyElement? Subject {  get; set; }
        public SocietyElement Object {  get;}
        public Actions Act { get;}
        public Action(SocietyElement obj, Actions action) 
        {
            Act = action;
            Object = obj;
        }
        public string Do()
        {
            string result = "";
            switch (Act)
            {
                case Actions.Wait:
                    {
                        result = Wait();
                    }
                    break;
            }
            return result;
        }

        private string Wait()
        {
            string result = "";
            Console.WriteLine(Object.GetType);
            return result;
        }

    }
}
