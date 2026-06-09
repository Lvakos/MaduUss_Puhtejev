using System;
using System.Collections.Generic;
using System.Threading;

namespace MaduUss_Puhtejev
{
    // Ühe mängija režiim
    public class UksikmängijaRežiim
    {
        private Manguseaded seaded;

        public UksikmängijaRežiim(Manguseaded seaded)
        {
            this.seaded = seaded;
        }

        public void Käivita()
        {
            Kaart kaart = new Kaart(seaded.Laius, seaded.Kõrgus);
            Uss uss = new Uss(seaded.Laius / 2, seaded.Kõrgus / 2, 3, Suund.Paremale, ConsoleColor.Green);
            Toit toit = new Toit(seaded.Laius, seaded.Kõrgus, uss.HangiKeha());
            int skoor = 0;

            kaart.Joonista();
            toit.Joonista();
            KuvaSkoor(skoor);

            bool mängLäbi = false;

            while (!mängLäbi)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo klahv = Console.ReadKey(true);
                    if (klahv.Key == ConsoleKey.UpArrow && uss.PraeguneSuund != Suund.Alla) uss.PraeguneSuund = Suund.Üles;
                    else if (klahv.Key == ConsoleKey.DownArrow && uss.PraeguneSuund != Suund.Üles) uss.PraeguneSuund = Suund.Alla;
                    else if (klahv.Key == ConsoleKey.LeftArrow && uss.PraeguneSuund != Suund.Paremale) uss.PraeguneSuund = Suund.Vasakule;
                    else if (klahv.Key == ConsoleKey.RightArrow && uss.PraeguneSuund != Suund.Vasakule) uss.PraeguneSuund = Suund.Paremale;
                    else if (klahv.Key == ConsoleKey.Escape) { mängLäbi = true; continue; }
                }

                uss.Liigu();
                // Joonistame toidu uuesti igal sammul — väldib emoji kustumist
                // kui uss liigub selle kõrvalt mööda ja kustutab poole sümbolist
                toit.Joonista();
                Punkt pea = uss.HangiPea();

                if (pea.X <= 0 || pea.X >= seaded.Laius - 1 || pea.Y <= 0 || pea.Y >= seaded.Kõrgus - 1
                    || uss.PõrkasSiseendaga())
                {
                    mängLäbi = true;
                    continue;
                }

                if ((pea.X == toit.Asukoht.X || pea.X == toit.Asukoht.X + 1) && pea.Y == toit.Asukoht.Y)
                {
                    uss.Kasva();
                    skoor += 10;
                    toit.LooUusToit(uss.HangiKeha());
                    toit.Joonista();
                    KuvaSkoor(skoor);
                }

                Thread.Sleep(seaded.KiirusMS);
            }

            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Write($"Mäng läbi! Sinu skoor: {skoor}. Sisesta oma nimi: ");
            string nimi = Console.ReadLine() ?? "Mängija";
            if (string.IsNullOrWhiteSpace(nimi)) nimi = "Mängija";
            Edetabel.Salvesta(nimi, skoor);
            Edetabel.KuvaEdetabel();
        }

        private void KuvaSkoor(int skoor)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(2, 0);
            Console.Write($" 🍎 Skoor: {skoor} ");
            Console.ResetColor();
        }
    }

    // Kahe mängija režiim
    public class KaksMängijatRežiim
    {
        private Manguseaded seaded;

        public KaksMängijatRežiim(Manguseaded seaded)
        {
            this.seaded = seaded;
        }

        public void Käivita()
        {
            Kaart kaart = new Kaart(seaded.Laius, seaded.Kõrgus);

            // Mängija 1 (roheline) — vasakul, liigub paremale
            // Mängija 2 (sinine) — paremal, liigub vasakule
            Uss uss1 = new Uss(seaded.Laius / 4, seaded.Kõrgus / 2, 3, Suund.Paremale, ConsoleColor.Green);
            Uss uss2 = new Uss(seaded.Laius * 3 / 4, seaded.Kõrgus / 2, 3, Suund.Vasakule, ConsoleColor.Cyan);

            // Üks ühine toit mõlemale
            var koguKeha = new List<Punkt>();
            koguKeha.AddRange(uss1.HangiKeha());
            koguKeha.AddRange(uss2.HangiKeha());
            Toit toit = new Toit(seaded.Laius, seaded.Kõrgus, koguKeha);

            int skoor1 = 0, skoor2 = 0;

            kaart.Joonista();
            toit.Joonista();
            KuvaSkooriKahe(skoor1, skoor2);
            KuvaJuhised();

            bool mängLäbi = false;

            while (!mängLäbi)
            {
                // Sisend — mõlemad mängijad samaaegselt
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo klahv = Console.ReadKey(true);

                    // Mängija 1: nooleklahvid
                    if (klahv.Key == ConsoleKey.UpArrow && uss1.PraeguneSuund != Suund.Alla) uss1.PraeguneSuund = Suund.Üles;
                    else if (klahv.Key == ConsoleKey.DownArrow && uss1.PraeguneSuund != Suund.Üles) uss1.PraeguneSuund = Suund.Alla;
                    else if (klahv.Key == ConsoleKey.LeftArrow && uss1.PraeguneSuund != Suund.Paremale) uss1.PraeguneSuund = Suund.Vasakule;
                    else if (klahv.Key == ConsoleKey.RightArrow && uss1.PraeguneSuund != Suund.Vasakule) uss1.PraeguneSuund = Suund.Paremale;

                    // Mängija 2: WASD
                    else if (klahv.Key == ConsoleKey.W && uss2.PraeguneSuund != Suund.Alla) uss2.PraeguneSuund = Suund.Üles;
                    else if (klahv.Key == ConsoleKey.S && uss2.PraeguneSuund != Suund.Üles) uss2.PraeguneSuund = Suund.Alla;
                    else if (klahv.Key == ConsoleKey.A && uss2.PraeguneSuund != Suund.Paremale) uss2.PraeguneSuund = Suund.Vasakule;
                    else if (klahv.Key == ConsoleKey.D && uss2.PraeguneSuund != Suund.Vasakule) uss2.PraeguneSuund = Suund.Paremale;

                    else if (klahv.Key == ConsoleKey.Escape) { mängLäbi = true; break; }
                }
                if (mängLäbi) break;

                uss1.Liigu();
                uss2.Liigu();
                // Sama parandus — joonistame toidu pärast mõlema ussi liikumist
                toit.Joonista();

                Punkt pea1 = uss1.HangiPea();
                Punkt pea2 = uss2.HangiPea();

                // Kontrollime surma tingimusi
                bool uss1Sureb = pea1.X <= 0 || pea1.X >= seaded.Laius - 1 || pea1.Y <= 0 || pea1.Y >= seaded.Kõrgus - 1
                                 || uss1.PõrkasSiseendaga() || uss1.PõrkasTeiseUssiga(uss2);

                bool uss2Sureb = pea2.X <= 0 || pea2.X >= seaded.Laius - 1 || pea2.Y <= 0 || pea2.Y >= seaded.Kõrgus - 1
                                 || uss2.PõrkasSiseendaga() || uss2.PõrkasTeiseUssiga(uss1);

                if (uss1Sureb || uss2Sureb)
                {
                    mängLäbi = true;
                    string võitja;
                    if (uss1Sureb && uss2Sureb)
                        võitja = "Viik!";
                    else if (uss1Sureb)
                        võitja = "Mängija 2 (WASD) võidab!";
                    else
                        võitja = "Mängija 1 (nooled) võidab!";

                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(võitja);
                    Console.WriteLine($"Mängija 1 skoor: {skoor1}  |  Mängija 2 skoor: {skoor2}");
                    continue;
                }

                // Toit — mõlemad mängijad saavad süüa
                var koguKeha2 = new List<Punkt>();
                koguKeha2.AddRange(uss1.HangiKeha());
                koguKeha2.AddRange(uss2.HangiKeha());

                bool sõiToit1 = (pea1.X == toit.Asukoht.X || pea1.X == toit.Asukoht.X + 1) && pea1.Y == toit.Asukoht.Y;
                bool sõiToit2 = (pea2.X == toit.Asukoht.X || pea2.X == toit.Asukoht.X + 1) && pea2.Y == toit.Asukoht.Y;

                if (sõiToit1)
                {
                    uss1.Kasva();
                    skoor1 += 10;
                    toit.LooUusToit(koguKeha2);
                    toit.Joonista();
                    KuvaSkooriKahe(skoor1, skoor2);
                }
                else if (sõiToit2)
                {
                    uss2.Kasva();
                    skoor2 += 10;
                    toit.LooUusToit(koguKeha2);
                    toit.Joonista();
                    KuvaSkooriKahe(skoor1, skoor2);
                }

                Thread.Sleep(seaded.KiirusMS);
            }

            Console.WriteLine("\nVajuta Enter...");
            Console.ReadLine();
        }

        private void KuvaSkooriKahe(int s1, int s2)
        {
            // Mängija 1 skoor vasakul
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(2, 0);
            Console.Write($" M1: {s1} ");

            // Mängija 2 skoor paremal
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(seaded.Laius - 12, 0);
            Console.Write($" M2: {s2} ");

            Console.ResetColor();
        }

        private void KuvaJuhised()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            int keskX = seaded.Laius / 2 - 10;
            Console.SetCursorPosition(keskX, 0);
            Console.Write(" M1:nooled  M2:WASD ");
            Console.ResetColor();
        }
    }

    public class MiiniväljRežiim
    {
        private Manguseaded seaded;
        private const int VõiduSkoor = 150;
        private const int PommiIntervall = 3000;

        public MiiniväljRežiim(Manguseaded seaded)
        {
            this.seaded = seaded;
        }

        public void Käivita()
        {
            Kaart kaart = new Kaart(seaded.Laius, seaded.Kõrgus);
            Uss uss = new Uss(seaded.Laius / 2, seaded.Kõrgus / 2, 3, Suund.Paremale, ConsoleColor.Green);

            var keelatud = new List<Punkt>(uss.HangiKeha());
            Toit toit = new Toit(seaded.Laius, seaded.Kõrgus, keelatud);

            var pommid = new List<Pomm>();
            int skoor = 0;

            kaart.Joonista();
            toit.Joonista();
            KuvaSkoor(skoor);
            KuvaProgressiriba(skoor);

            bool mängLäbi = false;
            bool võitis = false;

            // Aeg viimase pommi ilmumisest
            DateTime viimanePoмm = DateTime.Now;

            while (!mängLäbi)
            {
                // Sisend
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo klahv = Console.ReadKey(true);
                    if (klahv.Key == ConsoleKey.UpArrow && uss.PraeguneSuund != Suund.Alla) uss.PraeguneSuund = Suund.Üles;
                    else if (klahv.Key == ConsoleKey.DownArrow && uss.PraeguneSuund != Suund.Üles) uss.PraeguneSuund = Suund.Alla;
                    else if (klahv.Key == ConsoleKey.LeftArrow && uss.PraeguneSuund != Suund.Paremale) uss.PraeguneSuund = Suund.Vasakule;
                    else if (klahv.Key == ConsoleKey.RightArrow && uss.PraeguneSuund != Suund.Vasakule) uss.PraeguneSuund = Suund.Paremale;
                    else if (klahv.Key == ConsoleKey.Escape) { mängLäbi = true; continue; }
                }

                if ((DateTime.Now - viimanePoмm).TotalMilliseconds >= PommiIntervall)
                {

                    var keelatudNüüd = new List<Punkt>(uss.HangiKeha());
                    keelatudNüüd.Add(toit.Asukoht);
                    foreach (var p in pommid) keelatudNüüd.Add(p.Asukoht);

                    var uusPomm = new Pomm(seaded.Laius, seaded.Kõrgus, keelatudNüüd);
                    pommid.Add(uusPomm);
                    uusPomm.Joonista();
                    viimanePoмm = DateTime.Now;
                }

                uss.Liigu();
                toit.Joonista();


                foreach (var p in pommid) p.Joonista();

                Punkt pea = uss.HangiPea();

                // Sein või iseenda keha
                if (pea.X <= 0 || pea.X >= seaded.Laius - 1 || pea.Y <= 0 || pea.Y >= seaded.Kõrgus - 1
                    || uss.PõrkasSiseendaga())
                {
                    mängLäbi = true;
                    continue;
                }

                // Pomm?
                Pomm tabatudPomm = pommid.Find(p => p.PõrkasUssiga(pea));
                if (tabatudPomm != null)
                {
                    mängLäbi = true;
                    continue;
                }

                // Toit?
                if ((pea.X == toit.Asukoht.X || pea.X == toit.Asukoht.X + 1) && pea.Y == toit.Asukoht.Y)
                {
                    uss.Kasva();
                    skoor += 10;

                    var keelatudNüüd = new List<Punkt>(uss.HangiKeha());
                    foreach (var p in pommid) keelatudNüüd.Add(p.Asukoht);

                    toit.LooUusToit(keelatudNüüd);
                    toit.Joonista();
                    KuvaSkoor(skoor);
                    KuvaProgressiriba(skoor);

                    // Võit!
                    if (skoor >= VõiduSkoor)
                    {
                        mängLäbi = true;
                        võitis = true;
                        continue;
                    }
                }

                Thread.Sleep(seaded.KiirusMS);
            }


            Console.Clear();
            Console.SetCursorPosition(0, 0);

            if (võitis)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"🎉 Sa võitsid! Skoor: {skoor}/{VõiduSkoor}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"💥 Mäng läbi! Skoor: {skoor}");
                Console.ResetColor();
            }

            Console.Write("Sisesta oma nimi: ");
            string nimi = Console.ReadLine() ?? "Mängija";
            if (string.IsNullOrWhiteSpace(nimi)) nimi = "Mängija";
            Edetabel.Salvesta(nimi, skoor);
            Edetabel.KuvaEdetabel();
        }

        private void KuvaSkoor(int skoor)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(2, 0);
            Console.Write($" 🍎{skoor}/{VõiduSkoor} ");
            Console.ResetColor();
        }

        private void KuvaProgressiriba(int skoor)
        {
            int ribaLaius = 15;
            int täidetud = (int)((double)skoor / VõiduSkoor * ribaLaius);
            täidetud = Math.Min(täidetud, ribaLaius);

            string riba = "[" + new string('█', täidetud) + new string('░', ribaLaius - täidetud) + "]";

            Console.ForegroundColor = skoor >= VõiduSkoor * 0.75 ? ConsoleColor.Green : ConsoleColor.DarkYellow;
            Console.SetCursorPosition(seaded.Laius - ribaLaius - 5, 0);
            Console.Write(riba);
            Console.ResetColor();
        }
    }
}