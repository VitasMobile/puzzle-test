using System.Collections;
using UnityEngine;

namespace PuzzleTest
{
    public class TileBackground : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _pictureBackground;


        private void Start()
        {
            GameManager.ChangeGameStateEvent += OnChangedGameState;
        }

        private void OnDestroy()
        {
            GameManager.ChangeGameStateEvent -= OnChangedGameState;
        }

        private void OnChangedGameState(GameStateTypes gameStateType)
        {
            switch (gameStateType)
            {
                case GameStateTypes.PLAY:
                case GameStateTypes.TUTORIAL:
                    SetBackgroundTransparent(0.2f);
                    break;

                case GameStateTypes.COMPLETE:
                    SetBackgroundTransparent(1.0f);
                    break;

                default:
                    SetBackgroundTransparent(0.0f);
                    break;
            }
        }

        public void SetBackground(Texture2D picture)
        {
            _pictureBackground.sprite = Sprite.Create(picture, new Rect(0.0f, 0.0f, picture.width, picture.height), Vector2.one * 0.5f);
        }

        private void SetBackgroundTransparent(float alpha)
        {
            StartCoroutine(StartBackgroundAnimation(alpha));
        }

        private IEnumerator StartBackgroundAnimation(float alpha)
        {
            WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(0.1f);
            Color color = _pictureBackground.color;
            float step = color.a > alpha ? -0.3f : 0.3f;

            while (true)
            {
                color.a += step;
                if ((step < 0 && color.a <= alpha) || (step > 0 && color.a >= alpha))
                {
                    color.a = alpha;
                }
                _pictureBackground.color = color;

                if (color.a == alpha)
                {
                    break;
                }

                yield return waitForSecondsRealtime;
            }
        }
    }
}
