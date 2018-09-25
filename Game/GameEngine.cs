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
        static Timer timer;

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
            Game.heroShip.MessageDie += Finish;
            timer = new Timer() { Interval = 20 };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private static void Finish()
        {
            timer.Stop();
            MessageBox.Show("Ты умер.");
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
                foreach (BaseObject bs in _objs_ingame)
                {
                    bs.Update();
                    for (int i = 0; i < _objs_bullets.Count; i++)
                    {
                        if (_objs_bullets[i].Rect.X > GameEngine.Width)
                        {
                            _objs_bullets[i].Dispose();
                            _objs_bullets.RemoveAt(i);
                        }
                        if (bs.GetType() != typeof(HeroShip))
                            if ((bs as Asteroid).Collision(_objs_bullets[i]))
                            {
                                (bs as Asteroid).SetPosX(-bs.Rect.Width - 100);
                                _objs_bullets[i].Dispose();
                                _objs_bullets.RemoveAt(i);
                                System.Media.SystemSounds.Hand.Play();
                                Game.heroShip.Score++;
                            }
                    }
                    if (bs.GetType() == typeof(Asteroid))
                    {
                        if (bs.Collision(Game.heroShip))
                        {
                            Game.heroShip.Damage(1);
                            System.Media.SystemSounds.Hand.Play();
                            if (Game.heroShip.Health <= 0) Game.heroShip?.Die();
                        }
                    }
                }
            }
        }
    }

    
}