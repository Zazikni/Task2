namespace Task2
{
    internal class Securities : IItem
    {
        public string Name { get; }
        public SocietyElement? Ovner { get; set; }
        public Securities(string name)
        {
            Name = name;
        }
    }
}
