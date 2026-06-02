using System;
using System.Threading;

namespace MaduUss_Puhtejev
{
    class Program
    {
        static void Main()
        {
            // Seadistame konsooli akna
            Console.WindowHeight = 20;
            Console.WindowWidth = 50;
            Console.CursorVisible = false;

            // Loome objektid
            Uss uss = new Uss(10, 10, 3);
            Toit toit = new Toit(Console.WindowWidth, Console.WindowHeight);

            // MÄNGU TSÜKKEL
            while (true)
            {
                // 1. Sisendi lugemine (kas kasutaja vajutas nooleklahvi?)
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo klahv = Console.ReadKey(true);
                    // Muudame suunda, aga väldime tagurdamist
                    if (klahv.Key == ConsoleKey.UpArrow && uss.PraeguneSuund != Suund.Alla)
                        uss.PraeguneSuund = Suund.Üles;
                    else if (klahv.Key == ConsoleKey.DownArrow && uss.PraeguneSuund != Suund.Üles)
                        uss.PraeguneSuund = Suund.Alla;
                    else if (klahv.Key == ConsoleKey.LeftArrow && uss.PraeguneSuund != Suund.Paremale)
                        uss.PraeguneSuund = Suund.Vasakule;
                    else if (klahv.Key == ConsoleKey.RightArrow && uss.PraeguneSuund != Suund.Vasakule)
                        uss.PraeguneSuund = Suund.Paremale;
                }

                // 2. Objektide uuendamine
                uss.Liigu();
                Punkt pea = uss.HangiPea();

                // 3. Kas uss sõi toidu ära?
                if (pea.X == toit.Asukoht.X && pea.Y == toit.Asukoht.Y)
                {
                    uss.Kasva();
                    toit.LooUusToit();
                }

                // 4. Kas uss põrkas vastu seina? (Lihtne mängu lõpu kontroll)
                if (pea.X <= 0 || pea.X >= Console.WindowWidth - 1 ||
                    pea.Y <= 0 || pea.Y >= Console.WindowHeight - 1)
                {
                    Console.SetCursorPosition(15, 10);
                    Console.WriteLine("MÄNG LÄBI!");
                    break; // Murrame tsüklist välja
                }

                // Mängu kiirus (mida väiksem number, seda kiirem uss)
                Thread.Sleep(150);
            }

            Console.ReadLine();
        }
    }
}
