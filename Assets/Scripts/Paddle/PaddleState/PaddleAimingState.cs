using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public class PaddleAimingState : BasePaddelState
    {
        public PaddleAimingState(ISMContext<PaddleStateID> context, PaddelController paddelController) : base(context, paddelController)
        {
            id = PaddleStateID.Aiming;
        }

        public override IEnumerator UpdateState()
        {
            while (true)
            {
                if (InputManager.Instance.CalculateIsTouching())
                    paddelController.Aiming.CalculatingPoints();
                else
                {
                    paddelController.Aiming.Shooting();
                    context.ChangeState(PaddleStateID.Catching);
                    yield break;
                }

                yield return null;
            }
        }
    }
}