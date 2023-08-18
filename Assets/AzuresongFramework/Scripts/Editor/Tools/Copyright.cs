//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：
//  Description：
//------------------------------------------------------------
using System.IO;


namespace Azuresong.Editor
{
    public class Copyright : UnityEditor.AssetModificationProcessor
    {
        private static void OnWillCreateAsset(string path)
        {
            if (path.EndsWith(".meta"))
            {
                path = path.Replace(".meta", "");
                if (path.EndsWith(".cs"))
                {
                    string allText = File.ReadAllText(path);
                    if (allText.Contains("#CreationTime"))
                    {
                        allText = allText.Replace("#CreationTime", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        File.WriteAllText(path, allText);
                        //UnityEditor.AssetDatabase.Refresh();
                    }
                }
            }
        }
    }
}