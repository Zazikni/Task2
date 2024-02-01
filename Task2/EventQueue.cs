using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    internal class EventQueue
    {
        private SocietyElement _subject;
        private Queue<Action> action_queue = new Queue<Action>();
        public SocietyElement Subject { get { return _subject; }}
        public EventQueue(SocietyElement subject) 
        {
            _subject = subject;
        }
        public void Add(Action action)
        {
            action.Subject = action.Subject == null ? Subject: action.Subject;
            action_queue.Enqueue(action);
        }
        public void SetNewEvent(SocietyElement subject)
        {
            _subject = subject;
            action_queue.Clear();
        }
        public string Start()
        {
            bool isFirst =true;
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
                    event_result += ", "+  action.Do() + '.';
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
    }
}
