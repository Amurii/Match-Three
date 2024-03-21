using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [Header("Tile settings")]
    public int x;
    public int y;
    public Item item;
    public bool isClicked;
    public Transform ItemTransform;

    public TextMeshProUGUI Place;

    float xSize;
    float ySize;
    public float TileWidth;

    private void Start()
    {
         xSize = item.transform.GetComponent<RectTransform>().rect.width;
         ySize = item.transform.GetComponent<RectTransform>().rect.height;
        TileWidth = GetComponent<RectTransform>().rect.height;
        Place.text = "" + x + "," + y;
       
    }
    public void TileClick(bool clicked)
    {
        isClicked = clicked;
        
    }

    public void TilePointerEnter()
    {
        if (Board.instance.isPointerDown)
        {
            TilePointerDown();
            TilePointerUp();
        }
    }
    public void TilePointerUp()
    {
        if (Board.instance.TileClickedList.Count == 2)
        {
           
            if (IsNeighbor())
            {
                if (Board.instance.TileClickedList[0].item.item != Board.instance.TileClickedList[1].item.item)
                    Board.instance.TileExit();
                else
                {
                    ClearTileList();
                }
            }
            else ClearTileList();
        }
        else
        
            ClearTileList();
        

    }
    void ClearTileList()
    {
        foreach (Tile t in Board.instance.TileClickedList)
            t.TileClick(false);
        Board.instance.TileClickedList.Clear();
        Board.instance.isPointerDown = false;

    }
    bool IsNeighbor()
    {
        List<Tile> tiles = Board.instance.TileClickedList;
        int x = Mathf.Abs(tiles[0].x - tiles[1].x);
        int y= Mathf.Abs(tiles[0].y - tiles[1].y);
        return (x == 0 && y == 1) || (y == 0 && x == 1);
    }
    public void TilePointerDown()
    {
        if (Board.instance.TileClickedList.Count != 2)
        {
            if (!Board.instance.TileClickedList.Contains(this))
            {
                Board.instance.isPointerDown = true;
                Board.instance.TileEnter(this);
                TileClick(!isClicked);
              //  Place.text = "" + x + "," + y;

            }
        }
    }

}
