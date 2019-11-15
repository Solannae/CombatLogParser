namespace Custom_WoL
{
    public class Spell
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int School { get; set; }

        public Spell(int _id, string _name, int _school)
        {
            ID = _id;
            Name = _name;
            School = _school;
        }
    }
}
