namespace Task2
{
    /// <summary>
    /// Класс обозначающий абстрактное понятие элемента общества.
    /// </summary>
    internal abstract class SocietyElement : INameable
    {
        #region fields
        public abstract string Name { get; }
        #endregion
    }
}
