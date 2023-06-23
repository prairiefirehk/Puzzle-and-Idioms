using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpecialTileFactory : MonoBehaviour, IFactory
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
        Debug.Log($"{Time.time} SpecialTileFactory.Awake (start)");
        
        // Not ideal place
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();

        Debug.Log($"{Time.time} SpecialTileFactory.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} SpecialTileFactory.OnEnable (start)");
        Debug.Log($"{Time.time} SpecialTileFactory.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} SpecialTileFactory.Start (start)");

        //roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();

        Debug.Log($"{Time.time} SpecialTileFactory.Start (end)");
    }

    void Update()
    {
        
    }
    
    void OnDisable()
    {
        Debug.Log($"{Time.time} SpecialTileFactory.OnDisable (start)");
        Debug.Log($"{Time.time} SpecialTileFactory.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} SpecialTileFactory.OnDestroy (start)");
        Debug.Log($"{Time.time} SpecialTileFactory.OnDestroy (end)");
    }
    #endregion

    #region Factory functions
    public SpecialTile CreateTile(int tileCellPosition, GameObject parent, Vector2 tileLocation, int maxSpawnTileLevel, int tileEffectID)
    {
        Debug.Log($"{Time.time} SpecialTileFactory.CreateTile (start)");    

        SpecialTile specialTile = Instantiate(tilePrefab, parent.transform).GetComponent<SpecialTile>();
        specialTile.tileType = "special";

        
        specialTile.tileLocation = tileLocation;
        specialTile.SetTileLocation();
        specialTile.tileCellPosition = tileCellPosition;
        specialTile.tileCellPositionAtStart = tileCellPosition;

        specialTile.tileLevel = Random.Range(0, maxSpawnTileLevel);
        specialTile.tileLevelText.text = specialTile.tileLevel.ToString();

        specialTile.tileMainEffect = specialTile.GetTileMainEffect(roundData.player, tileEffectID);
        specialTile.tileMainEffect.isSource = true;
        //specialTile.tileColor = new Color32(240, 200, 210, 255);
        //specialTile.transform.GetChild(0).GetComponent<Image>().color = specialTile.tileColor;
        specialTile.tileSpecialIcon = specialTile.transform.GetChild(2).GetComponent<Image>();

        specialTile.tileSpawnedTurns = roundData.currentTurn;
        specialTile.tileExistedTurns = 1;

        specialTile.outputPower = UnityEngine.Random.Range(minInclusive: 80, 100);

        specialTile.currentState = TileState.State.Idle;

        // Default setting
        specialTile.isControllable = true;

        Debug.Log($"{Time.time} SpecialTileFactory.CreateTile, return specialTile(local var): {specialTile.name}(specialTile.name only) (end)");  
        return specialTile;
    }
    #endregion
}