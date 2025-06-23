using UnityEngine;
using UnityEditor;
using System.IO;

namespace BullBrukBruker
{
#if UNITY_EDITOR
    public static class BYDataTableMaker
    {
        [MenuItem("Assets/BY/Create BinaryFile", false, 1)]
        private static void CreateBinaryFile()
        {
            foreach (Object obj in Selection.objects)
            {
                TextAsset txtFile = (TextAsset)obj;
                string tableName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(txtFile));

                ScriptableObject scriptable = ScriptableObject.CreateInstance(tableName);
                if (scriptable == null)
                    return;

                AssetDatabase.CreateAsset(scriptable, "Assets/Resources/Configs/" + tableName + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                BYDataBase bYDataBase = (BYDataBase)scriptable;
                bYDataBase.CreateBinaryFile(txtFile);
                EditorUtility.SetDirty(bYDataBase);
            }
        }
    }
    #endif
}