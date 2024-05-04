using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        string sciezkaPlikuZrodlowego = "plik1.txt";
        string sciezkaPlikuDocelowego = "plik2.txt";
        long rozmiarPlikuMB = 300;

        GenerujPlik(sciezkaPlikuZrodlowego, rozmiarPlikuMB);

        Console.WriteLine("Kopiowanie pliku przy użyciu .NET Framework...");
        MierzWydajnosc(() => KopiujPlikZaPomocaFrameworkaDotNet(sciezkaPlikuZrodlowego, sciezkaPlikuDocelowego));

        Console.WriteLine("Kopiowanie pliku przy użyciu .NET Core...");
        MierzWydajnosc(() => KopiujPlikZaPomocaDotNetCore(sciezkaPlikuZrodlowego, sciezkaPlikuDocelowego));

        File.Delete(sciezkaPlikuZrodlowego);
        File.Delete(sciezkaPlikuDocelowego);
    }

    static void GenerujPlik(string sciezkaPliku, long rozmiarPlikuMB)
    {
        byte[] bufor = new byte[1024 * 1024];
        Random losowa = new Random();

        using (FileStream fs = new FileStream(sciezkaPliku, FileMode.Create, FileAccess.Write))
        {
            for (long i = 0; i < rozmiarPlikuMB; i++)
            {
                losowa.NextBytes(bufor);
                fs.Write(bufor, 0, bufor.Length);
            }
        }
    }

    static void KopiujPlikZaPomocaFrameworkaDotNet(string sciezkaPlikuZrodlowego, string sciezkaPlikuDocelowego)
    {
        File.Copy(sciezkaPlikuZrodlowego, sciezkaPlikuDocelowego);
    }

    static void KopiujPlikZaPomocaDotNetCore(string sciezkaPlikuZrodlowego, string sciezkaPlikuDocelowego)
    {
        using (FileStream strumienZrodlowy = new FileStream(sciezkaPlikuZrodlowego, FileMode.Open, FileAccess.Read))
        using (FileStream strumienDocelowy = new FileStream(sciezkaPlikuDocelowego, FileMode.Create, FileAccess.Write))
        {
            byte[] bufor = new byte[8192];
            int iloscPrzeczytanychBajtow;

            while ((iloscPrzeczytanychBajtow = strumienZrodlowy.Read(bufor, 0, bufor.Length)) > 0)
            {
                strumienDocelowy.Write(bufor, 0, iloscPrzeczytanychBajtow);
            }
        }
    }

    static void MierzWydajnosc(Action akcja)
    {
        Stopwatch stoper = Stopwatch.StartNew();
        akcja();
        stoper.Stop();
        Console.WriteLine("Czas wykonania: " + stoper.Elapsed);
    }
}
