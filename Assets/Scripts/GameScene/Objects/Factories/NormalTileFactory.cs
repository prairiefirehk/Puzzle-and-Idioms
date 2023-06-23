using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NormalTileFactory : MonoBehaviour, IFactory
{
    #region Scripts
    public RoundData roundData;
    #endregion

    #region Game object references
    public GameObject tilePrefab;
    public GameObject answerTilesCells;
    public GameObject answerTilesSpawner;
    #endregion
    
    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} NormalTileFactory.Awake (start)");

        // Not ideal place
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();

        Debug.Log($"{Time.time} NormalTileFactory.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} NormalTileFactory.OnEnable (start)");
        Debug.Log($"{Time.time} NormalTileFactory.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} NormalTileFactory.Start (start)");

        //roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();

        Debug.Log($"{Time.time} NormalTileFactory.Start (end)");
    }

    void Update()
    {
        
    }
    
    void OnDisable()
    {
        Debug.Log($"{Time.time} NormalTileFactory.OnDisable (start)");
        Debug.Log($"{Time.time} NormalTileFactory.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} NormalTileFactory.OnDestroy (start)");
        Debug.Log($"{Time.time} NormalTileFactory.OnDestroy (end)");
    }
    #endregion

    #region Factory functions
    // ---------------------------------------------------------------------------------------------------------------------------------------------------------v Tile effect?
    public NormalTile CreateTile(int tileCellPosition, GameObject parent, Vector2 tileLocation, int randomIdiomID, int randomWordPosition, int maxSpawnTileLevel)
    {
        Debug.Log($"{Time.time} NormalTileFactory.CreateTile (start)");

        NormalTile normalTile = Instantiate(tilePrefab, parent.transform).GetComponent<NormalTile>();
        // Or maybe just use Tag in unity?
        normalTile.tileType = "normal";

        normalTile.tileIdiomID = randomIdiomID;
        normalTile.tileIdiom = ImportData.idioms.idiom[normalTile.tileIdiomID];
        normalTile.tileIdiomName = normalTile.tileIdiom.name;
        normalTile.tileWord = normalTile.tileIdiomName[randomWordPosition].ToString();
        normalTile.tileText.text = normalTile.tileWord;
        normalTile.tileWordOrder = randomWordPosition;

        // Need to be search and create word data
        //normalTile.tileWordID = ???

        normalTile.tileLocation = tileLocation;
        normalTile.SetTileLocation();
        normalTile.tileCellPosition = tileCellPosition;
        normalTile.tileCellPositionAtStart = tileCellPosition;

        normalTile.tileLevel = Random.Range(0, maxSpawnTileLevel);
        normalTile.tileLevelText.text = normalTile.tileLevel.ToString();
        normalTile.tileColor = new Color32(230, 230, 230, 255);
        normalTile.transform.GetChild(0).GetComponent<Image>().color = normalTile.tileColor;

        normalTile.tileSpawnedTurns = roundData.currentTurn;
        normalTile.tileExistedTurns = 1;

        normalTile.baseValueModifier = 69;
        normalTile.outputPower = UnityEngine.Random.Range(minInclusive: 80, 100);
        //Debug.Log("I set the base score last: " + normalTile.baseValueModifier);

        normalTile.currentState = TileState.State.Idle;

        // Default setting
        normalTile.isControllable = true;
        normalTile.isAnswer = false;

        //Debug.Log("2. " + normalTile.tileText.text + " Tile position right after create: " + normalTile.tileCellPosition.ToString());

        Debug.Log($"{Time.time} NormalTileFactory.CreateTile, return normalTile(local var): {normalTile.name}(normalTile.name only) (end)");
        return normalTile;
    }

    public void DrawCurrentIdiom(IdiomList idiomData, int maxIdiomDataLength)
    {
        Debug.Log($"{Time.time} NormalTileFactory.DrawCurrentIdiom (start)");

        int random = Random.Range(0, maxIdiomDataLength);
        //Debug.Log("random idiom ID: " + (random + 1));

        if (roundData.askedAnswerIdiomsIDs.Contains(random))
        {
            random = Random.Range(0, maxIdiomDataLength);
            //Debug.Log("random idiom ID: " + (random + 1));
        }

        //Debug.Log(idiomData.idiom[random]);
        //roundData.currentIdiom = idiomData.idiom[random];
        //roundData.currentIdiomID = idiomData.idiom[random].id;
        //roundData.askedIdiomsID.Add(roundData.currentIdiomID);
        //Debug.Log("drew current idiom!");

        Debug.Log($"{Time.time} NormalTileFactory.DrawCurrentIdiom (end)");
    }
    #endregion
}
