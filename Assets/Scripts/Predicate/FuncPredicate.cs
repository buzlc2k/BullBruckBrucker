using System;

namespace BullBrukBruker
{
    public class FuncPredicate : Predicate
    {
        private readonly Func<bool> func;

        public FuncPredicate(Func<bool> func){
            this.func = func;
        }

        public override bool Evaluate()
        {
            return func();
        }
    }
}