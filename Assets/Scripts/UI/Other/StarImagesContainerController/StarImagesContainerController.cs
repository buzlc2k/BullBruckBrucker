using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BullBrukBruker{
    public class StarImagesContainerController : MonoBehaviour
    {
        [SerializeField] protected Sprite unActiveStar;
        [SerializeField] protected Sprite activeStar;

        [SerializeField] protected List<Image> starImages;

        protected virtual void Awake()
        {
            if (starImages == null || starImages.Count == 0)
                starImages = GetComponentsInChildren<Image>().ToList();
        }

        public virtual void ActiveStars(int num)
        {
            for (int i = 0; i < starImages.Count; i++)
                starImages[i].sprite = i < num ? activeStar : unActiveStar;
        }
    }
}