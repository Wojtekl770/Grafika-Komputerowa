using System.Numerics;

using System.Numerics;

public class PunktKontrolny
{
    // Właściwości punktu kontrolnego
    public Vector3 Pozycja { get; set; }       // Pozycja punktu kontrolnego w przestrzeni 3D
    public Vector3 TangentU { get; set; }      // Wektor styczny w kierunku U
    public Vector3 TangentV { get; set; }      // Wektor styczny w kierunku V
    public Vector3 Normalna { get; set; }      // Wektor normalny w punkcie


    public PunktKontrolny(Vector3 pozycja)
    {
        Pozycja = pozycja;
        TangentU = Vector3.Zero; // Domyślnie zerowe
        TangentV = Vector3.Zero; // Domyślnie zerowe
        Normalna = Vector3.Zero; // Domyślnie zerowe
    }

    public PunktKontrolny(Vector3 pozycja, Vector3 tangentU, Vector3 tangentV)
    {
        Pozycja = pozycja;
        TangentU = tangentU;
        TangentV = tangentV;

        // Obliczenie wektora normalnego jako iloczynu wektorowego TangentU i TangentV
        Vector3 normalna = Vector3.Cross(tangentU, tangentV);

        // Sprawdzenie, czy normalna ma długość różną od zera
        if (normalna.Length() > 0)
        {
            Normalna = Vector3.Normalize(normalna); // Normalizujemy wektor normalny
        }
        else
        {
            Normalna = Vector3.Zero; // Jeśli długość = 0, ustawiamy wektor zerowy
        }
    }


    public void AktualizujTangenty(Vector3 tangentU, Vector3 tangentV)
    {
        TangentU = tangentU;
        TangentV = tangentV;

        // Obliczenie nowego wektora normalnego
        Vector3 normalna = Vector3.Cross(tangentU, tangentV);

        if (normalna.Length() > 0)
        {
            Normalna = Vector3.Normalize(normalna); // Normalizujemy wektor normalny
        }
        else
        {
            Normalna = Vector3.Zero; // Jeśli długość = 0, ustawiamy wektor zerowy
        }
    }

    /// <summary>
    /// Debugowa metoda do wyświetlania szczegółów punktu kontrolnego.
    /// </summary>
    /// <returns>String z opisem punktu kontrolnego</returns>
    public override string ToString()
    {
        return $"Pozycja: {Pozycja}, TangentU: {TangentU}, TangentV: {TangentV}, Normalna: {Normalna}";
    }
}

