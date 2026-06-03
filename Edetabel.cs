using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MaduUss_Puhtejev
{
    public static class Edetabel
    {
        private static string failiTee = "punktid.txt";

        public static void Salvesta(string nimi, int skoor)
        {
            try
            {
                // VIGA FIX: Kaitseme erimärkide eest nimedes (semikoolon lõhub parsimise)
                string ohutNimi = nimi.Replace(";", "").Replace("\n", "");
                File.AppendAllLines(failiTee, new[] { $"{ohutNimi};{skoor}" });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Punkti salvestamine ebaõnnestus: {e.Message}");
            }
        }

        public static void KuvaEdetabel()
        {
            Console.WriteLine("\n--- TOP 5 EDETABEL ---");

            if (!File.Exists(failiTee))
            {
                Console.WriteLine("(edetabel on tühi)");
                return;
            }

            try
            {
                var skoorid = File.ReadAllLines(failiTee)
                    .Select(rida => rida.Split(';'))
                    // VIGA FIX: Kontrollime, et real on täpselt 2 osa ja skoor on number
                    .Where(osad => osad.Length == 2 && int.TryParse(osad[1], out _))
                    .Select(osad => new { Nimi = osad[0], Punktid = int.Parse(osad[1]) })
                    .OrderByDescending(x => x.Punktid)
                    .Take(5);

                int koht = 1;
                foreach (var s in skoorid)
                    Console.WriteLine($"{koht++}. {s.Nimi}: {s.Punktid}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Edetabeli lugemine ebaõnnestus: {e.Message}");
            }
        }
    }
}