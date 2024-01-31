
namespace Task2
{
    internal class Storage:IItem
    {
        public string Name { get;}
        public SocietyElement? Ovner { get; set; }
        public Storage(string name)
        {
            Name = name;
        }
    }
}
