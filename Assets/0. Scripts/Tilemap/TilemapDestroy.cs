using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDestroy : MonoBehaviour
{
    public Tilemap tilemap;
    public List<Vector3Int> allTilePos = new List<Vector3Int>();
    public float destroyTime = 2f;
    //public Sprite breakingSprite;

    void Start()
    {
        GetTiles();
        //StartCoroutine(DestroyTiles());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GetTiles()
    {
        BoundsInt boundsInt = tilemap.cellBounds;
        foreach (Vector3Int tilePos in boundsInt.allPositionsWithin)
        {
            if (tilemap.HasTile(tilePos))
            {
                allTilePos.Add(tilePos);
            }
        }

    }

    public IEnumerator DestroyTiles()
    {
        while (allTilePos.Count>0) 
        {
            int randomIndex = Random.Range(0, allTilePos.Count);
            //Debug.Log(randomIndex);
            Vector3Int tileToDestroy = allTilePos[randomIndex];
            //Tile currentTile = tilemap.GetTile<Tile>(tileToDestroy);
            //currentTile.sprite = breakingSprite;
            //Color originalColor = tilemap.GetColor(tileToDestroy);
            float timeElapsed = 0;
            
            while (timeElapsed < destroyTime)
            {   
                
                timeElapsed += Time.deltaTime;
                //Color newColor = Color.Lerp(originalColor, Color.black, timeElapsed/destroyTime);
                //tilemap.SetColor(tileToDestroy, newColor);
                //Debug.Log(newColor);
                yield return null;
            }
            tilemap.SetTile(tileToDestroy,null);
            allTilePos.RemoveAt(randomIndex);
            yield return null;

        }
    }
}
