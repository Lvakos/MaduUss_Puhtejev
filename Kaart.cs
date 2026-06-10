using System;
using System.Collections.Generic;

namespace MaduUss_Puhtejev
{
    public class Kaart
    {
        public int Laius { get; private set; }
        public int Kõrgus { get; private set; }
        private List<Punkt> takistused = new List<Punkt>();

        private const string SeinaSimbol = "█";

        public Kaart(int laius, int kõrgus)
        {
            Laius = laius;
            Kõrgus = kõrgus;

            for (int x = 0; x < laius; x++)
            {
                takistused.Add(new Punkt(x, 0, SeinaSimbol));
                takistused.Add(new Punkt(x, kõrgus - 1, SeinaSimbol));
            }
            for (int y = 1; y < kõrgus - 1; y++)
            {
                takistused.Add(new Punkt(0, y, SeinaSimbol));
                takistused.Add(new Punkt(laius - 1, y, SeinaSimbol));
            }
        }

        public void Joonista()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var p in takistused)
                p.Joonista();
            Console.ResetColor();
        }
    }
}