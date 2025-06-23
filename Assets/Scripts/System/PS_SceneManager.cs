using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BullBrukBruker
{
    public class PS_SceneManager : SingletonMono<PS_SceneManager>
    {
        public bool IsLoading { get; private set; }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public IEnumerator InitSceneManager()
        {
            IsLoading = true;
            yield return null;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => IsLoading = false;

        public IEnumerator LoadScene(int index, float offsetTime = 0)
        {
            IsLoading = true;
            float currentTime = 0;
            while (currentTime <= offsetTime)
            {
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }

            AsyncOperation reloadAsync = SceneManager.LoadSceneAsync(index);
            while (!reloadAsync.isDone)
                yield return null;
        }

        public IEnumerator LoadScene(string name, float offsetTime = 0)
        {
            IsLoading = true;
            float currentTime = 0;
            while (currentTime <= offsetTime)
            {
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }

            AsyncOperation reloadAsync = SceneManager.LoadSceneAsync(name);
            while (!reloadAsync.isDone)
                yield return null;
        }
    }
}