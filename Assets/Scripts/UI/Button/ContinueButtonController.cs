using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BullBrukBruker{
    public class ContinueButtonController : ButtonController
    {    
        protected override void OnClick()
        {
            Observer.PostEvent(EventID.ContinueButton_Clicked, null);
        }
    }
}