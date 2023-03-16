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
        private void Teszt()
        {
            int w = 200;
            int h = 200;
            Bitmap memoryImage = new Bitmap(w, h);

            Graphics memoryGraphics = Graphics.FromImage(memoryImage);

            int scw = 1920;
            int sch = 1080;

            memoryGraphics.CopyFromScreen(scw / 2 - w / 2 , sch / 2 - h / 2 - 100, 0, 0, new Size(memoryImage.Width, memoryImage.Height));
            memoryImage.Save("vaneimage.png");
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
        private void BetukrolKepek()
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
        private void IdoTeszt()
        {
            int k = 0;
            // most meg kb 1 másodpercenként csinál egy printscreen-t addig amíg nem lesz kapása
            do
            {
                Stopwatch sw = Stopwatch.StartNew();
                if (vanEBetuAKepen(k)) break;
                k++;
                sw.Stop();
                Debug.WriteLine("MS: "+sw.ElapsedMilliseconds.ToString());
                Sleep(200);
            } while (true);
        }
    }
}
