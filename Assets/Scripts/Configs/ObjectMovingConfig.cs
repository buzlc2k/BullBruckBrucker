using UnityEngine;
using System;

namespace BullBrukBruker
{
    [Serializable]
    public class ObjectMovingConfigRecord
    {
        public ObjectID ID;
        public float Speed;
    }

    public class ObjectMovingConfig : BYDataTable<ObjectMovingConfigRecord>
    {
        public override ConfigCompare<ObjectMovingConfigRecord> DefineConfigCompare()
        {
            configCompare = new ConfigCompare<ObjectMovingConfigRecord>("ID");
            return configCompare; 
        }
    }   
}