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

                Console.WriteLine("╔══════════════════════╗");
                Console.WriteLine("║      SNAKE  GAME     ║");
                Console.WriteLine("╠══════════════════════╣");
                Console.WriteLine("║  1 - Üks mängija     ║");
                Console.WriteLine("║  2 - 1v1 (kaks maja) ║");
                Console.WriteLine("║  0 - Välju           ║");
                Console.WriteLine("╚══════════════════════╝");
                Console.Write("\nVali režiim: ");

                if (!int.TryParse(Console.ReadLine(), out int režiim)) continue;
                if (režiim == 0) break;
                if (režiim != 1 && režiim != 2) continue;

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

                if (režiim == 1)
                {
                    var mäng = new UksikmängijaRežiim(seaded);
                    mäng.Käivita();
                }
                else
                {
                    var mäng = new KaksMängijatRežiim(seaded);
                    mäng.Käivita();
                }

                Console.WriteLine("\nUuesti mängida? (J/N)");
                string vastus = Console.ReadLine()?.Trim().ToUpper();
                if (vastus != "J") break;
            }
        }
    }
}