using System.Collections.Generic;
using UnityEngine;

namespace PuzzleTest
{
    public class TilePool
    {
        private List<Tile> _tiles;
        private Transform _parent;

        public TilePool(Transform parent)
        {
            _tiles = new List<Tile>();

            _parent = parent;
        }

        public void Clean()
        {
            _tiles.Clear();
        }

        public void Push(Tile tile)
        {
            tile.transform.parent = _parent;
            _tiles.Add(tile);
        }

        public Tile GetRndFreeTile()
        {
            int rndIndex = Random.Range(0, _tiles.Count);
            return GetTileByIndex(rndIndex);
        }

        public Tile GetTileByIndex(int index)
        {
            if (_tiles.Count > 0 && index < _tiles.Count)
            {
                Tile tile = _tiles[index];
                _tiles.RemoveAt(index);
                return tile;
            }

            return null;
        }
    }
}
