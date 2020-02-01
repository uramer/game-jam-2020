using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTile : Tile
{
    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private float[] spriteWeights;

    [SerializeField]
    private bool animated = false;

    [SerializeField]
    private float minAnimationSpeed = 1f;
    [SerializeField]
    private float maxAnimationSpeed = 1f;

    [SerializeField]
    private Sprite preview;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        float totalWeight = 0;
        for(int i = 0; i < sprites.Length; i++)
        {
            totalWeight += spriteWeights[i];
        }

        float random = Random.Range(0f, totalWeight);

        float weight = 0f;
        int chosen = 0;
        for(chosen = 0; chosen < sprites.Length; chosen ++)
        {
            weight += spriteWeights[chosen];
            if (weight > random) break;
        }
        tileData.sprite = sprites[chosen];
    }

    public override bool GetTileAnimationData(Vector3Int location, ITilemap tileMap, ref TileAnimationData tileAnimationData)
    {
        if (animated)
        {
            tileAnimationData.animatedSprites = sprites;
            tileAnimationData.animationSpeed = Random.Range(minAnimationSpeed, maxAnimationSpeed);
            tileAnimationData.animationStartTime = Random.Range(0f, tileAnimationData.animationSpeed * sprites.Length);
            return true;
        }
        return false;
    }

    #if UNITY_EDITOR
    [MenuItem("Assets/Create/Random Tile")]
    public static void CreateRandomTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save RandomTile", "New RandomTile", "asset", "Save RandomTile", "Assets");
        if (path == "")
        {
            return;
        }

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<RandomTile>(), path);
    }
    #endif
}