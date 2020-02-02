using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloydWarshall
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Przykładowa implementacja algorytmu Floyda-Warshalla");

            //przygotowanie danych
            //utworzenie tablicy
            int rozmiar_tablicy = 5; //ilosc wierzchołków grafu
            var tablica = new int[rozmiar_tablicy, rozmiar_tablicy]; 

            //wypełnienie tablicy

            for (int i = 0; i < rozmiar_tablicy; i++)
                for (int j = 0; j < rozmiar_tablicy; j++)
                {
                    tablica[i, j] = Floyd.INFINITI;
                }

            //uzupełnienie macierzy sąsiedztwa
            //tablica zaharkodowana, najlepiej by było przekazywać macierz jako parametr
            tablica[0, 1] = 2;
            tablica[0, 2] = 3;
            tablica[1, 3] = 3;
            tablica[1, 4] = 7;
            tablica[2, 3] = 4;
            tablica[3, 4] = 1;

            //utworzenie instancji klasy
            var floyd = new Floyd();
            //wyliczenie odległości między wierzchołkami
            floyd.Evaluate(rozmiar_tablicy, tablica);

            //pobranie ścieżki między wierzchołkami podanymi w parametrze metody
            var test = floyd.GetPath(0, 4);

            foreach (var item in test)
            {
                Console.WriteLine(item.ToString());
            };

            Console.ReadLine();
        }
    }

    class Floyd
    {
        //stała - nieskończoność
        public const int INFINITI = int.MaxValue;

        //tymczasowa zmienna, podczas iteracji przechowuje wartosc obliczaną wg algorytmu
        int[,] nastepnik;

        //ścieżka 
        int[,] sciezka;

        //wyliczenie odległości między wierzchołkami w grafie
        public void Evaluate(int rozmiar_tablicy, int[,] sciezka)
        {
            this.sciezka = sciezka;
            nastepnik = new int[rozmiar_tablicy, rozmiar_tablicy];
            for (int i = 0; i < rozmiar_tablicy; i++)
                for (int j = 0; j < rozmiar_tablicy; j++)
                    if (sciezka[i, j] == INFINITI)
                        nastepnik[i, j] = INFINITI;
                    else
                        nastepnik[i, j] = i;
            for (int k = 0; k < rozmiar_tablicy; k++)
                for (int i = 0; i < rozmiar_tablicy; i++)
                    for (int j = 0; j < rozmiar_tablicy; j++)
                        if (sciezka[i, k] != INFINITI && sciezka[k, j] != INFINITI &&
                sciezka[i, k] + sciezka[k, j] < sciezka[i, j])
                        {
                            sciezka[i, j] = sciezka[i, k] + sciezka[k, j];
                            nastepnik[i, j] = k;
                        }

            //sprawdzenie czy nie ma ujemnych ścieżek
            for (int i = 0; i < rozmiar_tablicy; i++)
                if (sciezka[i, i] < 0)
                    throw new ArgumentException("Negative cycles");
        }

        //pobranie najkrótszej ścieżki 
        public List<int> GetPath(int start, int meta)
        {
            var path = new List<int>();

            if (nastepnik == null)
                return null;
            if (nastepnik[start, meta] == INFINITI)
                return new List<int>();
            if (start == meta)
                return new List<int>() { start };
            getPathRekonstruktor(path, start, meta);
            path.Add(meta);

            return path;
        }

        //metoda rekursywna do pobierania ścieżki między węzłami
        private void getPathRekonstruktor(List<int> sciezka, int start, int meta)
        {
            int posredni = nastepnik[start, meta];
            if (posredni == start)
            {
                sciezka.Add(start);
                return;
            }
            getPathRekonstruktor(sciezka, start, posredni);
            getPathRekonstruktor(sciezka, posredni, meta);
        }
    }
}
