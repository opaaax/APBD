using ConsoleApp1;

Kontener kontener = new KontenerNaPlyny(0, 5, 12, 5, 0, 20);
kontener.ZaladujKontener(10);
Console.WriteLine(kontener.NrSeryjny);

Kontener kontener2 = new KontenerNaPlyny(0, 5, 12, 5, 0, 20);
kontener.ZaladujKontener(10);
Console.WriteLine(kontener2.NrSeryjny);