using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    /// <summary>
    /// Статический класс загрузки BackGround'a, здесь создаем все изображения и запихиваем в list, который и возвращаем.
    /// </summary>
    static class BackGround
    {
        public static List<BaseObject> CreateObjectsList()
        {
            List<BaseObject> list = new List<BaseObject>();
            Random c = new Random();
            for (int i = 0; i < 7; i++)
                list.Add(new Galaxy(new Point(c.Next(0, GameEngine.Width), c.Next(0, GameEngine.Width)), new Point(1, 1), new Size(0, 0)));
            for (int i = 0; i < 100; i++)
            {
                int size = c.Next(2, 7);
                if (i % 5 == 0)
                    list.Add(new Dust(new Point(c.Next(0, GameEngine.Width), c.Next(0, GameEngine.Height)), new Point(c.Next(50, 150), c.Next(50, 150)), new Size(size, size)));
                list.Add(new Star(new Point(c.Next(0, GameEngine.Width), c.Next(0, GameEngine.Width)), new Point(c.Next(2, 4), c.Next(2, 4)), new Size(size * 3, size * 3)));
            }
            list.Add(new Ship(new Point(-2200, 20), new Point(1, 1), new Size(0, 0)));
            
            return list;
        }
    }
}