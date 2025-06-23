using System.Collections.Generic;
using UnityEngine;

namespace BullBrukBruker
{
    public interface ISMContext<T_ID>
    {
        BaseSMState<T_ID> CurrentState { get; set; }
        Dictionary<T_ID, BaseSMState<T_ID>> States { get; set; }

        void InitializeStates();

        public void ChangeState(T_ID id)
        {
            ExitPreviousState();
            SetCurrentState(id);
        }

        private void ExitPreviousState()
        {
            if (CurrentState == null) return;

            CurrentState.ExitState();
        }

        private void SetCurrentState(T_ID id)
        {
            if (!States.TryGetValue(id, out var state)) return;
            CurrentState = state;
            state.EnterState();
        }
    }
}