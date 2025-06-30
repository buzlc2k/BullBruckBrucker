using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BullBrukBruker{
    public class SelectLevelCanvasController : BaseCanvasController
    {
        [SerializeField] GameObject levelButtonPrefab;
        [SerializeField] RectTransform levelButtonGrid;

        public override IEnumerator InitCanvas()
        {
            var (currentHighestLevel, totalLevel, starsPerLevel) = GetLevelsData();

            ResizeLevelButtonGrid(totalLevel);
            CanvasManager.Instance.StartCoroutine(GenerateLevelButton(currentHighestLevel, totalLevel, starsPerLevel));
            yield return null;
        }

        private (int currentHighestLevel, int totalLevel, List<int> starsPerLevel) GetLevelsData()
        {
            int currentHighestLevel = DataManager.Instance.GetHighestLevel();
            int totalLevel = ConfigsManager.Instance.LevelConfig.GetTotalLevels();
            List<int> starsPerLevel = DataManager.Instance.GetStarsPerLevel();

            return (currentHighestLevel, totalLevel, starsPerLevel);
        }

        private IEnumerator GenerateLevelButton(int currentHighestLevel, int totalLevel, List<int> starsPerLevel)
        {
            for (int i = 1; i <= totalLevel; i++)
            {
                LevelButtonController button = Instantiate(levelButtonPrefab.GetComponent<LevelButtonController>(), levelButtonGrid);

                int numStars = i < starsPerLevel.Count ? starsPerLevel[i] : 0;
                button.SetIndexForButton(i, i <= currentHighestLevel, numStars);

                yield return null;
            }
        }

        private void ResizeLevelButtonGrid(int totalLevel)
        {
            var gridLayout = levelButtonGrid.GetComponent<GridLayoutGroup>();
            int columns = gridLayout.constraintCount + 1;
            int rows = Mathf.CeilToInt((float)totalLevel / columns);
            levelButtonGrid.sizeDelta = new Vector2(levelButtonGrid.sizeDelta.x, rows * gridLayout.cellSize.y + rows * gridLayout.spacing.y);

            levelButtonGrid.anchoredPosition = new Vector2(0, -levelButtonGrid.sizeDelta.y / 2);
        }
    }
}