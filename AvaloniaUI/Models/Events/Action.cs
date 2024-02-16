namespace Events

{
    #region enums
    enum Actions
    {
        Wait,
        Upload,
        Receive,
        Purchase,
        Exchange,
        Leave,
        Sell,
        Go,
        Put,
        WishBuy,
        AttractAttention,
        Notify,
        Stay,
        Sold,
        NotifyReason
    }
    #endregion
    /// <summary>
    /// Класс реализующий какое-либо действие.
    /// </summary>
    internal class Action
    {
        #region fields
        public SocietyElement? Subject { get; set; }
        public Storage? StorageFrom {  get; }
        public int? Amount {  get; }
        public Storage? StorageTo {  get; }
        public Storage? FromStorage {  get; }
        public IItem? Receive {  get; }
        public Place? GoFrom {  get; }
        public Place? Where {  get; }
        public string? Reason {  get; }
        public Place? GoTo { get; }
        public string Time { get; }
        public INameable? Object {  get; }
        public IItem? ItemObject {  get; }
        public Actions? Act { get; }
        private bool _isGroup;
        #endregion
        #region constructors
        public Action(Actions? action = null, INameable obj = null, IItem itemobj = null, string? reason = null, int? amount = null, Storage? storageTo = null, Storage? storageFrom = null, string time = "", Place? goFrom = null, Place? where = null, Place? goTo = null, IItem? receive = null, SocietyElement? sbj = null) 
        {
            Act = action;
            Where = where;
            Reason = reason;
            Object = obj;
            Subject = sbj;
            Time = time;
            StorageFrom = storageFrom;
            StorageTo = storageTo;
            Receive = receive;
            GoFrom = goFrom; 
            GoTo = goTo;
            ItemObject = itemobj;
            Amount = amount;
        }
        #endregion
        #region methods
        /// <summary>
        /// Метод запускающий действие.
        /// </summary>
        public string Do()
        {
           if (Subject != null)
            {
                _isGroup = typeof(SocietyGroup) == Subject.GetType();
            }

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
                case Actions.Put:
                    {
                        result = _Put();
                    }
                    break;
                case Actions.WishBuy:
                    {
                        result = _WishBuy();
                    }
                    break;
                case Actions.AttractAttention:
                    {
                        result = _AttractAttention();
                    }
                    break;
                case Actions.Notify:
                    {
                        result = _Notify();
                    }
                    break;
                case Actions.NotifyReason:
                    {
                        result = _NotifyReason();
                    }
                    break;
                case Actions.Stay:
                    {
                        result = _Stay();
                    }
                    break;
                case Actions.Sold:
                    {
                        result = _Sold();
                    }
                    break;

            }
            return result;
        }

        private string _Stay()
        {
            string result = $"{Subject.Name} {(_isGroup ? "стояли" : "стоял")} на {Where.Name}";
            return result;
        }

        private string _NotifyReason()
        {
            string result = $"что {ItemObject.Name} принаджлежащие {ItemObject.Ovner.Name} {Reason}";
            return result;
        }

        private string _Notify()
        {
            string result = $"{Subject.Name}{(Where != null? " в "+Where.Name:"")} стало известно";
            return result;
        }

        private string _AttractAttention()
        {
            string result = $"{(Subject != null? Subject.Name:"Кто-то")} {(_isGroup ? "привлекли" : "привлек")} внимание {Object.Name}";
            return result;
        }

        private string _WishBuy()
        {
            string result = $"{Subject.Name} {(_isGroup ? "хотели" : "хотел")} купить {Object.Name}{(Reason != null ? ", " + Reason : "")}";
            return result;
        }

        private string _Put()
        {
            string result = $"{Subject.Name} {(_isGroup ? "сложили" : "сложил")} {Object.Name}{(StorageTo != null ? " в " + StorageTo.Name : "")}";
            return result;
        }

        private string _Exchange()
        {
            string result = $"{Subject.Name} {(_isGroup ? "обменивали" : "обменивал")} {Object.Name}";
            return result;
        }
        private string _Sell()
        {
            string result = $"{Subject.Name} {(_isGroup ? "продавали" : "продавал")} {Object.Name}{(Time != null ? " " + Time : "")}{(Reason!=null? " " + Reason:"")}";
            return result;
        }
        private string _Sold()
        {
            string result = $"{(Amount!=null?Amount:"")}{(Amount != null ? " " + Object.Name : Object.Name)}{(StorageFrom!=null?" из " + StorageFrom.Name+",":"")} были проданы";
            return result;
        }

        private string _Leave()
        {
            string result = $"{Subject.Name} ушел{(GoFrom != null? " из " + GoFrom.Name:"")}";
            return result;
        }
        private string _Go()
        {

            string result = $"{Subject.Name} {(_isGroup?"пришли":"пришел")}{(GoTo != null ? " в " + GoTo.Name : "")}{(Time != null?" " + Time:"")}";
            return result;
        }

        private string _Receive()
        {
            string result = $"{Subject.Name} получил {Object.Name}";
            return result;
        }

        private string _Wait()
        {
            string result = $"{Subject.Name} {(_isGroup?"ждали":"ждал")} {Object.Name}{(Time!=null?" "+Time:"")}{(Reason!=null?" "+Reason:"")}";
            return result;
        }
        private string _Upload ()
        {
            string result = $"{Subject.Name} выложил {Object.Name} из {StorageFrom.Name}";
            return result;
        }
        #endregion
    }
}
