using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BullBrukBruker{
    public class StarImagesContainerController : MonoBehaviour
    {
        [SerializeField] private Sprite unActiveStar;
        [SerializeField] private Sprite activeStar;

        [SerializeField] private List<Image> starImages;

        private void Awake()
        {
            if (starImages == null || starImages.Count == 0)
                starImages = GetComponentsInChildren<Image>().ToList();
        }

        private void OnEnable()
        {
            ActiveAllStars();
        }

        private void Update()
        {
            UpdateStarImages();
        }

        private void ActiveAllStars()
        {
            foreach (var starImage in starImages)
                starImage.sprite = activeStar;
        }

        private void UpdateStarImages()
        {
            int remainAttemps = LevelManager.Instance.RemainAttemps;

            for (int i = 0; i < starImages.Count; i++)
            {
                if (i < remainAttemps)
                    starImages[i].sprite = activeStar;
                else
                    starImages[i].sprite = unActiveStar;
            }
        }
    }
}