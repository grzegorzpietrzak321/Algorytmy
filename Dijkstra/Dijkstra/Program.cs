using System;

namespace Dijkstra
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Przykładowa implementacja algorytmu Dijkstry");

            //przygotowanie danych
            //utworzenie tablicy
            int rozmiar_tablicy = 5; //ilosc wierzchołków grafu
            var tablica = new int[rozmiar_tablicy, rozmiar_tablicy];

            //wypełnienie tablicy

            for (int i = 0; i < rozmiar_tablicy; i++)
                for (int j = 0; j < rozmiar_tablicy; j++)
                {
                    tablica[i, j] = Dijkstra.INFINITI;
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
            var dijkstra = new Dijkstra(tablica);

            //pobranie ścieżki między wierzchołkami podanymi w parametrze metody
            var test = dijkstra.GetPath(0, 4);

            foreach (var item in test)
            {
                Console.WriteLine(item.ToString());
            };

            Console.ReadLine();
        }

        class Dijkstra
        {
            //stała - nieskończoność
            public const int INFINITI = int.MaxValue;
            public int[,] Tablica { get; set; }
            public Dijkstra(int[,] tablica)
            {
                Tablica = tablica;
            }


            public int[] GetPath(int start, int meta)
            {
                int rozmiarTablicy = Tablica.GetLength(0);
                int[] odleglosc = new int[rozmiarTablicy];
                int[] poprzednik = new int[rozmiarTablicy];
                int[] wezel = new int[rozmiarTablicy];

                for (int i = 0; i < odleglosc.Length; i++)
                {
                    odleglosc[i] = poprzednik[i] = INFINITI;
                    wezel[i] = i;
                }

                odleglosc[start] = 0;

                do
                {
                    int najmniejszy = wezel[0];
                    int najmniejszyIndex = 0;
                    for (int i = 0; i < rozmiarTablicy; i++)
                    {
                        if (odleglosc[wezel[i]] < odleglosc[najmniejszy])
                        {
                            najmniejszy = wezel[i];
                            najmniejszyIndex = i;
                        }
                    }

                    rozmiarTablicy--;
                    wezel[najmniejszyIndex] = wezel[rozmiarTablicy];

                    if (odleglosc[najmniejszy] == INFINITI || najmniejszy == meta)
                    {
                        break;
                    }

                    for (int i = 0; i < rozmiarTablicy; i++)
                    {
                        int j = wezel[i];
                        int nowaOdleglosc = odleglosc[najmniejszy] + Tablica[najmniejszy, j];
                        if (nowaOdleglosc < odleglosc[j])
                        {
                            odleglosc[j] = nowaOdleglosc;
                            poprzednik[j] = najmniejszy;
                        }
                    }
                }
                while (rozmiarTablicy > 0);

                return RekonstruktorSciezki(poprzednik, start, meta);
            }

            public int[] RekonstruktorSciezki(int[] prev, int start, int meta)
            {

                int[] ret = new int[prev.Length];
                int currentNode = 0;
                ret[currentNode] = meta;
                while (ret[currentNode] != INFINITI && ret[currentNode] != start)
                {
                    ret[currentNode + 1] = prev[ret[currentNode]];
                    currentNode++;
                }
                if (ret[currentNode] != start)
                    return null;
                int[] reversed = new int[currentNode + 1];
                for (int i = currentNode; i >= 0; i--)
                    reversed[currentNode - i] = ret[i];
                return reversed;
            }
        }
    }
}
