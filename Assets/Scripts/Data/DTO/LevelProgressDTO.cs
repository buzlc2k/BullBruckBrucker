using System;
using System.Collections.Generic;

namespace BullBrukBruker
{
    [Serializable]
    public class LevelProgressDTO
    {
        public int CurrentLevel;
        public int HighestLevel;
        public List<int> StarsPerLevel;
    }
}