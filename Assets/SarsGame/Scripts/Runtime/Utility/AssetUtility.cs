//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/08/14 10:15:44
//  Description：AssetUtility
//------------------------------------------------------------


namespace SarsGame.Runtime
{
    public static class AssetUtility
    {
        public static string GetDataTablePath(string assetName, bool useBytes)
        {
            if (useBytes)
            {
                return $"Assets/SarsGame/DataTables/{assetName}.bytes";
            }

            return $"Assets/SarsGame/DataTables/BinaryFiles/{assetName}.txt";
        }

        public static string GetScenePath(string assetName)
        {
            return $"Assets/SarsGame/Scenes/{assetName}.unity";
        }

        public static string GetUIFormPath(string assetName)
        {
            return $"Assets/SarsGame/UI/Forms/{assetName}.prefab";
        }

        public static string GetEntityPath(string assetName)
        {
            return $"Assets/SarsGame/Entity/{assetName}.prefab";
        }

        public static string GetPlayerPath(string assetName)
        {
            return $"Assets/SarsGame/Entity/Player/{assetName}.prefab";
        }
    }
}