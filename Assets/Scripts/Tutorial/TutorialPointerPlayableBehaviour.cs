using UnityEngine;
using UnityEngine.Playables;

namespace PuzzleTest
{
    public class TutorialPointerPlayableBehaviour : PlayableBehaviour
    {
        public Transform cursor;
        public TileBar tileBar;
        public int tileIndex;
        public Tile tile;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            tile = tileBar.GetTileByIndex(tileIndex);
            tile?.StartDrag(cursor.position);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            tile?.EndDrag();
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            tile?.UpdateDrag(cursor.position);
        }
    }
}
