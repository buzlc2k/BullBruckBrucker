using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public class PauseState : BaseGameState
    {
        private readonly Predicate continueButtonPredicate;

        public PauseState(GameManager gameManager, ISMContext<GameStateID> context) : base(gameManager, context)
        {
            id = GameStateID.Pause;

            continueButtonPredicate = new EventPredicate(EventID.ContinueButton_Clicked);
        }

        public override void EnterState()
        {
            Time.timeScale = 0;
            base.EnterState();
        }

        public override IEnumerator UpdateState()
        {
            yield return gameManager.StartCoroutine(base.UpdateState());

            while (true)
            {
                if (continueButtonPredicate.Evaluate())
                {
                    context.ChangeState(GameStateID.Play);
                    yield break;
                }

                if (PS_SceneManager.Instance.IsLoading)
                {
                    context.ChangeState(GameStateID.Load);
                    yield break;
                }

                yield return null;
            }
        }

        public override void ExitState()
        {
            Time.timeScale = 1;
            continueButtonPredicate.StopPredicate();
            base.ExitState();
        }
    }
}