using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BoosterAbility
{
    ChangeIcons,
    RefreshGrid

}
public class BoosterItem : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Properties")]
    public int itemIndex;
    public int itemCout;
    public Image image;
    public BoosterAbility boosterAbility;
    public LineController ElectricLine;
    [SerializeField] TextMeshProUGUI CounterTxt;
    [SerializeField] GameObject board;

    void Start()
    {
        board = GameObject.Find("FullBoard");
        CounterTxt.text = itemCout.ToString();
    }

    public void BoosterUse()
    {
      //  if (itemCout == 0) return;
        List<Tile> items = new List<Tile>();
        List<LineController> lines = new List<LineController>();
        Sprite ranomSprite = Board.instance.board[Random.Range(0, Board.instance.board.GetLength(0)), Random.Range(0, Board.instance.board.GetLength(1))].item.item;
        for (int x = 0; x < Board.instance.board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.instance.board.GetLength(1); y++)
            {
                if (Board.instance.board[x, y].item.item == ranomSprite)
                {
                    LineController line = Instantiate(ElectricLine) ;
                    line.AssignTarget(transform.position, Board.instance.board[x, y].item.transform);
                    lines.Add(line);
                    items.Add(Board.instance.board[x, y]);
                }
            }
        }
       
        BoosterManager.instance.allBoosters[itemIndex].itemCout--;
        CounterTxt.text = itemCout.ToString();
        //   BoosterManager.instance.boostersUsedInSession.Add(itemIndex, this);
        StartCoroutine(DestroyItems(lines, items));
       
    }
    void ApplyEvent(BoosterAbility ability)
    {
        switch (ability)
        {
            case BoosterAbility.ChangeIcons:
                break;
            case BoosterAbility.RefreshGrid:
                break;
        }
    }
    IEnumerator DestroyItems(List<LineController> lines,List<Tile> items)
    {
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < lines.Count; i++)
        {
            Destroy(lines[i].gameObject);
            Destroy(items[i].item.gameObject);
            items[i].item=null;
            
        }
        Board.instance.FillAfterDestroy();
        if (itemCout <= 0)
            Destroy(gameObject, 0.5f);
      
    }
}
