using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public class OverState : BaseGameState
    {
        public OverState(GameManager gameManager, ISMContext<GameStateID> context) : base(gameManager, context)
        {
            id = GameStateID.Over;
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
            base.ExitState();
        }
    }
}