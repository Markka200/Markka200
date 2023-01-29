using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public List<Sprite> Sprites = new List<Sprite>();
    public GameObject TilePrefab;
    public int GridDimension = 8;  //´kuinka monta Tileä on gridillä // tekee pakosta neliön
    public float Distance = 1.0f;   // välin koko tilojen välillä
    private GameObject[,] Grid;  

    private void Start()
    {
        Grid = new GameObject[GridDimension, GridDimension];
        InitGrid();
    }
    public void SwapTiles(Vector2Int tile1Position, Vector2Int tile2Position)
    { 
        GameObject Tile1 = Grid[tile1Position.x, tile1Position.y];
        GameObject Tile2 = Grid[tile2Position.x, tile2Position.y];
        SpriteRenderer renderer1 = Tile1.GetComponent<SpriteRenderer>();
        SpriteRenderer renderer2 = Tile2.GetComponent<SpriteRenderer>();

        Sprite temp = renderer1.sprite;
        renderer1.sprite = renderer2.sprite;
        renderer2.sprite = temp;

        CheckMatches();
    }

    void FillHoles()
    {
        for (int column = 0; column < GridDimension; column++) // rivit käydään läpi alhaalta ylös
        {
            for (int row = 0; row < GridDimension; row++)  // kolumnit käydään läpi vasemmalta oikeelle 
            {
                while (GetSpriteRendererAt(column, row).sprite == null) // getspriterenderer returnaa rendererin jos etitty Tile on gridillä
                {
                    for (int filler = row; filler < GridDimension - 1; filler++)   // kolumni käydään läpi alhaalta ylös
                    {
                        SpriteRenderer current = GetSpriteRendererAt(column, filler);   //etitään renderer
                        SpriteRenderer next = GetSpriteRendererAt(column, filler + 1);  //etitään ylemmän renderer
                        current.sprite = next.sprite;                                                       //spritet vaihtaa paikkoja
                    }
                    SpriteRenderer last = GetSpriteRendererAt(column, GridDimension - 1); //ylintä Tileä ei voi täyttää ylemmällä, joten annetaan sille uusi random sprite
                    last.sprite = Sprites[Random.Range(0, Sprites.Count)];  
                }
            }
        }
    }

    bool CheckMatches()
    {
        HashSet<SpriteRenderer> matchedtiles = new HashSet<SpriteRenderer>();
        for (int row = 0;   row < GridDimension; row++)
        {
            for (int col = 0; col < GridDimension; col++)
            {
                SpriteRenderer current = GetSpriteRendererAt(col, row);
                List<SpriteRenderer> horizontalMatches = FindColumnMatchForTile(col, row, current.sprite);
                if(horizontalMatches.Count >= 2)
                {
                    matchedtiles.UnionWith(horizontalMatches);
                    matchedtiles.Add(current);
                }

                List<SpriteRenderer> verticalMatches = FindRowMatchForTile(col, row, current.sprite);
                if(verticalMatches.Count >= 2)
                {
                    matchedtiles.UnionWith(verticalMatches);
                    matchedtiles.Add(current);
                }
            }
        }
        
        foreach(SpriteRenderer renderer in matchedtiles)
        {
            renderer.sprite = null;
        }
        FillHoles();
        return matchedtiles.Count > 0;
    }
     
    List<SpriteRenderer> FindColumnMatchForTile(int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = col + 1; i < GridDimension; i++)
        {
            SpriteRenderer nextColumn = GetSpriteRendererAt(i, row);
            if (nextColumn.sprite != sprite)
            {
                break;
            }
            result.Add(nextColumn);
        }
        return result;
    }
    List<SpriteRenderer> FindRowMatchForTile(int col, int row, Sprite sprite)
    {
        List<SpriteRenderer> result = new List<SpriteRenderer>();
        for (int i = row + 1; i < GridDimension; i++)
        {
            SpriteRenderer nextRow = GetSpriteRendererAt(col, i);
            if (nextRow.sprite != sprite)
            {
                break;
            }
            result.Add(nextRow);
        }
        return result;
    }


    SpriteRenderer GetSpriteRendererAt(int column, int row) 
    {
        if (column < 0 || column >= GridDimension || row < 0 || row >= GridDimension) //jos etitty Tile on gridin ulkopuolella, niin return null;
            return null;
        GameObject tile = Grid[column, row];    //etitään grid listalta kyseinen gameobject 
        SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>(); // etitään gameobjectin renderer
        return renderer; 
    }
    void InitGrid() // tekee gridin
    {

            Vector3 positionOffset = transform.position - new Vector3(GridDimension * Distance / 2.0f, GridDimension * Distance / 2.0f, 0);   // offset mihin grid tulee
            for (int row = 0; row < GridDimension; row++)  // rivin for loop
            for (int column = 0; column < GridDimension; column++) // kolumnin for loop
            {
                List<Sprite> possibleSprites = new List<Sprite>(Sprites); 

                //eliminoi spritet mitkä loisi kolmen sarjan
                Sprite left1 = GetSpriteAt(column - 1, row); 
                Sprite left2 = GetSpriteAt(column - 2, row);
                if (left2 != null && left1 == left2) 
                {
                    possibleSprites.Remove(left1); 
                }

                Sprite down1 = GetSpriteAt(column, row - 1); 
                Sprite down2 = GetSpriteAt(column, row - 2);
                if (down2 != null && down1 == down2)
                {
                    possibleSprites.Remove(down1);
                }


                GameObject newTile = Instantiate(TilePrefab);   //luo prefabin 
                SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>(); //etsii spriterendererin prefabistä 
                renderer.sprite = possibleSprites[Random.Range(0, possibleSprites.Count)];  //antaa spriten prefabille
                newTile.transform.parent = transform;
                newTile.name = column + ", " + row;
                newTile.transform.position = new Vector3(column * Distance, row * Distance, 0) + positionOffset;  // asettaa objektin oikeaan paikkaan 
                Tile tile = newTile.AddComponent<Tile>();
                tile.Position = new Vector2Int(column, row);
                tile.sprite = renderer.sprite;

                Grid[column, row] = newTile;    //varastoi informaation prefabin sijainnista
            }

            Sprite GetSpriteAt(int column, int row)
            {
                if (column < 0 || column >= GridDimension
                    || row < 0 || row >= GridDimension)
                    return null;
                GameObject tile = Grid[column, row];
                SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
                return renderer.sprite;
            }
            
    }

}
