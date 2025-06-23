using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public class SelectLevelState : BaseGameState
    {
        private readonly Predicate returnMenuPredicate;

        public SelectLevelState(GameManager gameManager, ISMContext<GameStateID> context) : base(gameManager, context)
        {
            id = GameStateID.SelectLevel;

            returnMenuPredicate = new EventPredicate(EventID.ReturnMenuButton_Clicked);
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

                if (returnMenuPredicate.Evaluate())
                {
                    context.ChangeState(GameStateID.MainMenu);
                    yield break;
                }

                yield return null;
            }
        }

        public override void ExitState()
        {
            returnMenuPredicate.StopPredicate();

            base.ExitState();
        }
    }
}