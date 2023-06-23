using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Board : MonoBehaviour
{
    #region Scripts
    #endregion

    #region Data importing
    // Loading idiom data
    public IdiomList idiomData;
    public int idiomDataSize;
    public RoundData roundData;
    #endregion

    #region Game object references
    public GameObject answerTilesSpawner;
    public GameObject answerTilesCells;
    public Image boardFog;
    #endregion

    #region Board data? However they should import from player which import from player data, should delete****
    // Tile spawning stat, should import from roundData(which import from outside data aswell), so need to be delete after testing
    private int _normalTileSpawnRate = 80;
    public int normalTileSpawnRate { get { return _normalTileSpawnRate; } set { _normalTileSpawnRate = value; } }
    private int _specialTileSpawnRate = 80;
    public int specialTileSpawnRate { get { return _specialTileSpawnRate; } set { _specialTileSpawnRate = value; } }
    private int _specialTileLimit = 2;
    public int specialTileLimit { get { return _specialTileLimit; } set { _specialTileLimit = value; } }
    #endregion

    #region Factories
    private NormalTileFactory normalTileFactory;
    private SpecialTileFactory specialTileFactory;
    #endregion

    #region References
    private Tile _answerTile;
    public Tile answerTile { get { return _answerTile; } set { _answerTile = value; } }
    #endregion

    #region Board elements counting
    public int[] tileCell = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public List<Tile> tilesInBoard;
    public int[] answerCount = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} Board.Awake (start)");
        
        // Not ideal place
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();

        idiomData = ImportData.idioms;
        idiomDataSize = idiomData.idiom.Length;
        normalTileFactory = gameObject.GetComponent<NormalTileFactory>();
        specialTileFactory = gameObject.GetComponent<SpecialTileFactory>();

        Debug.Log($"{Time.time} Board.Awake (end)");
    }
    void OnEnable()
    {
        Debug.Log($"{Time.time} Board.OnEnable (start)");

        // Subscribe to the game events and listen
        //Tile.OnEndTurnEvent += EndTurn;

        Debug.Log($"{Time.time} Board.OnEnable (end)");
    }
    void Start()
    {
        Debug.Log($"{Time.time} Board.Start (start)");

        //SpawnTiles(CheckBlankCell());
        //DrawAnswer();
        //roundData.BeforeTurnStart();

        Debug.Log($"{Time.time} Board.Start (end)");
    }
    void Update()
    {
        
    }

    void OnDisable()
    {
        Debug.Log($"{Time.time} Board.OnDisable (start)");

        // Unsubscribe to the game events
        //Tile.OnEndTurnEvent -= EndTurn;

        Debug.Log($"{Time.time} Board.OnDisable (end)");
    }

    void OnDestroy()
    {
        Debug.Log($"{Time.time} Board.OnDestroy (start)");

        Debug.Log($"{Time.time} Board.OnDestroy (end)");
    }
    #endregion

    #region Board functions
    // Spawn tile function here, more like an order

    // Controlling the access of the board
    public void EnableBoard()
    {
        Debug.Log($"{Time.time} Board.EnableBoard (start)");

        boardFog.gameObject.SetActive(false);

        Debug.Log($"{Time.time} Board.EnableBoard (end)");
    }

    public void DisableBoard()
    {
        Debug.Log($"{Time.time} Board.DisableBoard (start)");

        boardFog.gameObject.SetActive(true);

        Debug.Log($"{Time.time} Board.DisableBoard (end)");
    }

    public void SpawnTiles(int spawnNum)
    {
        Debug.Log($"{Time.time} Board.SpawnTiles (start)");

        // Order factories to manufacture tiles, loop as to fill all blank cell in board
        while (spawnNum > 0)
        {
            // Check if there is any blank cell, if there's no then break
            int blankCells = CheckBlankCell();

            if (blankCells == 0)
            {
                break;
            }
            else
            {
                //List<int> filledTilePosition = new List<int>();

                // If there's blank cell, randomize the position
                int randomTilePosition = UnityEngine.Random.Range(0, answerTilesCells.transform.childCount);
                //Debug.Log("Random tile position: " + randomTilePosition);

                // Draw the randomTilePosition until no blank tile cell
                if (tileCell[randomTilePosition] == 0)
                {
                    // Decide the type of tile that will be spawn
                    int spawnTypeRandom = UnityEngine.Random.Range(0, 100);
                    //Debug.Log("Spawn control number: " + spawnTypeRandom);

                    //Debug.Log("1. Should spawn to: " + random.ToString() + GameObject.Find("Answer tiles cell (" + random.ToString() + ")").gameObject.transform.position);
                    // Decide type of tile to be spawn, may need to rewrite
                    if (spawnTypeRandom < (100 - specialTileSpawnRate))
                    {
                        //Debug.Log("specialTileSpawnRate: " + specialTileSpawnRate);
                        //Debug.Log("Condition 1 output value 1: " + (spawnTypeRandom > 0));
                        //Debug.Log("Condition 1 output value 2: " + (spawnTypeRandom < (100 - specialTileSpawnRate)));

                        // Only spawning normal tile needs to be checking duplication
                        int randomWordPosition = UnityEngine.Random.Range(0, roundData.roundIdiomSize);
                        int randomIdiomID = UnityEngine.Random.Range(0, ImportData.idioms.idiom.Length);
                        string randomWord = ImportData.idioms.idiom[randomIdiomID].name[randomWordPosition].ToString();

                        // Check any duplicated idiom tile in board 
                        if ((roundData.tilesWordIdiomsID.Contains(randomIdiomID) == true) || (roundData.tilesWord.Contains(randomWord) == true))
                        {
                            Debug.Log($"{Time.time} Duplicated word! Draw another ID");
                            //int duplicatedRandom = random;
                            //string duplicatedRandomWord = randomWord;
                            do
                            {
                                randomWordPosition = UnityEngine.Random.Range(0, roundData.roundIdiomSize);
                                randomIdiomID = UnityEngine.Random.Range(0, ImportData.idioms.idiom.Length);
                                randomWord = ImportData.idioms.idiom[randomIdiomID].name[randomWordPosition].ToString();
                                //Debug.Log(i + "draw another random ID: " + (random + 1) + " word = " + randomWord + " round missing word = " + gameData.roundMissingWord);
                            }
                            while ((roundData.tilesWordIdiomsID.Contains(randomIdiomID) == true) || (roundData.tilesWord.Contains(randomWord) == true));
                        }

                        //Debug.Log($"After checking: tile idiom ID: {randomIdiomID} word = {randomWord}");
                        // Add answers into the answer list, according to the round missing word order
                        roundData.tilesWordIdiomsID.Add(randomIdiomID);
                        roundData.tilesWord.Add(randomWord);

                        // Spawning normal tile
                        Tile newTile = normalTileFactory.CreateTile(randomTilePosition, answerTilesSpawner, answerTilesCells.transform.GetChild(randomTilePosition).gameObject.transform.position, randomIdiomID, randomWordPosition, MaxSpawnTileLevel());
                        newTile.name = "Tile " + randomTilePosition.ToString();
                        //Debug.Log($"Successfully spawned a normal tile: {newTile.name} {newTile.transform.GetChild(2).GetComponent<Text>().text} in position {randomTilePosition}");

                        tileCell[randomTilePosition] = 1;
                        tilesInBoard.Add(newTile);
                        spawnNum = spawnNum - 1;
                    }
                    else if (CheckTileTypeCount("special") < specialTileLimit)
                    {
                        //Debug.Log("specialTileSpawnRate: " + specialTileSpawnRate);
                        //Debug.Log("Condition 2 output value 1: " + (spawnTypeRandom > (100 - specialTileSpawnRate)));
                        //Debug.Log("Condition 2 output value 2: " + (spawnTypeRandom < 100));
                        //Debug.Log("Condition 2 output value 3: " + (CheckTileTypeCount("special") < specialTileLimit));
                        // Another random to control special tile element?
                        int randomTileEffect = UnityEngine.Random.Range(0, ImportData.tileEffectDictionary.Count);

                        // Spawning special tile
                        Tile newTile = specialTileFactory.CreateTile(randomTilePosition, answerTilesSpawner,answerTilesCells.transform.GetChild(randomTilePosition).gameObject.transform.position, MaxSpawnTileLevel(), randomTileEffect);
                        newTile.name = "Tile " + randomTilePosition.ToString();
                        Debug.Log($"{Time.time} Successfully spawned a special tile: {newTile.name} in position {randomTilePosition}");

                        tileCell[randomTilePosition] = 2;
                        tilesInBoard.Add(newTile);
                        spawnNum = spawnNum - 1;
                    }
                    else
                    {
                        //Debug.Log("Nope");
                    }
                    //Debug.Log("3. (Spawning order)After spawning: " + newTile.transform.GetSiblingIndex() + GameObject.Find("Answer tiles cell (" + random.ToString() + ")").gameObject.transform.position);
                    
                }
            }
            
            // Break pt for finishing spawning
            if (spawnNum == 0)
            {
                //DrawAnswer();
                break;
            }
        }

        Debug.Log($"{Time.time} Board.SpawnTiles (end)");
    }
    public int MaxSpawnTileLevel()
    {
        Debug.Log($"{Time.time} Board.MaxSpawnTileLevel (start)");

        int maxTileLevel = 0;
        int[] tileLevelScoreArr = { 0, 500, 1500, 3000, 5000, 7500, 10500, 14000, 18000, 22500};

        for(int i = 0; i < tileLevelScoreArr.Length; i++)
        {
            if (roundData.currentPowerScore <= tileLevelScoreArr[i])
            {
                maxTileLevel = i;
                break;
            }
        };

        Debug.Log($"{Time.time} Board.MaxSpawnTileLevel, return maxTileLevel(local var): {maxTileLevel} (end)");
        return maxTileLevel;
    }
    public void DisplayTileCell()
    {
        //Debug.Log($"{Time.time} Board.DisplayTileCell (start)");

        string tileCellMap = "";
        for (int i = 0; i < tileCell.Length; i++)
        {
            if (i != tileCell.Length)
            {
                tileCellMap = tileCellMap + tileCell[i].ToString() + ", ";
            }
            else 
            {
                tileCellMap = tileCellMap + tileCell[i].ToString();
                break;
            }

            if (i != 0 && (i+1)%5 == 0)
            {
                tileCellMap = tileCellMap + "\n";
            }
        }

        Debug.Log($"{Time.time} board.answerCell now: \n" + tileCellMap);

        //Debug.Log($"{Time.time} Board.DisplayTileCell (end)");
    }
    public int CheckBlankCell()
    {
        //Debug.Log($"{Time.time} Board.CheckBlankCell (start)");

        int blankCellsCount = 0;
        for (int i = 0; i < tileCell.Length; i++)
        {
            if (tileCell[i] == 0)
            {
                blankCellsCount = blankCellsCount + 1;
            }
        }

        //Debug.Log($"{Time.time} Board.CheckBlankCell, return blankCellsCount(local var): {blankCellsCount} (end)");
        return blankCellsCount;
    }

    public void CheckAndDestroyTiles()
    {
        Debug.Log($"{Time.time} Board.CheckAndDestroyTiles (start)");

        Debug.Log($"{Time.time} ^6.4 Check and destroy tiles(from board)");
        List<int> tilesToBeDestroyed = new List<int>();
        string destroyTilesString = "";

        for (int i = 0; i < tileCell.Length; i++)
        {
            //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
            if (tileCell[i] == -1)
            {
                Tile tile = GameObject.Find("Tile " + i).GetComponent<Tile>();
                tile.toBeDestroyed = true;
            }
        }

        for (int i = 0; i < tilesInBoard.Count; i++)
        {
            if (tilesInBoard[i].toBeDestroyed == true)
            {
                tilesToBeDestroyed.Add(tilesInBoard[i].tileCellPositionAtStart);
                destroyTilesString = destroyTilesString + tilesInBoard[i].tileCellPositionAtStart + ", ";
                Debug.Log($"destroyTilesString = {destroyTilesString}");
            }
        }

        foreach (int tilesPosition in tilesToBeDestroyed)
        {
            Tile tile = GameObject.Find("Tile " + tilesPosition).GetComponent<Tile>();
            tile.DestroyTile(tile);
        }

        Debug.Log($"{Time.time} Board.CheckAndDestroyTiles (end)");
    }

    public int CheckTileTypeCount(string tiletype)
    {
        Debug.Log($"{Time.time} Board.CheckTileTypeCount (start)");

        int tileTypeCount = 0;
        for (int i = 0; i < tilesInBoard.Count; i++)
        {
            if (tilesInBoard[i].tileType == tiletype)
            {
                tileTypeCount = tileTypeCount + 1;
            }
        }

        Debug.Log($"{Time.time} Board.CheckTileTypeCount, return tileTypeCount(local var): {tileTypeCount} (end)");
        return tileTypeCount;
    }

    public void DrawAnswer()
    {
        Debug.Log($"{Time.time} Board.DrawAnswer (start)");

        bool isReDrawable = false;
        int answerWordPosition = -1;
        List<int> answerListForDrawing = new List<int>();

        //Debug.Log("answerTilesSpawner.transform.childCount = " + answerTilesSpawner.transform.childCount);
        // Check any answer tiles in board
        for (int i = 0; i < answerTilesSpawner.transform.childCount; i++)
        {
            //Debug.Log("DrawAns loop i = " + i);
            Tile tile = GameObject.Find("Tile " + i).GetComponent<Tile>();
            if (tile.isAnswer)
            {
                //Debug.Log("Board has a answer tile here: " + tile.name + " ,no need to choose another answer tile!");
                //isReDrawable = false;
                break;
            }

            if (tile.CompareTag("NormalTile"))
            {
                answerListForDrawing.Add(i);
                //Debug.Log("Tile " + i + " had been add to the drawing list for answer");
            }
            
            // If there's no answer tile in board, turn on the answer flag
            if (i == answerTilesSpawner.transform.childCount - 1)
            {
                //Debug.Log("No answer in board!");
                isReDrawable = true;
                break;
            }
        }

        // After checking, start drawing answer from the answerBox list
        if (isReDrawable)
        {
            //System.Random random = new System.Random();
            answerWordPosition = UnityEngine.Random.Range(0, answerListForDrawing.Count);

            // Remember we are drawing the array order from the answerDrawBox
            // At this moment, the old answer tile still here and not being destroyed(If it draw the same position, so they will have the same fucking game object name)
            string answerTileName = "Answer tiles spawner/Tile " + answerListForDrawing[answerWordPosition];
            //Debug.Log(answerTileName);
            answerTile = GameObject.Find(answerTileName).GetComponent<Tile>();
            //Debug.Log($"{Time.time} Answer tile is: Tile {answerListForDrawing[answerWordPosition]}");

            answerTile.isAnswer = true;
            //Debug.Log($"{Time.time} Is {answerTile.GetComponent<NormalTile>().tileText.text} answer? -> {answerTile.isAnswer}");

            // Register idiom data to roundData, **assume all answer tile are normal tile**
            roundData.currentIdiom = answerTile.GetComponent<NormalTile>().tileIdiom;
            //Debug.Log($"{Time.time} roundData.currentIdiom = {roundData.currentIdiom}");
            roundData.currentIdiomID = answerTile.GetComponent<NormalTile>().tileIdiomID;
            //Debug.Log($"{Time.time} roundData.currentIdiomID = {roundData.currentIdiomID}");
            
            roundData.currentAnswerWord = answerTile.GetComponent<NormalTile>().tileText.text;
            //Debug.Log($"{Time.time} roundData.roundMissingWord = {roundData.currentAnswerWord}");
            roundData.currentAnswerWordOrder = answerTile.GetComponent<NormalTile>().tileWordOrder;
            //Debug.Log($"{Time.time} roundData.roundMissingWordOrder = {roundData.currentAnswerWordOrder}");

            //answerTile.transform.GetChild(2).GetComponent<TMP_Text>().color = new Color32(160, 60, 60, 255);

            roundData.askedAnswerWords.Add(roundData.currentAnswerWord);
            roundData.askedAnswerIdiomsIDs.Add(roundData.currentIdiomID);
            roundData.askedAnswerPositions.Add(roundData.currentAnswerTilePosition);
        }

        // Find a better place for displaying idiom!!
        roundData.firstWordText.text = roundData.currentIdiom.firstWord;
        roundData.secondWordText.text = roundData.currentIdiom.secondWord;
        roundData.thirdWordText.text = roundData.currentIdiom.thirdWord;
        roundData.fourthWordText.text = roundData.currentIdiom.fourthWord;
        roundData.currentIdiomIDText.text = roundData.currentIdiomID.ToString();
        roundData.missingWordTile.transform.position = roundData.questionTiles.transform.GetChild(roundData.currentAnswerWordOrder).transform.position;

        Debug.Log($"{Time.time} Board.DrawAnswer (end)");
    }

    public void UpdateTileCell()
    {
        Debug.Log($"{Time.time} Board.UpdateTileCell (start)");

        Debug.Log($"{Time.time} ^5.4 Update tileCell(from board)");
        for (int i = 0; i < answerTilesSpawner.transform.childCount; i++)
        {
            Tile tile = answerTilesSpawner.transform.GetChild(i).GetComponent<Tile>();
            if (tile.toBeDestroyed)
            {
                tileCell[tile.tileCellPositionAtStart] = -1;
            }
        }

        Debug.Log($"{Time.time} Board.UpdateTileCell (end)");
    }

    public int CheckAnswerTile()
    {
        Debug.Log($"{Time.time} Board.CheckAnswerTile (start)");

        Debug.Log($"{Time.time} ^6.6B Check any answer in board");

        int answerTilesCount = 0;

        for (int i = 0; i < answerTilesSpawner.transform.childCount; i++)
        {
            Tile tile = answerTilesSpawner.transform.GetChild(i).GetComponent<Tile>();
            if (tile.isAnswer)
            {
                answerTilesCount = answerTilesCount + 1;
            }
        }

        if (answerTilesCount == 0)
        {
            answerTile = null;
            SpawnTiles(CheckBlankCell());
            DrawAnswer();
        }

        Debug.Log($"{Time.time} Board.CheckAnswerTile, return answerTilesCount(local var): {answerTilesCount} (end)");
        return answerTilesCount;
    }
    
    public void RenameTiles()
    {
        Debug.Log($"{Time.time} Board.RenameTiles (start)");

        Debug.Log($"{Time.time} ^6.6C Rename tiles(from board)");
        //List<Tile> newTilesInBoard = new List<Tile>();
        // Change tile name according to it's position
        for (int i = 0; i < answerTilesSpawner.transform.childCount; i++)
        {
            Tile tile = answerTilesSpawner.transform.GetChild(i).GetComponent<Tile>();
            tile.name = "Tile " + tile.tileCellPositionAtStart;
        }
        
        Debug.Log($"{Time.time} Board.RenameTiles (end)");
    }
    #endregion
}