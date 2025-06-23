using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BullBrukBruker{
    public class NextLevelButtonController : ButtonController
    {    
        protected override void OnClick()
        {
            Observer.PostEvent(EventID.NextLevelButton_Clicked, null);
        }
    }
}