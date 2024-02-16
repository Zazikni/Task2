namespace Events
/// <summary>
/// Интерфейс гарантирующий наличие свойства Owner у объекта.
/// </summary>
{
    internal interface IItem:INameable
    {
        #region fields
        SocietyElement Ovner { get; set; }
        #endregion
    }
}
