using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class TilemapToBreakable : MonoBehaviour {
    
    // Start is called before the first frame update
    void Start() {
        Tilemap tilemap = GetComponent<Tilemap>();
        
        List<Vector3Int> positions = new List<Vector3Int>();
        
        for(int x = 0; x < tilemap.size.x; ++x){
            for(int y = 0; y < tilemap.size.y; ++y){
                for(int z = 0; z < tilemap.size.z; ++z){
                    Vector3Int position = tilemap.origin + new Vector3Int(x, y, z);
                    TileBase tile = tilemap.GetTile(position);
                    
                    if(tile != null) {
                        positions.Add(position);
                    }
                }
            } 
        }
        
        foreach(Vector3Int position in positions) {
            GameObject gameObject = new GameObject();
            
            gameObject.transform.position = position + tilemap.tileAnchor;
            
            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = tilemap.GetSprite(position);
            spriteRenderer.color = tilemap.GetColor(position);
            
            Damageable damageable = gameObject.AddComponent<Damageable>();
            damageable.health = 1;
            
            gameObject.AddComponent<BoxCollider2D>();
            
            gameObject.layer = LayerMask.NameToLayer("Ground");
            
            gameObject.transform.SetParent(transform);
            
        }
        
        tilemap.ClearAllTiles();
        
    }
    
    // Update is called once per frame
    void Update() {
        
    }
    
}
