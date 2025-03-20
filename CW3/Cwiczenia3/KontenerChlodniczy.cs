namespace ConsoleApp1;

public class KontenerChlodniczy : Kontener
{
    public Produkt RodzajProduktu { get; set; }
    public double Temperatura { get; set; }

    public KontenerChlodniczy(double masaLadunku, double wysokosc, double wagaWlasna, double glebokosc, double maxLadownosc, string literaNrSer, Produkt rodzajProduktu, double temperatura) : base(masaLadunku, wysokosc, wagaWlasna, glebokosc, maxLadownosc, literaNrSer)
    {
        RodzajProduktu = rodzajProduktu;
        Temperatura = temperatura;

        if (temperatura < RodzajProduktu.WymaganaTemperatura)
        {
            throw new TooLowTempException();
        }
        
    }
}