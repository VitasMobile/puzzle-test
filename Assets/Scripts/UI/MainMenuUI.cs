using UnityEngine;
using UnityEngine.UI;

namespace PuzzleTest
{
    public class MainMenuUI : MonoBehaviour
    {
        #region EVENTS
        public static event System.Action ClickPlayGameButtonEvent;
        public static event System.Action OnExitMainMenuEvent;
        #endregion

        [SerializeField] private Animator _animator;
        [SerializeField] private Button _playGameButton;


        private void Awake()
        {
            GameManager.ChangeGameStateEvent += OnChangedGameState;
            _playGameButton.onClick.AddListener(OnPlayGameButtonClicked);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GameManager.ChangeGameStateEvent -= OnChangedGameState;
            _playGameButton.onClick.RemoveListener(OnPlayGameButtonClicked);
        }

        private void OnChangedGameState(GameStateTypes gameStateType)
        {
            gameObject.SetActive(gameStateType == GameStateTypes.MAIN_MENU | gameObject.activeInHierarchy);
        }

        private void OnPlayGameButtonClicked()
        {
            _animator.SetTrigger("hide");
            GameManager.SetState(GameStateTypes.LOADING);
            ClickPlayGameButtonEvent?.Invoke();
        }

        private void AnHandle_HideCompleted()
        {
            OnExitMainMenuEvent?.Invoke();

            gameObject.SetActive(false);
        }
    }
}
