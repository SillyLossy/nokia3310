namespace Nokia3310.Applications.Common
{
    public class StateManager
    {
        public GameState CurrentState { get; private set; }
        public GameState PreviousState { get; private set; }

        public StateManager(GameState initialState)
        {
            CurrentState = initialState;
            PreviousState = initialState;
        }

        public void ChangeState(GameState state)
        {
            PreviousState = CurrentState;
            CurrentState = state;
        }
    }
}
