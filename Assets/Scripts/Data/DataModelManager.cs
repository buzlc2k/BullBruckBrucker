using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BullBrukBruker
{
    public class DataModelManager
    {
        private Dictionary<DataID, IDataModel> dataModels;

        public IEnumerator LoadModels()
        {
            dataModels = new()
            {
                { DataID.LevelProgress, new LevelProgressDataModel() },
            };

            foreach (var model in dataModels.Values)
                yield return DataManager.Instance.StartCoroutine(model.LoadData());
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