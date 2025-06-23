using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Reflection;
using System;
using SABI;
using UnityEditor;

namespace BullBrukBruker
{
    public class BYDataBase : ScriptableObject
    {
        public virtual void CreateBinaryFile(TextAsset csvText)
        {

        }
    }
    
    public class ConfigCompare<T> : IComparer<T> where T : class, new()
    {
        private List<FieldInfo> keyInfos = new();
        public ConfigCompare(params string[] keyInfoNames) // ConfigCompareKey("a","b","c")
        {
            for (int i = 0; i < keyInfoNames.Length; i++)
            {
                FieldInfo keyInfo = typeof(T).GetField(keyInfoNames[i], BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                keyInfos.Add(keyInfo);
            }

        }
        public int Compare(T x, T y)
        {
            int result = 0;
            for (int i = 0; i < keyInfos.Count; i++)
            {
                object val_x = keyInfos[i].GetValue(x);
                object val_y = keyInfos[i].GetValue(y);

                result = ((IComparable)val_x).CompareTo(val_y);

                if (result != 0)
                {
                    break;
                }
            }

            return result;
        }

        public T SetValueSearch(params object[] value)
        {
            T key = new();

            for (int i = 0; i < value.Length; i++)
                keyInfos[i].SetValue(key, value[i]);

            return key;
        }
    }

    public abstract class BYDataTable<T> : BYDataBase where T : class, new()
    {
        [SerializeField, SerializeReference] protected internal List<T> Records = new();
        protected ConfigCompare<T> configCompare;

        public abstract ConfigCompare<T> DefineConfigCompare();

        public void OnEnable()
        {
            DefineConfigCompare();
        }

#if UNITY_EDITOR
        [Button(height: 17, textSize: 15)]
        protected void AddRecord(string typeName)
        {
            Assembly assembly = typeof(T).Assembly;
            Type recordType = assembly.GetType($"{GetType().Namespace}.{typeName}");

            if (recordType.IsSubclassOf(typeof(T)) || recordType.Equals(typeof(T)))
            {
                var newInstance = Activator.CreateInstance(recordType);
                Records.Add((T)newInstance);
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();

                Debug.Log("======CREATE NEW RECORD SUCESSS, DONT FORGET TO MODIFY IN CSV FILE======");
            }
            else
            {
               Debug.Log($"======{typeName} IS NOT {typeof(T)}======"); 
            }
        }

        public override void CreateBinaryFile(TextAsset csvText)
        {
            DefineConfigCompare();

            Records ??= new();
            Records.Clear();

            List<List<string>> grids = SplitCSVFile(csvText);

            Assembly assembly = typeof(T).Assembly;

            for (int i = 0; i < grids.Count; i++)
            {
                List<string> dataLine = grids[i];

                string typeName = dataLine[0];
                Type recordType = assembly.GetType($"{GetType().Namespace}.{typeName}");
                FieldInfo[] fieldInfos = Utils.GetSortedFieldInfo(recordType);

                string jsonText = "{";
                for (int x = 0; x < fieldInfos.Length; x++)
                {
                    if (x > 0)
                        jsonText += ",";

                        if(fieldInfos[x].FieldType.IsGenericType && fieldInfos[x].FieldType.GetGenericTypeDefinition() == typeof(List<>))
                            jsonText += "\"" + fieldInfos[x].Name + "\":" + "[" + dataLine[x + 1]+ "]";

                        else if (fieldInfos[x].FieldType != typeof(string))
                        jsonText += "\"" + fieldInfos[x].Name + "\":" + dataLine[x + 1];

                    else
                        jsonText += "\"" + fieldInfos[x].Name + "\":\"" + dataLine[x + 1] + "\"";
                    }
                    jsonText += "}";
                    Debug.Log(jsonText);

                    T r = (T)JsonUtility.FromJson(jsonText, recordType);
                    Records.Add(r);
            }

            Records.Sort(configCompare);
        }

        private List<List<string>> SplitCSVFile(TextAsset csvText)
        {
            List<List<string>> grids = new();

            string[] lines = csvText.text.Split('\n');

            if (lines.Length <= 1) Debug.Log("Empty CSV");

            for (int i = 1; i < lines.Length; i++)
            {
                string s = lines[i];
                if (s.CompareTo(string.Empty) != 0)
                {
                    string pattern =  @",(?=(?:[^""]*""[^""]*"")*[^""]*$)";
                    string[] lineData = Regex.Split(s, pattern);
                    List<string> lsLine = new();
                    foreach (string e in lineData)
                    {
                        string newchar = Regex.Replace(e, @"\t|\n|\r", "");
                        newchar = Regex.Replace(newchar, @"""", @""); 
                        newchar = Regex.Replace(newchar, @"\\", @"\\"); 

                        if (!String.IsNullOrEmpty(newchar))
                            lsLine.Add(newchar);
                    }

                    grids.Add(lsLine);
                }
            }
            return grids;
        }
#endif

        public List<T> GetAllRecord() => Records;

        public T GetRecordByKeySearch(params object[] key)
        {
            T objectkey = configCompare.SetValueSearch(key);

            int index = Records.BinarySearch(objectkey, configCompare);

            if (index >= 0 && index < Records.Count)
                return Records[index];
            else
                return null;
        }
    }
}