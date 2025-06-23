using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BullBrukBruker{
    public class PauseButtonController : ButtonController
    {    
        protected override void OnClick()
        {
            Observer.PostEvent(EventID.PauseGameButton_Clicked, null);
        }
    }
}