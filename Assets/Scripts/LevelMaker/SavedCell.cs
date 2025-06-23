using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BullBrukBruker
{
    [Serializable]
    public class SavedCell
    {
        public Vector3Int Position;
        public int NumCollide;
        public ItemID ItemInside;
        public Tile Tile;
    }
}