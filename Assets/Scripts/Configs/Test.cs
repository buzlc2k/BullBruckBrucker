using UnityEngine;
using System;
using SABI;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BullBrukBruker
{
    [Serializable]
    public class ParentRecord
    {
        public int Field1;
        public int Field2;
    }

    [Serializable]
    public class Child1Record : ParentRecord
    {
        public int Field3;
    }

    [Serializable]
    public class Child2Record : ParentRecord
    {
        public List<int> Field4;
    }
    
    public class Test : BYDataTable<ParentRecord>
    {
        [SerializeField] TextAsset csvFile;

        [Button(height: 17, textSize: 15)]
        private void Testing()
        {
            string[] lines = csvFile.text.Split('\n');

            for (int i = 1; i < lines.Length; i++)
            {
                string s = lines[i];
                if (s.CompareTo(string.Empty) != 0)
                {
                    string pattern =  @",(?=(?:[^""]*""[^""]*"")*[^""]*$)";
                    string[] lineData = Regex.Split(s, pattern);
                    foreach (string e in lineData)
                    {
                        string newchar = Regex.Replace(e, @"\t|\n|\r", "");
                        newchar = Regex.Replace(newchar, @"""(?=$)", @"]"); 
                        newchar = Regex.Replace(newchar, @"""(?!$)", @"[");
                        newchar = Regex.Replace(newchar, @"\\", @"\\"); 
                        Debug.Log(newchar);
                    }
                }
            }
        }

        public override ConfigCompare<ParentRecord> DefineConfigCompare()
        {
            configCompare = new ConfigCompare<ParentRecord>("Field1");
            return configCompare;
        }
    }
}