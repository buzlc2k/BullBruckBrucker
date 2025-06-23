using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BullBrukBruker
{
    public class GridCell
    {
        private readonly Vector3Int index;
        private int numCollide;
        private readonly ItemID itemInside;
        public Dictionary<ObjectID, List<ObjectCollision>> Objects { get; private set; }

        public GridCell(Vector3Int index, int numCollide, ItemID itemInside)
        {
            this.index = index;
            this.numCollide = numCollide;
            this.itemInside = itemInside;

            Objects = new();
        }

        public Vector3 GetCellPos() {
            return GridManager.Instance.Tilemap.GetCellCenterWorld(index);
        } 

        public bool CanCollide()
        {
            if (numCollide <= 0) return false;

            numCollide--;

            if (numCollide == 0)
            {
                GridManager.Instance.RemoveCellInGrid(index);

                if (!itemInside.Equals(ItemID.None))
                    ItemSpawner.Instance.SpawnItem(itemInside, GetCellPos());
            }

            return true;
        }

        public void RegisterObject(ObjectID id, ObjectCollision obj)
        {
            if (!Objects.ContainsKey(id))
                Objects.Add(id, new List<ObjectCollision>());

            Objects[id].Add(obj);
        }

        public void UnRegisterObject(ObjectID id, ObjectCollision obj)
        {
            if (!Objects.ContainsKey(id)) return;

            Objects[id].Remove(obj);
        }
    }

    public class GridManager : SingletonMono<GridManager>
    {
        private int remainCells = 0;
        private Dictionary<Vector3Int, GridCell> cellGrid;

        [field: SerializeField] public List<LevelBoundController> LevelBounds { get; private set; }
        [field: SerializeField] public LevelBoundController DeathZone { get; private set; }
        [field: SerializeField] public Tilemap Tilemap { get; private set; }
        public float CellSize { get => 0.25f; }

        public void InitGridCells(List<SavedCell> savedCells)
        {
            cellGrid ??= new();
            cellGrid.Clear();

            Tilemap.ClearAllTiles();

            remainCells = 0;

            FillNonEmpyCells(savedCells);

            FillEmptyCells();

            CreateGridBounds();
        }

        private void FillNonEmpyCells(List<SavedCell> savedCells)
        {
            foreach (var savedCell in savedCells)
            {
                var pos = savedCell.Position;
                cellGrid.Add(pos, new GridCell(pos, savedCell.NumCollide, savedCell.ItemInside));
                Tilemap.SetTile(pos, savedCell.Tile);

                if (savedCell.NumCollide == 1) remainCells++;
            }
        }

        private void FillEmptyCells()
        {
            var screenWidth = ScreenManager.Instance.ScreenWidth;
            var screenHeight = ScreenManager.Instance.ScreenHeight;

            var totalXBlocks = Mathf.FloorToInt(screenWidth / (CellSize * 2)) + 1;
            var totalYBlocks = Mathf.FloorToInt(screenHeight / (CellSize * 2)) + 1;

            for (int u = -totalXBlocks; u <= totalXBlocks; u++)
            {
                for (int v = -totalYBlocks; v <= totalYBlocks; v++)
                {
                    var index = new Vector3Int(u, v, 0);
                    if (!cellGrid.ContainsKey(index))
                    {
                        cellGrid.Add(index, new GridCell(index, 0, ItemID.None));
                    }
                }
            }
        }

        private void CreateGridBounds()
        {
            var horizontalBounds = LevelBounds.Where(b => b.ID.Equals(ObjectID.HorizontalLevelBound))
                                    .Prepend(DeathZone).ToList();

            var verticalBounds = LevelBounds.Where(b => !b.ID.Equals(ObjectID.HorizontalLevelBound)).ToList();

            for (int i = 0; i < horizontalBounds.Count; i++)
            {
                int sign = (i % 2 == 0) ? -1 : 1;
                horizontalBounds[i].transform.position = new Vector3(0, sign * (ScreenManager.Instance.ScreenHeight + CellSize), 0);
                horizontalBounds[i].gameObject.SetActive(true);
                horizontalBounds[i].InitLevelBound();
            }

            for (int i = 0; i < verticalBounds.Count; i++)
            {
                int sign = (i % 2 == 0) ? -1 : 1;
                verticalBounds[i].transform.position = new Vector3(sign * (ScreenManager.Instance.ScreenWidth + CellSize), 0, 0);
                verticalBounds[i].gameObject.SetActive(true);
                verticalBounds[i].InitLevelBound();
            }
        }

        public List<GridCell> GetCurrentCells(Vector3 position, float width, float height)
        {
            var currentCells = new HashSet<GridCell>();

            var originIndex = Tilemap.WorldToCell(position);
            if (!cellGrid.ContainsKey(originIndex)) return currentCells.ToList();

            currentCells.Add(cellGrid[originIndex]);

            var totalXBlocks = Mathf.FloorToInt(Mathf.Abs(width - CellSize) / (CellSize * 2));
            var totalYBlocks = Mathf.FloorToInt(Mathf.Abs(height - CellSize) / (CellSize * 2));

            for (int u = -totalXBlocks; u <= totalXBlocks; u++)
            {
                for (int v = -totalYBlocks; v <= totalYBlocks; v++)
                {
                    var newIndex = originIndex + new Vector3Int(u, v, 0);
                    if (cellGrid.TryGetValue(newIndex, out var cell))
                        currentCells.Add(cell);
                }
            }

            return currentCells.ToList();
        }

        public void RemoveCellInGrid(Vector3Int index)
        {
            Tilemap.SetTile(index, null);
            remainCells -= 1;

            if (remainCells <= 0)
                Observer.PostEvent(EventID.OutOfCells, null);
        }
    }
}