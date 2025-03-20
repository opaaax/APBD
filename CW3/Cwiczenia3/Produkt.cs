namespace ConsoleApp1;

public class Produkt(string nazwa, double wymaganaTemperatura)
{
    public string Nazwa { get; set; } = nazwa;
    public double WymaganaTemperatura { get; set; } = wymaganaTemperatura;
}