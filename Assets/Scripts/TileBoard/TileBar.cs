using UnityEngine;

namespace PuzzleTest
{
    public class TileBar : MonoBehaviour
    {
        // Mono Functions
        private void Start()
        {
            Tile.UpdateTileStateEvent += OnUpdateTileState;
        }

        private void OnDestroy()
        {
            Tile.UpdateTileStateEvent -= OnUpdateTileState;
        }


        // Public Functions
        public void UpdateTileBar()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).childCount > 0)
                {
                    continue;
                }

                if ((i + 1) < transform.childCount && transform.GetChild(i + 1).childCount > 0)
                {
                    Tile tile = transform.GetChild(i + 1).GetChild(0).GetComponent<Tile>();
                    tile.transform.parent = transform.GetChild(i);
                    tile.Fly(Vector2.zero, Vector2.one * 0.5f);
                }
                else
                {
                    Tile tile = GetFreeTile();
                    if (tile)
                    {
                        tile.transform.parent = transform.GetChild(i);
                        tile.transform.localPosition = Vector2.zero;
                        tile.transform.localScale = Vector3.zero;
                        tile.Fly(Vector2.zero, Vector2.one * 0.5f);
                    }
                }
            }
        }

        public bool IsEmpty()
        {
            bool isEmpty = true;

            foreach (Transform child in transform)
            {
                isEmpty &= child.childCount == 0;
            }
            return isEmpty;
        }

        public void Clean()
        {
            foreach (Transform child in transform)
            {
                if (child.childCount > 0)
                {
                    Destroy(child.GetChild(0).gameObject);
                }
            }
        }

        // Private Functions
        private Tile GetFreeTile()
        {
            Tile tile = GameManager.TilePool.GetRndFreeTile();
            if (tile)
            {
                tile.gameObject.SetActive(true);
            }
            return tile;
        }

        // Callback Functions
        private void OnUpdateTileState(Tile tile, TileStateType tileStateType)
        {
            UpdateTileBar();
        }

    }
}