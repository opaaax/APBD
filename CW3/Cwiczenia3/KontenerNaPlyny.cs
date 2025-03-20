namespace ConsoleApp1;

public class KontenerNaPlyny(
    double masaLadunku,
    double wysokosc,
    double wagaWlasna,
    double glebokosc,
    double maxLadownosc
)
    : Kontener(masaLadunku, wysokosc, wagaWlasna, glebokosc, maxLadownosc, "L"), IHazardNotifier
{
    public double MasaNiebezpiecznegoLadunku { get; set; }
    public void Notify()
    {
        Console.WriteLine("Niebezpieczna sytuacja!! Kontener: " + NrSer);
    }

    public override void Zaladuj(double masa)
    {
        var potentialMasa = MasaLadunku + masa;
        if (potentialMasa > MaxLadownosc)
        {
            throw new OverfillException();
        }

        if (potentialMasa > (MasaNiebezpiecznegoLadunku > 0
                ? MasaNiebezpiecznegoLadunku / 2
                : MasaNiebezpiecznegoLadunku * 0.9))
        {
            Notify();
        }
        else
        {
            MasaLadunku += masa;
        }
    }
}