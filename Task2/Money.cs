namespace Task2
{
    /// <summary>
    /// Класс реализующий деньги.
    /// </summary>
    internal class Money : IItem
    {
        #region fields
        public string Name { get; }
        public SocietyElement? Ovner { get; set; }
        #endregion
        #region constructors
        public Money(string name)
        {
            Name = name;
        }
        #endregion
    }
}
