namespace Task2
{
    #region enums
    enum Gender
    {
        Male,
        Female
    }
    enum Pronouns
    {
        FirstPers,
        SecondPers,
        ThirdPers,
        NS


    }
    #endregion
    /// <summary>
    /// Класс реализующий конкретную личность.
    /// </summary>
    internal class Person : SocietyElement
    {
        #region fields
        private string _name;
        public int Age { get; }
        public Pronouns? Prn { get; set; }
        public Gender Gender { get; }
        public override string Name
        {
            get 
            { 
                switch (Prn)
                {
                    case Pronouns.FirstPers:
                    {
                        return "Я";
                    }
                    case Pronouns.SecondPers:
                    {
                        return "Ты";
                    }
                    case Pronouns.ThirdPers:
                    {
                        return "Он";
                    }
                    default:
                    {
                        return _name;
                    }
                }
            }
        }
        #endregion
        #region constructors
        public Person(string name, int age, Gender gender)
        { 
            Age = age;
            _name = name;
            Gender = gender;
            Prn = Pronouns.NS;
        }
        #endregion
        #region methods
        /// <summary>
        /// Метод возвращения полей объекта в исходное состояние.
        /// </summary>
        public void SetDefault()
        {
            Prn = Pronouns.NS;
        }
        #endregion

    }

}
