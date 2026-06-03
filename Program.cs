using System;
using System.Threading;

namespace MaduUss_Puhtejev
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Vali raskus (1 - Lihtne, 2 - Keskmine, 3 - Raske): ");
                if (!int.TryParse(Console.ReadLine(), out int tase) || tase < 1 || tase > 3)
                    tase = 1;

                Manguseaded seaded = new Manguseaded(tase);

                try
                {
                    Console.SetWindowSize(seaded.Laius, seaded.Kõrgus);
                    Console.SetBufferSize(seaded.Laius, seaded.Kõrgus);
                }
                catch { }

                Kaart kaart = new Kaart(seaded.Laius, seaded.Kõrgus);
                Uss uss = new Uss(seaded.Laius / 2, seaded.Kõrgus / 2, 3);
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
                        if (klahv.Key == ConsoleKey.UpArrow && uss.PraeguneSuund != Suund.Alla)
                            uss.PraeguneSuund = Suund.Üles;
                        else if (klahv.Key == ConsoleKey.DownArrow && uss.PraeguneSuund != Suund.Üles)
                            uss.PraeguneSuund = Suund.Alla;
                        else if (klahv.Key == ConsoleKey.LeftArrow && uss.PraeguneSuund != Suund.Paremale)
                            uss.PraeguneSuund = Suund.Vasakule;
                        else if (klahv.Key == ConsoleKey.RightArrow && uss.PraeguneSuund != Suund.Vasakule)
                            uss.PraeguneSuund = Suund.Paremale;
                        else if (klahv.Key == ConsoleKey.Escape)
                        {
                            mängLäbi = true;
                            continue;
                        }
                    }

                    uss.Liigu();
                    Punkt pea = uss.HangiPea();

                    if (pea.X <= 0 || pea.X >= seaded.Laius - 1 ||
                        pea.Y <= 0 || pea.Y >= seaded.Kõrgus - 1)
                    {
                        mängLäbi = true;
                        continue;
                    }

                    if (uss.PõrkasSiseendaga())
                    {
                        mängLäbi = true;
                        continue;
                    }

                    // Kontrollime toiduga kokkupuudet — arvestame emoji 2-veergu laiust
                    if ((pea.X == toit.Asukoht.X || pea.X == toit.Asukoht.X + 1) &&
                         pea.Y == toit.Asukoht.Y)
                    {
                        uss.Kasva();
                        skoor += 1;
                        toit.LooUusToit(uss.HangiKeha());
                        toit.Joonista();
                        KuvaSkoor(skoor);
                    }

                    Thread.Sleep(seaded.KiirusMS);
                }

                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.Write($"Mäng läbi! Sinu punktid: {skoor}. Sisesta oma nimi: ");
                string nimi = Console.ReadLine() ?? "Mängija";
                if (string.IsNullOrWhiteSpace(nimi)) nimi = "Mängija";

                Edetabel.Salvesta(nimi, skoor);
                Edetabel.KuvaEdetabel();

                Console.WriteLine("\nUuesti mängida? (J/N)");
                string vastus = Console.ReadLine()?.Trim().ToUpper();
                if (vastus != "J") break;
            }
        }

        static void KuvaSkoor(int skoor)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(2, 0);
            Console.Write($" 🍎 Punktid: {skoor} ");
            Console.ResetColor();
        }
    }
}