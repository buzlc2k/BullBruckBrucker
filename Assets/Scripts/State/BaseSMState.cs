using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public abstract class BaseSMState<T_ID>
    {
        protected T_ID id;
        protected Coroutine updateState;
        protected ISMContext<T_ID> context;

        #region Properties
        public T_ID ID { get => id; }
        #endregion

        public BaseSMState(ISMContext<T_ID> context)
        {
            this.context = context;
            updateState = null;
        }

        public virtual void EnterState()
        {
            updateState = GameManager.Instance.StartCoroutine(UpdateState());
        }
        public abstract IEnumerator UpdateState();
        public virtual void ExitState()
        {
            if (updateState == null) return;
            
            GameManager.Instance.StopCoroutine(updateState);
            updateState = null;
        }
    } 
}