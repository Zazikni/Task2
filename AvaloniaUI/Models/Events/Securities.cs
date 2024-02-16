namespace Events
{
    /// <summary>
    /// Класс реализующий объект ценных бумаг.
    /// </summary>
    internal class Securities : IItem
    {
        #region fields
        public string Name { get; }
        public SocietyElement? Ovner { get; set; }
        #endregion
        #region constructors
        public Securities(string name)
        {
            Name = name;
        }
        #endregion
    }
}
