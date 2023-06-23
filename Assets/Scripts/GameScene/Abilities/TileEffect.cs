using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TileEffect
{
    #region Scripts
    public RoundData roundData;
    public Board board;
    #endregion

    #region Game object references
    public Image tileEffectIcon;
    #endregion

    #region Tile effect data
    private int _tileEffectID;
    public int tileEffectID { get { return _tileEffectID; } set { _tileEffectID = value; } }
    private string _tileEffectName;
    public string tileEffectName { get { return _tileEffectName; } set { _tileEffectName = value; } }
    private string _tileEffectDesc;
    public string tileEffectDesc { get { return _tileEffectDesc; } set { _tileEffectDesc = value; } }
    private string _tileEffectType;
    public string tileEffectType { get { return _tileEffectType; } set { _tileEffectType = value; } }
    private string _tileEffectIconPicName;
    public string tileEffectIconPicName { get { return _tileEffectIconPicName; } set { _tileEffectIconPicName = value; } }

    private Entity _tileEffectTarget;
    public Entity tileEffectTarget { get { return _tileEffectTarget; } set { _tileEffectTarget = value; } }
    private int _tileEffectTurns;
    public int tileEffectTurns { get { return _tileEffectTurns; } set { _tileEffectTurns = value; } }
    private int _tileEffectRemainingTurns;
    public int tileEffectRemainingTurns { get { return _tileEffectRemainingTurns; } set { _tileEffectRemainingTurns = value; } }
    private int _tileEffectLevel;
    public int tileEffectLevel { get { return _tileEffectLevel; } set { _tileEffectLevel = value; } }

    // Boolean
    private bool _isSource;
    public bool isSource { get { return _isSource; } set { _isSource = value; } }
    #endregion

    #region Tile effect functions
    // For all tiles
    public virtual void OnInit(Tile tile, Entity selfEntity, int turns)
    {
        Debug.Log($"{Time.time} TileEffect.OnInit (start)");

        tileEffectTarget = selfEntity;
        tileEffectTurns = turns;
        tileEffectRemainingTurns = tileEffectTurns;

        tileEffectIcon = tile.tileEffectIcon;
        tile.tileEffectTurnsText.text = tileEffectRemainingTurns.ToString();
        Sprite tileEffectOrgImage = Resources.Load<Sprite>($"Icons/{tileEffectIconPicName}");
        tile.tileEffectIcon.sprite = tileEffectOrgImage;

        // Make sure the effect icon/text object is active
        tile.tileEffectParent.SetActive(true);

        // Setting up default settings
        switch (tileEffectName)
        {
            // Let the tile unable to drag/react
            case "ice":
                tile.isControllable = false;
                break;
        }

        Debug.Log($"{Time.time} TileEffect.OnInit (end)");
    }

    // Only for special tile to init it's main tile effect
    public virtual void OnInit(Tile tile, Entity selfEntity, Image icon)
    {
        Debug.Log($"{Time.time} TileEffect.OnInit (start)");

        tileEffectTarget = selfEntity;
        Sprite tileEffectOrgImage = Resources.Load<Sprite>($"Icons/{tileEffectIconPicName}");
        icon.sprite = tileEffectOrgImage;

        // Make sure the effect icon/text object is NOT active
        tile.tileEffectParent.SetActive(false);

        Debug.Log($"{Time.time} TileEffect.OnInit (end)");
    }

    public void OnDropEffect()
    {
        Debug.Log($"{Time.time} TileEffect.OnDropEffect (start)");

        switch (tileEffectName)
        {
            case "heart":
                tileEffectTarget.Heal(tileEffectTarget.currentMaxHealthValue * 0.1f);
                break;

            case "power":
                roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
                roundData.currentPowerScore += 1000;
                break;

            case "ice":
                tileEffectTarget.TakeDamage(tileEffectTarget.currentMaxHealthValue * 0.2f);
                break;

            case "fire":
                tileEffectTarget.TakeDamage(tileEffectTarget.currentMaxHealthValue * 0.15f);
                break;
        }

        Debug.Log($"{Time.time} TileEffect.OnDropEffect (end)");
    }

    public virtual void OnRemove(Tile tile)
    {
        Debug.Log($"{Time.time} TileEffect.OnRemove (start)"); 

        tile.currentTileEffect = null;

        Debug.Log($"{Time.time} TileEffect.OnRemove (end)"); 
    }

    public void OnBeforeTurnStart(Tile tile)
    {
        Debug.Log($"{Time.time} TileEffect.OnBeforeTurnStart (start)");

        // Temp: spread
        board = GameObject.Find("Board").GetComponent<Board>();
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        int randomTileNumber = UnityEngine.Random.Range(0, board.tilesInBoard.Count);
        Tile newAffectedTile = GameObject.Find("Answer tiles spawner").transform.GetChild(randomTileNumber).GetComponent<Tile>();

        // Source tile effects from special tiles
        if (isSource)
        {
            switch (tileEffectName)
            {
                // Let the tile unable to drag/react
                case "ice":
                    newAffectedTile.GetTileEffect(roundData.player, 2, 5);
                    break;

                // Spread the fire tile effect to another tile
                case "fire":
                    newAffectedTile.GetTileEffect(roundData.player, 3, 5);
                    break;
            }
        }
        
        // Continous tile effects here

        
        

        Debug.Log($"{Time.time} TileEffect.OnBeforeTurnStart (end)");
    }

    public virtual void OnTurnEnd(Tile tile)
    {
        Debug.Log($"{Time.time} TileEffect.OnTurnEnd (start)"); 

        if (tileEffectRemainingTurns <= 0)
        {
            // Exectue tile end turn actions
            switch (tileEffectName)
            {
                // Let the tile able to drag/react again
                case "ice":
                    tile.isControllable = true;
                    break;

                // Set tile to be destroyed
                case "fire":
                    tile.toBeDestroyed = true;
                    break;
            }

            OnRemove(tile);
            tile.tileEffectParent.SetActive(false);
        }

        Debug.Log($"{Time.time} TileEffect.OnTurnEnd (start)"); 
    }
    #endregion
}
