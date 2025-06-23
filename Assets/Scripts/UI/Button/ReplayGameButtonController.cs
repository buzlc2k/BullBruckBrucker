using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BullBrukBruker{
    public class ReplayGameButtonController : ButtonController
    {    
        protected override void OnClick()
        {
            Observer.PostEvent(EventID.ReplayButton_Clicked, null);
        }
    }
}