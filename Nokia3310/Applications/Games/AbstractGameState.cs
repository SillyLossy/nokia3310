using Nokia3310.Applications.Common;
using OpenTK.Input;
using RLNET;

namespace Nokia3310.Applications.Games
{
    public abstract class AbstractGameState
    {
        public StateManager StateManager { get; }

        protected AbstractGameState(GameState initialState)
        {
            StateManager = new StateManager(initialState);
        }

        public abstract void Update(KeyboardState state, RLKeyPress keyPress);
    }
}
