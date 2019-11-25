namespace Custom_WoL
{
    public class Entity
    {
        public ObjectGuid Guid { get; }
        public string Name { get; set; }
        public int Flags { get; set; }
        public int Flags2 { get; set; }
        public bool IsSummoned { get; set; }

        public Entity(ulong guid, string name, int flags, int flags2)
        {
            Guid = new ObjectGuid(guid);
            Name = name;
            Flags = flags;
            Flags2 = flags2;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Entity temp))
                return false;

            return (temp.Guid == Guid && temp.Name == Name);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }
}
