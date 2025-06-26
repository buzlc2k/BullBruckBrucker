using System;
using UnityEngine;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;

namespace BullBrukBruker
{
    public abstract class DataModel<DTO> : IDataModel where DTO : class
    {
        protected string dataKey;
        private DTO data;

        protected abstract DTO CreateDefaultData();

        public virtual IEnumerator LoadData()
        {
            while (data == null)
            {
                string jsonString = SaveSystem.HasKey(dataKey)
                    ? SaveSystem.GetString(dataKey)
                    : null;

                data = !string.IsNullOrEmpty(jsonString)
                        ? JsonUtility.FromJson<DTO>(jsonString)
                        : CreateDefaultData();

                yield return null;
            }
        }   

        public virtual T Read<T>(Expression<Func<object, T>> fieldSelector)
        {
            var func = fieldSelector.Compile();

            return func(data);
        }

        public virtual void Write<T>(Expression<Func<object, T>> fieldSelector, T overridedValue)
        {
            if (fieldSelector.Body is MemberExpression memberExpression
                && memberExpression.Member is FieldInfo fieldInfo)
            {
                fieldInfo.SetValue(data, overridedValue);
            }
        }
        
        public virtual void SaveData()
        {
            string jsonString = JsonUtility.ToJson(data);

            SaveSystem.SetString(dataKey, jsonString);
            SaveSystem.SaveToDisk();
        }
    }
}