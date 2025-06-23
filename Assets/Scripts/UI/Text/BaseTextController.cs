using System.Collections;
using TMPro;
using UnityEngine;

namespace BullBrukBruker
{
    public abstract class BaseTextController : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI text;

        protected Coroutine updateText;

        protected virtual void Awake()
        {
            if (text == null) text = GetComponent<TextMeshProUGUI>();
        }

        protected virtual void OnEnable()
        {
            if (updateText != null)
            {
                StopCoroutine(updateText);
                updateText = null;
            }
            updateText = StartCoroutine(UpdateText());
        }

        protected abstract IEnumerator UpdateText();
    }   
}