using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ItemType
{
    Normal,
    RowPop,
    CoulmunPop,
    ItemsPop,
    BombArea,
}
public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Sprite item;
    [SerializeField] public ItemType itemType;
    [SerializeField] public int index;
    [SerializeField] Image icon;

    public ItemType GetItemType()
    {
        return itemType;

    }
    private void Awake()
    {
       

    }
    private void Start()
    {
        this.GetComponent<Image>().sprite = item;
        Rect rect = new Rect();
        rect.x = 0;
        rect.y = 0;
        name = item.name;
        Vector2 size = new Vector2(this.transform.parent.gameObject.transform.parent.GetComponent<RectTransform>().rect.width / 2, this.transform.parent.gameObject.transform.parent.GetComponent<RectTransform>().rect.height / 2);
        this.GetComponent<RectTransform>().sizeDelta = size;
       
    }
    public void ResetTransform()
    {
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }
    public void OnDestroy()
    {
        if (itemType != ItemType.Normal)
        {
            Board.instance.overPowerCreated = false;
            switch (itemType)
            {
                case ItemType.CoulmunPop:
                    CoulmunDestroy();
                    break;
                case ItemType.ItemsPop:
                    SimilurItemsDestroy();
                    break;
                case ItemType.RowPop:
                    RowDestroy();
                    break;
                case ItemType.BombArea:
                    BombAreaDestroy();
                    break;
                default:
                    break;

            }
        }
    }
    void CoulmunDestroy()
    {
        Tile currrentTile = transform.parent.gameObject.GetComponentInParent<Tile>();
        for (int y = 0; y < Board.instance.board.GetLength(0); y++)
        {
            if (Board.instance.board[ currrentTile.x,y].item != null)
            {
                Destroy(Board.instance.board[ currrentTile.x,y].item.gameObject);
                Board.instance.board[ currrentTile.x,y].item = null;
            }
        }
       Board.instance.FillAfterDestroy();
    }
    void SimilurItemsDestroy()
    {
        for(int y = 0; y < Board.instance.board.GetLength(0); y++)
        {
            for (int x = 0; x < Board.instance.board.GetLength(1); x++)
            {
                if (item == Board.instance.board[y, x].item.item)
                {
                    Destroy(Board.instance.board[y,x].item.gameObject);
                    Board.instance.board[y, x].item = null;
                }
            }
        }
        Board.instance.FillAfterDestroy();
    }
    void RowDestroy()
    {
        Tile currrentTile = transform.parent.gameObject.GetComponentInParent<Tile>();
        for (int x = 0; x < Board.instance.board.GetLength(0); x++)
        {
            if (Board.instance.board[x, currrentTile.y].item != null)
            {
                Destroy(Board.instance.board[x, currrentTile.y].item.gameObject);
                Board.instance.board[x, currrentTile.y].item = null;
            }
        }
        Board.instance.FillAfterDestroy();
    }
    void BombAreaDestroy()
    {
        Tile currrentTile = transform.parent.gameObject.GetComponentInParent<Tile>();
        //There is 8 Neighbor at max
        for(int i =currrentTile.x-1;i<currrentTile.x+2;i++)
        {
            for(int j = currrentTile.y - 1; j < currrentTile.y + 2; j ++)
            {
                if (i < 0 || i >= Board.instance.board.GetLength(0)) break;
                if (j >= 0 && j < Board.instance.board.GetLength(0))
                {
                    if (!(i == currrentTile.x && j == currrentTile.y))
                    {
                        if (Board.instance.board[i, j].item != null)
                        {
                            Destroy(Board.instance.board[i, j].item.gameObject);
                            Board.instance.board[i, j].item = null;
                        }
                    }
                }
            }
        }
        Board.instance.FillAfterDestroy();
    }
}
