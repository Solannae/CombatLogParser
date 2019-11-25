namespace Custom_WoL
{
    public class Spell
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int School { get; set; }

        public Spell(int id, string name, int school)
        {
            ID = id;
            Name = name;
            School = school;
        }
    }
}
