//------------------------------------------------------------
//  Copyright © Chen Jie. All rights reserved.
//  CreationTime：2023/07/20 17:16:24
//  Description：ImportModelAssetPostprocessor
//------------------------------------------------------------
using System.IO;
using UnityEditor;


namespace Sars.Editor
{
    public class ImportModelAssetPostprocessor : AssetPostprocessor
    {
        //[MenuItem("SarsTools/SetModel", false)]
        private static void SetModelIm()
        {
            return;

            string[] models = AssetDatabase.FindAssets("t:Model", new string[]
            {
            "Assets/Art/1F",
            });

            if (models != null && models.Length > 0)
            {
                string rootPath = "Assets/ArtNew";
                string materialPath = rootPath + "/Materials";
                string TexturePath = rootPath + "/Textures";

                if (!AssetDatabase.IsValidFolder(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }

                if (!AssetDatabase.IsValidFolder(materialPath))
                {
                    Directory.CreateDirectory(materialPath);
                }

                if (!AssetDatabase.IsValidFolder(TexturePath))
                {
                    Directory.CreateDirectory(TexturePath);
                }

                for (int i = 0; i < models.Length; i++)
                {
                    ModelImporter importer = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(models[i])) as ModelImporter;

                    importer.materialImportMode = ModelImporterMaterialImportMode.ImportViaMaterialDescription;
                    importer.materialLocation = ModelImporterMaterialLocation.External;
                    importer.materialName = ModelImporterMaterialName.BasedOnModelNameAndMaterialName;
                    importer.materialSearch = ModelImporterMaterialSearch.RecursiveUp;

                    string fileName = importer.assetPath.Remove(0, importer.assetPath.LastIndexOf("/") + 1);
                    AssetDatabase.MoveAsset(importer.assetPath, rootPath + "/" + fileName);
                    AssetDatabase.Refresh();

                    importer.SaveAndReimport();
                    importer.SearchAndRemapMaterials(ModelImporterMaterialName.BasedOnModelNameAndMaterialName, ModelImporterMaterialSearch.RecursiveUp);

                    //importer.materialLocation = ModelImporterMaterialLocation.InPrefab;

                    //Debug.Log(importer.ExtractTextures(TexturePath)) ;
                    //importer.SaveAndReimport();
                }

                AssetDatabase.Refresh();
            }

            //if (keys.Count > 0 || !importer.importMaterials || importer.materialLocation != ModelImporterMaterialLocation.InPrefab)
            //{
            //    var kind = typeof(UnityEngine.Material);
            //    foreach (var it in keys)
            //    {
            //        var id = new AssetImporter.SourceAssetIdentifier();
            //        id.name = it.Key;
            //        id.type = kind;
            //        importer.RemoveRemap(id);
            //        importer.AddRemap(id, tmpmat);
            //    }
            //    importer.materialLocation = ModelImporterMaterialLocation.InPrefab;
            //    importer.SaveAndReimport();
            //}
        }

        private void OnPreprocessModel()
        {
            //ModelImporter model = (ModelImporter)assetImporter;

            //if (model.assetPath.StartsWith("Assets/Art"))
            //{
            //    Debug.LogFormat("Import model: {0}", model.assetPath);

            //}
            //FileInfo modelFile = new FileInfo(model.assetPath);
            //if(modelFile.)
            //model.materialImportMode = ModelImporterMaterialImportMode.ImportViaMaterialDescription;
            //model.materialLocation = ModelImporterMaterialLocation.External;

            //Path.Combine(model.dir)

            //model.ExtractTextures
        }
    }
}
