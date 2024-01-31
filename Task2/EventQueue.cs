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
            Console.WriteLine("ADD");
            Console.WriteLine(Subject.Name);
            action.Subject = Subject;
            action_queue.Enqueue(action);
        }
        public void Start()
        {
            Console.WriteLine("NEW EVENT");
            foreach(var action in action_queue)
            {
                Console.WriteLine(action.Subject.Name);
                Console.WriteLine(action.Object.Name);
                Console.WriteLine(action.Act);


            }
        }
    }
}
