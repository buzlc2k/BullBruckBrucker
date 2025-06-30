using System;
using System.Collections;
using System.Linq.Expressions;

namespace BullBrukBruker
{
    public interface IDataModel
    {
        public IEnumerator LoadData();
        public T Read<T>(Expression<Func<object, T>> fieldSelector);
        public void Write<T>(Expression<Func<object, T>> fieldSelector, T overridedValue);
        public IEnumerator SaveData();
    }
}