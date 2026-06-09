using System;
using System.Collections.Generic;
using System.Linq;

namespace MaduUss_Puhtejev
{
    public class Pomm
    {
        public Punkt Asukoht { get; private set; }
        private static Random rnd = new Random();

        private const string PommSimbol = "💣";

        public Pomm(int laius, int kõrgus, List<Punkt> keelatud)
        {
            int x, y, katsed = 0;
            do
            {

                x = rnd.Next(1, (laius - 3) / 2) * 2;
                y = rnd.Next(2, kõrgus - 2);
                katsed++;
            }

            while (keelatud.Any(p => (p.X == x || p.X == x + 1) && p.Y == y) && katsed < 1000);

            Asukoht = new Punkt(x, y, PommSimbol);
        }

        public void Joonista()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Asukoht.Joonista();
            Console.ResetColor();
        }

        public void Kustuta()
        {
            Asukoht.Kustuta();
        }

 
        public bool PõrkasUssiga(Punkt pea)
        {
            return (pea.X == Asukoht.X || pea.X == Asukoht.X + 1) && pea.Y == Asukoht.Y;
        }
    }
}