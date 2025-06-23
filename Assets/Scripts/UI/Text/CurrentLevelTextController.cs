using System.Collections;
using TMPro;
using UnityEngine;

namespace BullBrukBruker
{
    public class CurrentLevelTextController : BaseTextController
    {
        protected override IEnumerator UpdateText()
        {
            text.text = $"Level {LevelManager.Instance.CurrentIndex}";
            yield return null;
        }
    }
}