using UnityEngine;
using System;

namespace BullBrukBruker
{
    [Serializable]
    public class BlockConfigRecord
    {
        public BlockID ID;
        public int NumCollide;
    }

    public class BlockConfig : BYDataTable<BlockConfigRecord>
    {
        public override ConfigCompare<BlockConfigRecord> DefineConfigCompare()
        {
            configCompare = new ConfigCompare<BlockConfigRecord>("ID");
            return configCompare; 
        }
    }   
}