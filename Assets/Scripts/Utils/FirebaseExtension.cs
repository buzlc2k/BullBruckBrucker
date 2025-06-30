using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

namespace BullBrukBruker
{
    public static class FirebaseExtension
    {
        public static bool IsVariantObject(object obj)
        {
            if (obj == null) return true;

            return IsVariantType(obj.GetType());
        }

        public static bool IsVariantType(Type type)
        {

            if (type.Equals(typeof(int)) || type.Equals(typeof(long)) || type.Equals(typeof(double)) ||
                type.Equals(typeof(string)) || type.Equals(typeof(bool)))
            {
                return true;
            }

            if (type.IsArray)
            {
                return IsVariantType(type.GetElementType());
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return IsVariantType(type.GetGenericArguments()[0]);
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var keyType = type.GetGenericArguments()[0];
                var valueType = type.GetGenericArguments()[1];
                return keyType == typeof(string) && IsVariantType(valueType);
            }

            return false;
        }

        public static async Task SetValueAnsyc(DatabaseReference dbRef, object value)
        {
            if (IsVariantObject(value))
                await dbRef.SetValueAsync(value);
            else
                await dbRef.SetRawJsonValueAsync(JsonUtility.ToJson(value));
        }
    }
}