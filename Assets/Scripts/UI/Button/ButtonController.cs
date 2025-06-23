using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BullBrukBruker{
    public abstract class ButtonController : MonoBehaviour
    {
        [SerializeField] protected Button button;

        protected UnityAction onButtonClickAction;

        protected virtual void Awake()
        {
            SetButton();
        }

        protected virtual void OnEnable()
        {
            AddButtonClickAction();
        }

        protected virtual void OnDisable()
        {
            button.onClick.RemoveListener(onButtonClickAction);
        }

        protected virtual void SetButton(){
            if (button == null) button = GetComponent<Button>();
        }

        private void AddButtonClickAction()
        {
            onButtonClickAction ??= () => {
                OnClick();
            };

            button.onClick.AddListener(onButtonClickAction);
        }

        protected abstract void OnClick();
    }
}
