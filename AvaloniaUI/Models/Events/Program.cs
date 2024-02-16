namespace Events
{
    public static class EventsProgram
    {
        public static string Run ()
        {
            string result = string.Empty;
            #region obj init
            // Persons
            Person neznayka = new Person("Незнайка", 16, Gender.Male);
            Person kozlik = new Person("Козлик", 17, Gender.Male);
            Person korotishka = new Person("Коротышка", 17, Gender.Male);
            Person miga = new Person("Мига", 17, Gender.Male);
            // Groups
            SocietyGroup neznayka_kozlik_group = new SocietyGroup(neznayka, kozlik);
            SocietyGroup buyers = new SocietyGroup(neznayka, kozlik, korotishka, miga);
            SocietyGroup citizens = new SocietyGroup(neznayka, kozlik, korotishka, miga);
            SocietyGroup giant_plants_group = new SocietyGroup("Общество гигантских растений");
            SocietyGroup strangers = new SocietyGroup("Случайные люди");
            SocietyGroup affluent_peoples = new SocietyGroup("Те кому удалось сберечь сотню - другую фертингов");
            // Storages
            Storage karman = new Storage("Карман");
            Storage heat_res_case = new Storage("Несгораемый шкаф");
            // Money 
            Money frank =  new Money("Фертинги");
            // Securities
            Securities stocks = new Securities("Акции");
            // Places
            Place bank = new Place("Банк");
            Place organization = new Place("Контора");
            Place city = new Place("Город");
            Place street = new Place("Улица");
            #endregion

            #region program

            //Коротышка выложил из кармана денежки и, получив акции, удалился.
            EventQueue eventQueue = new EventQueue(korotishka);
            eventQueue.Add(new Action(obj: frank, action: Actions.Upload, storageFrom:karman));
            eventQueue.Add(new Action(action: Actions.Receive, obj: stocks));
            eventQueue.Add(new Action(action: Actions.Leave));
            result += eventQueue.Start();
            korotishka.SetDefault();

            // А желающих приобрести акции с каждым днем становилось все больше.
            eventQueue.SetNewEvent(buyers);
            buyers.SocialRole = SocialRole.Buyer;
            eventQueue.Add(new Action(action: Actions.WishBuy, obj:stocks));
            result += eventQueue.Start();
            buyers.SetDefault();

            //Незнайка и Козлик с утра до вечера продавали акции, Мига же только и делал, что ездил в банк.
            eventQueue.SetNewEvent(neznayka_kozlik_group);
            eventQueue.Add(new Action(obj: stocks, action: Actions.Sell, time:"с утра до вечера"));
            eventQueue.Add(new Action(sbj:miga, action: Actions.Go, goTo:bank));
            result += eventQueue.Start();
            neznayka_kozlik_group.SetDefault();

            //Там он обменивал вырученные от продажи мелкие деньги на крупные и складывал их в несгораемый шкаф. 
            eventQueue.SetNewEvent(miga);
            miga.Prn = Pronouns.ThirdPers;
            eventQueue.Add(new Action(obj: frank, action: Actions.Exchange));
            eventQueue.Add(new Action(obj: frank, action: Actions.Put, storageTo:heat_res_case));
            result += eventQueue.Start();
            miga.SetDefault();

            //Многие покупатели являлись в контору слишком рано.
            eventQueue.SetNewEvent(buyers);
            buyers.SocialRole = SocialRole.Buyer;
            eventQueue.Add(new Action(goTo: organization, action: Actions.Go, time:"слишком рано"));
            result += eventQueue.Start();
            buyers.SetDefault();

            //От нечего делать они толклись на улице, дожидаясь открытия конторы.
            eventQueue.SetNewEvent(buyers);
            buyers.Prn = PronounsGroups.ThirdPers;
            eventQueue.Add(new Action(action: Actions.Stay, where:street));
            eventQueue.Add(new Action(action: Actions.Wait, obj:organization, reason:"откроется"));
            result += eventQueue.Start();
            buyers.SetDefault();

            //Это привлекало внимание прохожих. 
            eventQueue.SetNewEvent(buyers);
            buyers.Prn = PronounsGroups.ThirdPers;
            strangers.SocialRole = SocialRole.Strangers;
            eventQueue.Add(new Action(action:Actions.AttractAttention, obj:strangers));
            result += eventQueue.Start();
            strangers.SetDefault();
            buyers.SetDefault();

            //Постепенно всем в городе стало известно, что акции Общества гигантских растений пользуются большим спросом.
            eventQueue.SetNewEvent(citizens);
            citizens.SocialRole = SocialRole.Citizens;
            stocks.Ovner = giant_plants_group;
            eventQueue.Add(new Action(action: Actions.Notify));
            eventQueue.Add(new Action(action: Actions.NotifyReason, itemobj:stocks ,reason:"пользуются большим спросом"));
            result += eventQueue.Start();
            citizens.SetDefault();

            //Наслушавшись подобных рассказов, каждый, кому удалось сберечь на черный день сотню - другую фертингов, спешил накупить гигантских акций, с тем чтоб продать их, как только они повысятся в цене.
            eventQueue.SetNewEvent(affluent_peoples);
            eventQueue.Add(new Action(action: Actions.WishBuy, obj:stocks, reason: "чтоб продать их, как только они повысятся в цене"));
            result += eventQueue.Start();

            //В результате два миллиона акций,  хранившиеся в двух несгораемых сундуках, были быстро распроданы.
            eventQueue.SetNewEvent();
            eventQueue.Add(new Action(action: Actions.Sold, obj:stocks, amount:2000000, storageFrom:heat_res_case));
            result += eventQueue.Start();
            #endregion
            return result;
        }
    }
}
