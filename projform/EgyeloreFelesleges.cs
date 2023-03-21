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
        // Rakj egy F_ suffixet minden function elé, ha felesleges
        void F_FelrakEgyCsalit()
        {
            Lenyom(Keys.M);
            Sleep(50);

            Lenyom(Keys.I);
            Sleep(50);

            Billentyuzet.MoveMouseTo(835, 512); // csali helye
            Sleep(1000);

            tabolEgyszer();
            MouseOperations.RightClick(); // rájobbklikkel a csalira

            Billentyuzet.MoveMouseTo(940, 550); // ráviszi a kurzort a használat gombra
            tabolEgyszer();
            MouseOperations.LeftClick(); // végül rákattint a használat gombra
        }
    }
}
