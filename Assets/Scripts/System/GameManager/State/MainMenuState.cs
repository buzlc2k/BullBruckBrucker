using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BullBrukBruker
{
    public class MainMenuState : BaseGameState
    {
        private readonly Predicate selectLevelPredicate;

        public MainMenuState(GameManager gameManager, ISMContext<GameStateID> context) : base(gameManager, context)
        {
            id = GameStateID.MainMenu;

            selectLevelPredicate = new EventPredicate(EventID.SelectLevelButton_Clicked);
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

                if (LevelManager.Instance.IsLoading)
                {
                    context.ChangeState(GameStateID.Load);
                    yield break;
                }

                if (selectLevelPredicate.Evaluate())
                {
                    context.ChangeState(GameStateID.SelectLevel);
                    yield break;
                }

                yield return null;
            }
        }
        
        public override void ExitState()
        {
            selectLevelPredicate.StopPredicate();

            base.ExitState();
        }
    }
}