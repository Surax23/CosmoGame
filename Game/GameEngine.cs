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

    /// <summary>
    /// Базовый класс отрисовываемых объектов.
    /// </summary>
    class BaseObject
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;
        protected Pen Pen;

        /// <summary>
        /// Создает экземпляр базового класса всех объектов.
        /// </summary>
        /// <param name="pos">Позиция объекта.</param>
        /// <param name="dir">Направление движения объекта.</param>
        /// <param name="size">Размер объекта.</param>
        public BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
        }

        /// <summary>
        /// Метод отрисовки объекта.
        /// </summary>
        public virtual void Draw()
        {
            GameEngine.Buffer.Graphics.DrawEllipse(Pens.Aqua, new Rectangle(Pos, Size));
        }

        /// <summary>
        /// Метод обновления позиции объекта.
        /// </summary>
        public virtual void Update()
        {
            Pos.X += Dir.X;
            Pos.Y += Dir.Y;
            if (Pos.X < 0 || Pos.X > GameEngine.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0 || Pos.Y > GameEngine.Height) Dir.Y = -Dir.Y;
        }
    }

    /// <summary>
    /// Объект "Звезда", наследник BaseObject.
    /// </summary>
    class Star : BaseObject
    {
        Bitmap img;
        public Star(Point pos, Point dir, Size size):base(pos, dir, size)
        {
            Pen = new Pen(Color.FromArgb(50,50,50));
            Bitmap tmp = new Bitmap("star.png");
            img = new Bitmap(tmp, size);
        }

        public override void Draw()
        {
            //GameEngine.Buffer.Graphics.DrawLine(Pen, new Point(Pos.X, Pos.Y), new Point(Pos.X + Size.Width, Pos.Y + Size.Height));
            //GameEngine.Buffer.Graphics.DrawLine(Pen, new Point(Pos.X, Pos.Y + Size.Height), new Point(Pos.X + Size.Width, Pos.Y));
            GameEngine.Buffer.Graphics.DrawImage(img, Pos);
        }

        public override void Update()
        {
            Pos.X -= Dir.X;
            if (Pos.X < -img.Width) Pos.X = GameEngine.Width + Size.Width;
            
        }
    }

    /// <summary>
    /// Объект "Пыль", наследник BaseObject.
    /// </summary>
    class Dust : BaseObject
    {
        public Dust(Point pos, Point dir, Size size):base(pos, dir, size)
        {
            Random c = new Random();
            Pen = new Pen(Color.FromArgb(c.Next(0, 255), c.Next(0, 255), c.Next(0, 255)), 0.2f);
        }

        public override void Draw()
        {
            GameEngine.Buffer.Graphics.DrawLine(Pen, Pos.X, Pos.Y, Pos.X + Size.Width, Pos.Y);
        }

        public override void Update()
        {
            
            Pos.X -= Dir.X;
            if (Pos.X < 0)
            {
                Random c = new Random(DateTime.Now.Millisecond);
                Dir.X = c.Next(50, 150);
                Pos.X = GameEngine.Width + Size.Width;
                Pos.Y = c.Next(0, GameEngine.Width);
                Pen = new Pen(Color.FromArgb(c.Next(0, 256), c.Next(0, 256), c.Next(0, 256)), 0.2f);
            }
        }
    }

    /// <summary>
    /// Объект "Галактика", наследник BaseObject.
    /// </summary>
    class Galaxy : BaseObject
    {
        Bitmap img;

        public Galaxy(Point pos, Point dir, Size size):base(pos, dir, size)
        {
            Random c = new Random();
            img = NextGalaxy();
            Pos = new Point(GameEngine.Width + img.Width*c.Next(1, 4), c.Next(0 - img.Height / 2, GameEngine.Height + img.Height / 2));
        }

        public Bitmap NextGalaxy()
        {
            Random c = new Random();
            switch (c.Next(0,4))
            {
                case 0: return new Bitmap("galaxy1.png");
                case 1: return new Bitmap("galaxy2.png");
                case 2: return new Bitmap("galaxy3.png");
                default: return new Bitmap("galaxy4.png");
            }
        }

        public override void Draw()
        {
            GameEngine.Buffer.Graphics.DrawImage(img, Pos);
        }

        public override void Update()
        {

            Pos.X -= Dir.X;
            if (Pos.X < -img.Width-200)
            {
                Random c = new Random(DateTime.Now.Millisecond);
                Pos = new Point(GameEngine.Width + img.Width * c.Next(1, 15), c.Next(0 - img.Height / 2, GameEngine.Height + img.Height / 2));
            }
        }
    }

    /// <summary>
    /// Объект "Корабль", наследник BaseObject.
    /// </summary>
    class Ship : BaseObject
    {
        Bitmap img;

        public Ship(Point pos, Point dir, Size size):base(pos, dir, size)
        {
            img = new Bitmap("ship.png");
        }

        public override void Draw()
        {
            GameEngine.Buffer.Graphics.DrawImage(img, Pos);
        }

        public override void Update()
        {

            Pos.X += Dir.X;
            if (Pos.X > GameEngine.Width + 25)
                Pos.X = -img.Width - 500;
        }
    }
}