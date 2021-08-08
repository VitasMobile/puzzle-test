using UnityEngine;
using UnityEngine.Playables;

namespace PuzzleTest
{
    public class TutorialScreenStage : MonoBehaviour, IState
    {
        public GameManager GameManager { get; private set; }
        [SerializeField] private PlayableDirector _playableDirector;

        private int _tileCounter;

        public void Bind(GameManager gameManager)
        {
            GameManager = gameManager;
        }

        public void Enter()
        {
            Tile.UpdateTileStateEvent += OnUpdateTileState;
            GameManager.TileBar.UpdateTileBar();

            _playableDirector.Play();
        }

        public void Exit()
        {
            Tile.UpdateTileStateEvent -= OnUpdateTileState;
            GameManager.IsTutorial = false;
            gameObject.SetActive(false);
        }

        private void OnUpdateTileState(Tile tile, TileStateType tileStateType)
        {
            switch (tileStateType)
            {
                case TileStateType.DONE:
                    _tileCounter++;
                    break;
            }

            if (_tileCounter == GameManager.TileGenerator.GetTileCount())
            {

                GameManager.SetState(GameStateTypes.COMPLETE);
            }
        }
    }
}
