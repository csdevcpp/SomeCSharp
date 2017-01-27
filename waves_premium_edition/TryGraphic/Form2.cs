using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TryGraphic
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Size resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            this.Height = resolution.Height;
            this.Width = resolution.Width;

            button1.Location = new Point(this.Width/2 - button1.Width/2, this.Height/2);
            button2.Location = new Point(this.Width / 2 - button1.Width / 2, this.Height / 2 + this.Height / 4);
            button3.Location = new Point(this.Width / 2 - button1.Width / 2, this.Height / 2 + this.Height / 10);

            pictureBox1.Location = new Point(10, 10);
            label4.Location = new Point(40, pictureBox1.Height+10);

            pictureBox2.Location = new Point(this.Width - pictureBox2.Width - 10, 10);
            label5.Location = new Point(this.Width-pictureBox2.Width + 10, pictureBox2.Height + 10);

            label2.Location = new Point(this.Width/3 , this.Height/3 + 50);

            label3.Location = new Point(this.Width / 3+20, this.Height / 3 + 5);
            label1.Location = new Point(this.Width / 3, this.Height / 12 + 5);

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
           // form1.MyEvent += this.Handler;
            //form1.WindowState = FormWindowState.Normal;
            form1.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
          //  form1.Bounds = Screen.PrimaryScreen.Bounds;
            form1.ShowDialog();
           
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

           // this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
          //  this.Bounds = Screen.PrimaryScreen.Bounds;
            Size resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            this.Height = resolution.Height;
            this.Width = resolution.Width;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
