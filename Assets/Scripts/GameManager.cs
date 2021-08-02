using System.Collections.Generic;
using UnityEngine;

namespace PuzzleTest
{
    public class GameManager : MonoBehaviour
    {
        public delegate void ChangeGameStateHandle(GameStateTypes gameStateType);
        public static event ChangeGameStateHandle ChangeGameStateEvent;



        private static Dictionary<GameStateTypes, IState> states;
        private static GameStateTypes _gameStateType;
        [SerializeField] private GameScreenStage _gameStage;
        [SerializeField] private CongratulationScreenStage _congratulationStage;
        private static IState _currentScreenState;


        public static TilePool TilePool { get; private set; }
        internal TileBar TileBar { get; private set; }
        internal TileGenerator TileGenerator { get; private set; }

        #region SIMPLE_INSTANCE
        private static GameManager _gameManagerInstance;
        #endregion

        // Mono Functions
        private void Awake()
        {
            _gameManagerInstance = this;
        }

        private void Start()
        {
            TileBar = FindObjectOfType<TileBar>();
            TileGenerator = FindObjectOfType<TileGenerator>();

            TilePool = new TilePool(TileGenerator.TileContainer);

            states = new Dictionary<GameStateTypes, IState>();
            states.Add(GameStateTypes.MAIN_MENU, null);
            states.Add(GameStateTypes.LOADING, new LoadingScreenStage());
            states.Add(GameStateTypes.CREATE_LEVEL, null);
            states.Add(GameStateTypes.PLAY, _gameStage);
            states.Add(GameStateTypes.COMPLETE, _congratulationStage);

            SetState(GameStateTypes.MAIN_MENU);
        }

        // Public Functons
        public static void SetState(GameStateTypes gameStateType)
        {
            if (_gameStateType == gameStateType)
            {
                return;
            }

            _gameStateType = gameStateType;

            if (states[_gameStateType] != null)
            {
                if (_currentScreenState != null)
                {
                    _currentScreenState.Exit();
                }

                _currentScreenState = states[_gameStateType];

                _currentScreenState?.Bind(_gameManagerInstance);
                _currentScreenState?.Enter();
            }


            ChangeGameStateEvent?.Invoke(_gameStateType);
        }
    }
}
