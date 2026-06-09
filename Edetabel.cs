namespace MaduUss_Puhtejev
{
    public static class Edetabel
    {
        private static string failiTee = "skoorid.txt";

        private static readonly string[] Medalid = { "🥇", "🥈", "🥉", "  ", "  " };

        public static void Salvesta(string nimi, int skoor)
        {
            try
            {
                string ohutNimi = nimi.Replace(";", "").Replace("\n", "");
                File.AppendAllLines(failiTee, new[] { $"{ohutNimi};{skoor}" });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Skoori salvestamine ebaõnnestus: {e.Message}");
            }
        }

        public static void KuvaEdetabel()
        {
            Console.Clear();

            string pealkiri = "  🏆  TOP 10 EDETABEL  🏆  ";
            string piir = new string('═', 36);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"╔{piir}╗");
            Console.WriteLine($"║{pealkiri.PadLeft(28).PadRight(36)}║");
            Console.WriteLine($"╠{piir}╣");
            Console.ResetColor();

            if (!File.Exists(failiTee))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"║{"(edetabel on tühi)".PadLeft(27).PadRight(36)}║");
                Console.ResetColor();
            }
            else
            {
                try
                {
                    var skoorid = File.ReadAllLines(failiTee)
                        .Select(rida => rida.Split(';'))
                        .Where(osad => osad.Length == 2 && int.TryParse(osad[1], out _))
                        .Select(osad => new { Nimi = osad[0], Punktid = int.Parse(osad[1]) })
                        .OrderByDescending(x => x.Punktid)
                        .Take(10)
                        .ToList();

                    if (skoorid.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"║{"(edetabel on tühi)".PadLeft(27).PadRight(36)}║");
                        Console.ResetColor();
                    }
                    else
                    {
                        for (int i = 0; i < skoorid.Count; i++)
                        {
                            string medal = (i < 3) ? Medalid[i] : $"{i + 1,2}.";

                            string nimi = skoorid[i].Nimi.Length > 18
                                            ? skoorid[i].Nimi[..18]
                                            : skoorid[i].Nimi;
                            string punktid = skoorid[i].Punktid.ToString();
                            int tühikud = 30 - nimi.Length - punktid.Length;
                            string rida = $" {medal} {nimi}{new string('.', Math.Max(1, tühikud))}{punktid} ";

                            Console.ForegroundColor = i == 0 ? ConsoleColor.Yellow
                                                    : i == 1 ? ConsoleColor.Gray
                                                    : i == 2 ? ConsoleColor.DarkYellow
                                                    : ConsoleColor.White;
                            Console.WriteLine($"║{rida.PadRight(36)}║");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.ResetColor();
                    Console.WriteLine($"║ Viga: {e.Message,-29}║");
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"╚{piir}╝");
            Console.ResetColor();
        }
    }
}