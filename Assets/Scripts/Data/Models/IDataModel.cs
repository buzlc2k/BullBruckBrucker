using System;
using System.Linq.Expressions;

namespace BullBrukBruker
{
    public interface IDataModel
    {
        public void LoadData();
        public T Read<T>(Expression<Func<object, T>> fieldSelector);
        public void Write<T>(Expression<Func<object, T>> fieldSelector, T overridedValue);
        public void SaveData();
    }
}