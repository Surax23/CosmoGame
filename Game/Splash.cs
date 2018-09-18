using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    static class Splash
    {
        static Form Form_splash;
        static Button start_btn, record_btn, exit_btn, author;
        public static void Init(Form splash)
        {
            Form_splash = splash;
            start_btn = new Button();
            start_btn.Text = "Начать игру";
            start_btn.Left = 25;
            start_btn.Top = 25;
            start_btn.Width = 230;
            start_btn.Height = 50;            
            splash.Controls.Add(start_btn);
            start_btn.Click += start_btn_Click;

            record_btn = new Button();
            record_btn.Text = "Рекорды";
            record_btn.Left = 50 + 230;
            record_btn.Top = 25;
            record_btn.Width = 230;
            record_btn.Height = 50;
            splash.Controls.Add(record_btn);
            record_btn.Click += record_btn_Click;

            exit_btn = new Button();
            exit_btn.Text = "Выход";
            exit_btn.Left = 75 + 230*2;
            exit_btn.Top = 25;
            exit_btn.Width = 230;
            exit_btn.Height = 50;
            splash.Controls.Add(exit_btn);
            exit_btn.Click += exit_btn_Click;

            author = new Button();
            author.FlatStyle = FlatStyle.Flat;
            author.Text = "Автор: Сурков Александр";
            author.Enabled = false;
            author.Left = 25;
            author.Top = 500;
            author.Width = 230;
            author.Height = 50;
            splash.Controls.Add(author);

            splash.KeyDown += form_KeyDown;
        }

        private static void form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Init(Form_splash);
            }
        }

        private static void start_btn_Click(object sender, EventArgs e)
        {
            start_btn.Dispose();
            record_btn.Dispose();
            exit_btn.Dispose();
            author.Dispose();
        }

        private static void record_btn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void exit_btn_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Точно хотите выйти?", "Выход", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
