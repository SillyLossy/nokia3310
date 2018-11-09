using System;
using RLNET;

namespace Nokia3310.Applications.Common
{
    public abstract class NokiaApp
    {
        public static Random Random { get; } = new Random();

        public virtual void Run()
        {
            Console.Render += Render;
            Console.Update += Update;
        }

        public virtual void Destroy()
        {
            Parent?.Run();
            Console.Render -= Render;
            Console.Update -= Update;
        }

        public abstract void Update(object sender, UpdateEventArgs e);
        public abstract void Render(object sender, UpdateEventArgs e);
        public abstract string Name { get; }

        protected RLRootConsole Console;
        protected NokiaApp Parent;

        public const int FPS = 60;
        public const int ScreenWidth = 28;
        public const int ScreenHeight = 16;

        protected static readonly RLColor BackgroundColor = new RLColor(125, 170, 130);
        protected static readonly RLColor ForegroundColor = new RLColor(40, 40, 40);

        protected NokiaApp(NokiaApp parent)
        {
            Parent = parent;
        }
    }
}
