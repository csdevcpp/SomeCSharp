using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TestTask_Sukhov
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        
      


        
        private void button_start_Click(object sender, EventArgs e)
        {

           
            backgroundWorker1.RunWorkerAsync();
            Program.progress_sort=0;
            progressBar1.Visible = true;
            progressBar1.Value = Program.progress_sort;
            
            
        }

        

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = Program.progress_sort;
            textBox1.Text = Program.textbox_mess;
            if (progressBar1.Value == 100)
            {
                textBox1.Text = "Готово! Результат сортировки в файле result.txt";
                progressBar1.Visible = false;
            }
            if (backgroundWorker1.IsBusy) button_start.Enabled = false;
            else button_start.Enabled = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            divide();
            sort_divided_files();
            merge_sorted_files();
        }


        static void sort_divided_files()
        {
            Program.textbox_mess = "Сортируем разделенные файлы";

            foreach (string path in Directory.GetFiles(Environment.CurrentDirectory, "split*.dat"))
            {
                // Read all lines into an array
                string[] contents = File.ReadAllLines(path);
                // Sort the in-memory array
                //////////////////////////////////////////////
                //Array.Sort(contents);

                LinkedList<Num_Str> ns_contents = new LinkedList<Num_Str>();

                for (int i = 0; i < contents.Length; i++)
                {

                    Num_Str temp = new Num_Str(contents[i]);



                    if (i == 0)
                    {
                        ns_contents.AddFirst(temp);
                        continue;
                    }
                                       
                    bool added = false;
                    foreach (var s in ns_contents)
                    {                       
                        if (s > temp) { ns_contents.AddBefore(ns_contents.Find(s), temp); added = true; break; } //ns_contents.AddBefore();
                       
                    }
                    if (!added) ns_contents.AddLast(temp);


                }

                for (int i = 0; i < contents.Length; i++)
                {
                    contents[i] = ns_contents.First.Value.to_str();
                    ns_contents.RemoveFirst();                   
                }



                //////////////////////////////////////////////
                // Create the 'sorted' filename
                string newpath = path.Replace("split", "sorted");
                // Write it
                File.WriteAllLines(newpath, contents, Encoding.GetEncoding(1251));
                
                File.Delete(path);
                // Free the memory
                contents = null;
                ns_contents = null;
                Program.progress_sort = 15;
                GC.Collect();
            }
        }



        long start_file_size = 0;
        void divide()
        {
            Program.textbox_mess = "Разделяем файлы";
            int split_num = 1;
            StreamWriter sw = new StreamWriter(string.Format("split{0:d5}.dat", split_num), false); // new record only
            long read_line = 0;
            FileInfo a = new FileInfo("rdfile.txt");
            start_file_size = a.Length;
            using (StreamReader sr = new StreamReader("rdfile.txt", Encoding.GetEncoding(1251)))
            {

                string str = null;
                str = sr.ReadLine();
                while (str != null)
                {
                    if (str == null) break;

                    sw.WriteLine(str);
                    read_line++;

                    if (sr.EndOfStream) break;

                    if (read_line > 1000000)
                    {
                        sw.Flush();
                        sw.Close();
                        split_num++;
                        sw = new StreamWriter(string.Format("split{0:d5}.dat", split_num), false);
                        read_line = 0;

                    }

                    str = sr.ReadLine();

                }

                sw.Flush();

                sr.Close();
                sw.Close();
            }

            Program.progress_sort = 5;

        }

        string[] all_sort_files;
        int prev_progress = 0;
        void merge_sorted_files()
        {
            // готовим стартовые значения и открываем потоки
            all_sort_files = Directory.GetFiles(Environment.CurrentDirectory, "sort*.dat");

            StreamReader[] links = new StreamReader[all_sort_files.Length];  //= new StreamReader(
            Queue<Num_Str>[] str_links = new Queue<Num_Str>[all_sort_files.Length];

            int j = 0;
            Num_Str str_max = null;
            int str_max_num = -1;
            for (int i = 0; i < all_sort_files.Length; i++) 
            {
                links[i] = new StreamReader(all_sort_files[i], Encoding.GetEncoding(1251));
                j = 0;

                str_links[i] = new Queue<Num_Str>();
               
                str_links[i].Enqueue(new Num_Str(links[i].ReadLine()));
                while (true)
                {
                    j++;
                    if (j > 100000 || links[i].EndOfStream) break;

                    str_links[i].Enqueue(new Num_Str(links[i].ReadLine()));


                }

                
            }


            // main merge

            StreamWriter sw = new StreamWriter("result.txt", false, Encoding.GetEncoding(1251)); // new record only
            
            while (true) // основной цикл слияния
            {
               
                


                str_max = null;
                str_max_num = -1;
              
                for (int i = 0; i < all_sort_files.Length; i++)
                {
                    if (str_links[i].Count < 1)
                    {
                        int k = 0;
                        if (links[i].EndOfStream) continue;
                        while (true)                                                    // подкачка
                        {
                            if (links[i].EndOfStream || k > 100000) break;

                            str_links[i].Enqueue(new Num_Str(links[i].ReadLine()));
                            k++;

                        }
                       

                    }
                    

                    if (str_max == null || str_max > str_links[i].Peek())
                    {
                        str_max = str_links[i].Peek();
                        str_max_num = i;
                    }

                }




                if (str_max_num < 0) break;

                prev_progress = Program.progress_sort;
                Program.progress_sort = 15 + (int)((75 * sw.BaseStream.Length) / start_file_size);
                if(prev_progress!=Program.progress_sort)Program.textbox_mess ="Идет основная сортировка: " + Program.progress_sort.ToString() + "%";
                if (Program.progress_sort % 26 <= 1) sw.Flush();

                sw.WriteLine(str_max.to_str());  // запись в основной файл 

                str_links[str_max_num].Dequeue();


            }

            sw.Flush();
            sw.Close();
            Program.progress_sort = 100;
            foreach (var s in links) s.Close();

            foreach (var s in all_sort_files) File.Delete(s);
            all_sort_files = null;
           
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //backgroundWorker1.CancelAsync();
            //if(all_sort_files!=null)foreach (var s in all_sort_files) File.Delete(s);
            //foreach (var s in Directory.GetFiles(Environment.CurrentDirectory, "sort*.dat")) { File.Delete(s); }
            //foreach (var s in Directory.GetFiles(Environment.CurrentDirectory, "split*.dat")) File.Delete(s);
       
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var s in Directory.GetFiles(Environment.CurrentDirectory, "sort*.dat")) { File.Delete(s); }
            foreach (var s in Directory.GetFiles(Environment.CurrentDirectory, "split*.dat")) File.Delete(s);

            Program.textbox_mess = "Здравствуйте!";
        }

        
    }
}
