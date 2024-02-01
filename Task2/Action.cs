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
        Fold,
        Leave,
        Sell,
        Go
    }
    internal class Action
    {
        public SocietyElement? Subject { get; set; }
        public Storage? UploadFrom {  get; }
        public Storage? UploadTo {  get; }
        public IItem? Receive {  get; }
        public Place? GoFrom {  get; }
        public Place? GoTo { get; }
        public string Time { get; }
        public INameable? Object {  get; }
        public Actions? Act { get; }
        public Action( Actions? action = null, INameable obj = null, Storage? uploadTo = null, Storage? uploadFrom = null, string time = "", Place? goFrom = null, Place? goTo = null, IItem? receive = null, SocietyElement? sbj = null) 
        {
            Act = action;
            Object = obj;
            Subject = sbj;
            Time = time;
            UploadFrom = uploadFrom;
            UploadTo = uploadTo;
            Receive = receive;
            GoFrom = goFrom; 
            GoTo = goTo;

        }
        public string Do()
        {
            string result = "";
            switch (Act)
            {
                case Actions.Wait:
                    {
                        result = _Wait();
                    }
                    break;
                case Actions.Upload:
                    {
                        result = _Upload();
                    }
                    break;
                case Actions.Receive:
                    {
                        result = _Receive();
                    }
                    break;
                case Actions.Leave:
                    {
                        result = _Leave();
                    }
                    break;
                case Actions.Exchange:
                    {
                        result = _Exchange();
                    }
                    break;
                case Actions.Sell:
                    {
                        result = _Sell();
                    }
                    break;
                case Actions.Go:
                    {
                        result = _Go();
                    }
                    break;

            }
            return result;
        }

        private string _Exchange()
        {
            string result = $"{Subject.Name} обменивал {Object.Name}";
            return result;
        }
        private string _Sell()
        {
            string result = $"{Subject.Name} продавал {Object.Name}{(Time != null ? " " + Time : "")}";
            return result;
        }

        private string _Leave()
        {
            string result = $"{Subject.Name} ушел{(GoFrom != null? " из " + GoFrom.Name:"")}";
            return result;
        }
        private string _Go()
        {
            string result = $"{Subject.Name} ездил{(GoTo != null ? " в " + GoTo.Name : "")}";
            return result;
        }

        private string _Receive()
        {
            string result = $"{Subject.Name} получил {Object.Name}";
            return result;
        }

        private string _Wait()
        {
            string result = $"{Subject.Name} ждал {Object.Name} {Time}";
            return result;
        }
        private string _Upload ()
        {
            string result = $"{Subject.Name} выложил {Object.Name} из {UploadFrom.Name}";
            return result;
        }

    }
}
