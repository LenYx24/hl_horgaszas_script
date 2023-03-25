using IronOcr;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace projform
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Az összes olyan gomb ami nem a startgomb!
        /// </summary>
        private void BalklikkTeszt()
        {
            MouseOperations.LeftClick();
        }

        private void JobbklikkTeszt()
        {
            MouseOperations.RightClick();
        }
        void Teszt()
        {
            for (int i = 0; i < 200; i++)
            {
                Sleep(7);
                MouseOperations.Move(0, 1);
            }
        }
        private void Kepernyokep()
        {
            Bitmap memoryImage;
            memoryImage = new Bitmap(25, 280);

            Graphics memoryGraphics = Graphics.FromImage(memoryImage);

            memoryGraphics.CopyFromScreen(1880, 420, 0, 0, new Size(memoryImage.Width, memoryImage.Height));

            //That's it! Save the image in the directory and this will work like charm.  
            string fileName = string.Format("Screenshot_" + DateTime.Now.ToString("(dd_MMMM_hh_mm_ss_tt)") + ".png");
            // save it  
            memoryImage.Save(fileName);
        }
         void BetukrolKepek()
        {
            int i = 1;
            while (true)
            {
                Bitmap memoryImage = new Bitmap(12, 12);

                Graphics memoryGraphics = Graphics.FromImage(memoryImage);

                memoryGraphics.CopyFromScreen(1056, 555, 0, 0, new Size(memoryImage.Width, memoryImage.Height));
                memoryImage.Save($"betu{i}.png");
                i++;
                Sleep(2000);
            }
        }
        private void BetuFelismerKeprol()
        {
            var ocr = new IronTesseract();
            using (var input = new OcrInput())
            {
                input.AddImage(textBox2.Text);
                OcrResult result = ocr.Read(input);
                string text = result.Text;
                MessageBox.Show(text);
            }
        }
         void IdoTeszt()
        {
            Stopwatch sw = Stopwatch.StartNew();
            SpamKey(Interceptor.Keys.M);
            sw.Stop();
            Debug.WriteLine("MS: "+sw.ElapsedMilliseconds.ToString());
        }
    }
}
