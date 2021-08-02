using System.Collections;
using UnityEngine;

namespace PuzzleTest
{
    public class LoadingScreenStage : IState
    {
        public GameManager GameManager { get; private set; }


        public void Bind(GameManager gameManager)
        {
            GameManager = gameManager;
        }

        public void Enter()
        {
            GameManager.ChangeGameStateEvent += OnChangedGameState;
        }

        public void Exit()
        {
            GameManager.ChangeGameStateEvent -= OnChangedGameState;
        }

        private IEnumerator NewLevel()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);

            GameManager.TileBar.Clean();
            while (!GameManager.TileBar.IsEmpty())
            {
                yield return waitForSeconds;
            }

            GameManager.TileGenerator.Clean();
            while (!GameManager.TileGenerator.IsEmpty())
            {
                yield return null;
            }

            GameManager.TileGenerator.GenerateNewLevel();
        }

        private void OnChangedGameState(GameStateTypes gameStateType)
        {
            if (gameStateType == GameStateTypes.CREATE_LEVEL)
            {
                CreateNewLevel();
            }
        }

        private void CreateNewLevel()
        {
            GameManager.StartCoroutine(NewLevel());
        }
    }
}
