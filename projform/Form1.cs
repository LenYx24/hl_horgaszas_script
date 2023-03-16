using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Interceptor;
using IronOcr;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace projform
{
    public partial class Form1 : Form
    {
        /// <summary>
        ///  TODO:
        /// -> hotkeyre abbahagyja a program futását
        /// -> ha nem tudja bedobni a pecát, akkor addig mozgatja a kurzort még nem sikerül neki
        /// -> az elején a pecát is felrakja
        /// -> beállítja magának a kamerát az elején
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
        static Color ZOLD = Color.FromArgb(88,165,66);
        static Input Billentyuzet = new Input(); // ez az object kezeli a billentyűzetet (nyomja le a gombokat)
        static float varakozasiIdo = 5.0f;
        IronTesseract ocrSingle = new IronTesseract();
        IronTesseract ocrText = new IronTesseract();

        private void button_Click(object sender, EventArgs eventArgs)
        {
            string varakozas = textBox1.Text;
            try
            {
                varakozasiIdo = int.Parse(varakozas);
                if (0 <= varakozasiIdo && varakozasiIdo <= 20)
                {
                    Thread.Sleep(Convert.ToInt32(varakozasiIdo)*1000);
                }
                else return;
            }
            catch
            {
                MessageBox.Show("Rossz számot adtál meg te buta (min: 0 sec, max: 20 sec)");
                return;
            }
            switch (((Button)sender).Name)
            {
                case "button1": { Start(); break; }
                case "button2": { BalklikkTeszt(); break; }
                case "button3": { JobbklikkTeszt(); break; }
                case "button4": { Teszt(); break; }
                case "button5": { Kepernyokep(); break; }
                case "button6": { BetukrolKepek(); break; }
                case "button7": { BetuFelismerKeprol(); break; }
                case "button8": { IdoTeszt(); break; }
            }
        }
        
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = $"Egérpozíció: ({Cursor.Position.X},{Cursor.Position.Y})";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Be sure to set your keyboard filter to be able to capture key presses and simulate key presses
            // KeyboardFilterMode.All captures all events; 'Down' only captures presses for non-special keys; 'Up' only captures releases for non-special keys; 'E0' and 'E1' capture presses/releases for special keys
            Billentyuzet.KeyboardFilterMode = KeyboardFilterMode.All;
            // You can set a MouseFilterMode as well, but you don't need to set a MouseFilterMode to simulate mouse clicks
            Billentyuzet.ClickDelay = 40;
            // Finally, load the driver
            Billentyuzet.Load();
            ocrSingle.Configuration.PageSegmentationMode = TesseractPageSegmentationMode.SingleChar;
        }
    }
}
