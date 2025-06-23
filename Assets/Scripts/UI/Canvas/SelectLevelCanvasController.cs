using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BullBrukBruker{
    public class SelectLevelCanvasController : BaseCanvasController
    {
        [SerializeField] GameObject levelButtonPrefab;
        [SerializeField] RectTransform levelButtonGrid;

        public override IEnumerator InitCanvas()
        {
            var (currentHighestLevel, totalLevel) = GetLevelsData();

            ResizeLevelButtonGrid(totalLevel);
            CanvasManager.Instance.StartCoroutine(GenerateLevelButton(currentHighestLevel, totalLevel));
            yield return null;
        }

        private (int currentHighestLevel, int totalLevel) GetLevelsData()
        {
            //Craft logic
            int currentHighestLevel = 1;
            int totalLevel = 50;

            return (currentHighestLevel, totalLevel);
        }

        private IEnumerator GenerateLevelButton(int currentHighestLevel, int totalLevel)
        {
            for (int i = 1; i <= totalLevel; i++)
            {
                LevelButtonController button = Instantiate(levelButtonPrefab.GetComponent<LevelButtonController>(), levelButtonGrid);
                button.SetIndexForButton(i, i <= currentHighestLevel);
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