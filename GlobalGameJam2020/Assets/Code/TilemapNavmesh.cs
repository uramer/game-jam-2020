using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapNavmesh : MonoBehaviour
{
    /*[SerializeField]
    private Tilemap tilemap = null;
    [SerializeField]
    private GameObject collider = null;

    private List<GameObject> navMeshElements = new List<GameObject>();

    public void Start () {
        int agentTypeCount = NavMesh.GetSettingsCount();
        if (agentTypeCount < 1) { return; }
        
        BoundsInt bounds = tilemap.cellBounds;
        for(int x = bounds.xMin; x <= bounds.xMax; x++) {
            for(int y = bounds.yMin; y <= bounds.yMax; y++) {
                Vector3Int coords = new Vector3Int(x, y, 0);
                TileBase tileBase = tilemap.GetTile(coords);
                //if(tileBase.GetTileData(coords, ))
                navMeshElements.Add(Instantiate(collider, tilemap.CellToWorld(coords), new Vector3(0, 0, 0)));
            }
        }

        //for (int i = 0; i < navMeshElements.Count; ++i) { navMeshElements[i].transform.SetParent(navMeshRoot.transform, true); }
        for (int i = 0; i < agentTypeCount; ++i) {
            NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex(i);
            NavMeshSurface navMeshSurface = environment.AddComponent<NavMeshSurface>();
            navMeshSurface.agentTypeID = settings.agentTypeID;

            NavMeshBuildSettings actualSettings = navMeshSurface.GetBuildSettings();
            navMeshSurface.useGeometry = NavMeshCollectGeometry.PhysicsColliders; // or you can use RenderMeshes

            navMeshSurface.BuildNavMesh();
        }

    }*/
}
