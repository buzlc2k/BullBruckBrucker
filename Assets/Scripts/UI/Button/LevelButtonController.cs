using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BullBrukBruker{
    public class LevelButtonController : ButtonController
    {        
        private int levelIndex;
        [SerializeField] private TextMeshProUGUI levelIndexText;
        [SerializeField] private Image buttonImage;

        public void SetIndexForButton(int levelIndex, bool available)
        {
            this.levelIndex = levelIndex;
            levelIndexText.text = $"{levelIndex}";

            if (available) buttonImage.color = Color.white;
            else buttonImage.color = Color.grey;

            button.interactable = available;
        }

        protected override void OnClick()
        {
            Observer.PostEvent(EventID.LevelButton_Clicked, levelIndex);
        }
    }
}