using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BullBrukBruker{
    public class StarImagesContainerInHeaderController : StarImagesContainerController
    {
        private void OnEnable()
        {
            ActiveStars(3);
        }

        private void Update()
        {
            UpdateStarImages();
        }

        private void UpdateStarImages()
        {
            int remainAttemps = LevelManager.Instance.RemainAttemps;

            ActiveStars(remainAttemps);
        }
    }
}