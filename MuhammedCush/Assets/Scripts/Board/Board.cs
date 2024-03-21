using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public enum PatternShape
    {
        Vertical,
        Horizontal,
        L,
        T,
        H,
        None
    }

    [Header("Board Setting")]
    [SerializeField] GameObject Row;
    [SerializeField] Tile TileObject;
    
    [HideInInspector]public List<Tile> TileClickedList=new List<Tile>();
    [Header("Items")]
    [SerializeField] List<Item> itemsList;
    [SerializeField] List<Item> overPowerList;
    private Dictionary<int, ItemChance> _item_Chance = new Dictionary<int, ItemChance>();

    //  List<Tile> MatchedList = new List<Tile>();

    [Header("Private Settings")]
    [HideInInspector]public bool isPointerDown = false;
    [HideInInspector] public bool overPowerCreated = false;
    private Item OverPowerTile;
    private ItemType overPowerType;
    bool isMatchedDestroy;
    bool isTilesCheked = false;
    bool TileMoved = false;
    Vector2 firstItemOriginalPos;
    Vector2 secondItemOriginalPos;
    

    public static Board instance;
    private void Awake()
    {
        instance = this;
    }

    public Tile[,] board;
    private void Start()
    {
        ChanceSetup();
     //   if (SaveManager.instance.state.currentBoard == null)
            Setup();
      //  else LoadBoard();
        
    }
    private void ChanceSetup()
    {
        int BoardSize = (int)GameManager.instance.GetGridSize().x * (int)GameManager.instance.GetGridSize().x;
        int itemChance = BoardSize / itemsList.Count;
        int moudChance = BoardSize % itemsList.Count;
        foreach (Item item in itemsList)
            _item_Chance.Add(item.index,new ItemChance(item.item, itemChance));
        foreach (var item in _item_Chance)
        {
            if (moudChance > 0)
            {
                _item_Chance[itemsList[Random.Range(0, itemsList.Count)].index].Chance += 1;
                moudChance--;
            }
        }
        


        ////Chcek the chance for every one
        //foreach (var item in _item_Chance)
        //{
        //    Debug.Log("In the item " + item.Value.Sprite + "There " + item.Value.Chance + " Chence");
        //}


    }
    private void Setup()
    {
        Vector2 boardSize = GameManager.instance.GetGridSize();
        float tileSizeX = this.GetComponent<RectTransform>().rect.width/boardSize.x;
        float tileSizeY = this.GetComponent<RectTransform>().rect.height/ boardSize.y;

        int sizeX = (int)boardSize.x;
        int sizeY = (int)boardSize.y;
        board = new Tile[sizeX, sizeY];
        GameObject rowRefrence=new GameObject();
        //Generate the tiles and rows
        for (int x = 0; x < sizeX; x++)
        {
            rowRefrence = Instantiate(Row, this.gameObject.transform);
            //this.GetComponent<GridLayoutGroup>().cellSize =new Vector2(rowRefrence.transform.GetComponent<RectTransform>().rect.width, this.GetComponent<GridLayoutGroup>().cellSize.y);
            for (int y = 0; y < sizeY; y++)
            {
                Tile tile = Instantiate(TileObject, rowRefrence.transform);
                tile.x = x;
                tile.y = y;
                RectTransform rt  =tile.transform.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(tileSizeX, tileSizeY);
                // Item item= Instantiate(itemsList[GetItemIndexByChance()]).GetComponent<Item>();
                Item item = Instantiate(itemsList[Random.Range(0,itemsList.Count)]).GetComponent<Item>();
                tile.item = item;
                tile.item.gameObject.transform.SetParent(tile.ItemTransform, false);
                tile.item.ResetTransform();
                board[x, y] = tile;
                EnsureNoMatchingNeighbor(tile, x, y);
            }
        }
        this.GetComponent<GridLayoutGroup>().cellSize =new Vector2(board[0,0].GetComponent<RectTransform>().rect.width/2, this.GetComponent<GridLayoutGroup>().cellSize.y);
        this.GetComponent<GridLayoutGroup>().spacing = new Vector2(board[0, 0].GetComponent<RectTransform>().rect.width / 2, this.GetComponent<GridLayoutGroup>().spacing.y);
       // SaveManager.instance.state.currentBoard = board;
       // SaveManager.instance.Save();
    }   
    void EnsureNoMatchingNeighbor(Tile tile, int x,int y) 
    {
        if (x > 0 && TilesMatch(tile, board[x - 1, y]))
        {
            // Change the type or color of the current tile
            ChangeTileType(tile);
        }

        if (y > 0 && TilesMatch(tile, board[x, y - 1]))
        {
            // Change the type or color of the current tile
            ChangeTileType(tile);
        }
    }
    bool TilesMatch(Tile tile1, Tile tile2)
    {
        //Check if tile has ability
        if (tile1.item.GetItemType() != ItemType.Normal || tile2.item.GetItemType() != ItemType.Normal)
            return true;
        // Customize this based on your matching logic
        return tile1.item.item == tile2.item.item;
    }

    void ChangeTileType(Tile tile)
    {
        //Customize this based on your game's logic to change the tile type or color
        Sprite sprite = tile.item.item;

        _item_Chance[tile.item.index].Chance++;
       // Debug.Log(tile.x + "" + tile.y + "and it was" + tile.item.name);
        //  var copyTile = tile.item;
        //      Destroy(copyTile.gameObject,0.3f);
        while (tile.item.item == sprite)
        {
            //TileClickedList.Add(tile);
            //Tile t = board[Random.Range(0, board.GetLength(0)), Random.Range(0, board.GetLength(1))];
            //while (t==null)
            //    t = board[Random.Range(0, board.GetLength(0)), Random.Range(0, board.GetLength(1))];
            //TileClickedList.Add(t);
            //TileItemsChange();
            //TileClickedList.Clear();
            tile.item.item = itemsList[Random.Range(0, itemsList.Count)].item;
;        }
        tile.item.GetComponent<Image>().sprite = tile.item.item;
        tile.item.name = tile.item.item.name;
       // Instantiate(tile.item, tile.ItemTransform);

    }
    public void TileEnter(Tile clickedTile)
    {
        TileClickedList.Add(clickedTile);
         if (TileClickedList.Count == 1) 
            firstItemOriginalPos = TileClickedList[0].gameObject.transform.position;
         else if (TileClickedList.Count == 2) 
            secondItemOriginalPos = TileClickedList[1].gameObject.transform.position;
    }
    public void TileExit()
    {
        if (GameManager.instance.GetMoves() == 0)
        {
            TileClickedList.Clear();
            return;
        }
      
         MoveTiles();
        Tile temp;
        for (int i = 0; i < TileClickedList.Count; i++)
            if (TileClickedList[i].item == OverPowerTile)
            {
                OverPowerTile.itemType = overPowerType;
                

                if (overPowerType == ItemType.ItemsPop)
                {
                    if (i == 0) temp = TileClickedList[1];
                    else temp = TileClickedList[0];
                    OverPowerTile.item = temp.item.item;
                }


            }
            
       // TileItemsChange();
    }
    void TileItemsChange()
    {
        //Swap
        if (TileClickedList.Count == 2)
        {
            Item item = TileClickedList[1].item;
            TileClickedList[0].item.transform.SetParent(TileClickedList[1].ItemTransform);
            TileClickedList[1].item.transform.SetParent(TileClickedList[0].ItemTransform);
            //Items change
            TileClickedList[1].item = TileClickedList[0].item;
            TileClickedList[0].item = item;
            //Update matrix
            board[TileClickedList[0].x, TileClickedList[0].y].item = TileClickedList[0].item;
            board[TileClickedList[1].x, TileClickedList[1].y].item = TileClickedList[1].item;

            
        }
        
        //Clear tiles
       

        //  CheckMatches();

        isPointerDown = false;
    }
    void MoveTiles()
    {
        if (!TileMoved)
        {
            TileMoved = true;
           
            LeanTween.move(TileClickedList[1].item.gameObject, firstItemOriginalPos, 0.3f);
            LeanTween.move(TileClickedList[0].item.gameObject, secondItemOriginalPos, 0.3f);
            TileItemsChange();
            StartCoroutine(LeanTweenCompleate());
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator LeanTweenCompleate()
    {
        yield return new WaitForSeconds(0.5f);
        TileMoved = false;
        
        if (CheckMatchedTiles())
        {
            //he changed
            GamePlay.instance.ItemMove();
        }
        else
        {
            if (TileClickedList.Count > 0)
            {
                GamePlay.instance.ItemMove();
                //Re back tile to orignail
                //Swap tiles
                List<Tile> temp = new List<Tile>();
                temp.Add(TileClickedList[1]);
                temp.Add(TileClickedList[0]);
                firstItemOriginalPos = TileClickedList[1].gameObject.transform.position;
                secondItemOriginalPos = TileClickedList[0].gameObject.transform.position;
                TileClickedList.Clear();
                TileClickedList = temp;
                MoveTiles();

                //Fail sound on
                SoundManager.instance.AudioPlay(AudioClips.Fail);
            }

        }

        foreach (Tile t in TileClickedList)
            t.TileClick(false);
        TileClickedList.Clear();
        firstItemOriginalPos = Vector2.zero;
        secondItemOriginalPos = Vector2.zero;   
    }
    /// <summary>
    /// Will fill the null value after destroyed
    /// </summary>
    public void FillAfterDestroy()
    {
        int nullCounter = 0;
        for(int y = 0; y< board.GetLength(0) ; y++)
        {
            nullCounter = 0;
            for (int x = 0; x < board.GetLength(0) ; x++)
            {
                if (board[y, x].item == null)
                {
                    nullCounter++;
                }
            }
            if (nullCounter > 0) 
                ItemsDown(y,nullCounter);
        }
    }
    void ItemsDown(int y,int nullCounter)
    {
        List<Item> localItemList = new List<Item>();
        ChangeItemsPostions(y);
        Item localItem;
        for (int i = 0; i < nullCounter; i++)
        {
            if (!overPowerCreated)
            {
                int chance = GameManager.instance.GetAbilityChance();
                int rand = Random.Range(0, 101);
                if (rand < chance)
                {
                    overPowerCreated = true;
                    localItem = Instantiate(overPowerList[Random.Range(0, overPowerList.Count - 1)], new Vector3(0, 50f, 0), Quaternion.identity);
                    overPowerType = localItem.GetItemType();
                    OverPowerTile = localItem;
                    localItem.itemType = ItemType.Normal;
                    Debug.Log(rand);
                }
                else
                    localItem = Instantiate(itemsList[Random.Range(0, itemsList.Count)], new Vector3(0, 50f, 0), Quaternion.identity);
            }
            else localItem = Instantiate(itemsList[Random.Range(0,itemsList.Count)], new Vector3(0, 50f, 0), Quaternion.identity);
            localItemList.Add(localItem);
            board[y, i].item = localItem;
            localItem.transform.SetParent(board[y, i].ItemTransform,false);
        }
        for(int i = localItemList.Count-1,xPostion=0; i >=0; i--,xPostion++) 
            board[y,xPostion].item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, board[y, i].GetComponent<RectTransform>().rect.height * ((i+board[y,xPostion].y)+1));

        ChangeItemsPostions(y);
        //Audio ON !
       
    }

    void LeanTweenMovment(Item item)
    {
       if(!LeanTween.isTweening(item.gameObject))
        LeanTween.move(item.GetComponent<RectTransform>(), new Vector2(0, 0), GameManager.instance.GetTileMovmentSpeed()).setEase(GameManager.instance.leanTweenType);
        
    }


    void ChangeItemsPostions(int y)
    {
        for (int x = board.GetLength(1) - 1; x >= 0; x--)
        {
            int localX = x;
            while (board[y, x].item == null)
            {
                if (localX == 0) break;
                if (board[y, localX - 1].item != null)
                {
                    board[y, x].item = board[y, localX - 1].item;
                    board[y, x].item.transform.SetParent(board[y, x].ItemTransform);
                    board[y, localX - 1].item = null;
                }
                localX--;
            }
            if (board[y, x].item != null) 
                LeanTweenMovment(board[y, x].item);
        }
        StartCoroutine(LeanTweenMovmentCompleate(0.7f));
    }

    //This function that find all matched items in the board
    IEnumerator LeanTweenMovmentCompleate(float time)
    {
        yield return new WaitForSeconds(time);
      //  Debug.Log("End");
       // SoundManager.instance.AudioPlay(AudioClips.ItemsFall);
        CheckMatchedTiles();
    }
    /// <summary>
    /// Check all the matched tile and return if at least there is three items matched
    /// </summary>
    /// <returns></returns>
   bool CheckMatchedTiles()
    {
        isTilesCheked = true;
        bool isDestroyed=false;
        List<List<Tile>> matchedPatterns=  CheckMatchingPatterns(board);
        foreach (var pattern in matchedPatterns)
        {
            isDestroyed = true;
            DestroyItems(pattern);
        }
        FillAfterDestroy();
        return isDestroyed;
        // GamePlay.instance.TargetUpdate(matched.Count);
        ////Debug.Log(counter++);
        // for(int x = 0; x < board.GetLength(0); x++)
        // {
        //     for (int y = 0; y< board.GetLength(1); y++)
        //     {
        //         List<Tile> verticalToHorizontal = TileMatched(board[x, y],"Vertical");
        //         List<Tile> horizontalToVertical = TileMatched(board[x, y],"Horizontal");
        //         if (verticalToHorizontal.Count >= 3)
        //         {
        //             DestroyItems(verticalToHorizontal);
        //             verticalToHorizontal.Clear();
        //             return true;
        //         }
        //         if (horizontalToVertical.Count >= 3)
        //         {
        //             DestroyItems(horizontalToVertical);
        //             horizontalToVertical.Clear();
        //             return true;

        //         }
        //     }
        // }


    }
   
    /// <summary>
    /// Destroy the item in every tile in the list
    /// </summary>
    /// <param name="matched"></param>
    void DestroyItems(List<Tile> matched)
    {
        //if (matched.Count > 2)
        //{
        //    CreatePowerupByPatternShape(CheckMatchedListShape(matched), matched);
        //}
       // else
      //  {
            for (int i = 0; i < matched.Count; i++)
            {
                //_item_Chance[matched[i].item.index].Chance++;
                if (matched[i].item != null)
                {
                    Destroy(matched[i].item.gameObject);
                    matched[i].item = null;
                }

            }
       // }

    }
    void CreatePowerupByPatternShape(PatternShape shape, List<Tile> matched)
    {
        switch (shape)
        {
            case PatternShape.Horizontal:
            case  PatternShape.Vertical:
                if (matched.Count == 3)
                {
                    //Destroy random tile item
                    int rand = Random.Range(0, matched.Count);
                    Destroy(matched[rand].item.gameObject);
                    matched[rand].item = null;

                    //Give row or similir type
                    overPowerCreated = true;
                    int randOP= Random.Range(0, 2);
                    Item localItem = Instantiate(overPowerList[randOP], matched[rand].transform);
                    overPowerType = localItem.GetItemType();
                    OverPowerTile = localItem;
                    if (randOP == 1)
                        localItem.itemType = ItemType.RowPop;
                    else localItem.itemType = ItemType.ItemsPop;
                    matched[rand].item = localItem;
                    //Destroy items
                    for (int i = 0; i < matched.Count; i++)
                    {
                        //_item_Chance[matched[i].item.index].Chance++;
                        if (matched[i].item != null&&i!=rand)
                        {
                            Destroy(matched[i].item.gameObject);
                            matched[i].item = null;
                        }

                    }

                }
                else if (matched.Count == 5)
                {
                    //Give Bomb
                }
                break;
            default:
                for (int i = 0; i < matched.Count; i++)
                {
                    //_item_Chance[matched[i].item.index].Chance++;
                    if (matched[i].item != null)
                    {
                        Destroy(matched[i].item.gameObject);
                        matched[i].item = null;
                    }

                }
                break;
        }
        Debug.LogWarning(shape);
    }
    PatternShape CheckMatchedListShape(List<Tile> matched)
    {
        ///////////////////////////////////////Check the vertical////////////////////////////////////
        bool isVertical = true;
        for (int i = 0; i < matched.Count-1; i++)
        {
            if (matched[i].x != matched[i + 1].x)
            {
                isVertical = false;
                break;
            }
        }
        if(isVertical)
        return PatternShape.Vertical;

        ////////////////////////////////Check the horizontal//////////////////////////
        bool isHorizontal = true;
        for (int i = 0; i < matched.Count - 1; i++)
        {
            if (matched[i].y != matched[i + 1].y)
            {
                isHorizontal = false;
                break;
            }
        }
        if (isHorizontal)
            return PatternShape.Horizontal;

        //////////////////////////////////////////Check the L Shape///////////////////////////////
        // Sort matched tiles by x and y positions
        matched.Sort((a, b) =>
        {
            int cmp = a.x.CompareTo(b.x);
            return cmp == 0 ? a.y.CompareTo(b.y) : cmp;
        });
        // Check for horizontal L shape
        for (int i = 0; i < matched.Count - 2; i++)
        {
            if (matched[i].y == matched[i + 1].y &&
                matched[i + 1].y == matched[i + 2].y &&
                matched[i].x + 1 == matched[i + 1].x &&
                matched[i].x + 1 == matched[i + 2].x)
                return PatternShape.L;

        }
        // Check for vertical L shape
        for (int i = 0; i < matched.Count - 2; i++)
        {
            if (matched[i].x == matched[i + 1].x &&
                matched[i + 1].x == matched[i + 2].x &&
                matched[i].y + 1 == matched[i + 1].y &&
                matched[i].y + 1 == matched[i + 2].y)
                return PatternShape.L;
        }

        ////////////////////////////////////////Check for T Shape///////////////////////////
        matched.Sort((a, b) =>
        {
            int cmp = a.x.CompareTo(b.x);
            return cmp == 0 ? a.y.CompareTo(b.y) : cmp;
        });
        // Check for horizontal T shape
        for (int i = 0; i < matched.Count - 3; i++)
        {
            if (matched[i].y == matched[i + 1].y &&
                matched[i + 1].y == matched[i + 2].y &&
                matched[i + 1].x == matched[i + 2].x &&
                matched[i + 2].y + 1 == matched[i + 3].y)
                return PatternShape.T;
        }

        // Check for vertical T shape
        for (int i = 0; i < matched.Count - 3; i++)
        {
            if (matched[i].x == matched[i + 1].x &&
                matched[i + 1].x == matched[i + 2].x &&
                matched[i + 1].y == matched[i + 2].y &&
                matched[i + 2].x + 1 == matched[i + 3].x)
                return PatternShape.T;
        }
        return PatternShape.None;
    }
    List<Tile> TileMatched(Tile tile,string Dir)
    {
        if(Dir=="Vertical")
        return VerticalTileMatched(tile,new List<Tile>());
        else return HorizontalTileMatched(tile, new List<Tile>());

    }
    bool IsNeighbor(Tile firstTile,Tile secondTile)
    {
        //Check if tile has ability
        if (firstTile.item.GetItemType() != ItemType.Normal || secondTile.item.GetItemType() != ItemType.Normal)
            return true;
        return firstTile.item.item == secondTile.item.item;
    }
    List<Tile> HorizontalTileMatched(Tile tile,List<Tile> verticalTiles)
    {
        if (!verticalTiles.Contains(tile))
            verticalTiles.Add(tile);
        for (int x= tile.x; x < board.GetLength(0) - 1; x++)
        {
            if (IsNeighbor(board[x, tile.y], board[x + 1, tile.y]))
            {
                if (!(verticalTiles.Contains(board[x + 1, tile.y])))
                {
                    verticalTiles.Add(board[x + 1, tile.y]);
                   verticalTiles=VerticalTileMatched(board[x + 1, tile.y],verticalTiles);
                }
            }
            else  return verticalTiles;
            
        }
        return verticalTiles;
    }
    List<Tile> VerticalTileMatched(Tile tile,List<Tile> verticalTiles)
    {
        if(!verticalTiles.Contains(tile))
        verticalTiles.Add(tile);
        for (int y = tile.y; y < board.GetLength(0)-1; y++)
        {

            if (IsNeighbor(board[tile.x, y], board[tile.x, y + 1]))
            {
                if(!(verticalTiles.Contains(board[tile.x, y + 1])))
                verticalTiles.Add(board[tile.x, y + 1]);
                 verticalTiles=(HorizontalTileMatched(board[tile.x, y],verticalTiles));
            }
            else return verticalTiles;
        }
        return verticalTiles;
    }
    // Function to check for matching patterns in the matrix
    public List<List<Tile>> CheckMatchingPatterns(Tile[,] matrix)
    {
        List<List<Tile>> matchedPatterns = new List<List<Tile>>();

        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        // Helper function to check if a sequence of tiles matches a pattern
        bool CheckSequence(List<Tile> sequence)
        {
            if (sequence.Count < 3) // Minimum pattern length
                return false;

            Sprite tileType = sequence[0].item.item;
            foreach (Tile tile in sequence)
            {
                if (tile.item.item != tileType)
                    return false;
            }
            return true;
        }

        ///////////////////////////////////////////// Check for horizontal matches
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols - 2; c++)
            {
                List<Tile> sequence = new List<Tile>();
                for (int i = 0; i < 3; i++)
                {
                    sequence.Add(matrix[r, c + i]);
                }
                if (CheckSequence(sequence))
                    matchedPatterns.Add(sequence);
            }
        }

        ///////////////////////////////////////////// Check for vertical matches
        for (int c = 0; c < cols; c++)
        {
            for (int r = 0; r < rows - 2; r++)
            {
                List<Tile> sequence = new List<Tile>();
                for (int i = 0; i < 3; i++)
                {
                    sequence.Add(matrix[r + i, c]);
                }
                if (CheckSequence(sequence))
                    matchedPatterns.Add(sequence);
            }
        }

        ///////////////////////////////////////////// Check for L-shape matches
        for (int r = 0; r < rows - 1; r++)
        {
            for (int c = 0; c < cols - 2; c++)
            {
                // L-shape: 
                // 1 0
                // 1 1 1
                List<Tile> sequence1 = new List<Tile>();
                sequence1.Add(matrix[ c,r]);
                sequence1.Add(matrix[c,r + 1]);
                sequence1.Add(matrix[c + 1, r + 1]);
                sequence1.Add(matrix[c + 2,r + 1]);
                if (CheckSequence(sequence1))
                    matchedPatterns.Add(sequence1);

                // Inverted L-shape: 
                // 0 1
                // 1 1
                if (c + 1 < cols && r + 2 < rows)
                {
                    List<Tile> sequence2 = new List<Tile>();
                    sequence2.Add(matrix[c + 1, r]);
                    sequence2.Add(matrix[c, r + 1]);
                    sequence2.Add(matrix[c + 1, r + 1]);
                    sequence2.Add(matrix[c + 1, r + 2]);
                    if (CheckSequence(sequence2))
                        matchedPatterns.Add(sequence2);
                }

                // Upside down L-shape: 
                // 1 1
                // 1 0
                List<Tile> sequence3 = new List<Tile>();
                sequence3.Add(matrix[c, r]);
                sequence3.Add(matrix[c + 1,r]);
                sequence3.Add(matrix[ c + 2,r]);
                sequence3.Add(matrix[c,r + 1]);
                if (CheckSequence(sequence3))
                    matchedPatterns.Add(sequence3);

                // Inverted upside down L-shape: 
                // 1 1
                // 0 1
                List<Tile> sequence4 = new List<Tile>();
                sequence4.Add(matrix[ c,r]);
                sequence4.Add(matrix[ c + 1,r]);
                sequence4.Add(matrix[c + 2,r]);
                sequence4.Add(matrix[ c + 2, r + 1]);
                if (CheckSequence(sequence4))
                    matchedPatterns.Add(sequence4);
            }
        }

        /////////////////////////////////////// Check for T-shape matches
        for (int r = 0; r < rows - 1; r++)
        {
            for (int c = 0; c < cols - 2; c++)
            {
                // T-shape: 
                // 1 1 1
                //   1
                if (r + 1 < rows && c + 2 < cols)
                {
                    List<Tile> sequence1 = new List<Tile>();
                    sequence1.Add(matrix[ c,r]);
                    sequence1.Add(matrix[c+1, r]);
                    sequence1.Add(matrix[ c+2,r]);
                    sequence1.Add(matrix[c+1,r+1]);
                    if (CheckSequence(sequence1))
                        matchedPatterns.Add(sequence1);
                }

                //// Inverted T-shape:
                ////   1
                //// 1 1 1
                ////   1
                if (r + 2 < rows && c - 1 >= 0 && c + 1 < cols)
                {
                    List<Tile> sequence2 = new List<Tile>();
                    sequence2.Add(matrix[c, r]);
                    sequence2.Add(matrix[c,r + 1]);
                    sequence2.Add(matrix[ c - 1, r + 1]);
                    sequence2.Add(matrix[ c + 1, r + 1]);
                    sequence2.Add(matrix[c,r + 2]);
                    if (CheckSequence(sequence2))
                        matchedPatterns.Add(sequence2);
                }

                //// Inverted T-shape:
                ////   1
                //// 1 1 1
                if (r + 2 < rows && c - 1 >= 0 && c + 1 < cols)
                {
                    List<Tile> sequence5 = new List<Tile>();
                    sequence5.Add(matrix[c, r]);
                    sequence5.Add(matrix[c, r + 1]);
                    sequence5.Add(matrix[c - 1, r + 1]);
                    sequence5.Add(matrix[c + 1, r + 1]);
                    if (CheckSequence(sequence5))
                        matchedPatterns.Add(sequence5);
                }

                // Upward T-shape:
                // 1
                // 1 1 1
                // 1
                if (r - 1 >= 0 && r + 1 < rows && c + 1 < cols)
                {
                    List<Tile> sequence3 = new List<Tile>();
                    sequence3.Add(matrix[c, r]);
                    sequence3.Add(matrix[c, r - 1]);
                    sequence3.Add(matrix[c, r + 1]);
                    sequence3.Add(matrix[ c + 1,r]);
                    sequence3.Add(matrix[c+2, r]);
                    if (CheckSequence(sequence3))
                        matchedPatterns.Add(sequence3);
                }

            }
        }
        //////////////////////////////////////// Check for H-shape matches
        for (int r = 0; r < rows - 2; r++)
        {
            for (int c = 0; c < cols - 2; c++)
            {
                // H-shape: 
                // 1 0 1
                // 1 1 1
                // 1 0 1
                List<Tile> sequence = new List<Tile>();
                sequence.Add(matrix[c, r]);
                sequence.Add(matrix[ c + 2,r]);
                sequence.Add(matrix[c,r + 1]);
                sequence.Add(matrix[c+1,r + 1]);
                sequence.Add(matrix[ c + 2,r+1]);
                sequence.Add(matrix[c,r + 2]);
                sequence.Add(matrix[c+2,r + 2]);
                if (CheckSequence(sequence))
                    matchedPatterns.Add(sequence);
            }
        }
                return matchedPatterns;
    }


}
public class ItemChance
{
    public Sprite Sprite;
    public int Chance;
    public ItemChance(Sprite sprite,int chance)
    {
        Sprite = sprite;
        Chance = chance;
    }
}
