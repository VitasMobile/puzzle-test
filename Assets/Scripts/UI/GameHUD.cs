using UnityEngine;
using UnityEngine.UI;

namespace PuzzleTest
{
    public class GameHUD : MonoBehaviour
    {
        public static event System.Action MainMenuEvent;

        [SerializeField] private Animator _animator;
        [SerializeField] private Button _mainMenuButton;

        private void Awake()
        {
            GameManager.ChangeGameStateEvent += OnChangedGameState;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GameManager.ChangeGameStateEvent -= OnChangedGameState;
        }

        private void OnEnable()
        {
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }

        private void OnDisable()
        {
            _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
        }

        private void OnChangedGameState(GameStateTypes gameStateType)
        {
            gameObject.SetActive(gameStateType == GameStateTypes.PLAY | gameObject.activeInHierarchy);

            if (gameStateType == GameStateTypes.COMPLETE)
            {
                _animator.SetTrigger("hide");
            }
        }


        private void OnMainMenuButtonClicked()
        {
            _animator.SetTrigger("hide");
            MainMenuEvent?.Invoke();
        }

        private void AnHandle_HideCompleted()
        {
            gameObject.SetActive(false);
        }
    }
}
