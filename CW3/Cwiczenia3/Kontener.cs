namespace ConsoleApp1;

public abstract class Kontener
{
    private static int NrSerCounter;
    public double MasaLadunku { set; get; }
    public double Wysokosc { set; get; }
    public double WagaWlasna { set; get; }
    public double Glebokosc { set; get; }
    public double MaxLadownosc { set; get; }
    public string LiteraNrSer { set; get; }
    public string NrSer { set; get; }
    
    public Kontener(double masaLadunku, double wysokosc, double wagaWlasna, double glebokosc, double maxLadownosc, string literaNrSer)
    {
        MasaLadunku = masaLadunku;
        Wysokosc = wysokosc;
        WagaWlasna = wagaWlasna;
        Glebokosc = glebokosc;
        MaxLadownosc = maxLadownosc;
        LiteraNrSer = literaNrSer;
        
        NrSer = "KON-" + literaNrSer + "-" + NrSerCounter;
        NrSerCounter++;
    }

    public virtual void OproznijLadunek()
    {
        MasaLadunku = 0;
    }

    public virtual void Zaladuj(double masa)
    {
        var potentialMasa = MasaLadunku + masa;
        if (potentialMasa > MaxLadownosc)
        {
            throw new OverfillException();
        }
        MasaLadunku += masa;
    }

    public override string ToString()
    {
        return "NrSer: " + NrSer
            + ", Wysokosc: " + Wysokosc
            + ", WagaWlasna: " + WagaWlasna
            + ", Glebokosc: " + Glebokosc
            + ", MaxLadownosc: " + MaxLadownosc;
    }

    
    
}