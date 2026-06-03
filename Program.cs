using System;

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

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("╔══════════════════════════╗");
                Console.WriteLine("║       SNAKE  GAME        ║");
                Console.WriteLine("╠══════════════════════════╣");
                Console.ResetColor();
                Console.WriteLine("║  1 - Üks mängija         ║");
                Console.WriteLine("║  2 - 1v1 (kaks mängijat) ║");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("║  3 - 🏆 Edetabel         ║");
                Console.ResetColor();
                Console.WriteLine("║  0 - Välju               ║");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("╚══════════════════════════╝");
                Console.ResetColor();
                Console.Write("\nVali: ");

                if (!int.TryParse(Console.ReadLine(), out int valik)) continue;

                if (valik == 0) break;

                if (valik == 3)
                {
                    Edetabel.KuvaEdetabel();
                    Console.WriteLine("\nVajuta Enter, et tagasi minna...");
                    Console.ReadLine();
                    continue;
                }

                if (valik != 1 && valik != 2) continue;

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

                Console.Clear();

                if (valik == 1)
                    new UksikmängijaRežiim(seaded).Käivita();
                else
                    new KaksMängijatRežiim(seaded).Käivita();

                Console.WriteLine("\nUuesti mängida? (J/N)");
                string vastus = Console.ReadLine()?.Trim().ToUpper();
                if (vastus != "J") break;
            }
        }
    }
}