using UnityEngine;

namespace BullBrukBruker
{
    public abstract class Predicate
    {
        public abstract bool Evaluate();
        public virtual void StopPredicate()
        {
            //For Override
        }
    }   
}