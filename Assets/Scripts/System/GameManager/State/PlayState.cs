using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public class PlayState : BaseGameState
    {
        private readonly Predicate outOfBallPredicate;
        private readonly Predicate outOfCellPredicate;
        private readonly Predicate pauseButtonPredicate;

        public PlayState(GameManager gameManager, ISMContext<GameStateID> context) : base(gameManager, context)
        {
            id = GameStateID.Play;

            outOfBallPredicate = new EventPredicate(EventID.OutOfBalls, 0, true);
            outOfCellPredicate  = new EventPredicate(EventID.OutOfCells);
            pauseButtonPredicate = new EventPredicate(EventID.PauseGameButton_Clicked);
        }

        public override IEnumerator UpdateState()
        {
            yield return gameManager.StartCoroutine(base.UpdateState());

            while (true)
            {
                if (pauseButtonPredicate.Evaluate())
                {
                    context.ChangeState(GameStateID.Pause);
                    yield break;
                }

                if (outOfBallPredicate.Evaluate())
                {
                    context.ChangeState(GameStateID.Over);
                    yield break;
                }

                if (outOfCellPredicate.Evaluate())
                {
                    context.ChangeState(GameStateID.Win);
                    yield break;
                }

                yield return null;
            }
        }

        public override void ExitState()
        {
            pauseButtonPredicate.StopPredicate();
            outOfBallPredicate.StopPredicate();
            outOfCellPredicate.StopPredicate();

            base.ExitState();
        }
    }
}