﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public delegate void Message();
    public delegate void Alert(object o);

    interface ICollision
    {
        bool Collision(ICollision obj);
        Rectangle Rect { get; }
    }

    /// <summary>
    /// Базовый класс отрисовываемых объектов.
    /// </summary>
    abstract class BaseObject : IDisposable, ICollision
    {
        /// <summary>
        /// Позиция объекта.
        /// </summary>
        protected Point Pos;
        /// <summary>
        /// Направление движения объекта.
        /// </summary>
        protected Point Dir;
        /// <summary>
        /// Размер объекта.
        /// </summary>
        protected Size Size;
        /// <summary>
        /// Цвет объекта
        /// </summary>
        protected Pen Pen;
        /// <summary>
        /// Рандомизатор.
        /// </summary>
        protected Random rand;

        public event Alert BaseObjectActions;

        /// <summary>
        /// Создает экземпляр базового класса всех объектов.
        /// </summary>
        /// <param name="pos">Позиция объекта.</param>
        /// <param name="dir">Направление движения объекта.</param>
        /// <param name="size">Размер объекта.</param>
        public BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            if ((Pos.X < -10000 || Pos.X > 26000) || (Pos.Y < -10000 || Pos.Y > 26000))
                throw new GameObjectException();
            Dir = dir;
            if ((Dir.X < -1000 || Dir.X > 2600) || (Dir.Y < -1000 || Dir.Y > 2600))
                throw new GameObjectException();
            Size = size;
            rand = new Random(DateTime.Now.Millisecond);
            Observer o = new Observer();
            BaseObjectActions += o.PrintMsg;
            BaseObjectActions?.Invoke(this);
        }
        /// <summary>
        /// Возвращает прямоугольную область для проверки столкновений.
        /// </summary>
        public Rectangle Rect => new Rectangle(Pos, Size);

        /// <summary>
        /// Проверка столкновений.
        /// </summary>
        /// <param name="obj">Объект, наследуемый от ICollision</param>
        /// <returns>Возвращает истину, если два прямоугольника пересеклись, ложь в противном случае.</returns>
        public bool Collision(ICollision obj)
        {
            return obj.Rect.IntersectsWith(this.Rect);
        }


        /// <summary>
        /// Удаляет картинку в случае некоторых наследников.
        /// </summary>
        public abstract void Dispose();

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
            Bitmap tmp = new Bitmap("img/star.png");
            img = new Bitmap(tmp, size);
            tmp.Dispose();
        }

        public override void Draw()
        {
            GameEngine.Buffer.Graphics.DrawImage(img, Pos);
        }

        public override void Update()
        {
            Pos.X -= Dir.X;
            if (Pos.X < -img.Width) Pos.X = GameEngine.Width + Size.Width;

        }

        public override void Dispose()
        {
            img.Dispose();
        }
    }

    /// <summary>
    /// Объект "Пыль", наследник BaseObject.
    /// </summary>
    class Dust : BaseObject
    {
        public Dust(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Pen = new Pen(Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)), 0.2f);
        }

        public override void Dispose()
        {

        }

        ~Dust()
        {

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
                Dir.X = rand.Next(25, 100);
                Pos.X = GameEngine.Width + Size.Width;
                Pos.Y = rand.Next(0, GameEngine.Height);
                Pen.Color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
                Pen.Width = 0.2f;
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
            img = NextGalaxy();
            Pos = new Point(GameEngine.Width + img.Width * rand.Next(1, 4), rand.Next(0 - img.Height / 2, GameEngine.Height + img.Height / 2));
        }

        public Bitmap NextGalaxy()
        {
            switch (rand.Next(0, 4))
            {
                case 0: return new Bitmap("img/galaxy1.png");
                case 1: return new Bitmap("img/galaxy2.png");
                case 2: return new Bitmap("img/galaxy3.png");
                default: return new Bitmap("img/galaxy4.png");
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
                Pos.X = GameEngine.Width + img.Width * rand.Next(1, 15);
                Pos.Y = rand.Next(0 - img.Height / 2, GameEngine.Height + img.Height / 2);
            }
        }

        public override void Dispose()
        {
            img.Dispose();
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
            img = new Bitmap("img/ship.png");
        }

        ~Ship()
        {
            img.Dispose();
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

        public override void Dispose()
        {
            img.Dispose();
        }
    }

    /// <summary>
    /// Объект "Астероид", наследник BaseObject.
    /// </summary>
    class Asteroid : BaseObject
    {
        Bitmap img;
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            img = new Bitmap("img/asteroid.png");
            Size.Width = img.Width + 15;
            Size.Height = img.Height + 15;
        }

        public override void Dispose()
        {
            img.Dispose();
        }

        public override void Draw()
        {
            GameEngine.Buffer.Graphics.DrawImage(img, Pos);
            //GameEngine.Buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(Pos, Size));
        }

        public override void Update()
        {
            base.Pos.X -= Dir.X;
            base.Pos.Y -= Dir.Y;
            if (base.Pos.X < -Size.Width - 100 || base.Pos.Y + Size.Height < 0 || base.Pos.Y > GameEngine.Height)
            {
                base.Pos.X = GameEngine.Width;
                base.Pos.Y = rand.Next(GameEngine.Height / 4, GameEngine.Height - GameEngine.Height / 4);
            }
        }

        public void SetPosX(int x)
        {
            Pos.X = x;
        }
    }

    /// <summary>
    /// Объект "Пуля", наследник BaseObject.
    /// </summary>
    class Bullet : BaseObject
    {
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Pen = new Pen(Color.OrangeRed, 3f);
        }

        public override void Dispose()
        {

        }

        ~Bullet()
        {

        }

        public override void Draw()
        {
            GameEngine.Buffer.Graphics.DrawLine(Pen, Pos.X, Pos.Y, Pos.X + Size.Width, Pos.Y);
        }

        public override void Update()
        {
            Pos.X += Dir.X;
        }

        public void SetPosX(int x)
        {
            Pos.X = x;
        }
    }

    /// <summary>
    /// Корабль, управляемый игроком, наследник BaseObject.
    /// </summary>
    class HeroShip : BaseObject
    {
        public event Message MessageDie;
        Bitmap img;
        public int Health { get; set; }
        public int Score { get; set; }

        public HeroShip(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            img = new Bitmap("img/heroship.png");
            Size.Width = img.Width + 15;
            Size.Height = img.Height + 15;
            Health = 100;
            Score = 0;
        }

        public void Damage(int damage)
        {
            Health -= damage;
        }

        public void Aid(int aid)
        {
            Health += aid;
        }

        public override void Dispose()
        {
            img.Dispose();
        }

        public override void Draw()
        {
            GameEngine.Buffer.Graphics.DrawImage(img, Pos);
            //GameEngine.Buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(Pos, Size));
            GameEngine.Buffer.Graphics.DrawString($"Здоровье: {Health}, Счет: {Score}", SystemFonts.DefaultFont, Brushes.LawnGreen, new PointF(GameEngine.Width/2, 10));
        }

        public void Up()
        {
            if (Pos.Y > 0) Pos.Y -= Dir.Y;
        }

        public void Down()
        {
            if (Pos.Y < GameEngine.Height) Pos.Y += Dir.Y;
        }

        public override void Update()
        {
            //throw new NotImplementedException();
        }
        public void Die()
        {
            MessageDie?.Invoke();
        }
    }

    /// <summary>
    /// Аптечка
    /// </summary>
    class Aid : BaseObject
    {
        Bitmap img;
        public Aid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            img = new Bitmap("img/plus.png");
            Size.Width = img.Width;
            Size.Height = img.Height;
        }

        public override void Dispose()
        {
            img.Dispose();
        }

        public override void Draw()
        {
            GameEngine.Buffer.Graphics.DrawImage(img, Pos);
            //GameEngine.Buffer.Graphics.DrawRectangle(Pens.White, new Rectangle(Pos, Size));
        }

        public override void Update()
        {
            base.Pos.X -= Dir.X;
            base.Pos.Y -= Dir.Y;
            if (base.Pos.X < -Size.Width - 100 || base.Pos.Y + Size.Height < 0 || base.Pos.Y > GameEngine.Height)
            {
                base.Pos.X = GameEngine.Width;
                base.Pos.Y = rand.Next(GameEngine.Height / 4, GameEngine.Height - GameEngine.Height / 4);
            }
        }

        public void SetPosX(int x)
        {
            Pos.X = x;
        }
    }
}