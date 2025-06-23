using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BullBrukBruker
{
    [Serializable]
    public class LevelConfigRecord
    {
        public int Index;
        public List<SavedCell> Cells;
    }

    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private List<LevelConfigRecord> records = new();

        public int GetTotalLevels() => records.Count;

        public LevelConfigRecord GetRecord(int index)
        {
            return records.FirstOrDefault(r => r.Index == index);
        }

        public void AddRecord(LevelConfigRecord newRecord) => records.Add(newRecord);

        public void RemoveRecord(LevelConfigRecord existingRecord) => records.Remove(existingRecord);
    }   
}