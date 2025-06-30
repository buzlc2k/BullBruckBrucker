using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BullBrukBruker{
    public class LevelButtonController : ButtonController
    {        
        private int levelIndex;
        [SerializeField] private TextMeshProUGUI levelIndexText;
        [SerializeField] private StarImagesContainerController starImagesContainer;
        [SerializeField] private Image buttonImage;

        public void SetIndexForButton(int levelIndex, bool available, int numStart)
        {
            this.levelIndex = levelIndex;
            levelIndexText.text = $"{levelIndex}";

            buttonImage.color = available ? Color.white : Color.grey;
            button.interactable = available;

            starImagesContainer.ActiveStars(numStart);
        }

        protected override void OnClick()
        {
            Observer.PostEvent(EventID.LevelButton_Clicked, levelIndex);
        }
    }
}