using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BullBrukBruker
{
    public class LevelManager : SingletonMono<LevelManager>
    {
        [SerializeField] GameObject PaddlePrefab;

        public int RemainAttemps { get; private set; }
        public int CurrentIndex { get; private set; }
        public bool IsLoading { get; private set; }
        public PaddelController CurrentPaddle { get; private set; }
        public List<GameObject> CurrentItems { get; private set; }
        public List<GameObject> CurrentBalls { get; private set; }

        private Action<object> loadHighestLevel;
        private Action<object> reloadLevel;
        private Action<object> loadLevel;
        private Action<object> loadNextLevel;

        private void InitializeDelegate()
        {
            loadHighestLevel ??= (param) =>
            {
                LoadHighestLevel();
            };

            reloadLevel ??= (param) =>
            {
                ReloadLevel();
            };

            loadLevel ??= (param) =>
            {
                LoadLevel((int)param);
            };

            loadNextLevel ??= (param) =>
            {
                LoadNextLevel();
            };
        }

        private void OnEnable()
        {
            InitializeDelegate();
            RegisterEvent();
        }

        private void OnDisable()
        {
            UnRegisterEvent();
        }

        private void RegisterEvent()
        {
            Observer.AddListener(EventID.PlayButton_Clicked, loadHighestLevel);
            Observer.AddListener(EventID.LevelButton_Clicked, loadLevel);
            Observer.AddListener(EventID.ReplayButton_Clicked, reloadLevel); 
            Observer.AddListener(EventID.NextLevelButton_Clicked, loadNextLevel);
        }

        private void UnRegisterEvent()
        {
            Observer.RemoveListener(EventID.PlayButton_Clicked, loadHighestLevel);
            Observer.RemoveListener(EventID.LevelButton_Clicked, loadLevel);
            Observer.RemoveListener(EventID.ReplayButton_Clicked, reloadLevel);
            Observer.RemoveListener(EventID.NextLevelButton_Clicked, loadNextLevel);
        }

        public void LoadHighestLevel()
        {
            //Craft logic
            LoadLevel(1);
        }

        public void ReloadLevel()
        {
            LoadLevel(CurrentIndex);
        }

        public void LoadNextLevel()
        {
            LoadLevel(CurrentIndex + 1);
        }

        public void LoadLevel(int index)
        {
            StartCoroutine(C_LoadLevel(index));
        }

        private IEnumerator C_LoadLevel(int index)
        {
            IsLoading = true;

            CurrentIndex = index;

            yield return StartCoroutine(C_LoadLevelObjects());

            yield return StartCoroutine(C_LoadLevelGrid(CurrentIndex));

            yield return new WaitForSecondsRealtime(.5f);

            IsLoading = false;

            LoadAttemps();
        }

        private IEnumerator C_LoadLevelObjects()
        {
            LoadPaddel();

            yield return null;

            LoadBalls();

            yield return null;

            LoadItem();
        }

        private void LoadPaddel()
        {
            CurrentPaddle = CurrentPaddle != null ? CurrentPaddle : Instantiate(PaddlePrefab).GetComponent<PaddelController>();
            CurrentPaddle.InitPaddle();
        }

        private void LoadBalls()
        {
            CurrentBalls ??= new();
            CurrentBalls.ForEach(ball => ball.SetActive(false));
            CurrentBalls.Clear();
        }

        private void LoadItem()
        {
            CurrentItems ??= new();
            CurrentItems.ForEach(item => item.SetActive(false));
            CurrentItems.Clear();
        }

        private IEnumerator C_LoadLevelGrid(int index)
        {
            LevelConfig levelConfig = null;

            while (levelConfig == null)
            {
                levelConfig = ConfigsManager.Instance.LevelConfig;
                yield return null;
            }

            var currentLevelRecord = levelConfig.GetRecord(index);

            if (currentLevelRecord == null)
            {
                Observer.PostEvent(EventID.OutOfLevels, null);
                yield break;
            }

            GridManager.Instance.InitGridCells(currentLevelRecord.Cells);
        }

        private void LoadAttemps() => RemainAttemps = 3;

        public void AddBall(GameObject ballAdded) => CurrentBalls.Add(ballAdded);

        public void RemoveBall(GameObject ballRemoved) => Utils.StartSafeCourotine(this, C_RemoveActiveBall(ballRemoved));

        private IEnumerator C_RemoveActiveBall(GameObject ballRemoved)
        {
            yield return new WaitForEndOfFrame();
            CurrentBalls.Remove(ballRemoved);

            if (CurrentBalls.Count == 0)
                HandleOutOfBalls();
        }

        private void HandleOutOfBalls()
        {
            RemainAttemps--;

            Observer.PostEvent(EventID.OutOfBalls, RemainAttemps);            
            if (RemainAttemps > 0)
                LoadItem();
        }
        
        public void AddItem(GameObject item) => CurrentItems.Add(item);

        public void RemoveItem(GameObject itemRemoved) => Utils.StartSafeCourotine(this, C_RemoveActiveItem(itemRemoved));

        private IEnumerator C_RemoveActiveItem(GameObject itemRemoved)
        {
            yield return new WaitForEndOfFrame();
            CurrentItems.Remove(itemRemoved);
        }
    }
}