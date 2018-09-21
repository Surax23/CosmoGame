using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game
{
    /// <summary>
    /// Перечисление состояния игры.
    /// </summary>
    enum GameStatus
    {
        menu, game
    }

    static class Game
    {
        public static GameStatus gameStatus;
        static Button menu_btn, start_btn, record_btn, exit_btn, author;

        static void Main(string[] args)
        {
            gameStatus = GameStatus.menu;
            Form form = new Form();

            #region Создание кнопок меню
            menu_btn = new Button();
            menu_btn.Text = "Меню";
            menu_btn.Left = 10;
            menu_btn.Top = 10;
            menu_btn.Width = 80;
            menu_btn.Height = 30;
            form.Controls.Add(menu_btn);
            menu_btn.Click += menu_btn_Click;
            menu_btn.Visible = false;

            start_btn = new Button();
            start_btn.Text = "Начать игру";
            start_btn.Left = 25;
            start_btn.Top = 25;
            start_btn.Width = 230;
            start_btn.Height = 50;
            form.Controls.Add(start_btn);
            start_btn.Click += start_btn_Click;

            record_btn = new Button();
            record_btn.Text = "Рекорды";
            record_btn.Left = 50 + 230;
            record_btn.Top = 25;
            record_btn.Width = 230;
            record_btn.Height = 50;
            form.Controls.Add(record_btn);
            record_btn.Click += record_btn_Click;

            exit_btn = new Button();
            exit_btn.Text = "Выход";
            exit_btn.Left = 75 + 230 * 2;
            exit_btn.Top = 25;
            exit_btn.Width = 230;
            exit_btn.Height = 50;
            form.Controls.Add(exit_btn);
            exit_btn.Click += exit_btn_Click;

            author = new Button();
            author.FlatStyle = FlatStyle.Flat;
            author.Text = "Автор: Сурков Александр";
            author.Enabled = false;
            author.Left = 25;
            author.Top = 500;
            author.Width = 230;
            author.Height = 50;
            form.Controls.Add(author);
            #endregion

            form.Width = 800;
            form.Height = 600;
            GameEngine.Init(form);

            GameEngine.Load(BackGround.CreateObjectsList());

            Random c = new Random();
            for (int i = 0; i < 7; i++)
            {
                GameEngine._objs_ingame.Add(new Asteroid(new System.Drawing.Point(GameEngine.Width, 100), new System.Drawing.Point(4, 0), new System.Drawing.Size(0,0)));
            }

            for (int i = 0; i < 1; i++)
            {
                GameEngine._objs_bullets.Add(new Bullet(new Point(0 + i*100, 100), new Point(40, 0), new Size(20, 5)));
            }


            form.Show();
            //GameEngine.Draw();
            Application.Run(form);
        }

        /// <summary>
        /// Нажатие кнопки "Меню" в режиме "игра".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void menu_btn_Click(object sender, EventArgs e)
        {
            start_btn.Visible = true;
            record_btn.Visible = true;
            exit_btn.Visible = true;
            author.Visible = true;
            menu_btn.Visible = false;
            gameStatus = GameStatus.menu;
        }

        /// <summary>
        /// Нажатие кнопки "Меню" на экране меню.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void start_btn_Click(object sender, EventArgs e)
        {
            menu_btn.Visible = true;
            start_btn.Visible = false;
            record_btn.Visible = false;
            exit_btn.Visible = false;
            author.Visible = false;
            gameStatus = GameStatus.game;
        }

        /// <summary>
        /// Нажатие кнопки "Рекорды" на экране меню.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void record_btn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Тут будут рекорды.");
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Нажатие кнопки "Выход" на экране меню.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void exit_btn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Точно хотите выйти?", "Выход", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}