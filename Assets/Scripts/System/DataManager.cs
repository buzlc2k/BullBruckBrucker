using System;
using System.Collections;
using System.Collections.Generic;

namespace BullBrukBruker
{
    public class DataManager : SingletonMono<DataManager>
    {
        private DataModelManager dataModelManager;

        private void OnDestroy()
        {
            SaveAllData();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            SaveAllData();
        }

        private void OnApplicationQuit() {
            SaveAllData();
        }

        public IEnumerator InitDataManager()
        {
            dataModelManager = new();

            yield return StartCoroutine(dataModelManager.LoadModels());
        }

        #region Level Progress

        private LevelProgressDataModel GetLevelProgressModel()
            => dataModelManager.GetDataModel(DataID.LevelProgress) as LevelProgressDataModel;
    
        public int GetCurrentLevel()
            => GetLevelProgressModel().Read(x => ((LevelProgressDTO)x).CurrentLevel);
        
        public int GetHighestLevel()
            => GetLevelProgressModel().Read(x => ((LevelProgressDTO)x).HighestLevel);

        public List<int> GetStarsPerLevel()
            => GetLevelProgressModel().Read(x => ((LevelProgressDTO)x).StarsPerLevel);

        public void WriteCurrentLevel(int currentLevel)
            => GetLevelProgressModel().Write(x => ((LevelProgressDTO)x).CurrentLevel, currentLevel);

        public void WriteHighestLevel(int highestLevel)
            => GetLevelProgressModel().Write(x => ((LevelProgressDTO)x).HighestLevel, highestLevel);

        public void AddStarsPerLevel(int star)
        {
            var stars = new List<int>(GetStarsPerLevel())
            {
                star
            };

            GetLevelProgressModel().Write(x => ((LevelProgressDTO)x).StarsPerLevel, stars);
        }

        #endregion

        private void SaveAllData()
            => dataModelManager.SaveAll();
    }
}