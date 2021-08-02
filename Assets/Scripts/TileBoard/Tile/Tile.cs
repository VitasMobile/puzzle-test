using UnityEngine;
using UnityEngine.EventSystems;

namespace PuzzleTest
{
    public class Tile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public delegate void UpdateTileStateHandle(Tile tile, TileStateType tyleStateType);
        public static event UpdateTileStateHandle UpdateTileStateEvent;

        public float flySpeed = 30.0f;
        public float scaleSpeed = 20.0f;

        private Camera _mainCamera;
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider;
        private int _prevSortingOrder;
        private Vector2 _correctPosition;
        private Vector2 _toFlyPosition;
        private Vector2 _toFlyScale;
        private Vector2 _dragOffset;
        private TileStateType _tileStateType = TileStateType.HIDDEN;


        // MONO FUNCTIONS
        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            if (_collider == null)
            {
                _collider = GetComponent<Collider2D>();
            }
        }

        private void Update()
        {
            if (_tileStateType != TileStateType.FLYING)
            {
                return;
            }

            transform.localPosition = Vector2.MoveTowards(transform.localPosition, _toFlyPosition, flySpeed * Time.deltaTime);
            transform.localScale = Vector2.Lerp(transform.localScale, _toFlyScale, scaleSpeed * Time.deltaTime);

            if ((Vector2)transform.localPosition == _toFlyPosition && (Vector2)transform.localScale == _toFlyScale)
            {
                ResetSprite();
                _collider.enabled = true;
                SetState(TileStateType.CASSETE);
            }
        }


        // PUBLIC FUNTIONS
        public void SetState(TileStateType tileStateType, bool isInvoking = true)
        {
            if (_tileStateType == tileStateType || !isInvoking)
            {
                return;
            }

            _tileStateType = tileStateType;
            UpdateTileStateEvent?.Invoke(this, tileStateType);
        }

        public void Fly(Vector2 position, Vector2 scale)
        {
            _collider.enabled = false;
            _toFlyPosition = position;
            _toFlyScale = scale;

            SetState(TileStateType.FLYING);
        }

        public Vector2 GetSize()
        {
            return _spriteRenderer.bounds.size; 
        }

        public void SetCorrectPosition(Vector2 position)
        {
            _correctPosition = position;
        }

        // PRIVATE FUNCTIONS
        private bool IsCorrect()
        {
            float distance = Vector2.Distance(_correctPosition, (Vector2)transform.position);
            return distance < 0.5f;
        }

        private void ResetSprite()
        {
            _spriteRenderer.sortingOrder = _prevSortingOrder;
            _spriteRenderer.color = Color.white;
        }


        // CALLBACK FUNCTIONS
        public void OnBeginDrag(PointerEventData eventData)
        {
            _prevSortingOrder = _spriteRenderer.sortingOrder;
            _spriteRenderer.sortingOrder = 10;

            Color color = _spriteRenderer.color;
            color.a = 0.8f;
            _spriteRenderer.color = color;

            transform.localScale = Vector3.one;
            _dragOffset = ((Vector2)transform.position - (Vector2)_mainCamera.ScreenToWorldPoint(Input.mousePosition));
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition + _dragOffset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsCorrect())
            {
                ResetSprite();

                transform.position = _correctPosition;
                _collider.enabled = false;

                SetState(TileStateType.DONE);
            }
            else
            {
                Fly(Vector2.zero, Vector2.one * 0.5f);
            }
        }
    }
}
