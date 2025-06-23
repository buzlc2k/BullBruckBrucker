using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public class PaddleCatchingState : BasePaddelState
    {
        private readonly Predicate outOfBallPredicate;

        public PaddleCatchingState(ISMContext<PaddleStateID> context, PaddelController paddelController) : base(context, paddelController)
        {
            id = PaddleStateID.Catching;

            outOfBallPredicate = new EventPredicate(EventID.OutOfBalls, 0, false);
        }

        public override IEnumerator UpdateState()
        {
            while (true)
            {
                if (InputManager.Instance.CalculateIsTouching())
                {
                    paddelController.Moving.Move();

                    paddelController.Collision.CalculateCellGroup();
                    paddelController.Collision.CalculateCollision();
                }

                if (outOfBallPredicate.Evaluate())
                {
                    while (InputManager.Instance.CalculateIsTouching())
                        yield return null;
                    context.ChangeState(PaddleStateID.Ideling);
                    yield break;
                }

                yield return null;
            }
        }

        public override void ExitState()
        {
            outOfBallPredicate.StopPredicate();

            base.ExitState();
        }
    }
}