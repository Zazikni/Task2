namespace Task2
{
    internal class Money : IItem
    {
        public string Name { get; }
        public SocietyElement? Ovner { get; set; }
        public Money(string name)
        {
            Name = name;
        }
    }
}
