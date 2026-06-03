using System;
using System.Collections.Generic;
using System.Linq;

namespace MaduUss_Puhtejev
{
    public class Toit
    {
        private Random rnd = new Random();
        private int ekraaniLaius;
        private int ekraaniKõrgus;
        public Punkt Asukoht { get; private set; }

        // Emoji on 2 tühiku lai — liigume ainult paarisveeergudel
        private const string ToiduSimbol = "🍎";

        public Toit(int laius, int kõrgus, List<Punkt> ussiKeha)
        {
            ekraaniLaius = laius;
            ekraaniKõrgus = kõrgus;
            LooUusToit(ussiKeha);
        }

        public void LooUusToit(List<Punkt> ussiKeha)
        {
            int x, y;
            int katsed = 0;

            do
            {
                // Emoji võtab 2 veergu — kasutame ainult paarisarve X koordinaadile
                // ja jätame seina ääred vahele (2..laius-3)
                x = rnd.Next(1, (ekraaniLaius - 3) / 2) * 2;
                y = rnd.Next(2, ekraaniKõrgus - 2);
                katsed++;
            }
            while (ussiKeha.Any(p => (p.X == x || p.X == x + 1) && p.Y == y) && katsed < 1000);

            Asukoht = new Punkt(x, y, ToiduSimbol);
        }

        public void Joonista()
        {
            Asukoht.Joonista();
        }
    }
}