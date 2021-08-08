using UnityEngine;

namespace PuzzleTest
{
    public class LoadingScreenUI : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private GameManager _gameManager;


        private void Awake()
        {
            GameManager.ChangeGameStateEvent += OnChangedGameState;
            TileGenerator.CompleteGenerationLevelEvent += OnCompletedGenerationLevel;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GameManager.ChangeGameStateEvent -= OnChangedGameState;
            TileGenerator.CompleteGenerationLevelEvent -= OnCompletedGenerationLevel;
        }

        private void OnChangedGameState(GameStateTypes gameStateType)
        {
            gameObject.SetActive(gameStateType == GameStateTypes.LOADING | gameObject.activeInHierarchy);
        }

        private void OnCompletedGenerationLevel()
        {
            _animator.SetTrigger("hide");
        }


        private void AnHandle_ShowCompleted()
        {
            GameManager.SetState(GameStateTypes.CREATE_LEVEL);
        }

        private void AnHandle_HideCompleted()
        {
            if (GameManager.IsTutorial)
            {
                GameManager.SetState(GameStateTypes.TUTORIAL);
            }
            else
            {
                GameManager.SetState(GameStateTypes.PLAY);
            }
            gameObject.SetActive(false);
        }
    }
}
