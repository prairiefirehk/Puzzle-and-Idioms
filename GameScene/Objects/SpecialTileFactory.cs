using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlueAndWhite.RoundManaging;
using BlueAndWhite.Entities;

namespace BlueAndWhite.Objects
{
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
            Debug.Log($"SpecialTileFactory.Awake (start)");
            
            // Not ideal place
            roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();

            Debug.Log($"SpecialTileFactory.Awake (end)");
        }

        void OnEnable()
        {
            Debug.Log($"SpecialTileFactory.OnEnable (start)");
            Debug.Log($"SpecialTileFactory.OnEnable (end)");
        }
        
        void Start()
        {
            Debug.Log($"SpecialTileFactory.Start (start)");

            //roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();

            Debug.Log($"SpecialTileFactory.Start (end)");
        }

        void Update()
        {
            
        }
        
        void OnDisable()
        {
            Debug.Log($"SpecialTileFactory.OnDisable (start)");
            Debug.Log($"SpecialTileFactory.OnDisable (end)");
        }

        void OnDestroy() 
        {
            Debug.Log($"SpecialTileFactory.OnDestroy (start)");
            Debug.Log($"SpecialTileFactory.OnDestroy (end)");
        }
        #endregion

        #region Factory functions
        public SpecialTile CreateTile(int tileCellPosition, GameObject parent, Vector2 tileLocation, int maxSpawnTileLevel, int tileEffect)
        {
            Debug.Log($"SpecialTileFactory.CreateTile (start)");    

            SpecialTile specialTile = Instantiate(tilePrefab, parent.transform).GetComponent<SpecialTile>();
            specialTile.tileType = "special";

            //specialTile.tileIcon = ;
            specialTile.tileLocation = tileLocation;
            specialTile.SetTileLocation();
            specialTile.tileCellPosition = tileCellPosition;
            specialTile.tileCellPositionAtStart = tileCellPosition;

            specialTile.tileLevel = Random.Range(0, maxSpawnTileLevel);
            specialTile.tileLevelText.text = specialTile.tileLevel.ToString();

            // Turn int into tile effect type object(enum?)
            specialTile.tileEffect = (Tile.TileEffect)tileEffect;
            //specialTile.tileColor = new Color32(240, 200, 210, 255);
            //specialTile.transform.GetChild(0).GetComponent<Image>().color = specialTile.tileColor;
            specialTile.tileIcon = specialTile.transform.GetChild(2).GetComponent<Image>();
            specialTile.tileIconName = specialTile.GetTileIconName(tileEffect);
            Sprite specialTileOrgImage = Resources.Load<Sprite>($"Prefabs/Tiles/{specialTile.tileIconName}");
            specialTile.tileIcon.sprite = specialTileOrgImage;

            specialTile.tileSpawnedTurns = roundData.currentTurn;
            specialTile.tileExistedTurns = 1;

            specialTile.outputScore = UnityEngine.Random.Range(minInclusive: 80, 100);

            Debug.Log($"SpecialTileFactory.CreateTile, return specialTile(local var): {specialTile.name}(specialTile.name only) (end)");  
            return specialTile;
        }
        #endregion
    }
}