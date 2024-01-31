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
            // Storages
            Storage karman = new Storage("Карман");
            // Money 
            Money frank =  new Money("Франки");
            // Program
            eventQueue.Add(new Action(obj:kozlik, action:Actions.Wait, time:"10 минут"));
            eventQueue.Add(new Action(Actions.Upload, obj:frank, uploadFrom:karman));
            string eventRes = eventQueue.Start();
            Console.WriteLine(eventRes);
            
        }
    }
}
