using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    /// <summary>
    /// Основной статический класс отрисовки объектов.
    /// </summary>
    static class GameEngine
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        public static int Width { get; set; }
        public static int Height { get; set; }

        /// <summary>
        /// Список объектов для отрисовки.
        /// </summary>
        public static List<BaseObject> _objs_back, _objs_ingame, _objs_bullets;

        public static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        /// <summary>
        /// Загрузка объектов в список.
        /// </summary>
        public static void Load(List<BaseObject> list)
        {
            if (_objs_back.Count != 0)
            {
                for (int i = 0; i < _objs_back.Count; i++)
                {
                    _objs_back[i].Dispose();
                    _objs_back.RemoveAt(i);
                }
            }
            _objs_back = list;
        }

        /// <summary>
        /// Инициализация пространства для отрисовки.
        /// </summary>
        /// <param name="form"></param>
        public static void Init(Form form)
        {
            Graphics g;
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            if (form.Width > 1600 || form.Width < 0)
                throw new ArgumentOutOfRangeException();
            Width = form.Width;
            if (form.Height > 1600 || form.Height < 0)
                throw new ArgumentOutOfRangeException();
            Height = form.Height;
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            //Load();
            _objs_back = new List<BaseObject>();
            _objs_ingame = new List<BaseObject>();
            _objs_bullets = new List<BaseObject>();
            Timer timer = new Timer() { Interval = 20 };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        /// <summary>
        /// Метод отрисовки.
        /// </summary>
        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject bs in _objs_back)
                if (!(bs is Ship && Game.gameStatus == GameStatus.game))
                    bs.Draw();
            if (Game.gameStatus == GameStatus.game)
            {
                foreach (BaseObject bs in _objs_ingame)
                    bs.Draw();
                foreach (BaseObject bs in _objs_bullets)
                    bs.Draw();
            }
            Buffer.Render();
        }

        /// <summary>
        /// Обновление информации об объектах в списке.
        /// </summary>
        public static void Update()
        {
            foreach (BaseObject bs in _objs_back)
                bs.Update();

            if (Game.gameStatus == GameStatus.game)
            {
                foreach (BaseObject bs in _objs_bullets)
                    bs.Update();
                foreach (Asteroid bs in _objs_ingame)
                {
                    bs.Update();
                    foreach (Bullet bo in _objs_bullets)
                        if (bs.Collision(bo))
                        {
                            bs.SetPosX(-bs.Rect.Width - 100);
                            bo.SetPosX(GameEngine.Width + 1);
                            System.Media.SystemSounds.Hand.Play();
                        }
                }
            }
        }
    }
}