using Nokia3310.Applications.Common;
using OpenTK.Input;
using RLNET;

namespace Nokia3310.Applications.Games
{
    public abstract class AbstractGame<TGameState> : NokiaApp where TGameState : AbstractGameState, new()
    {
        protected TGameState State;

        protected AbstractGame(NokiaApp parent, RLRootConsole console) : base(parent)
        {
            Console = console;
            State = new TGameState();
        }

        public override void Run()
        {
            base.Run();
            State = new TGameState();
        }

        public override void Update(object sender, UpdateEventArgs e)
        {
            State.Update(Keyboard.GetState(), Console.Keyboard.GetKeyPress());

            if (State.StateManager.CurrentState == GameState.Destroyed)
            {
                Destroy();
            }
        }
    }
}
