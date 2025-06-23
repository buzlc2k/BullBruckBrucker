using UnityEngine;
using UnityEditor;
using SABI;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BullBrukBruker
{
#if UNITY_EDITOR
    public class LevelMaker : MonoBehaviour
    {
        [Header("Maker")]
        [SerializeField] private int currentLevel;
        [SerializeField] private Tilemap blockMapTile;
        [SerializeField] private Tilemap itemMapTile;

        private ItemID GetItem(Vector3Int position)
        {
            if (itemMapTile.HasTile(position))
            {
                var id = ((Tile)itemMapTile.GetTile(position)).sprite.name;
                return (ItemID)Enum.Parse(typeof(ItemID), id);
            }

            return ItemID.None;
        }

        private int GetNumCollide(string ID)
        {
            return ConfigsManager.Instance.BlockConfig.Records
                .FirstOrDefault(data => data.ID.ToString().Equals(ID)).NumCollide;
        }

        private SavedCell CreateSavedTile(Tile tile, Vector3Int position)
        {
            var savedCell = new SavedCell
            {
                Tile = tile,
                Position = position,
                ItemInside = GetItem(position),
                NumCollide = GetNumCollide(tile.sprite.name)
            };

            return savedCell;
        }

        private List<SavedCell> GetAllTiles()
        {
            var savedTiles = new List<SavedCell>();

            foreach (var pos in blockMapTile.cellBounds.allPositionsWithin)
            {
                if (!blockMapTile.HasTile(pos)) continue;

                var tile = blockMapTile.GetTile<Tile>(pos);
                if (tile == null)
                {
                    Debug.LogError($"Tile at position {pos} is null!");
                    continue;
                }

                savedTiles.Add(CreateSavedTile(tile, pos));
            }

            return savedTiles;
        }

        [Button(height: 17, textSize: 15)]
        private void SaveLevel()
        {
            var levelConfig = ConfigsManager.Instance.LevelConfig;
            var existingRecord = levelConfig.GetRecord(currentLevel);

            if (existingRecord != null)
            {
                Debug.Log($"Overriding existing level {currentLevel}");
                levelConfig.RemoveRecord(existingRecord);
            }

            var newRecord = new LevelConfigRecord
            {
                Index = currentLevel,
                Cells = GetAllTiles()
            };

            levelConfig.AddRecord(newRecord);

            EditorUtility.SetDirty(levelConfig);
            AssetDatabase.SaveAssets();

            Debug.Log($"Level {currentLevel} saved successfully!");
        }

        private void LoadSavedCell(SavedCell cell)
        {
            blockMapTile.SetTile(cell.Position, cell.Tile);

            if (cell.ItemInside.Equals(ItemID.None)) return;

            var itemTile = Resources.Load<Tile>($"Art/TilePallet/Item/{cell.ItemInside}");
            itemMapTile.SetTile(cell.Position, itemTile);
        }

        [Button(height: 17, textSize: 15)]
        private void LoadLevel()
        {
            var record = ConfigsManager.Instance.LevelConfig.GetRecord(currentLevel);;

            if (record == null)
            {
                Debug.LogWarning($"Level {currentLevel} does not exist!");
                return;
            }

            ClearLevel();

            foreach (var cell in record.Cells)
                LoadSavedCell(cell);

            Debug.Log($"Level {currentLevel} loaded successfully!");
        }

        [Button(height: 17, textSize: 15)]
        private void ClearLevel()
        {
            blockMapTile.ClearAllTiles();

            itemMapTile.ClearAllTiles();
        }
    }
#endif
}