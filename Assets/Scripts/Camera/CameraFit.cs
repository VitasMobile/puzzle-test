using UnityEngine;

namespace PuzzleTest
{
    public class CameraFit : MonoBehaviour {

        public SpriteRenderer spriteToFitTo;

        void Start()
        {
            var bounds = spriteToFitTo.bounds.extents;
            var height = bounds.x / GetComponent<Camera>().aspect;
            if (height < bounds.y)
            {
                height = bounds.y;
            }
            GetComponent<Camera>().orthographicSize = height;
        }
    }
}
