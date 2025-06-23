using System.Collections;
using UnityEngine;

namespace BullBrukBruker{
    [RequireComponent(typeof(Animator))]
    public class ContainerUIElement : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private Animator animator;

        public IEnumerator TurnOnElement(){
            container.SetActive(true);
            animator.Play("On");
            yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
        }

        public IEnumerator TurnOffElement(){
            animator.Play("Off");
            yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);
            container.SetActive(false);
        }
    }
}