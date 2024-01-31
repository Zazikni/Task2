namespace Task2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Persons
            Person neznayka = new Person("Незнайка", 16, "home");
            Person kozlik = new Person("Козлик", 17, "home");
            Person korotishka = new Person("Коротышка", 17, "home");
            Person miga = new Person("Мига", 17, "home");
            // Groups
            SocietyGroup neznayka_kozlik_group = new SocietyGroup(neznayka, kozlik);
            // Queue
            EventQueue eventQueue = new EventQueue(neznayka_kozlik_group);
            eventQueue.Add(new Action(kozlik, Actions.Wait, "10 минут"));
            string eventRes = eventQueue.Start();
            Console.WriteLine(eventRes);
            
        }
    }
}
