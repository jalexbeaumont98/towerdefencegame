using UnityEngine;
using UnityEditor;

public class TextureImportSettings : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        TextureImporter importer = (TextureImporter)assetImporter;

        if (importer.assetPath.Contains("Sprites")) // Only apply to textures in "Sprites" folder
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spritePixelsPerUnit = 8f; // Set to your PPU
            importer.filterMode = FilterMode.Point; // Good for pixel art
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            //importer.spriteImportMode = SpriteImportMode.Single; // Or Multiple if you're slicing
        }
    }
}

