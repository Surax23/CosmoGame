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
        public static event Action<Asteroid> AddAsteroids;

        public static HeroShip heroShip;
        private static Bullet bullet;

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
            Random c = new Random();
            _objs_ingame.Add(new Asteroid(new Point(Width, c.Next(0, Height)), new Point(4, 0), new Size(0, 0)));
            heroShip = new HeroShip(new Point(80, 300), new Point(0, 10), new Size(0, 0));
            _objs_ingame.Add(heroShip);
            _objs_back = list;

            heroShip.MessageDie += Finish;
            AddAsteroids += AddAsteroids_method;
        }

        /// <summary>
        /// Инициализация пространства для отрисовки.
        /// </summary>
        /// <param name="form"></param>
        public static void Init(Form form)
        {
            form.KeyDown += Form_KeyDown;

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
            _objs_back = new List<BaseObject>();
            _objs_ingame = new List<BaseObject>();
            _objs_bullets = new List<BaseObject>();

            Load(BackGround.CreateObjectsList());

            timer = new Timer() { Interval = 20 };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private static void AddAsteroids_method(Asteroid obj)
        {
            obj.SetPosX(-obj.Rect.Width - 100);
            System.Media.SystemSounds.Hand.Play();
            Random c = new Random();
            _objs_ingame.Add(new Asteroid(new Point(Width + c.Next(0, 10) * 10, c.Next(-150, Height)), new Point(4, 0), new Size(0, 0)));
        }

        /// <summary>
        /// Обработчик нажатия клавиш клавиатуры.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                bullet = new Bullet(new Point(heroShip.Rect.X + 60, heroShip.Rect.Y + 35), new Point(15, 0), new Size(8, 3));
                GameEngine._objs_bullets.Add(bullet);
            }
            if (e.KeyCode == Keys.Up) heroShip.Up();
            if (e.KeyCode == Keys.Down) heroShip.Down();
        }

        /// <summary>
        /// Обработчик события смерти игрока
        /// </summary>
        private static void Finish()
        {
            Game.ChangeGameStatus();
            timer.Stop();
            MessageBox.Show("Ты умер.");

            // Проверяем бинарный файл с выигравшими
            // Если есть свободное место или результат больше, чем когда-либо,
            // выводим форму с инпутом (через ShowDialog, не забыть у кнопок:
            // "You have to set the DialogResult in the Button handler."
            //Form2 testDialog = new Form2();

            //// Show testDialog as a modal dialog and determine if DialogResult = OK.
            //if (testDialog.ShowDialog(this) == DialogResult.OK)
            //{
            //    // Read the contents of testDialog's TextBox.
            //    this.txtResult.Text = testDialog.TextBox1.Text;
            //}
            //else
            //{
            //    this.txtResult.Text = "Cancelled";
            //}
            //testDialog.Dispose();

            // Потом добавлние результата в файл и запись.
            // А теперь старт заново.

            heroShip.Health = 100;
            heroShip.Score = 0;
            for (int i = 0; i < _objs_ingame.Count; i++)
            {
                _objs_ingame[i].Dispose();
            }
            _objs_ingame = null;
            _objs_ingame = new List<BaseObject>();
            Load(BackGround.CreateObjectsList());
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
            Random c = new Random();
            if (c.Next(0, 501) == 1)
                _objs_ingame.Add(new Aid(new Point(Width, c.Next(0, Height)), new Point(4, 0), new Size(0, 0)));

            foreach (BaseObject bs in _objs_back)
                bs.Update();

            BaseObject aid = heroShip;
            if (Game.gameStatus == GameStatus.game)
            {
                foreach (BaseObject bs in _objs_bullets)
                    bs.Update();
                foreach (BaseObject bs in _objs_ingame.ToArray())
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
                            if (_objs_bullets.Count > 0 && bs.GetType() != typeof(Aid))
                                if ((bs as Asteroid).Collision(_objs_bullets[i]))
                                {
                                    AddAsteroids?.Invoke((bs as Asteroid));
                                    _objs_bullets[i].Dispose();
                                    _objs_bullets.RemoveAt(i);
                                    heroShip.Score++;
                                }
                    }
                    if (bs.GetType() == typeof(Asteroid))
                    {
                        if (bs.Collision(heroShip))
                        {
                            heroShip.Damage(1);
                            System.Media.SystemSounds.Hand.Play();
                            if (heroShip.Health <= 0) heroShip?.Die();
                        }
                    }
                    if (bs.GetType() == typeof(Aid))
                    {
                        if (bs.Collision(heroShip))
                        {
                            heroShip.Aid(10);
                            System.Media.SystemSounds.Exclamation.Play();
                            aid = bs;
                        }
                    }
                }
            }
            if (aid != heroShip)
            {
                _objs_ingame.Remove(aid);
                aid.Dispose();
                aid = null;
            }
        }
    }
}