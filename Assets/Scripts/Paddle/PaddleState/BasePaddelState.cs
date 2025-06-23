using System.Collections;
using UnityEngine;

namespace BullBrukBruker
{
    public abstract class BasePaddelState : BaseSMState<PaddleStateID>
    {
        protected readonly PaddelController paddelController;

        protected BasePaddelState(ISMContext<PaddleStateID> context, PaddelController paddelController) : base(context)
        {
            this.paddelController = paddelController;
        }
    }
}