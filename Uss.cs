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
        public ConsoleColor Värv { get; private set; }
        public bool OnElus { get; private set; } = true;

        public Uss(int algX, int algY, int pikkus, Suund algSuund, ConsoleColor värv)
        {
            PraeguneSuund = algSuund;
            Värv = värv;

            int samm = (algSuund == Suund.Paremale) ? -1 : 1;
            for (int i = 0; i < pikkus; i++)
            {
                string sümbol = (i == 0) ? PeaSimbol : KehaSimbol;
                Punkt p = new Punkt(algX + samm * i, algY, sümbol);
                keha.Add(p);
            }
            JoonistaSee();
        }

        // Viimane kustutatud sabaots — vajalik emoji taastamiseks
        public Punkt? ViimaneKustutatud { get; private set; } = null;

        public void Liigu()
        {
            if (!OnElus) return;

            ViimaneKustutatud = null;

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

            Console.ForegroundColor = Värv;
            uusPea.Joonista();
            keha[1].Joonista();
            Console.ResetColor();

            if (kasvaJärgmiseSammuga)
            {
                kasvaJärgmiseSammuga = false;
            }
            else
            {
                Punkt saba = keha.Last();
                ViimaneKustutatud = new Punkt(saba.X, saba.Y, saba.Sümbol);
                saba.Kustuta();
                keha.RemoveAt(keha.Count - 1);
            }
        }

        public Punkt HangiPea() => keha.First();

        public List<Punkt> HangiKeha() => keha;

        public bool PõrkasSiseendaga()
        {
            Punkt pea = keha.First();
            return keha.Skip(1).Any(p => p.X == pea.X && p.Y == pea.Y);
        }

        // Kontroll teise ussi kehaga kokkupõrkeks
        public bool PõrkasTeiseUssiga(Uss teine)
        {
            Punkt pea = keha.First();
            return teine.keha.Any(p => p.X == pea.X && p.Y == pea.Y);
        }

        public void Kasva() => kasvaJärgmiseSammuga = true;

        public void Tapa() => OnElus = false;

        private void JoonistaSee()
        {
            Console.ForegroundColor = Värv;
            foreach (var p in keha)
                p.Joonista();
            Console.ResetColor();
        }
    }
}