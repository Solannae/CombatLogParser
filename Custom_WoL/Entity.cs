using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Custom_WoL
{
    public class Entity
    {
        public ObjectGuid Guid { get; set; }
        public string Name { get; set; }
        public int Flags { get; set; }
        public int Flags2 { get; set; }

        public Entity(ulong _guid, string _name, int _flags, int _flags2)
        {
            Guid = new ObjectGuid(_guid);
            Name = _name;
            Flags = _flags;
            Flags2 = _flags2;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Entity obj_ = obj as Entity;
            if (obj_ == null)
                return false;

            return (obj_.Guid == Guid && obj_.Name == Name);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }
}
