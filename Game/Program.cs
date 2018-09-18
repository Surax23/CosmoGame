using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    enum GameStatus
    {
        menu, game, pause
    }

    static class Program
    {
        static GameStatus gameStatus;

        static void Main(string[] args)
        {
            gameStatus = GameStatus.menu;
            Form form = new Form();
            form.Width = 1600;
            form.Height = 1000;
            Splash.Init(form);
            GameEngine.Init(form);
            form.Show();
            //GameEngine.Draw();
            Application.Run(form);
        }
    }
}
