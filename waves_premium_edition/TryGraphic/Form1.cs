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
    public partial class Form1 : Form
    {

        Bitmap myBitmap;
        Graphics gr;
        SolidBrush brush_lastik;
        // елементы управления (для кнопок)
        bool leftFix = false, rightFix = false, mouse_catch = false;
        int mouse_X = 0, mouse_prev_X, touch_circle_X=44, touch_circle_Y=54, touch_circle_R=12;
        int graf_loc_y1, graf_loc_y2, graf_loc_y3, graf_loc_y; // 1/20 часть высоты поля для отрисовки 

         int ro = 4;
         double mult=200; // параметры отрисовки графиков и пружин
            int imax = 0;
          

        public Form1()
        {
           

            InitializeComponent();


            Size resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            this.Height = resolution.Height;
            this.Width = resolution.Width;

            
           
           
            pictureBox1.Location = new Point(0, 0);
           
            pictureBox1.Width = this.Width;
            pictureBox1.Height = this.Height*6 / 8 + 10;


            label2.Location = new Point(10, 10);
            label12.Location = new Point(this.Width / 3, this.Height / 5+ 10);
            label13.Location = new Point(2 * this.Width / 3, this.Height / 5 + 10);

           

            this.DoubleBuffered = true;
            myBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gr = Graphics.FromImage(myBitmap);

          
            pictureBox1.Image = myBitmap;

           // gr.DrawEllipse(pen_for_lines, pictureBox1.Width / 2, pictureBox1.Height / 2, 40, 40);
            //for (int q = 1; q < N; q++)
            //{
            //    gr.DrawLine(pen_for_lines, 50 + q * 5 - (int)xi[q - 1], 20, 50 + q * 5 - (int)xi[q - 1], 100);

            //    gr.DrawEllipse(pen_for_lines, 50 + q * 5, 250 - (int)xi[q - 1], 3, 3);
            //}
            button_Reset.Size = new System.Drawing.Size((int)button_Reset.Width , (int)button_Reset.Height * this.Height / 900);
            button_Exit.Size = new System.Drawing.Size((int)button_Reset.Width, (int)button_Reset.Height * this.Height / 900);
            Button_Start.Size = new System.Drawing.Size((int)button_Reset.Width , (int)button_Reset.Height * this.Height / 900);

            /*
             button_Reset.Size = new System.Drawing.Size((int)button_Reset.Width * this.Width / 1440, (int)button_Reset.Height * this.Height / 900);
            button_Exit.Size = new System.Drawing.Size((int)button_Reset.Width * this.Width / 1440, (int)button_Reset.Height * this.Height / 900);
            Button_Start.Size = new System.Drawing.Size((int)button_Reset.Width * this.Width / 1440, (int)button_Reset.Height * this.Height / 900);
            
             */
            button_Exit.Location = new Point(5, this.Height - button_Exit.Height - 5);
            button_Exit.Font = new Font(button_Exit.Font.Name, button_Exit.Font.Size * ((float)this.Width / 1024), button_Exit.Font.Style, button_Exit.Font.Unit);
            
            button_Reset.Location = new Point(5, this.Height - button_Exit.Height- button_Reset.Height - 10);
            button_Reset.Font = new Font(button_Reset.Font.Name, button_Reset.Font.Size * ((float)this.Width / 1024), button_Reset.Font.Style, button_Reset.Font.Unit);
                       
            Button_Start.Location = new Point(5, this.Height - button_Exit.Height - button_Reset.Height - Button_Start.Height - 15);
            //if(this.Width<1365)
               Button_Start.Font = new Font(Button_Start.Font.Name, Button_Start.Font.Size*((float)this.Width/1024), Button_Start.Font.Style, Button_Start.Font.Unit);


           button_Hit.Font = new Font(button_Hit.Font.Name, button_Hit.Font.Size * ((float)this.Width / 1024), button_Hit.Font.Style, button_Hit.Font.Unit);
           button_SepSin.Font = new Font(button_SepSin.Font.Name, button_SepSin.Font.Size * ((float)this.Width / 1024), button_SepSin.Font.Style, button_SepSin.Font.Unit);
           button_Swell.Font = new Font(button_Swell.Font.Name, button_Swell.Font.Size * ((float)this.Width / 1024), button_Swell.Font.Style, button_Swell.Font.Unit);
          
            //panel2.Size = new Size(this.Width / 4, this.Height / 4);
            panel2.Scale(new SizeF((float)this.Width / 1440, (float)this.Height / 900));

            panel2.Location = new Point(Button_Start.Width + 5, 18*this.Height/24);
            
            panel1.Scale(new SizeF((float)this.Width / 1440, (float)this.Height / 900));

            panel1.Location = new Point(panel2.Location.X + panel2.Width + 5, panel2.Location.Y);

            graf_loc_y =  pictureBox1.Height / 24;
            graf_loc_y1 =  12* pictureBox1.Height / 24;
            graf_loc_y2 =  16* pictureBox1.Height / 24;
            graf_loc_y3 =  20* pictureBox1.Height / 24;

            label11.Location = new Point(10, 8*graf_loc_y);
           
            label14.Location = new Point(10, 14 * graf_loc_y);
            label15.Location = new Point(10, 18 * graf_loc_y);
            label6.Location = new Point(this.Width - label6.Width - 10, 8 * graf_loc_y);

            for (imax = 0; imax < N - 30; imax++)
                if (50 + imax * ro - 50 > pictureBox1.Width) break;

            mult *= 1.0*this.Height / 900;
        }

        
        const int N = 480;

        

        Pen pen_for_lines = new Pen(Color.DarkGreen, (float)2);//Pen(Color.DarkBlue, 1);
       
        Pen pen_for_lines_1 = new Pen(Color.Red, (float)2);
        Pen pen_for_start = new Pen(Color.MediumBlue, (float)1);//Pen(Color.DarkBlue, 1);


        int i = 0;
        double dt = 0.1, vv = 68, t = 1000, RE = 1, h = 1, vv2 = 0, vv1 = 0, t1 = 1000, t2 = 1000, t3 = -1, t4 = 1000, Swell_Power = 5;
        int isin = 100;
        int[] vshift = new int[N]; 
        
        //double[] eta = new double[N];
        double[] xi = new double[N];
        double[] xi1 = new double[N];
        double[] xi2 = new double[N];

        //double[] cat = new double[N];

        bool flag_checkBox = false;
        bool funclaunch = false;
        bool was_drawn1 = false; // была ли нарисована красная линия
               

       

        private void timer1_Tick(object sender, EventArgs e)
        {

          
           // timer1.Interval = 40 - trackBar_timespeed.Value;
           
            // Метод Эйлера-Кромера
            t += dt;
            t1 += dt;
            t2 += dt;
           if(t3!=-1) t3 += dt;
            t4 += dt;
            // isin++;

            //vv = vv - vv1; // скорость распостронения первой половины

            funclaunch = false;

            if (isin < 3)
            {
                funclaunch = true;
                if (t4 < 2 * Math.PI) xi1[0] =  Math.Sin(t4)/10;
                else
                {
                    isin++;
                    t4 = 0;
                }
            }
            // else
            //   xi1[0] = 0;


            if (t3 != -1)
            {
                funclaunch = true;
                xi1[0] =  Math.Sin(4*t3/Math.PI)/10;

            }

            if (t2 < 15)
            {
                funclaunch = true;
                xi1[0] =  Math.Sqrt(1 / (2 * Math.PI)) * Math.Exp(-(t2 * t2 / 2));

            }

            if (t < 2 * Math.PI)
            {
                funclaunch = true;
                xi1[0] =  Math.Sin(t);//Math.Sqrt(t); // t < 6.28 для гармонических колебаний
            }

            if (t1 < 4 && !mouse_catch)
            {
                xi1[0] = mouse_X * t1 / 4;
                funclaunch = true;
            }
            else
            {
                // if (!leftFix)
                //  xi2[0] = 2 * xi1[0] - xi[0] + (vv + vv1) * (-2 * xi1[0] + xi1[1]) / h / h * dt * dt; // делаем свободным левый конец
                // else 
                //  xi1[0] = 0;
                if (t1 > 4) mouse_X = 0;
            }



            if (!leftFix || funclaunch)
                xi2[0] = 2 * xi1[0] - xi[0] + (vv + vv1) * (-2 * xi1[0] + xi1[1]) / h / h * dt * dt; // делаем свободным левый конец
            if (leftFix && !funclaunch)
                xi1[0] = 0;

            for (i = 1; i < imax / 2 - 2; i++) // просчет для первой
            {

                xi2[i] = 2 * xi1[i] - xi[i] + (vv + vv1) * (xi1[i - 1] - 2 * xi1[i] + xi1[i + 1]) / h / h * dt * dt;


            }


            //vv = vv - vv2; // скорость распостронения второй половины
            if (!Imped_Box.Checked)
                for (i = imax / 2 - 2; i < N - 2; i++)  // просчет для второй половины
                {

                    xi2[i] = 2 * xi1[i] - xi[i] + (vv + vv2) * (xi1[i - 1] - 2 * xi1[i] + xi1[i + 1]) / h / h * dt * dt;
                   
                }

            if (!rightFix) // фиксируем/освобождаем правый конец
            {
                xi2[i] = 2 * xi1[i] - xi[i] + (vv + vv2) * (xi1[i - 1] - 2 * xi1[i] + xi1[i + 1]) / h / h * dt * dt;

            }
            else
            {
                xi1[i] = 0;
            }

            double[] prevxi = new double[N];
            double k = 0.0018;
            for (i = 0; i < N; i++)
            {

                if (i > 380 && i < N)
                   // if (xi[i - 5] < k && xi[i - 5] > -k && xi1[i - 5] < k && xi1[i - 5] > -k)
                        if (xi[i - 2] < k && xi[i - 2] > -k )
                           
                            {
                                //xi2[i - 2] = 0;
                                //xi1[i - 2] = 0;
                                //xi[i - 2] = 0;

                                for (int jj = i - 1; jj < N; jj++)
                                {
                                    xi[jj] = 0;
                                    xi1[jj] = 0;
                                    xi2[jj] = 0;
                                }
                               //     xi2[j - 1] = 0;
                               //xi1[i - 1] = 0;
                               //xi[i - 1] = 0;


                                //xi[i] = 0;
                                //xi1[i] = 0;
                                //xi2[i] = 0;

                               

                            }
                prevxi[i] = xi[i];
               
                //if (Math.Abs(xi2[i]) < 0.001) xi2[i] = 0;
                //if (Math.Abs(xi1[i]) < 0.001) xi1[i] = 0;
                //if (Math.Abs(xi[i]) < 0.001) xi[i] = 0;
               // if (i > 450) RE = 0.999;
               // else RE = 1;
                xi[i] = xi1[i]*RE; // *RE для сопротивления
                xi1[i] = xi2[i] * RE;

                
                // if (Math.Abs(xi1[i]) < 0.01) xi1[i] = 0;
                // if (Math.Abs(xi[i]) < 0.01) xi[i] = 0;
                // if (Math.Abs(xi2[i]) < 0.01) xi2[i] = 0;
               
            }

          
            ///////////////////////// здесь вместо N теперь imax как количество точек ОТРИСОВАННЫХ на экране


                brush_lastik = new SolidBrush(this.BackColor);
            gr.FillRectangle(brush_lastik, 0, 0, pictureBox1.Width, pictureBox1.Height);

           
            
            gr.DrawLine(pen_for_lines, pictureBox1.Location.X + 50,  graf_loc_y1+2, pictureBox1.Location.X + pictureBox1.Width, graf_loc_y1+2);
            gr.DrawLine(pen_for_lines, pictureBox1.Location.X + 50,  graf_loc_y2+2, pictureBox1.Location.X + pictureBox1.Width, graf_loc_y2+2);
            gr.DrawLine(pen_for_lines, pictureBox1.Location.X + 50,  graf_loc_y3+2, pictureBox1.Location.X + pictureBox1.Width, graf_loc_y3+2);
            for (int q = 1; q < imax; q++)
            {

            //  if(checkBox_Back.Checked) gr.DrawLine(pen_for_start, 50 + q * ro, 20, 50 + q * ro, 100); // пружина в начальном состоянии

                if (q % 3 == 1) gr.DrawLine(pen_for_lines, 50 + q * ro - (int)(xi[q - 1] * mult), pictureBox1.Location.Y + pictureBox1.Height / 10, 50 + q * ro - (int)(xi[q - 1] * mult), pictureBox1.Location.Y + 3*pictureBox1.Height / 11); // пружина
               
                Pen pen_for_sterg = new Pen(Color.DarkBlue);
                if (q == (int)imax / 2) gr.DrawLine(pen_for_lines, 50 + q * ro, pictureBox1.Location.Y + pictureBox1.Height / 10, 50 + q * ro, pictureBox1.Location.Y + pictureBox1.Height); // стержень
              //  gr.DrawEllipse(pen_for_lines, touch_circle_X - mouse_X, touch_circle_Y, touch_circle_R, touch_circle_R);  // маус таргет

               // gr.DrawEllipse(pen_for_lines, 50 + q * 3, 200 + (int)Math.Abs(xi[q - 1] * 3), 3, 3); // 
                int aproxim_num = 3;
                double sum=0;
                if (q > aproxim_num && q < N - aproxim_num)
                {
                    for (int i1 = q - aproxim_num; i1 < q; i1++)
                        sum += xi[i1];
                    for (int i1 = q; i1 < q + aproxim_num; i1++)
                        sum -= xi[i1];
                }

                double sum2 = 0;
              //  if (q > 5 && q < N - 5)
                {
                    for (int i1 = q - aproxim_num; i1 < q+aproxim_num; i1++)
                        if (i1 > 0 && i1 < N-1) sum2 += xi2[i1];
                    for (int i1 = q-aproxim_num; i1 < q + aproxim_num; i1++)
                        if (i1 > 0 && i1 < N - 1) sum2 -= xi[i1];
                }
                //else
                //{
                //  if(q>0 && q<N-1) sum += (xi[q - 1] - xi[q + 1]) / 2 / dt;
                //}

                double multv = 40;
                double multp = 3;

                gr.DrawEllipse(pen_for_lines, 50 + q * ro, (graf_loc_y1 + (int)(xi[q - 1] * mult)), 4, 4);//график смещений
                                                                                                  //if(q>5 && q<N-5) gr.DrawEllipse(pen_for_lines, 50 + q * ro, (400 - (int)( 1*(sum) / (10 * dt))), 4, 4);//график скорости
                if(!Imped_Box.Checked)gr.DrawEllipse(pen_for_lines, 50 + q * ro, (graf_loc_y3 + (int)(mult * multv* sum2/30)), 4, 4); // скорость(?)
                else 
                    if(q>imax/2-1)gr.DrawEllipse(pen_for_lines, 50 + q * ro, graf_loc_y3, 4, 4);
                    else gr.DrawEllipse(pen_for_lines, 50 + q * ro, (graf_loc_y3 + (int)(mult * multv  * sum2 / 30)), 4, 4); // скорость(?)

                if (!Imped_Box.Checked) gr.DrawEllipse(pen_for_lines, 50 + q * ro, (graf_loc_y2 + (int)(mult * 1 * multp * (sum) / (25* dt))), 4, 4); // график давления (400 - (int)(mult  * 15 * (xi2[q] - xi[q]) / 2))
                else
                    if (q > imax / 2 - 1) gr.DrawEllipse(pen_for_lines, 50 + q * ro, graf_loc_y2, 4, 4);
                    else gr.DrawEllipse(pen_for_lines, 50 + q * ro, (graf_loc_y2 + (int)(mult * 1 * multp * (sum) / (25*  dt))), 4, 4); // график давления (400 - (int)(mult  * 15 * (xi2[q] - xi[q]) / 2))
                // скорость(?)

                int point_in_per = 30; //точек в периоде 
               // if (q == imax / 4 + 6) 
                if (q % 3 == 1 && q < imax/6 + 8 && !was_drawn1 && q > imax/7 )
                {
                    gr.DrawLine(pen_for_lines_1, 50 + q * ro, pictureBox1.Location.Y + pictureBox1.Height / 10, 50 + q * ro, pictureBox1.Location.Y + pictureBox1.Height);
                    was_drawn1 = true;
                    gr.DrawLine(pen_for_lines_1, 50 + (q+point_in_per) * ro, pictureBox1.Location.Y + pictureBox1.Height / 10, 50 + (q+point_in_per) * ro, pictureBox1.Location.Y + pictureBox1.Height);
                     gr.DrawLine(pen_for_lines_1, 50 + q * ro - (int)(xi[q - 1] * mult), pictureBox1.Location.Y + pictureBox1.Height / 10, 50 + q * ro - (int)(xi[q - 1] * mult), pictureBox1.Location.Y + 3 * pictureBox1.Height / 11); // пружина

                     gr.DrawLine(pen_for_lines_1, 50 + (q+point_in_per) * ro - (int)(xi[q - 1] * mult), pictureBox1.Location.Y + pictureBox1.Height / 10, 50 + (q+point_in_per) * ro - (int)(xi[q - 1] * mult), pictureBox1.Location.Y + 3 * pictureBox1.Height / 11); // пружина

                    
                }
               // if (q == imax / 8 + 19) 
                 // if (q % 3 == 1 && q < imax + 8) gr.DrawLine(pen_for_lines_1, 50 + q * ro, pictureBox1.Location.Y + pictureBox1.Height / 10, 50 + q * ro, pictureBox1.Location.Y + pictureBox1.Height);

                




            }

           
            pictureBox1.Image = myBitmap;
            was_drawn1 = false;

        }

        private void Button_Start_Click(object sender, EventArgs e)
        {
           
            if (!timer1.Enabled)
            {
                timer1.Enabled = true;
                Button_Start.Text = "Стоп";
               
            }
            else
            {
                timer1.Enabled = false;
                Button_Start.Text = "Старт";
            }
        }

        private void button_Hit_Click(object sender, EventArgs e)
        {
           
            t3 = 0;
           // t += dt;
           // if (t < 6.28) xi1[0] = 20 * Math.Sin(t); // 6.28 для гармонических колебаний
            
        }

        private void button_Gauss_Click(object sender, EventArgs e)
        {
            t2 = -6;
        }

        private void button_Swell_Click(object sender, EventArgs e) // удар
        {
            xi1[2] -= Swell_Power/100;
            xi1[0] = 0;
            xi1[1] = 0;
            xi1[3] = 0;
            xi2[0] = 0;
            xi2[1] = 0;
            xi2[3] = 0;
            //xi1[0] = xi[0];
           // xi1[1] += 15;
           // xi1[2] += 10;
           // xi1[3] += 5;
        }

        private void button_PlusResist_Click(object sender, EventArgs e)
        {
            RE -= 0.01;
            

          //  textBox_Resist.Text = (1 - RE).ToString();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Imped_Box_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Size resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            this.Height = resolution.Height;
            this.Width = resolution.Width;

           

        }

        private void button_MinusResist_Click(object sender, EventArgs e)
        {
            if(RE<1)RE += 0.01;
            //textBox_Resist.Text = (1 - RE).ToString();
        }

        private void trackBar_ChangeV_Scroll_1(object sender, EventArgs e)
        {
            //if (flag_checkBox == true)
            //{
            //    vv2 = Convert.ToDouble(trackBar_ChangeV.Value);
            //    vv1 = Convert.ToDouble(trackBar_ChangeV.Value);
            //}

            vv2 = Convert.ToDouble(trackBar_ChangeV.Value);
            if(flag_checkBox)
            {
                trackBar_ChangeV1.Value = trackBar_ChangeV.Value;
                vv1 = Convert.ToDouble(trackBar_ChangeV1.Value);
            }

        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            checkBox.Text = "Z1 = Z2";
            if (checkBox.Checked == true)
            {
                flag_checkBox = true;
                trackBar_ChangeV1.Enabled = false;
                trackBar_ChangeV1.Value = trackBar_ChangeV.Value;
                vv1 = Convert.ToDouble(trackBar_ChangeV1.Value);
              
            }
            else
            {
                flag_checkBox = false;
                trackBar_ChangeV1.Enabled = true;
            }

            if (checkBox1.Checked) label6.Text = "Бегущая волна в однородном стержне";
            else label6.Text = "Волны на границе раздела";

            label6.Location = new Point(this.Width - label6.Width - 10, 8 * graf_loc_y);
                
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawLine(pen_for_lines, 500 + i * 4 - (int)xi[i - 1], 20, 500 + i * 4 - (int)xi[i - 1], 100);

           // e.Graphics.DrawEllipse(pen_for_lines, 500 + i * 4, 550 - (int)xi[i - 1], 3, 3);
           // gr = gr = Graphics.FromImage(myBitmap);
        }

        //private void button_FixLeft_Click(object sender, EventArgs e)
        //{
        //    if (leftFix)
        //    {
        //        leftFix = false;
        //        button_FixLeft.Text = "Закрепить левый край";
        //    }
        //    else
        //    {
        //        leftFix = true;
        //        button_FixLeft.Text = "Отпустить левый край";
        //    }
        //}

        private void button_FixRight_Click(object sender, EventArgs e)
        {
            if (rightFix)
            {
                rightFix = false;
                //button_FixRight.Text = "Закрепить свободный край";
            }
            else
            {
                rightFix = true;
               // button_FixRight.Text = "Отпустить свободный край";
            }
        }

        private void button_Reset_Click(object sender, EventArgs e) // сброс
        {
            RE = 1;
            //textBox_Resist.Text = "0";
            t = 10;
            t1 = 10;
            t2 = 1000;
            t3 = -1;
            t4 = 10000;
            isin = 100;
            for (int q = 0; q < N; q++)
            {
                xi[q] = xi1[q] = xi2[q] = 0;

            }
            Refresh();
        }

       

        private void trackBar_ChangeV1_Scroll(object sender, EventArgs e) // меняет скорость распостр. 1 половины
        {
            if (flag_checkBox == false)
                vv1 = Convert.ToDouble(trackBar_ChangeV1.Value);
           
           // Refresh();
        }

        private void trackBar_ChangeV_Scroll(object sender, EventArgs e)
        {
           
           // vv1 = Convert.ToDouble(trackBar_ChangeV1.Value);

        }

        private void trackBar1_Scroll(object sender, EventArgs e) // меняет силу удара
        {
            Swell_Power = trackBar1.Value;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.X >= touch_circle_X - touch_circle_R - mouse_X && e.X <= touch_circle_X + touch_circle_R - mouse_X) && (e.Y >= touch_circle_Y - touch_circle_R && e.Y <= touch_circle_Y + touch_circle_R))
            {
                mouse_catch = true; mouse_prev_X = e.X;
                 t1 = 0; // разкоментить чтобы работало
            }
            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_catch = false;
            //mouse_X = 0;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse_catch)
            {
                mouse_X = -e.X + mouse_prev_X;
                
               // touch_circle_X = e.X-touch_circle_R/2; //+ mouse_prev_X;
                
            }
        }

        private void button_FewSin_Click(object sender, EventArgs e)
        {
            t3 = 0;
        }

        private void button_SepSin_Click(object sender, EventArgs e)
        {





































            isin = 0;
            t4 = 0;
        }

        private void button_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

        

       
      

       
       

        

       
      
    }
}
