using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public class LoadState : BaseGameState
    {
        public LoadState(GameManager gameManager, ISMContext<GameStateID> context) : base(gameManager, context)
        {
            id = GameStateID.Load;
        }

        public override IEnumerator UpdateState()
        {
            yield return gameManager.StartCoroutine(base.UpdateState());

            while (PS_SceneManager.Instance.IsLoading || LevelManager.Instance.IsLoading)
                yield return null;
                
            switch (gameManager.PreviousStateID)
            {
                case GameStateID.None:
                    context.ChangeState(GameStateID.MainMenu);
                    break;
                    
                default:
                    context.ChangeState(GameStateID.Play);
                    break;
            }
        }
    }
}