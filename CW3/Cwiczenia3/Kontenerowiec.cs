using System.Collections;

namespace ConsoleApp1;

public class Kontenerowiec
{
    public List<Kontener> Kontenery { get; set; }
    public double MaxPredkosc { get; set; }
    public int MaxLiczbaKontenerowiec { get; set; }
    public double MaxMasaKontenerow { get; set; }

    public Kontenerowiec(double maxPredkosc, int maxLiczbaKontenerowiec, double maxMasaKontenerow)
    {
        Kontenery = new List<Kontener>();
        MaxPredkosc = maxPredkosc;
        MaxLiczbaKontenerowiec = maxLiczbaKontenerowiec;
        MaxMasaKontenerow = maxMasaKontenerow;
    }

    public void ZaladujLadunekDoKontenera(Kontener kontener, double ladunek)
    {
        Kontenery.Find(e => e.Equals(kontener)).Zaladuj(ladunek);
    }

    public void ZaladujKontenerNaStatek(Kontener kontener)
    {
        Kontenery.Add(kontener);
    }

    public void ZaladujKonteneryNaStatek(List<Kontener> kontenerList)
    {
        Kontenery.AddRange(kontenerList);
    }

    public void UsunKontenerZeStatku(Kontener kontener)
    {
        Kontenery.Remove(kontener);
    }

    public void RozladujKontener(Kontener kontener)
    {
        Kontenery.Find(e => e.Equals(kontener)).OproznijLadunek();
    }

    public void ZastapKontener(string Numer, Kontener kontener)
    {
        Kontenery.Remove(Kontenery.Find(e => e.NrSer == Numer));
        Kontenery.Add(kontener);
    }

    public void PrzeniesKontener(Kontener kontener, Kontenerowiec kontenerowiec)
    {
        Kontenery.Remove(kontener);
        kontenerowiec.Kontenery.Add(kontener);
    }

    public static void WypiszInfoOKontenerze(Kontener kontener)
    {
        Console.WriteLine(kontener);
    }

    public void WypiszInfoStatku()
    {
        Console.WriteLine("Max predkosc: " + MaxPredkosc + "\n"
                          + "Max Liczba Kontenerow: " + MaxLiczbaKontenerowiec + "\n" 
                          + "Max masa kontenerow: " + MaxMasaKontenerow + "Kontenery: "
                          );
        Kontenery.ForEach(Console.WriteLine);
    }
    
}