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

    #if UNITY_EDITOR
    [MenuItem("Assets/Create/Random Tile")]
    public static void CreatePrisonTile()
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