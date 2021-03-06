﻿using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game
{

    public class Observer
    {
        public void PrintMsg(object o)
        {
            Console.WriteLine($"Объект типа {o.GetType()} был создан по координатам: {(o as BaseObject).Rect.X}:{(o as BaseObject).Rect.Y}");
        }
    }

    /// <summary>
    /// Перечисление состояния игры.
    /// </summary>
    enum GameStatus
    {
        menu, game
    }

    /// <summary>
    /// Маленький хак, чтобы кнопка меню в игре не забирала фокус.
    /// </summary>
    public class btn : Button
    {
        public btn()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }

    static class Game
    {
        public static GameStatus gameStatus;

        public static event Action<GameStatus> GameStatus_change;
        
        static Button menu_btn, start_btn, record_btn, exit_btn, author;
        static Form form;

        static void Main(string[] args)
        {
            form = new Form();

            #region Создание кнопок меню
            menu_btn = new btn();
            menu_btn.Text = "Меню";
            menu_btn.Left = 10;
            menu_btn.Top = 10;
            menu_btn.Width = 80;
            menu_btn.Height = 30;
            form.Controls.Add(menu_btn);
            menu_btn.Click += Menu_btn_Click;
            menu_btn.Visible = false;

            start_btn = new Button();
            start_btn.Text = "Начать игру";
            start_btn.Left = 25;
            start_btn.Top = 25;
            start_btn.Width = 230;
            start_btn.Height = 50;
            form.Controls.Add(start_btn);
            start_btn.Click += Start_btn_Click;

            record_btn = new Button();
            record_btn.Text = "Рекорды";
            record_btn.Left = 50 + 230;
            record_btn.Top = 25;
            record_btn.Width = 230;
            record_btn.Height = 50;
            form.Controls.Add(record_btn);
            record_btn.Click += Record_btn_Click;

            exit_btn = new Button();
            exit_btn.Text = "Выход";
            exit_btn.Left = 75 + 230 * 2;
            exit_btn.Top = 25;
            exit_btn.Width = 230;
            exit_btn.Height = 50;
            form.Controls.Add(exit_btn);
            exit_btn.Click += Exit_btn_Click;

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

            GameStatus_change += ChangeGameStatus;
            ChangeGameStatus(GameStatus.menu);
            GameEngine.Init(form);
            Application.Run(form);
            
        }

        public static void ChangeGameStatus(GameStatus status)
        {
            gameStatus = status;
            if (status == GameStatus.menu)
            {
                start_btn.Visible = true;
                record_btn.Visible = true;
                exit_btn.Visible = true;
                author.Visible = true;
                menu_btn.Visible = false;
            }
            else
            {
                menu_btn.Visible = true;
                start_btn.Visible = false;
                record_btn.Visible = false;
                exit_btn.Visible = false;
                author.Visible = false;
            }
        }

        public static void ChangeGameStatus()
        {
            if (gameStatus == GameStatus.menu)
            {
                ChangeGameStatus(GameStatus.game);
            }
            else
                ChangeGameStatus(GameStatus.menu);
        }

        /// <summary>
        /// Нажатие кнопки "Меню" в режиме "игра".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Menu_btn_Click(object sender, EventArgs e)
        {
            ChangeGameStatus(GameStatus.menu);
        }

        /// <summary>
        /// Нажатие кнопки "Меню" на экране меню.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Start_btn_Click(object sender, EventArgs e)
        {
            ChangeGameStatus(GameStatus.game);
            form.Focus();
        }

        /// <summary>
        /// Нажатие кнопки "Рекорды" на экране меню.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Record_btn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Тут будут рекорды.");
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Нажатие кнопки "Выход" на экране меню.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Exit_btn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Точно хотите выйти?", "Выход", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}