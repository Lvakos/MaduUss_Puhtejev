using System;

namespace MaduUss_Puhtejev
{
    public enum Suund
    {
        Üles, Alla, Vasakule, Paremale
    }

    public class Punkt
    {
        public int X { get; set; }
        public int Y { get; set; }
        // Sümbol asendatud stringiga — emoji võtab 2 tühikut laiust
        public string Sümbol { get; set; }

        public Punkt(int x, int y, string sümbol)
        {
            X = x;
            Y = y;
            Sümbol = sümbol;
        }

        public void Joonista()
        {
            if (X >= 0 && X < Console.WindowWidth && Y >= 0 && Y < Console.WindowHeight)
            {
                Console.SetCursorPosition(X, Y);
                Console.Write(Sümbol);
            }
        }

        public void Kustuta()
        {
            if (X >= 0 && X < Console.WindowWidth && Y >= 0 && Y < Console.WindowHeight)
            {
                Console.SetCursorPosition(X, Y);
                // Emoji on 2 tühiku lai — kustutame mõlemad
                Console.Write("  ");
            }
        }
    }
}