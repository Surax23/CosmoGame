using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    /// <summary>
    /// Базовый класс отрисовываемых объектов.
    /// </summary>
    abstract class BaseObject
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
        public abstract void Draw();

        /// <summary>
        /// Метод обновления позиции объекта.
        /// </summary>
        public abstract void Update();
    }

    /// <summary>
    /// Объект "Звезда", наследник BaseObject.
    /// </summary>
    class Star : BaseObject
    {
        Bitmap img;
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Pen = new Pen(Color.FromArgb(50, 50, 50));
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
        public Dust(Point pos, Point dir, Size size) : base(pos, dir, size)
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

        public Galaxy(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Random c = new Random();
            img = NextGalaxy();
            Pos = new Point(GameEngine.Width + img.Width * c.Next(1, 4), c.Next(0 - img.Height / 2, GameEngine.Height + img.Height / 2));
        }

        public Bitmap NextGalaxy()
        {
            Random c = new Random();
            switch (c.Next(0, 4))
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
            if (Pos.X < -img.Width - 200)
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

        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
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
