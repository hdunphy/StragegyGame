using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer Renderer;
    public Sprite[] TileGraphics;
    public Color highlightColor;
    public Color createColor;
    public bool isCreatable;
    public bool isWalkable;
    GameMaster gm;

    public LayerMask obstacleLayer;

    // Start is called before the first frame update
    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        int randTile = UnityEngine.Random.Range(0, TileGraphics.Length);
        Renderer.sprite = TileGraphics[randTile];
        gm = FindObjectOfType<GameMaster>();
    }

    private void OnMouseDown()
    {
        if (isWalkable && gm.SelectedUnit != null)
        {
            gm.SelectedUnit.Move(this.transform.position);
        }
        else if (isCreatable)
        {
            Debug.Log("Create");
            BarrackItem item = Instantiate(gm.purchasedItem, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            gm.ResetTiles();
            Unit unit = item.GetComponent<Unit>();
            if(unit != null)
            {
                unit.hasAttacked = true;
                unit.hasMoved = true;
            }
        }
    }

    public void SetCreatable()
    {
        Renderer.color = createColor;
        isCreatable = true;
    }

    public bool IsClear()
    {
        Collider2D obstacle = Physics2D.OverlapCircle(transform.position, 0.2f, obstacleLayer);
        return obstacle == null;
    }

    public void Highlight()
    {
        Renderer.color = highlightColor;
        isWalkable = true;
    }

    public void ResetTile()
    {
        Renderer.color = Color.white;
        isWalkable = false;
        isCreatable = false;
    }
}
