namespace ConsoleApp1;

public class KontenerNaGaz(
    double masaLadunku,
    double wysokosc,
    double wagaWlasna,
    double glebokosc,
    double maxLadownosc
)
    : Kontener(masaLadunku, wysokosc, wagaWlasna, glebokosc, maxLadownosc, "G"), IHazardNotifier
{
    public double Cisnienie { get; set; }

    public override void OproznijLadunek()
    {
        MasaLadunku = MasaLadunku * 0.05;
    }
    
    public void Notify()
    {
        Console.WriteLine("Niebezpieczna sytuacja!! Kontener: " + NrSer);
    }
    
}