using UnityEngine;

namespace PuzzleTest
{
    public class GameScreenStage : MonoBehaviour, IState
    {
        public GameManager GameManager { get; private set; }

        private int _tileCounter;


        public void Bind(GameManager gameManager)
        {
            GameManager = gameManager;
        }

        public void Enter()
        {
            Tile.UpdateTileStateEvent += OnUpdateTileState;
            GameHUD.MainMenuEvent += OnMainMenu;

            _tileCounter = 0;
            GameManager.TileBar.UpdateTileBar();
        }

        public void Exit()
        {
            Tile.UpdateTileStateEvent -= OnUpdateTileState;
            GameHUD.MainMenuEvent -= OnMainMenu;
        }


        private void OnMainMenu()
        {
            GameManager.SetState(GameStateTypes.MAIN_MENU);
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
