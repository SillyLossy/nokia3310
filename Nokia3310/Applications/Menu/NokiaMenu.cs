using System.Collections.Generic;
using System.Linq;
using Nokia3310.Applications.Common;
using Nokia3310.Applications.HelloWorld;
using Nokia3310.Applications.Snake;
using Nokia3310.Applications.Tanks;
using RLNET;

namespace Nokia3310.Applications.Menu
{
    public class NokiaMenu : NokiaApp
    {
        private readonly List<NokiaApp> menuItems;

        private int currentSelection;

        public NokiaMenu() : base(null)
        {
            var settings = new RLSettings
            {
                Title = "Nokia 3310",
                BitmapFile = "font_16x16.png",
                CharHeight = 16,
                CharWidth = 16,
                Width = ScreenWidth,
                Height = ScreenHeight,
                Scale = 1,
                ResizeType = RLResizeType.ResizeScale,
                StartWindowState = RLWindowState.Normal,
                WindowBorder = RLWindowBorder.Resizable
            };
            
            Console = new RLRootConsole(settings);
            menuItems = new List<NokiaApp>
            {
                new SnakeGame(this, Console),
                new TanksGame(this, Console),
                new HelloWorldApp(this, Console)
            };
        }

        public override void Render(object sender, UpdateEventArgs e)
        {
            Console.Clear(' ', BackgroundColor, BackgroundColor);

            Console.Print(0, 0, "           M E N U           ", BackgroundColor, ForegroundColor);

            const int marginX = 2;

            int offsetY = 2;

            const int itemsPerPage = 7;
            int currentPage = currentSelection / itemsPerPage;

            foreach (var item in menuItems.Skip(currentPage * itemsPerPage).Take(itemsPerPage))
            {
                int index = menuItems.IndexOf(item);
                bool isCurrent = index == currentSelection;
                var foreground = isCurrent ? BackgroundColor : ForegroundColor;
                var background = isCurrent ? ForegroundColor : BackgroundColor;

                Console.Print(2, offsetY, $"{index + 1}. {item.Name}".PadRight(ScreenWidth - marginX * 2), foreground, background);

                offsetY += 2;
            }

            Console.Draw();
        }

        public override string Name => "Menu";

        public override void Run()
        {
            base.Run();
            if (!isRunning)
            {
                isRunning = true;
                Console.Run(FPS);
            }
        }

        private bool isRunning;

        public void Exit()
        {
            Console.Close();
        }

        public override void Update(object sender, UpdateEventArgs e)
        {
            RLKey? key = Console.Keyboard.GetKeyPress()?.Key;

            switch (key)
            {
                case RLKey.Down:
                    currentSelection = currentSelection + 1 > menuItems.Count - 1 ? 0 : currentSelection + 1;
                    break;
                case RLKey.Up:
                    currentSelection = currentSelection - 1 < 0 ? menuItems.Count - 1 : currentSelection - 1;
                    break;
                case RLKey.Enter:
                case RLKey.Space:
                    RunApp();
                    break;
                case RLKey.Escape:
                    Exit();
                    break;
            }
        }

        private void RunApp()
        {
            Destroy();
            menuItems[currentSelection].Run();
        }
    }
}
