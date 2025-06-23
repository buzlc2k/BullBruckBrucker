using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public class PaddleIdelingState : BasePaddelState
    {
        public PaddleIdelingState(ISMContext<PaddleStateID> context, PaddelController paddelController) : base(context, paddelController)
        {
            id = PaddleStateID.Ideling;
        }

        public override IEnumerator UpdateState()
        {
            while (true)
            {
                if (InputManager.Instance.CalculateIsTouching())
                {
                    context.ChangeState(PaddleStateID.Aiming);
                    yield break;
                }

                yield return null;
            }
        }
    }
}