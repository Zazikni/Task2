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
            // Storages
            Storage karman = new Storage("Карман");
            // Money 
            Money frank =  new Money("Фертинги");
            //Securities
            Securities stocks = new Securities("Акции");
            // Places
            Place bank = new Place("Банк");
            // Program

            //Коротышка выложил из кармана денежки и, получив акции, удалился.
            EventQueue eventQueue = new EventQueue(korotishka);
            eventQueue.Add(new Action(obj: frank, action: Actions.Upload, uploadFrom:karman));
            eventQueue.Add(new Action(action: Actions.Receive, obj: stocks));
            eventQueue.Add(new Action(action: Actions.Leave));
            Console.WriteLine(eventQueue.Start());

            // А желающих приобрести акции с каждым днем становилось все больше.
            //Незнайка и Козлик с утра до вечера продавали акции, Мига же только и делал, что ездил в банк.
            eventQueue.SetNewEvent(neznayka_kozlik_group);
            eventQueue.Add(new Action(obj: stocks, action: Actions.Sell, time:"с утра до вечера"));
            eventQueue.Add(new Action(sbj:miga, action: Actions.Go, goTo:bank));
            Console.WriteLine(eventQueue.Start());

            //Там он обменивал вырученные от продажи мелкие деньги на крупные и складывал их в несгораемый шкаф. 
            //Многие покупатели являлись в контору слишком рано.
            //От нечего делать они толклись на улице, дожидаясь открытия конторы. 
            //Это привлекало внимание прохожих. 
            //Постепенно всем в городе стало известно, что акции Общества гигантских растений пользуются большим спросом.
            //Наслушавшись подобных рассказов, каждый, кому удалось сберечь на черный день сотню - другую фертингов, спешил накупить гигантских акций, с тем чтоб продать их, как только они повысятся в цене.
            //В результате два миллиона акций, хранившиеся в двух несгораемых сундуках, были быстро распроданы.



        }
    }
}
