using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    class PBEqualsComparer : IEqualityComparer<IMyProgrammableBlock>
    {
        public bool Equals(IMyProgrammableBlock x, IMyProgrammableBlock y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.EntityId == y.EntityId;
        }

        public int GetHashCode(IMyProgrammableBlock obj)
        {
            return obj.EntityId.GetHashCode();
        }
    }
}