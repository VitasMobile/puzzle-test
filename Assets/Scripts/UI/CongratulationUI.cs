using System;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleTest
{
    public class CongratulationUI : MonoBehaviour
    {
        public static event Action ClickNextLevelButtonEvent;
        public static event Action NextLevelEvent;

        [SerializeField] private Animator _animator;
        [SerializeField] private Button _nextLevelButton;


        private void Awake()
        {
            _nextLevelButton.onClick.AddListener(OnNextLevelClicked);
            GameManager.ChangeGameStateEvent += OnChangedGameState;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _nextLevelButton.onClick.RemoveListener(OnNextLevelClicked);
            GameManager.ChangeGameStateEvent -= OnChangedGameState;
        }

        private void OnChangedGameState(GameStateTypes gameStateTypes)
        {
            gameObject.SetActive(gameStateTypes == GameStateTypes.COMPLETE | gameObject.activeInHierarchy);
        }

        private void OnNextLevelClicked()
        {
            ClickNextLevelButtonEvent?.Invoke();
            _animator.SetTrigger("hide");
        }

        private void AnHandle_HideFinished()
        {
            NextLevelEvent?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
