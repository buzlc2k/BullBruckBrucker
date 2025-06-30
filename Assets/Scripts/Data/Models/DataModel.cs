using System;
using UnityEngine;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BullBrukBruker
{
    public abstract class DataModel<DTO> : IDataModel where DTO : class
    {
        protected string dataNode;
        protected string userID;
        protected DatabaseReference dbRef;
        protected DTO data;

        protected abstract DTO CreateDefaultData();

        public virtual IEnumerator LoadData()
        {
            var getValueTask = dbRef.GetValueAsync();

            yield return new YieldTask(getValueTask);

            DataSnapshot snapshot = getValueTask.Result;
            string jsonString = snapshot.GetRawJsonValue();

            if (!string.IsNullOrEmpty(jsonString))
            {
                data = JsonUtility.FromJson<DTO>(jsonString);
                yield return null;
            }
            else
            {
                data = CreateDefaultData();
                yield return DataManager.Instance.StartCoroutine(SaveData());
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

                DataManager.Instance.StartCoroutine(SaveFieldData(fieldInfo, overridedValue));
            }
        }

        protected virtual IEnumerator SaveFieldData<T>(FieldInfo fieldInfo, T overridedValue)
        {
            var fieldRef = dbRef.Child(fieldInfo.Name);
            yield return new YieldTask(FirebaseExtension.SetValueAnsyc(fieldRef, overridedValue));
        }

        public virtual IEnumerator SaveData()
        {
            yield return new YieldTask(FirebaseExtension.SetValueAnsyc(dbRef, data));
        }
    }
}