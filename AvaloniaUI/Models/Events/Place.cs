namespace Events
{
    /// <summary>
    /// Класс реализующий какое-либо место в реальном мире.
    /// </summary>
    internal record class Place: INameable
    {
        #region fields
        public string Name { get; }
        #endregion
        #region constructors
        public Place(string name)
        {
            Name = name;
        }
        #endregion
    }
}
