
namespace Task2
{
    /// <summary>
    /// Класс реализующий объект хранилища.
    /// </summary>
    internal class Storage:IItem
    {
        #region fields
        public string Name { get;}
        public SocietyElement? Ovner { get; set; }
        #endregion
        #region constructors
        public Storage(string name)
        {
            Name = name;
        }
        #endregion
    }
}
