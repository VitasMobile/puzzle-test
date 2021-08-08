using UnityEngine;
using UnityEngine.Playables;

namespace PuzzleTest
{
    [System.Serializable]
    public class TutorialPointerPlayableAsset : PlayableAsset
    {
        public ExposedReference<Transform> cursor;
        public ExposedReference<TileBar> tileBar;
        public int tileIndex = 0;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            var playable = ScriptPlayable<TutorialPointerPlayableBehaviour>.Create(graph);
            playable.GetBehaviour().tileBar = tileBar.Resolve(graph.GetResolver());
            playable.GetBehaviour().tileIndex = tileIndex;
            playable.GetBehaviour().cursor = cursor.Resolve(graph.GetResolver());
            return playable;
        }
    }
}
