namespace Task2
{
    /// <summary>
    /// Интерфейс гарантирующий наличие свойства Name у объекта.
    /// </summary>
    internal interface INameable
    {
        #region fields
        string Name { get; }
        #endregion
    }
}
