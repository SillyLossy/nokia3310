using Nokia3310.Applications.Common;
using RLNET;

namespace Nokia3310.Applications.HelloWorld
{
    public class HelloWorldApp : NokiaApp
    {
        public HelloWorldApp(NokiaApp parent, RLRootConsole console) : base(parent)
        {
            Console = console;
        }

        public override void Update(object sender, UpdateEventArgs e)
        {
            RLKey? key = Console.Keyboard.GetKeyPress()?.Key;

            if (key.HasValue)
            {
                Destroy();
            }
        }

        public override void Render(object sender, UpdateEventArgs e)
        {
            Console.Clear(' ', BackgroundColor, ForegroundColor);

            Console.Print(1, 1, "Hello, World!", ForegroundColor, BackgroundColor);

            Console.Draw();
        }

        public override string Name => "Hello World";
    }
}
