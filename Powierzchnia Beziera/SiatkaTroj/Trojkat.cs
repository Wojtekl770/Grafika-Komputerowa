using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SiatkaTroj
{
    public class Trojkat
    {
        public PunktKontrolny[] Wierzcholki { get; set; } = new PunktKontrolny[3];


        public Trojkat(PunktKontrolny[] wierzcholki)
        {
            if (wierzcholki.Length == 3)
            {
                Wierzcholki = wierzcholki;
            }
            else
            {
                throw new ArgumentException("Trojkat wymaga dokładnie 3 wierzchołków.");
            }
        }

        public Vector3 Srodek(Trojkat trojkat)
        {
            // Srodek trójkąta, używamy średniej z wierzchołków
            return (trojkat.Wierzcholki[0].Pozycja + trojkat.Wierzcholki[1].Pozycja + trojkat.Wierzcholki[2].Pozycja) / 3;
        }
    }
}
