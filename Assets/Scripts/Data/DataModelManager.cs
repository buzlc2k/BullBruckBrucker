using System.Collections.Generic;
using UnityEngine;

namespace BullBrukBruker
{
    public class DataModelManager
    {
        private Dictionary<DataID, IDataModel> dataModels;

        public DataModelManager()
        {
            LoadDataModels();
        }

        private void LoadDataModels()
        {
            dataModels = new()
            {
                { DataID.LevelProgress, new LevelProgressDataModel() },
            };
        }

        public IDataModel GetDataModel(DataID dataID)
        {
            if (dataModels.TryGetValue(dataID, out var dataModel))
                return dataModel;

            Debug.Log("Not Found Data Model");
            return null;
        }

        public void SaveAll()
        {
            foreach (var dataModel in dataModels)
                dataModel.Value.SaveData();
        }
    }
}