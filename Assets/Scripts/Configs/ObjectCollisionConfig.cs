using UnityEngine;
using System;
using System.Collections.Generic;

namespace BullBrukBruker
{
    [Serializable]
    public class ObjectCollisionConfigRecord
    {
        public ObjectID ID;
        public float Width;
        public float Height;
    }

    [Serializable]
    public class BallCollisionConfigRecord : ObjectCollisionConfigRecord
    {
        public float AddedBouncePerAngle;
    }

    public class ObjectCollisionConfig : BYDataTable<ObjectCollisionConfigRecord>
    {
        public override ConfigCompare<ObjectCollisionConfigRecord> DefineConfigCompare()
        {
            configCompare = new ConfigCompare<ObjectCollisionConfigRecord>("ID");
            return configCompare;
        }
    }   
}