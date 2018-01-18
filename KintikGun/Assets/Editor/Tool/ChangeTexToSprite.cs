using UnityEngine;
using System.Collections;
using UnityEditor;

public class ChangeTexToSprite : AssetPostprocessor {
    /*
    void OnPostprocessTexture(Texture2D texture) //when imported in unity
    {
        string lowerCaseAssetPath = assetPath.ToLower ();

        if (lowerCaseAssetPath.IndexOf ("/sprites/") != -1)  //is it in the sprite folder
        {
            TextureImporter textureImporter = (TextureImporter) assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.mipmapEnabled = false;
            textureImporter.maxTextureSize = 512;
            textureImporter.alphaSource = TextureImporterAlphaSource.FromInput;
            textureImporter.alphaIsTransparency = true;
            textureImporter.npotScale = TextureImporterNPOTScale.ToNearest;
        }
        else {
            TextureImporter textureImporter = (TextureImporter) assetImporter;
            textureImporter.textureType = TextureImporterType.Default;
        }
    }
    */
}
