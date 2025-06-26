using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BullBrukBruker
{
    public class GameManager : SingletonMono<GameManager>, ISMContext<GameStateID>
    {
        private BaseSMState<GameStateID> currentState;
        Dictionary<GameStateID, BaseSMState<GameStateID>> states;

        private Action<object> reloadGame;

        public GameStateID PreviousStateID { get; set; }
        public GameStateID CurrentStateID { get; set; }

        BaseSMState<GameStateID> ISMContext<GameStateID>.CurrentState { get => currentState; set => currentState = value; }
        Dictionary<GameStateID, BaseSMState<GameStateID>> ISMContext<GameStateID>.States { get => states; set => states = value; }

        private void OnEnable()
        {
            InitializeDelegate();
            RegisterEvent();
        }

        private void OnDisable()
        {
            UnRegisterEvent();
        }

        public IEnumerator InitGameManager()
        {
            InitializeStates();

            ((ISMContext<GameStateID>)this).ChangeState(GameStateID.Load);
            yield return null;
        }

        private void InitializeDelegate()
        {
            reloadGame ??= (param) =>
            {
                ReloadGame();
            };
        }

        private void RegisterEvent()
        {
            Observer.AddListener(EventID.HomeButton_Clicked, reloadGame);
            Observer.AddListener(EventID.OutOfLevels, reloadGame);
        }

        private void UnRegisterEvent()
        {
            Observer.RemoveListener(EventID.HomeButton_Clicked, reloadGame);
            Observer.RemoveListener(EventID.OutOfLevels, reloadGame);
        }

        public void InitializeStates()
        {
            states = new();

            var mainMenuState = new MainMenuState(this, this);
            var selectLevelState = new SelectLevelState(this, this);
            var loadState = new LoadState(this, this);
            var playState = new PlayState(this, this);
            var pauseState = new PauseState(this, this);
            var winState = new WinState(this, this);
            var overState = new OverState(this, this);

            states.Add(mainMenuState.ID, mainMenuState);
            states.Add(selectLevelState.ID, selectLevelState);
            states.Add(loadState.ID, loadState);
            states.Add(playState.ID, playState);
            states.Add(pauseState.ID, pauseState);
            states.Add(winState.ID, winState);
            states.Add(overState.ID, overState);
        }

        public void ReloadGame(float offsetTime = 0) => StartCoroutine(PS_SceneManager.Instance.LoadScene("Boot", offsetTime));

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); 
#endif
        }
    }
}