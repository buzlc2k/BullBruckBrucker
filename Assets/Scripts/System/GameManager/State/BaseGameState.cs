using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public abstract class BaseGameState : BaseSMState<GameStateID>
    {
        protected GameManager gameManager;

        public BaseGameState(GameManager gameManager, ISMContext<GameStateID> context) : base(context)
        {
            this.gameManager = gameManager;
        }

        public override void EnterState()
        {
            gameManager.CurrentStateID = id;
            base.EnterState();
        }

        public override IEnumerator UpdateState()
        {
            yield return gameManager.StartCoroutine(CanvasManager.Instance.SetActiveCanvas(id));
        }

        public override void ExitState()
        {
            gameManager.PreviousStateID = id;
            base.ExitState();
        }
    }
}