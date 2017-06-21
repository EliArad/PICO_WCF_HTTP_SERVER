using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicoHttpClient
{
    public partial class Form1 : Form
    {
        PicoClientApi pico;
        public Form1()
        {
            InitializeComponent();

            pico = new PicoClientApi("localhost");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string r;
            if ((r = pico.Open()) != "ok")
            {
                MessageBox.Show(r);
            }

            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string r;
            if ((r = pico.Close()) != "ok")
            {
                MessageBox.Show(r);
            }
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float [] result;
            textBox1.Clear();
            if (pico.ReadChannels(out result)== "ok")
            {                
                for (int i = 0 ; i < result.Length ;i++)
                {
                    textBox1.Text += result[i].ToString() + Environment.NewLine;
                }
            }
        }
    }
}
