using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    internal class EventQueue
    {
        private Queue<Action> action_queue = new Queue<Action>();
        public SocietyElement Subject {  get;}
        public EventQueue(SocietyElement subject) 
        {
            Subject = subject;
        }
        public void Add(Action action)
        {
            action.Subject = Subject;
            action_queue.Enqueue(action);
        }
        public string Start()
        {
            string event_result = "";
            foreach(var action in action_queue)
            {
                event_result += action.Do();


            }
            return event_result;
        }
    }
}
