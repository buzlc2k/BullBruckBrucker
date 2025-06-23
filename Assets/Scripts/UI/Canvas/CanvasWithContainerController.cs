using System.Collections;
using UnityEngine;

namespace BullBrukBruker{
    public class CanvasWithContainerController : BaseCanvasController
    {
        [SerializeField] private ContainerUIElement containerUIElement;
        public override IEnumerator TurnOffCanvas()
        {
            yield return StartCoroutine(containerUIElement.TurnOffElement());
        }

        public override IEnumerator TurnOnCanvas()
        {
            yield return StartCoroutine(containerUIElement.TurnOnElement());
        }
    }
}