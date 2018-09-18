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
        public static List<BaseObject> _objs;

        public static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        /// <summary>
        /// Загрузка объектов в список.
        /// </summary>
        public static void Load()
        {
            _objs = new List<BaseObject>();
            Random c = new Random();
            _objs.Add(new Galaxy(new Point(c.Next(0, GameEngine.Width), c.Next(0, GameEngine.Width)), new Point(1, 1), new Size(0, 0)));
            _objs.Add(new Galaxy(new Point(c.Next(0, GameEngine.Width), c.Next(0, GameEngine.Width)), new Point(1, 1), new Size(0, 0)));
            _objs.Add(new Galaxy(new Point(c.Next(0, GameEngine.Width), c.Next(0, GameEngine.Width)), new Point(1, 1), new Size(0, 0)));
            _objs.Add(new Galaxy(new Point(c.Next(0, GameEngine.Width), c.Next(0, GameEngine.Width)), new Point(1, 1), new Size(0, 0)));
            _objs.Add(new Galaxy(new Point(c.Next(0, GameEngine.Width), c.Next(0, GameEngine.Width)), new Point(1, 1), new Size(0, 0)));
            _objs.Add(new Galaxy(new Point(c.Next(0, GameEngine.Width), c.Next(0, GameEngine.Width)), new Point(1, 1), new Size(0, 0)));
            for (int i = 0; i < 100; i++)
            {
                int size = c.Next(2, 7);
                _objs.Add(new Dust(new Point(c.Next(0, GameEngine.Width), c.Next(0, GameEngine.Width)), new Point(c.Next(50, 150), c.Next(50, 150)), new Size(size, size)));
                _objs.Add(new Star(new Point(c.Next(0, GameEngine.Width), c.Next(0, GameEngine.Width)), new Point(c.Next(2, 4), c.Next(2, 4)), new Size(size*3, size*3)));
            }
            _objs.Add(new Ship(new Point(-2200, 20), new Point(1, 1), new Size(0, 0)));
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
            Width = form.Width;
            Height = form.Height;
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Load();
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
            foreach (BaseObject bs in _objs)
                bs.Draw();
            Buffer.Render();
        }

        /// <summary>
        /// Обновление информации об объектах в списке.
        /// </summary>
        public static void Update()
        {
            foreach (BaseObject bs in _objs)
                bs.Update();
        }

    }

    
}