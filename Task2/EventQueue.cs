namespace Task2
{    /// <summary>
     /// Класс реализующий очередь действий. На вход опционально принимает субъект этих действий.
     /// </summary>
    internal class EventQueue
    {
        #region fields
        private SocietyElement _subject;
        private Queue<Action> action_queue = new Queue<Action>();
        public SocietyElement? Subject { get { return _subject; }}
        #endregion
        #region constructors
        public EventQueue(SocietyElement? subject = null ) 
        {
            _subject = subject;
        }
        #endregion
        #region methods
        /// <summary>
        /// Метод для добавления действия в очередь.
        /// </summary>
        public void Add(Action action)
        {
            if (Subject != null)
            {
                action.Subject = action.Subject == null ? Subject : action.Subject;
            }
            action_queue.Enqueue(action);
        }
        /// <summary>
        /// Метод для задания нового судьекта действий и обнуления очереди.
        /// </summary>
        public void SetNewEvent(SocietyElement? subject = null)
        {
            _subject = subject;
            action_queue.Clear();
        }
        /// <summary>
        /// Метод для запуска всех действий в очереди.
        /// </summary>
        public string Start()
        {
            bool isFirst = true;
            string event_result = "";
            int counter = action_queue.Count;
            foreach(var action in action_queue)
            {
                if (isFirst & counter == 1)
                {
                    event_result += action.Do() + '.';

                }
                else if (isFirst)
                {
                    event_result += action.Do();
                }
                else if(counter == 1)
                {
                    event_result += ", " +  action.Do() + '.';
                }
                else
                {
                    event_result += ", ";
                    event_result += action.Do();
                }
                isFirst = false;
                counter--;
                

            }
            return event_result;
        }
        #endregion
    }
}
