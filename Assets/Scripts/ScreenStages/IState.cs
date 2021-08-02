namespace PuzzleTest
{
    public interface IState
    {
        GameManager GameManager { get; }

        void Bind(GameManager gameManager);
        void Enter();
        void Exit();
    }
}
