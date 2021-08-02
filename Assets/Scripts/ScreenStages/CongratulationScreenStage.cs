using UnityEngine;

namespace PuzzleTest
{
    public class CongratulationScreenStage : MonoBehaviour, IState
    {
        public GameManager GameManager { get; private set; }
        [SerializeField] private ParticleSystem _confettiFx;
        private ParticleSystem.EmissionModule _confittiEmission;

        private void Start()
        {
            _confittiEmission = _confettiFx.emission;
        }

        public void Bind(GameManager gameManager)
        {
            GameManager = gameManager;
        }

        public void Enter()
        {
            CongratulationUI.NextLevelEvent += OnNextLevel;
            CongratulationUI.ClickNextLevelButtonEvent += OnClickedNextLevelButton;

            _confittiEmission.enabled = true;
            _confettiFx.Play();
        }

        public void Exit()
        {
        }

        private void OnClickedNextLevelButton()
        {
            CongratulationUI.ClickNextLevelButtonEvent -= OnClickedNextLevelButton;
            _confittiEmission.enabled = false;
        }

        private void OnNextLevel()
        {
            CongratulationUI.NextLevelEvent -= OnNextLevel;
            GameManager.SetState(GameStateTypes.LOADING);
        }
    }
}
