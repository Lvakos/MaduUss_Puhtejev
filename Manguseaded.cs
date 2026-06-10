namespace MaduUss_Puhtejev
{
    public class Manguseaded
    {
        public int Laius { get; set; }
        public int Kõrgus { get; set; }
        public int KiirusMS { get; set; }

        public int VõiduSkoor { get; set; } // Miinivälja võiduskoor
        public int PommiIntervall { get; set; } // Millisekundites

        public Manguseaded(int tase)
        {
            switch (tase)
            {
                case 1: KiirusMS = 200; Laius = 60; Kõrgus = 25; VõiduSkoor = 100; PommiIntervall = 4000; break;
                case 2: KiirusMS = 100; Laius = 60; Kõrgus = 25; VõiduSkoor = 150; PommiIntervall = 3000; break;
                case 3: KiirusMS = 50; Laius = 60; Kõrgus = 25; VõiduSkoor = 200; PommiIntervall = 2000; break;
                default: KiirusMS = 200; Laius = 60; Kõrgus = 25; VõiduSkoor = 100; PommiIntervall = 4000; break;
            }
        }
    }
}