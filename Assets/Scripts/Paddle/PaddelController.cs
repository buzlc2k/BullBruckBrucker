using System.Collections.Generic;
using UnityEngine;

namespace BullBrukBruker
{
    public class PaddelController : MonoBehaviour, ISMContext<PaddleStateID>
    {
        BaseSMState<PaddleStateID> currentState;
        private Dictionary<PaddleStateID, BaseSMState<PaddleStateID>> states;

        //Components
        [field: SerializeField] public ObjectID ID { get; private set; }
        [field: SerializeField] public ObjectMoving Moving { get; private set; }
        [field: SerializeField] public ObjectCollision Collision { get; private set; }
        [field: SerializeField] public PaddleAiming Aiming { get; private set; }

        //StateMachine
        BaseSMState<PaddleStateID> ISMContext<PaddleStateID>.CurrentState { get => currentState; set => currentState = value; }
        Dictionary<PaddleStateID,BaseSMState<PaddleStateID>> ISMContext<PaddleStateID>.States { get => states; set => states = value; }

        private void Awake()
        {
            InitializeStates();
        }

        public void InitPaddle()
        {
            ((ISMContext<PaddleStateID>)this).ChangeState(PaddleStateID.Ideling);
            transform.position = new(0, -ScreenManager.Instance.ScreenHeight, 0);
        }

        public void InitializeStates()
        {
            states = new();

            var idelingState = new PaddleIdelingState(this, this);
            var catchingState = new PaddleCatchingState(this, this);
            var aimingState = new PaddleAimingState(this, this);

            states.Add(idelingState.ID, idelingState);
            states.Add(catchingState.ID, catchingState);
            states.Add(aimingState.ID, aimingState);
        }
    }
}