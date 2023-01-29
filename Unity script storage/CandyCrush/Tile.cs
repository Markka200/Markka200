using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private static Tile selected;
    private SpriteRenderer Renderer;
    private GridManager gridManager;
    public Vector2Int Position;
    public Sprite sprite;

    private void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        gridManager = GetComponentInParent<GridManager>();
    }

    public void Select()
    {
        Renderer.color = Color.grey;
    }

    public void Unselect()
    {
        Renderer.color = Color.white;
    }

    private void OnMouseDown()
    {
        if (selected != null)
        {

            if (selected == this) { return; }
            
            selected.Unselect();

            if (Vector2Int.Distance(Position, selected.Position) == 1 )
            {
                if (selected.Renderer.sprite!= this.Renderer.sprite)
                {
                    Debug.Log("Swapper");
                    gridManager.SwapTiles(Position, selected.Position);
                    selected = null;
                }
                else
                {
                    selected = this;
                    Select();
                }
            }
            else
            {
                selected = this;
                Select();
            }
        }
        else
        {
            selected = this;
            Select();
        }
    }

}
