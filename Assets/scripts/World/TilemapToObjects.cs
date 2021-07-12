using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class TilemapToObjects : MonoBehaviour {
    
    public GameObject prefab;
    
    // Start is called before the first frame update
    void Start() {
        Tilemap tilemap = GetComponent<Tilemap>();
        
        for(int x = 0; x < tilemap.size.x; ++x){
            for(int y = 0; y < tilemap.size.y; ++y){
                for(int z = 0; z < tilemap.size.z; ++z){
                    Vector3Int position = tilemap.origin + new Vector3Int(x, y, z);
                    TileBase tile = tilemap.GetTile(position);
                    
                    if(tile != null) {
                        GameObject gameObject = Instantiate(prefab);
                        
                        gameObject.transform.position = position + tilemap.tileAnchor;
                        gameObject.transform.SetParent(transform);
                        gameObject.SetActive(true);
                    }
                }
            } 
        }
        
        tilemap.ClearAllTiles();
        
    }
    
    // Update is called once per frame
    void Update() {
        
    }
}
