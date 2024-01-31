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
        public SocietyElement? Subject {  get; }
        public Storage? UploadFrom {  get; }
        public Storage? UploadTo {  get; }
        public IItem? Receive {  get; }
        public Place? GoFrom {  get; }
        public Place? GoTo { get; }
        public string Time { get; }
        public SocietyElement Object {  get; }
        public Actions Act { get; }
        public Action(SocietyElement obj, Actions action, Storage? uploadTo = null, Storage? uploadFrom = null, string time = "", Place? goFrom = null, Place? goTo = null, IItem? receive = null) 
        {
            Act = action;
            Object = obj;
            Time = time;
            UploadFrom = uploadFrom;
            UploadTo = uploadTo;
            Receive = receive;
            GoFrom = goTo; 
            GoTo = goFrom;

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
            string result = $"{Subject.Name} ждал {Object.Name} {Time}";
            return result;
        }
        private string Upload ()
        {
            string result = $"{Subject.Name} выложил {Object.Name} {Time}";
            return result;
        }

    }
}
