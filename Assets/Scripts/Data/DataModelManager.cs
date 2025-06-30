using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using UnityEngine;

namespace BullBrukBruker
{
    public class DataModelManager
    {
        private string userID;
        private Dictionary<DataID, IDataModel> dataModels;

        public IEnumerator LoadModels()
        {
            yield return DataManager.Instance.StartCoroutine(ConnectFirebase());

            yield return DataManager.Instance.StartCoroutine(LoadUser());

            dataModels = new()
            {
                { DataID.LevelProgress, new LevelProgressDataModel(userID) },
            };

            foreach (var model in dataModels.Values)
                yield return DataManager.Instance.StartCoroutine(model.LoadData());
        }

        private IEnumerator ConnectFirebase()
        {
            var checkAndFixDependenciesTask = FirebaseApp.CheckAndFixDependenciesAsync();
            yield return new YieldTask(checkAndFixDependenciesTask);

            if (checkAndFixDependenciesTask.Result != DependencyStatus.Available)
                Debug.Log("CANT CONNECT TO FIREBASE");
            else
                Debug.Log("CONNECTED TO FIREBASE");

            yield return null;
        }

        private IEnumerator LoadUser()
        {
            if (SaveSystem.HasKey(DataNodes.UserDataKey))
                userID = SaveSystem.GetString(DataNodes.UserDataKey);
            else
                yield return DataManager.Instance.StartCoroutine(CreateNewUser());
        }

        private IEnumerator CreateNewUser()
        {
            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DataNodes.UserDataKey);
            var getValueTask = dbRef.GetValueAsync();

            yield return new YieldTask(getValueTask);

            DataSnapshot snapshot = getValueTask.Result;

            long userCount = snapshot.ChildrenCount;
            userID = $"user_{userCount + 1}";

            yield return DataManager.Instance.StartCoroutine(SaveNewUser());
        }

        private IEnumerator SaveNewUser()
        {
            var dbRef = FirebaseDatabase.DefaultInstance.GetReference(DataNodes.UserDataKey).Child(userID);
            yield return new YieldTask(dbRef.SetValueAsync($"created at: {string.Format("{0:HH:mm:ss tt}", DateTime.Now)}"));

            SaveSystem.SetString(DataNodes.UserDataKey, userID);
            SaveSystem.SaveToDisk();
        }

        public IDataModel GetDataModel(DataID dataID)
        {
            if (dataModels.TryGetValue(dataID, out var dataModel))
                return dataModel;

            Debug.Log("Not Found Data Model");
            return null;
        }
    }
}