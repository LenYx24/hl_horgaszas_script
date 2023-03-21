using Interceptor;
using IronOcr;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Keys = Interceptor.Keys;
using System.IO;

namespace projform
{
    public partial class Form1 : Form 
    {
        void Start() // Ez a főgomb
        {
            bool kezdjeujra = false; // meddig folytassa
            do
            {
                // alapból a horgászbotnak a kézben kell lennie,
                // a csalinak megfelelő helyen (2. sor 2. oszlop)
                // és a víz felé kb 45 fokos szögben kell néznie
                FelrakEgyCsalit();

                // Használatba van a csali és az mta ablakja van kijelölve

                // most bedobjuk a csalit
                BedobjaACsalit();

                // elvileg bedobta a csalit

                //kapás része
                // most meg kb 1 másodpercenként csinál egy printscreen-t addig amíg nem lesz kapása
                int k = 0;
                do
                {
                    if (vanEKapasod(k)) break;
                    k++;
                     Sleep(40);
                } while (true);
                Debug.WriteLine("KILÉPETT A WHILE-BÓL");
                // Ha kilépett a while-ból, akkor lett kapás
                Sleep(5100);
                for (int j = 0; j < 4; j++)
                {
                    Bitmap mp1 = BeturolCsinalKepet("betu1.png");
                    mp1.Save($"file1{j}.png");
                    // csinál két képet, 0.5 ms eltéréssel és ha kb ugyanazok,
                    // akkor nem mozog a kör, tehát csak egyszer kell lenyomni a betűt
                    Sleep(550);
                    Bitmap mp2 = BeturolCsinalKepet("betu2.png");
                    mp2.Save($"file2{j}.png");
                    Bitmap bv = mp2.Clone(new RectangleF(35, 55, 35, 35), mp2.PixelFormat);
                    bv.Save($"cropped{j}.png");
                    char betu = MelyikBetu(bv);
                    if (betu == '1' || betu == '|') betu = 'i';
                    Keys key = Billentyuzet.CharacterToKeysEnum(betu).Item1;
                    Debug.WriteLine("EZ A BETŰŰŰŰŰŰ:"+betu);
                    if (KetKepMegegyezik(mp1,mp2))
                    {
                        // egyszer kell nyomni
                        Lenyom(key);
                    }
                    else
                    {
                        SpamKey(key);
                    }
                    Debug.WriteLine("Most vár egy picikét");
                    Sleep(3000);
                    Debug.WriteLine("Megvárta a picikét");
                }

                // utolsó lépés
                // a kamerát beállítsa alapállásba
                for (int i = 0; i < 200; i++)
                {
                     Sleep(7);
                    MouseOperations.Move(0, 1);
                }
            } while (kezdjeujra);
            MessageBox.Show("Befejeződött a horgászás!");
        }
         void FelrakEgyCsalit()
        {
            Lenyom(Keys.Two);
             Sleep(50);
        }
        void DebugPrtscreen(string filename)
        {
            int w = 400;
            int h = 400;
            Bitmap memoryImage = new Bitmap(w, h);

            Graphics memoryGraphics = Graphics.FromImage(memoryImage);

            int scw = 1920;
            int sch = 1080;

            memoryGraphics.CopyFromScreen(scw / 2 - w / 2, sch / 2 - h / 2 - 100, 0, 0, new Size(memoryImage.Width, memoryImage.Height));
            memoryImage.Save(filename);
        }
         void BedobjaACsalit()
        {
            Lenyom(Keys.M);
            Billentyuzet.MoveMouseTo(900, 250); // ráviszem az egeret a vízre
            tabolEgyszer(); // kitabolok, mert klikkelni kell
            MouseOperations.LeftClickHoldDown(); // bebalklikkeltem az ablakba és most tartja a klikket
            // most csinál egy képernyőképet és megnézi, hogy hol van a zöld négyzet,
            // majd kiszámolja, hogy hány másodperc múlva kell felengedni a balklikket
            var watch = Stopwatch.StartNew();
             Sleep(60); // azért kell, mert túl gyors a program, aztán megfogja és korán csinál képernyőképet

            int ido = MennyiIdeigVarjon();

            watch.Stop();
            Debug.WriteLine("Eltelt másodperc: " + watch.ElapsedMilliseconds);
             Sleep(ido);
            ablakotValt(); // gec nem kell felengedni az egeret, elég kialtabolni xddd
            tabolEgyszer(); // vissza is kell tabolni
            Lenyom(Keys.M);
        }
        int MennyiIdeigVarjon()
        {
            int teglalapmagassaga = 280; // az össz magasság amennyit megy a fehér vonal kb: 280 px

            float osszido = 3.45f; // ez x másodperc alatt történik meg (egy mérés alapján 3.45, lehet kell majd többször mérni!)

            // na most megkapjuk, hogy a zöld négyzetnek a közepe milyen magasan van, innen képlettel megkapjuk, hogy mennyi másodpercet kell várni

            float ido = (osszido - ((kisZoldNegyzetMagassaga() * osszido) / teglalapmagassaga)) * 1000; // megszorozzuk 1000-el mert ms-et kell megadni a Sleep()-nek
            Debug.WriteLine(ido);
            int t =Convert.ToInt32(ido);
            
            return t;// a sleep csak int-et fogad el
        }
        int kisZoldNegyzetMagassaga() // a zöld kis négyzet alulról számítva milyen magasan van
        {
            Bitmap memoryImage = new Bitmap(22, 280);

            Graphics memoryGraphics = Graphics.FromImage(memoryImage);

            memoryGraphics.CopyFromScreen(1880, 420, 0, 0, new Size(memoryImage.Width, memoryImage.Height));

            // megvan a kép a téglalapról
            // most alulról elkezdjük keresni a zöld négyzetet
            // (egyébként itt van egy jobb módszer is, az, hogy nem egyesével megyünk felfele, hanem kb 23-ával, mert ezzel megtaláljuk, hogy kb hol a zöld négyzet)

            memoryImage.Save("tegla.png");
            
            for (int i = 270; i > 0; i--)
            {
                if (memoryImage.GetPixel(10, i) == ZOLD)
                {
                    return i; // visszaküldjük a magasságot
                }
            }

            return 0;
        }
        bool vanEKapasod(int k)
        {
            int w = 100;
            int h = 25;
            Bitmap memoryImage = new Bitmap(w, h);

            Graphics memoryGraphics = Graphics.FromImage(memoryImage);

            memoryGraphics.CopyFromScreen(738, 955, 0, 0, new Size(memoryImage.Width, memoryImage.Height));
            //memoryImage.Save($"xd/kep{k}.png");
            using (var input = new OcrInput())
            {
                input.AddImage(memoryImage);
                OcrResult r = ocrText.Read(input);
                Debug.WriteLine("A text:"+r.Text);
                if (r.Text.Contains("van!")) return true;
            }
            return false;
        }
        bool vanEBetuAKepen(int k) 
        {
            // 100, hogyha a betű és a kör is kell
            // 30, hogyha csak a betű kell
            int w = 100; 
            int h = 100;
            int betuw = 50;
            int betuh = -50;
            Bitmap memoryImage = new Bitmap(w, h);

            Graphics memoryGraphics = Graphics.FromImage(memoryImage);

            int scw = 1920;
            int sch = 1080;

            memoryGraphics.CopyFromScreen(scw / 2+ betuw, sch / 2 +betuh, 0, 0, new Size(memoryImage.Width, memoryImage.Height));
            //memoryImage.Save($"xd/vaneimage{k}.png");
            using (var input = new OcrInput())
            {
                input.AddImage(memoryImage);
                input.Invert().Contrast().SaveAsImages($"xd/asd{k}");
            }
            return false;
        }
        private Bitmap BeturolCsinalKepet(string filename)
        {
            int w = 100;
            int h = 100;
            int betuw = 50;
            int betuh = -50;
            Bitmap memoryImage = new Bitmap(w, h);

            Graphics memoryGraphics = Graphics.FromImage(memoryImage);

            int scw = 1920;
            int sch = 1080;

            memoryGraphics.CopyFromScreen(scw / 2 + betuw, sch / 2 + betuh, 0, 0, new Size(memoryImage.Width, memoryImage.Height));
            return memoryImage;
        }
        private char MelyikBetu(Bitmap m)
        {
            var ocr = new IronTesseract();
            ocr.Configuration.PageSegmentationMode = TesseractPageSegmentationMode.SingleChar;
            using (var input = new OcrInput())
            {
                input.AddImage(m);
                OcrResult result = ocr.Read(input);
                Debug.WriteLine(result.Text);
                return result.Text[0];
            }
        }
        private bool KetKepMegegyezik(Bitmap m1, Bitmap m2)
        {
            List<bool> iHash1 = GetHash(m1);
            List<bool> iHash2 = GetHash(m2);

            int equalElements = iHash1.Zip(iHash2, (i, j) => i == j).Count(eq => eq);
            Debug.WriteLine("equal Elements: " + equalElements);
            if (equalElements > 9800) return true;
            return false;
        }
         void SpamKey(Keys key)
        {
            for (int i = 0; i < 30; i++)
            {
                Lenyom(key);
                 Sleep(50);
            }
        }
        public static List<bool> GetHash(Bitmap bmp)
        {
            List<bool> lResult = new List<bool>();
            Bitmap bmpMin = bmp; //  new Bitmap(bmp, new Size(24, 24))      ha nagy képet hozna létre, és sokáig tartana
            //create new image with 16x16 pixel
            for (int j = 0; j < bmpMin.Height; j++)
            {
                for (int i = 0; i < bmpMin.Width; i++)
                {
                    //reduce colors to true / false                
                    lResult.Add(bmpMin.GetPixel(i, j).GetBrightness() < 0.5f);
                }
            }
            return lResult;
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        void ablakotValt()
        {
            Process[] points = Process.GetProcessesByName("projform");
            if (points.Any())
            {
                IntPtr i = points[0].MainWindowHandle;
                SetForegroundWindow(i);
            }
        }
        void tabolEgyszer()
        {
            Billentyuzet.SendKey(Keys.RightAlt, KeyState.Down);
            Thread.Sleep(1000);
            Billentyuzet.SendKey(Keys.Tab);
            Thread.Sleep(1000);
            Billentyuzet.SendKey(Keys.RightAlt, KeyState.Up);

        }
        void Sleep(int ms)
        {
            Thread.Sleep(ms);
        }
        void Lenyom(Keys key)
        {
            Billentyuzet.SendKey(key);
        }
    }
}
