using System;
using System.Collections.Generic;

namespace BullBrukBruker
{
    [Serializable]
    public class LevelProgressDTO
    {
        public int CurrentLevel { get; set; }
        public int HighestLevel { get; set; }
        public List<int> StarsPerLevel { get; set; }
    }
}