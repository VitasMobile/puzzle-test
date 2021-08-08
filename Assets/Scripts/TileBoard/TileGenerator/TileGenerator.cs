using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PuzzleTest
{
    public class TileGenerator : MonoBehaviour
    {
        #region EVENTS
        public static event System.Action CompleteGenerationLevelEvent;
        #endregion

        [SerializeField] private MaskSO[] _masks;
        [SerializeField] private Texture2D[] _pictures;
        [SerializeField] private Transform _tileContainer;
        [SerializeField] private TileBackground _tileBackground;
        public Transform TileContainer => _tileContainer;



        private Texture2D _picture;
        private MaskSO _currentMask;
        private Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
        private int _tileCounter;


        private void Awake()
        {
            Tile.UpdateTileStateEvent += OnUpdateTileState;
        }

        private void OnDestroy()
        {
            Tile.UpdateTileStateEvent -= OnUpdateTileState;
        }

        // Public Functions
        public void GenerateNewLevel()
        {
            if (GameManager.IsTutorial)
            {
                _picture = _pictures[0];
                _currentMask = _masks[1];
            }
            else
            {
                _picture = GetRndPicture();
                _currentMask = GetRndMask();
            }

            _tileBackground.SetBackground(_picture);

            StartCoroutine(GenerateTiles());
        }

        public bool IsEmpty()
        {
            return _tileContainer.childCount == 0;
        }

        public void Clean()
        {
            foreach (Transform child in _tileContainer)
            {
                Destroy(child.gameObject);
            }

            _tileCounter = 0;
            _textures.Clear();
            GameManager.TilePool.Clean();
        }

        public int GetTileCount()
        {
            return _tileCounter;
        }

        // Private Functions
        private Texture2D GetRndPicture()
        {
            return _pictures[Random.Range(0, _pictures.Length)];
        }

        private MaskSO GetRndMask()
        {
            return _masks[Random.Range(0, _masks.Length)];
        }


        private TileList GetTileList()
        {
            TileList tileList = new TileList();
            JsonUtility.FromJsonOverwrite(_currentMask.maskJson.text, tileList);
            return tileList;
        }

        private IEnumerator GenerateTiles()
        {
            TileList tileList = GetTileList();

            foreach (var tile in tileList.tiles)
            {
                StartCoroutine(CreateTileTexture(tile));
            }

            foreach (TileData tileData in tileList.tiles)
            {
                Tile tile = CreateTile(tileData);

                tile.gameObject.SetActive(false);
                GameManager.TilePool.Push(tile);


                tile.transform.parent = _tileContainer;

                tile.transform.localPosition = new Vector2(tileData.offsetX, -tileData.offsetY) / 100.0f;
                tile.SetCorrectPosition(tile.transform.position);

                _tileCounter++;
            }


            yield return new WaitForSeconds(1.0f);
            CompleteGenerationLevelEvent?.Invoke();
        }

        private Tile CreateTile(TileData tileData)
        {
            Tile tile = new GameObject($"Tile_{tileData.name}").AddComponent<Tile>();

            SpriteRenderer spriteRenderer = tile.gameObject.AddComponent<SpriteRenderer>();
            Texture2D tileTexture = _textures[tileData.img];
            if (tileTexture == null)
            {
                throw new System.Exception("Текстуру по имени {tileData.img} не оказалось в списке");
            }
            spriteRenderer.sprite = Sprite.Create(tileTexture, new Rect(0, 0, tileTexture.width, tileTexture.height), Vector2.one * 0.5f);

            PolygonCollider2D collider = tile.gameObject.AddComponent<PolygonCollider2D>();
            collider.isTrigger = true;


            return tile;
        }

        private IEnumerator CreateTileTexture(TileData tileData)
        {
            Color[] picturePixels = _picture.GetPixels((int)tileData.texX, (int)tileData.texY, (int)tileData.width, (int)tileData.height);

            Texture2D maskTexture = _currentMask.GetTextureByImg(tileData.img);
            if (maskTexture == null)
            {
                throw new System.Exception($"Текстуры по имени {tileData.img} не оказалось в списке");
            }

            Color[] maskPixels = maskTexture.GetPixels(0, 0, (int)tileData.width, (int)tileData.height);
            for (int i = 0; i < picturePixels.Length; i++)
            {
                picturePixels[i].a = maskPixels[i].a;
            }

            Texture2D tileTexture = new Texture2D((int)tileData.width, (int)tileData.height);
            tileTexture.SetPixels(0, 0, (int)tileData.width, (int)tileData.height, picturePixels);

            _textures.Add(tileData.img, tileTexture);

            yield return null;

            tileTexture.Apply();
        }
 
        #region Callback Functions
        private void OnUpdateTileState(Tile tile, TileStateType tileStateType)
        {
            if (tileStateType == TileStateType.DONE)
            {
                tile.transform.parent = _tileContainer;
            }
        }
        #endregion

    }
}
