using System.Collections;
using UnityEngine;

namespace BullBrukBruker{
    public class BaseCanvasController : MonoBehaviour
    {
        public GameStateID RespondingState;

        public virtual IEnumerator InitCanvas()
        {
            yield return null;
        }

        public virtual IEnumerator TurnOffCanvas()
        {
            gameObject.SetActive(false);
            yield break;
        }

        public virtual IEnumerator TurnOnCanvas()
        {
            gameObject.SetActive(true);
            yield break;
        }
    }
}