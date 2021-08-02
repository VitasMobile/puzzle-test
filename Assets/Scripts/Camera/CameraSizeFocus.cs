using UnityEngine;

namespace PuzzleTest
{
    public class CameraSizeFocus : MonoBehaviour
    {
        public enum MODE {
            AUTO,
            HORIZONTAL,
            VERTICAL
        };

        private Camera _camera;
        public SpriteRenderer rink;
        public MODE mode = MODE.AUTO;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            UpdateCameraSize();
        }

        private void UpdateCameraSize()
        {
            if (rink == null)
            {
                return;
            }

            float orthoSize = _camera.orthographicSize;


            switch (mode)
            {
                case MODE.VERTICAL:
                    orthoSize = rink.bounds.size.x * (Screen.height * _camera.rect.height) / (Screen.width * _camera.rect.width) * 0.5f;
                    break;

                case MODE.HORIZONTAL:
                    orthoSize = rink.bounds.size.y / 2;
                    break;

                case MODE.AUTO:
                    float screenRatio = ((float)Screen.width * _camera.rect.width) / ((float)Screen.height * _camera.rect.height);
                    float targetRatio = rink.bounds.size.x / rink.bounds.size.y;

                    if (screenRatio <= targetRatio)
                    {
                        orthoSize = rink.bounds.size.y / 2;
                    }
                    else
                    {
                        float differenceInSize = targetRatio / screenRatio;
                        orthoSize = rink.bounds.size.y / 2 * differenceInSize;
                    }
                    break;
            }

            orthoSize = Mathf.Max(1.0f, orthoSize);
            _camera.orthographicSize = orthoSize;
        }
    }
}
