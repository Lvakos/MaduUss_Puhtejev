using System;
using System.Collections.Generic;
using System.Linq;

namespace MaduUss_Puhtejev
{
    public class Uss
    {
        private List<Punkt> keha = new List<Punkt>();
        private bool kasvaJärgmiseSammuga = false;

        private const string PeaSimbol = "■";
        private const string KehaSimbol = "□";

        public Suund PraeguneSuund { get; set; }

        public Uss(int algX, int algY, int pikkus)
        {
            PraeguneSuund = Suund.Paremale;
            for (int i = 0; i < pikkus; i++)
            {
                string sümbol = (i == 0) ? PeaSimbol : KehaSimbol;
                Punkt p = new Punkt(algX - i, algY, sümbol);
                keha.Add(p);
            }
            JoonistaSee();
        }

        public void Liigu()
        {
            // Vana pea muutub kehaks
            if (keha.Count > 0)
                keha[0].Sümbol = KehaSimbol;

            Punkt pea = keha.First();
            Punkt uusPea = new Punkt(pea.X, pea.Y, PeaSimbol);

            switch (PraeguneSuund)
            {
                case Suund.Paremale: uusPea.X++; break;
                case Suund.Vasakule: uusPea.X--; break;
                case Suund.Alla: uusPea.Y++; break;
                case Suund.Üles: uusPea.Y--; break;
            }

            keha.Insert(0, uusPea);

            Console.ForegroundColor = ConsoleColor.Green;
            uusPea.Joonista();
            // Uuenda eelmine pea (nüüd keha) väljanägemist
            keha[1].Joonista();
            Console.ResetColor();

            if (kasvaJärgmiseSammuga)
            {
                kasvaJärgmiseSammuga = false;
            }
            else
            {
                Punkt saba = keha.Last();
                saba.Kustuta();
                keha.RemoveAt(keha.Count - 1);
            }
        }

        public Punkt HangiPea()
        {
            return keha.First();
        }

        public List<Punkt> HangiKeha()
        {
            return keha;
        }

        public bool PõrkasSiseendaga()
        {
            Punkt pea = keha.First();
            return keha.Skip(1).Any(p => p.X == pea.X && p.Y == pea.Y);
        }

        public void Kasva()
        {
            kasvaJärgmiseSammuga = true;
        }

        private void JoonistaSee()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var p in keha)
                p.Joonista();
            Console.ResetColor();
        }
    }
}