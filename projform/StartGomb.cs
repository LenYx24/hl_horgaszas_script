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
        private void Start() // Ez a főgomb
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
                for(int j = 0; j < 4; j++)
                {
                    // most meg kb 1 másodpercenként csinál egy printscreen-t addig amíg nem lesz kapása
                    do
                    {
                        if (vanEKapasod()) break;
                        Sleep(40);
                    } while (true);
                    Debug.WriteLine("KILÉPETT A WHILE-BÓL");
                    // Ha kilépett a while-ból, akkor lett kapás
                    Sleep(5500);
                    Bitmap mp1 = BeturolCsinalKepet("betu1.png");
                    DebugPrtscreen("prt.png");
                    // csinál két képet, 0.5 ms eltéréssel és ha kb ugyanazok,
                    // akkor nem mozog a kör, tehát csak egyszer kell lenyomni a betűt
                    Sleep(200);
                    Bitmap mp2 = BeturolCsinalKepet("betu2.png");
                    mp2.Save("file.png");
                    char betu = MelyikBetu(mp2);
                    Keys key = Billentyuzet.CharacterToKeysEnum(betu).Item1;
                    Debug.WriteLine("EZ A BETŰŰŰŰŰŰ::::"+betu);
                    if (KetKepMegegyezik(mp1,mp2))
                    {
                        // egyszer kell nyomni
                        Lenyom(key);
                    }
                    else
                    {
                        SpamKey(key);
                    }
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
            Billentyuzet.MoveMouseTo(900, 250); // ráviszem az egeret a vízre
            ablakotValt(); // kitabolok, mert klikkelni kell
            Sleep(500);
            MouseOperations.LeftClickHoldDown(); // bebalklikkeltem az ablakba és most tartja a klikket
            // most csinál egy képernyőképet és megnézi, hogy hol van a zöld négyzet,
            // majd kiszámolja, hogy hány másodperc múlva kell felengedni a balklikket
            var watch = Stopwatch.StartNew();

            int ido = MennyiIdeigVarjon();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            randomInfoLabel.Text = elapsedMs.ToString();
            Sleep(ido);
            ablakotValt(); // gec nem kell felengedni az egeret, elég kialtabolni xddd
            tabolEgyszer(); // vissza is kell tabolni
            Lenyom(Keys.M);
            Lenyom(Keys.I);
        }
        int MennyiIdeigVarjon()
        {
            int teglalapmagassaga = 280; // az össz magasság amennyit megy a fehér vonal kb: 280 px

            float osszido = 3.45f; // ez x másodperc alatt történik meg (egy mérés alapján 3.45, lehet kell majd többször mérni!)

            // na most megkapjuk, hogy a zöld négyzetnek a közepe milyen magasan van, innen képlettel megkapjuk, hogy mennyi másodpercet kell várni

            float ido = ((osszido - ((kisZoldNegyzetMagassaga() * osszido) / teglalapmagassaga)) * 1000) + 60; // megszorozzuk 1000-el mert ms-et kell megadni a Sleep()-nek
            return Convert.ToInt32(ido); // a sleep csak int-et fogad el
        }
        int kisZoldNegyzetMagassaga() // a zöld kis négyzet alulról számítva milyen magasan van
        {
            Sleep(60); // azért kell, mert túl gyors a program, aztán megfogja és korán csinál képernyőképet
            Bitmap memoryImage = new Bitmap(25, 280);

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
        bool vanEKapasod()
        {
            int w = 100;
            int h = 25;
            Bitmap memoryImage = new Bitmap(w, h);

            Graphics memoryGraphics = Graphics.FromImage(memoryImage);

            memoryGraphics.CopyFromScreen(650, 956, 0, 0, new Size(memoryImage.Width, memoryImage.Height));
            using (var input = new OcrInput())
            {
                input.AddImage(memoryImage);
                OcrResult r = ocrText.Read(input);
                Debug.WriteLine("A text:"+r.Text);
                if (!r.Text.Contains("meg, ame")) return true;
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
            if (equalElements > 500) return true; // 24*24 = 576, tehát kb 500-től nagyobbnak kell lenni, hogy ugyanaz legyen
            return false;
        }
        private void SpamKey(Keys key)
        {
            for (int i = 0; i < 50; i++)
            {
                Lenyom(key);
                Sleep(100);
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
            Sleep(1000);
            Billentyuzet.SendKey(Keys.Tab);
            Sleep(1000);
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
