using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.DataFormats;

namespace SiatkaTroj
{
    public partial class Form1 : Form
    {
        private List<PunktKontrolny> punktyKontrolne;
        private List<Trojkat> siatkaTrojkatow;
        private int alpha = 0;
        private int beta = 0;
        private int resolution = 12;
        private bool fillMode = false;
        private int rozmiarPunktu = 5;
        private Vector3 lightPosition = new Vector3(0, 2, 0); // Pocz�tkowa pozycja �wiat�a



        // Parametry o�wietlenia
        private float kd = 0.5f; // wsp�czynnik rozproszony
        private float ks = 0.5f; // wsp�czynnik zwierciadlany
        private int m = 20; // wsp�czynnik po�ysku (zwierciadlany)
        private Vector3 IL = new Vector3(2, 2, 2);
        private Vector3 IO = new Vector3(255, 255, 255); // kolor obiektu (wybierany z menu)
        private bool useTexture = false; // Flaga: true - tekstura, false - sta�y kolor
        private Bitmap textureBitmap; // Przechowuje wczytan� tekstur�
        private Bitmap NormalMap;

        private float angle = 0; // K�t, kt�ry b�dzie kontrolowa� pozycj� punktu na okr�gu
        private float radius = 50; // Promie� okr�gu, po kt�rym porusza si� punkt �wietlny
        private System.Windows.Forms.Timer lightMovementTimer; // Timer, kt�ry b�dzie odpowiada� za ruch �wiat�a


        public Form1()
        {
            InitializeComponent();

            // Wczytanie punkt�w kontrolnych
            punktyKontrolne = WczytajPunktyKontrolne("sciezka_do_pliku.txt");

            // Generowanie siatki na podstawie punkt�w kontrolnych
            siatkaTrojkatow = GenerujSiatkeTrojkatow(punktyKontrolne, resolution);

            // Pod��czenie zdarze�
            pictureBoxCanvas.Paint += pictureBoxCanvas_Paint;

            // Inicjalizacja i uruchomienie timera do poruszania �wiat�em
            lightMovementTimer = new System.Windows.Forms.Timer();
            lightMovementTimer.Interval = 50; // co 50 ms
            lightMovementTimer.Tick += LightMovementTimer_Tick;
            lightMovementTimer.Start();
        }
        private void LightMovementTimer_Tick(object sender, EventArgs e)
        {
            // Zmieniamy pozycj� �wiat�a
            UpdateLightPosition();
            // Od�wie�enie rysunku, aby pokaza� now� pozycj� �wiat�a
            pictureBoxCanvas.Invalidate();
        }


        private void pictureBoxCanvas_Paint(object sender, PaintEventArgs e)
        {
            // Rysowanie siatki tr�jk�t�w
            RysujSiatke(e.Graphics, siatkaTrojkatow);
            if (GlobalVariables.ShowControlPoints)
            {
                RysujPunktyKontrolne(e.Graphics, punktyKontrolne);
            }
        }

        private void UpdateLightPosition()
        {
            // Parametry ruchu �wiat�a po okr�gu
            float radius = 2; // Promie� okr�gu
            float speed = 0.05f; // Szybko�� ruchu (k�t zmiany w czasie)

            // Zwi�ksz k�t, aby �wiat�o porusza�o si� wok� osi Y
            angle += speed; // U�ywamy zmiennej `angle` zamiast statycznej warto�ci
            if (angle > 2 * (float)Math.PI) angle -= 2 * (float)Math.PI;

            // Pozycja �wiat�a na okr�gu wok� osi Y
            lightPosition.X = radius * (float)Math.Cos(angle); // X zmienia si� w zale�no�ci od k�ta
            lightPosition.Y = 2; // Y pozostaje sta�e (na poziomie p�aszczyzny XZ)
            lightPosition.Z = radius * (float)Math.Sin(angle) + GlobalVariables.ZChange; // Z zmienia si� w zale�no�ci od k�ta
        }

        private void RysujPunktyKontrolne(Graphics g, List<PunktKontrolny> punkty)
        {
            Brush brush = new SolidBrush(Color.Red); // Kolor punkt�w kontrolnych

            foreach (var punktKontrolny in punkty)
            {
                // Obracanie i rzutowanie punktu kontrolnego na p�aszczyzn� 2D
                var punkt2D = ProjektujNa2D(ObracajWektor(punktKontrolny.Pozycja, beta, alpha));

                // Rysowanie punktu kontrolnego jako ma�ego k�ka
                g.FillEllipse(brush, punkt2D.X - rozmiarPunktu / 2, punkt2D.Y - rozmiarPunktu / 2, rozmiarPunktu, rozmiarPunktu);
            }
        }

        private void RysujSwiatlo(Graphics g)
        {
            Brush brush = new SolidBrush(Color.Yellow); // Kolor �r�d�a �wiat�a
            int rozmiarPunktu = 20;
            // Rysowanie punktu �r�d�a �wiat�a
            var swiatlo = ProjektujNa2D(ObracajWektor(lightPosition, beta, alpha));
            g.FillEllipse(brush, swiatlo.X - rozmiarPunktu / 2, swiatlo.Y - rozmiarPunktu / 2, rozmiarPunktu, rozmiarPunktu);
        }

        private void RysujSiatke(Graphics g, List<Trojkat> siatka)
        {
            foreach (var trojkat in siatka)
            {
                if (fillMode)
                {
                    // Do uzupelnienia
                    FillPolygonAET()
                }
                else
                {
                    // Rysujemy tylko kraw�dzie tr�jk�ta
                    g.DrawPolygon(...)
                }
            }

            // Rysowanie �wiat�a jako ostatni element, by by�o na wierzchu
            RysujSwiatlo(g);
        }




        private void FillPolygonAET(PointF[] vertices, Trojkat trojkat, Graphics g, Color lightColor, Color objectColor, Vector3 lightPosition, Vector3 viewDirection, float kd, float ks, float m)
        {
            // Znajd� ymin i ymax dla wszystkich wierzcho�k�w
            int ymin = (int)vertices.Min(v => v.Y);
            int ymax = (int)vertices.Max(v => v.Y);

            // Tablica list kraw�dzi (Edge Table)
            List<Edge>[] ET = new List<Edge>[ymax - ymin + 1];
            for (int i = 0; i < ET.Length; i++)
                ET[i] = new List<Edge>();

            // Tworzenie Edge Table (ET)
            for (int i = 0; i < vertices.Length; i++)
            {
                PointF p0 = vertices[i];
                PointF p1 = vertices[(i + 1) % vertices.Length]; // kolejny wierzcho�ek (zamkni�cie wielok�ta)

                if (p0.Y == p1.Y) continue; // pomijanie poziomych kraw�dzi

                // Sortujemy wierzcho�ki kraw�dzi wg Y
                if (p0.Y > p1.Y) (p0, p1) = (p1, p0);

                // Tworzenie nowej kraw�dzi
                Edge edge = new Edge
                {
                    x = p0.X,
                    ymax = (int)p1.Y,
                    dx = (p1.X - p0.X) / (p1.Y - p0.Y) // 1/m
                };

                // Dodajemy kraw�d� do odpowiedniej listy ET
                ET[(int)p0.Y - ymin].Add(edge);
            }

            // Inicjalizacja Active Edge Table (AET)
            List<Edge> AET = new List<Edge>();

            using (Pen pen = new Pen(Color.Black))
            {
                // Przechodzimy przez ka�d� scan-lini� od ymin do ymax
                for (int y = ymin; y <= ymax; y++)
                {
                    // Dodajemy nowe kraw�dzie przecinaj�ce aktualn� scan-lini�
                    AET.AddRange(ET[y - ymin]);

                    // Usuwamy kraw�dzie, kt�re ko�cz� si� na aktualnej scan-linii
                    AET.RemoveAll(e => e.ymax == y);

                    // Sortujemy AET wg warto�ci x
                    AET = AET.OrderBy(e => e.x).ToList();

                    // Wype�nianie pikseli mi�dzy parami kraw�dzi w AET
                    for (int i = 0; i < AET.Count; i += 2)
                    {
                        int xStart = (int)Math.Ceiling(AET[i].x);
                        int xEnd = (int)Math.Floor(AET[i + 1].x);

                        for (int x = xStart; x <= xEnd; x++)
                        {
                            // Interpolacja barycentryczna pozycji i normalnej
                            Vector3 interpolatedPosition = InterpolujPozycje(x, y, trojkat);
                            Vector3 interpolatedNormal = InterpolujNormalna(interpolatedPosition, trojkat);

                            // Obliczanie wektora do �wiat�a
                            Vector3 lightDirection = lightPosition - interpolatedPosition;

                            // Obliczanie koloru za pomoc� modelu Phonga
                            Color pixelColor = CalculateLighting(
                                interpolatedNormal,
                                lightDirection,
                                viewDirection,
                                lightColor,
                                objectColor,
                                kd,
                                ks,
                                (int)m
                            );

                            // Rysowanie piksela
                            g.FillRectangle(new SolidBrush(pixelColor), x, y, 1, 1);
                        }
                    }

                    // Aktualizacja wsp�rz�dnych x dla nowych punkt�w przeci�cia
                    foreach (var edge in AET)
                        edge.x += edge.dx;
                }
            }
        }

        private Vector3 InterpolujPozycje(int x, int y, Trojkat trojkat)
        {
            // Obliczenie barycentrycznych wsp�rz�dnych
            var barycentricCoords = ObliczWspolrzedneBarycentryczne(new Vector3(x, y, 0), trojkat);

            // Interpolacja pozycji w przestrzeni 3D
            Vector3 interpolatedPosition =
                barycentricCoords.X * trojkat.Wierzcholki[0].Pozycja +
                barycentricCoords.Y * trojkat.Wierzcholki[1].Pozycja +
                barycentricCoords.Z * trojkat.Wierzcholki[2].Pozycja;

            return interpolatedPosition;
        }


        public Color CalculateLighting(Vector3 normal, Vector3 lightDirection, Vector3 viewDirection, Color lightColor, Color objectColor, float kd, float ks, int m)
        {
            normal = Vector3.Normalize(normal);
            lightDirection = Vector3.Normalize(lightDirection);
            viewDirection = Vector3.Normalize(viewDirection);

            // Calculate the diffuse and specular components
            float cosNL = Math.Max(0, Vector3.Dot(normal, lightDirection));
            Vector3 R = 2 * cosNL * normal - lightDirection;
            R = Vector3.Normalize(R);
            float cosVR = Math.Max(0, Vector3.Dot(viewDirection, R));
            float specular = (float)Math.Pow(cosVR, m);

            // Interpolating color components (R, G, B)
            float r = (kd * lightColor.R * objectColor.R * cosNL + ks * lightColor.R * objectColor.R * specular) / 255;
            float g = (kd * lightColor.G * objectColor.G * cosNL + ks * lightColor.G * objectColor.G * specular) / 255;
            float b = (kd * lightColor.B * objectColor.B * cosNL + ks * lightColor.B * objectColor.B * specular) / 255;

            // Clamp values to the range [0, 255]
            r = Math.Min(255, Math.Max(0, r * 255));
            g = Math.Min(255, Math.Max(0, g * 255));
            b = Math.Min(255, Math.Max(0, b * 255));

            return Color.FromArgb((int)r, (int)g, (int)b);
        }


        private Color ObliczKolorSwiatla(Vector3 punkt, Vector3 normalna)
        {
            // Wektor do �wiat�a
            Vector3 L = ObliczWektorDoSwiatla(punkt);

            // Wektor obserwatora (sta�y dla uproszczenia)
            Vector3 V = new Vector3(0, 0, 1);

            // Iloczyn skalarny <N, L>
            float cosNL = Math.Max(0, Vector3.Dot(normalna, L));

            // Obliczenie wektora odbicia
            Vector3 R = 2 * cosNL * normalna - L;
            R = Vector3.Normalize(R);

            // Iloczyn skalarny <V, R>
            float cosVR = Math.Max(0, Vector3.Dot(V, R));
            float specular = (float)Math.Pow(cosVR, m);

            // Obliczenie koloru
            float r = (kd * IL.X * IO.X * cosNL + ks * IL.X * IO.X * specular) / 255;
            float g = (kd * IL.Y * IO.Y * cosNL + ks * IL.Y * IO.Y * specular) / 255;
            float b = (kd * IL.Z * IO.Z * cosNL + ks * IL.Z * IO.Z * specular) / 255;

            // Ograniczenie warto�ci do zakresu [0, 255]
            r = Math.Min(255, Math.Max(0, r * 255));
            g = Math.Min(255, Math.Max(0, g * 255));
            b = Math.Min(255, Math.Max(0, b * 255));

            return Color.FromArgb((int)r, (int)g, (int)b);
        }




        private Vector3 InterpolujNormalna(Vector3 punkt, Trojkat trojkat)
        {
            // Obliczenie barycentrycznych wsp�rz�dnych
            var barycentricCoords = ObliczWspolrzedneBarycentryczne(punkt, trojkat);

            // Interpolacja normalnych
            Vector3 interpolatedNormal =
                barycentricCoords.X * trojkat.Wierzcholki[0].Normalna +
                barycentricCoords.Y * trojkat.Wierzcholki[1].Normalna +
                barycentricCoords.Z * trojkat.Wierzcholki[2].Normalna;

            // Normalizacja interpolowanej normalnej
            if (interpolatedNormal.Length() > 0)
            {
                return Vector3.Normalize(interpolatedNormal);
            }
            else
            {
                return new Vector3(0, 0, 1); // Domy�lny wektor
            }
        }





        private Vector3 ObliczWspolrzedneBarycentryczne(Vector3 punkt, Trojkat trojkat)
        {
            Vector3 p0 = trojkat.Wierzcholki[0].Pozycja;
            Vector3 p1 = trojkat.Wierzcholki[1].Pozycja;
            Vector3 p2 = trojkat.Wierzcholki[2].Pozycja;

            Vector3 v0 = p1 - p0;
            Vector3 v1 = p2 - p0;
            Vector3 v2 = punkt - p0;

            float d00 = Vector3.Dot(v0, v0);
            float d01 = Vector3.Dot(v0, v1);
            float d11 = Vector3.Dot(v1, v1);
            float d20 = Vector3.Dot(v2, v0);
            float d21 = Vector3.Dot(v2, v1);

            float denom = d00 * d11 - d01 * d01;
            float v = (d11 * d20 - d01 * d21) / denom;
            float w = (d00 * d21 - d01 * d20) / denom;
            float u = 1.0f - v - w;

            return new Vector3(u, v, w); // barycentryczne wsp�rz�dne
        }



        private Vector3 GetTextureColor(float u, float v)
        {
            if (textureBitmap == null)
                return new Vector3(1, 1, 1); // Domy�lnie bia�y kolor, je�li tekstura nie jest dost�pna

            // Zap�tlenie UV, aby obs�u�y� warto�ci poza zakresem [0, 1]
            u = u % 1;
            if (u < 0) u += 1;
            v = v % 1;
            if (v < 0) v += 1;

            // Przeskalowanie UV na wsp�rz�dne tekstury
            int x = (int)(u * (textureBitmap.Width - 1));
            int y = (int)(v * (textureBitmap.Height - 1));

            // Pobranie koloru pikselu
            Color pixelColor = textureBitmap.GetPixel(x, y);

            // Zwr�cenie koloru jako wektora (R, G, B)
            return new Vector3(
                pixelColor.R,
                pixelColor.G,
                pixelColor.B
            );
        }


        private (float u, float v) ObliczUV(Vector3 punkt)
        {
            // Mapowanie wsp�rz�dnych punktu na zakres 0�1
            float u = (punkt.X + 1) / 2.0f; // Zakres X przesuni�ty z [-1,1] na [0,1]
            float v = (punkt.Z + 1) / 2.0f; // Zakres Z przesuni�ty z [-1,1] na [0,1]

            return (u, v);
        }



        private Vector3 ObliczWektorDoSwiatla(Vector3 punkt)
        {
            Vector3 L = lightPosition - punkt;

            if (L.Length() > 0)
            {
                return Vector3.Normalize(L);
            }
            else
            {
                Debug.WriteLine("Wektor do �wiat�a ma d�ugo�� 0!");
                return new Vector3(0, 0, 1); // Domy�lna warto��
            }
        }


        private float DegreesToRadians(float degrees)
        {
            return (float)(Math.PI / 180.0) * degrees;
        }

        // Reszta funkcji do wczytywania, obrotu, projekcji, itp., pozostaje niezmieniona.
        private Vector3 ObracajWektor(Vector3 punkt, float katX, float katY)
        {
            // Obr�t wok� osi Y (alfa - obr�t poziomy)
            float cosY = (float)Math.Cos(katY * Math.PI / 180);
            float sinY = (float)Math.Sin(katY * Math.PI / 180);
            float x = punkt.X * cosY + punkt.Z * sinY;
            float z = -punkt.X * sinY + punkt.Z * cosY;
            punkt.X = x;
            punkt.Z = z;

            // Obr�t wok� osi X (beta - obr�t pionowy)
            float cosX = (float)Math.Cos(katX * Math.PI / 180);
            float sinX = (float)Math.Sin(katX * Math.PI / 180);
            float y = punkt.Y * cosX - punkt.Z * sinX;
            z = punkt.Y * sinX + punkt.Z * cosX;
            punkt.Y = y;
            punkt.Z = z;

            return punkt;
        }

        private PointF ProjektujNa2D(Vector3 punkt3D)
        {
            float scale = 100;
            float x = punkt3D.X * scale + pictureBoxCanvas.Width / 2;
            float y = -punkt3D.Y * scale + pictureBoxCanvas.Height / 2;

            return new PointF(x, y);
        }


        private List<PunktKontrolny> WczytajPunktyKontrolne(string sciezka)
        {
            var punkty = new List<PunktKontrolny>();

            foreach (var linia in File.ReadLines(sciezka))
            {
                var wspolrzedne = linia.Split(' ')
                    .Select(s => float.Parse(s, CultureInfo.InvariantCulture))
                    .ToArray();

                if (wspolrzedne.Length >= 3)
                {
                    Vector3 pozycja = new Vector3(wspolrzedne[0], wspolrzedne[1], wspolrzedne[2]);
                    punkty.Add(new PunktKontrolny(pozycja));
                }
            }

            // Sprawdzenie, czy punkty kontrolne s� poprawnie zdefiniowane
            if (punkty.Count < 16)
            {
                Debug.WriteLine("B��d: za ma�o punkt�w kontrolnych!");
            }

            // Obliczanie tangent�w i normalnych
            for (int i = 0; i < 4; i++) // Wiersze
            {
                for (int j = 0; j < 4; j++) // Kolumny
                {
                    int index = i * 4 + j;

                    Vector3 tangentU = Vector3.Zero;
                    Vector3 tangentV = Vector3.Zero;

                    if (j < 3)
                    {
                        tangentU = punkty[index + 1].Pozycja - punkty[index].Pozycja;
                    }
                    if (i < 3)
                    {
                        tangentV = punkty[index + 4].Pozycja - punkty[index].Pozycja;
                    }

                    punkty[index].AktualizujTangenty(tangentU, tangentV);
                }
            }

            return punkty;
        }



        private Vector3 ObliczPunktBezier(float u, float v, List<PunktKontrolny> punkty)
        {
            Vector3 punkt = new Vector3();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    float bernsteinU = Bernstein(i, u);
                    float bernsteinV = Bernstein(j, v);

                    punkt += bernsteinU * bernsteinV * punkty[i * 4 + j].Pozycja;
                }
            }

            return punkt;
        }

        private float Bernstein(int i, float t)
        {
            switch (i)
            {
                case 0: return (1 - t) * (1 - t) * (1 - t);
                case 1: return 3 * t * (1 - t) * (1 - t);
                case 2: return 3 * t * t * (1 - t);
                case 3: return t * t * t;
                default: return 0;
            }
        }

        private List<Trojkat> GenerujSiatkeTrojkatow(List<PunktKontrolny> punkty, int resolution)
        {
            var siatka = new List<Trojkat>();
            float step = 1.0f / resolution;

            for (int i = 0; i < resolution; i++)
            {
                for (int j = 0; j < resolution; j++)
                {
                    // Indeksy parametr�w u i v
                    float u0 = i * step;
                    float v0 = j * step;
                    float u1 = (i + 1) * step;
                    float v1 = (j + 1) * step;

                    // Obliczenie punkt�w na powierzchni B�ziera
                    Vector3 p0 = ObliczPunktBezier(u0, v0, punkty);
                    Vector3 p1 = ObliczPunktBezier(u1, v0, punkty);
                    Vector3 p2 = ObliczPunktBezier(u0, v1, punkty);
                    Vector3 p3 = ObliczPunktBezier(u1, v1, punkty);

                    // Tworzenie dw�ch tr�jk�t�w na kwadrat powierzchni
                    Trojkat trojkat1 = new Trojkat(new PunktKontrolny[] {
                new PunktKontrolny(p0), new PunktKontrolny(p1), new PunktKontrolny(p3)
            });
                    Trojkat trojkat2 = new Trojkat(new PunktKontrolny[] {
                new PunktKontrolny(p0), new PunktKontrolny(p3), new PunktKontrolny(p2)
            });

                    siatka.Add(trojkat1);
                    siatka.Add(trojkat2);
                }
            }

            return siatka;
        }




        private void trackBarAlpha_Scroll(object sender, EventArgs e)
        {
            alpha = trackBarAlpha.Value;
            Od�wie�Renderowanie();
        }

        private void trackBarBeta_Scroll(object sender, EventArgs e)
        {
            beta = trackBarBeta.Value;
            Od�wie�Renderowanie();
        }

        private void trackBarResolution_Scroll(object sender, EventArgs e)
        {
            resolution = trackBarResolution.Value;
            siatkaTrojkatow = GenerujSiatkeTrojkatow(punktyKontrolne, resolution);
            Od�wie�Renderowanie();
        }

        private void kdTrackbar_Scroll(object sender, EventArgs e)
        {
            float k = kdTrackbar.Value;
            kd = k / 100;
            Od�wie�Renderowanie();
        }

        private void ksTrackBar_Scroll(object sender, EventArgs e)
        {
            float k = ksTrackBar.Value;
            ks = k / 100;
            Od�wie�Renderowanie();
        }

        private void mTrackBar_Scroll(object sender, EventArgs e)
        {
            m = mTrackBar.Value;
            Od�wie�Renderowanie();
        }

        private void radioButtonMode_CheckedChanged(object sender, EventArgs e)
        {
            fillMode = radioButtonFill.Checked;
            Od�wie�Renderowanie();
        }

        private void Od�wie�Renderowanie()
        {
            pictureBoxCanvas.Invalidate();
        }

        private void ControlCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobalVariables.ShowControlPoints)
            {
                GlobalVariables.ShowControlPoints = false;
            }
            else
            {
                GlobalVariables.ShowControlPoints = true;
            }
            pictureBoxCanvas.Invalidate();
        }

        private void StopLightButton_CheckedChanged(object sender, EventArgs e)
        {
            if (GlobalVariables.StopLight)
            {
                GlobalVariables.StopLight = false;
                lightMovementTimer.Interval = 50;
            }
            else
            {
                GlobalVariables.StopLight = true;
                lightMovementTimer.Interval = 500000000;
            }
            pictureBoxCanvas.Invalidate();
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    Color selectedColor = colorDialog.Color;
                    IL = new Vector3(
                        selectedColor.R / 100,
                        selectedColor.G / 100,
                        selectedColor.B / 100
                    );

                    this.Invalidate(); // Prze�aduj rysunek, aby zastosowa� nowy kolor
                }
            }
        }

        private void ObjectColorButton_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    Color selectedColor = colorDialog.Color;
                    IO = new Vector3(
                        selectedColor.R,
                        selectedColor.G,
                        selectedColor.B
                    );
                    useTexture = false;
                    this.Invalidate(); // Prze�aduj rysunek, aby zastosowa� nowy kolor
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            GlobalVariables.ZChange = ZTrackBar.Value;
        }

        private void TextureButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textureBitmap = new Bitmap(openFileDialog.FileName);

                    // Informacja zwrotna i prze�adowanie rysunku
                    MessageBox.Show("Tekstura za�adowana pomy�lnie!", "Sukces");
                    useTexture = true; // Prze��czenie na tryb tekstury
                    this.Invalidate(); // Prze�adowanie panelu graficznego
                }
            }
        }

        private void NormalVectorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (NormalVectorCheckBox.Checked)
            {
                foreach (var punkt in punktyKontrolne)
                {
                    // Oblicz nowy wektor normalny dla punktu
                    punkt.Normalna = ObliczNormalnaDlaPunktu(punkt);
                }
            }
            else
            {
                // Przywr�� domy�lne normalne (np. liczone z powierzchni)
                foreach (var punkt in punktyKontrolne)
                {
                    punkt.Normalna = WyliczNormalnaZPowierzchni(punkt);
                }
            }

            // Od�wie� rysunek, aby zobaczy� zmiany
            pictureBoxCanvas.Invalidate();
        }

        private Vector3 WyliczNormalnaZPowierzchni(PunktKontrolny punkt)
        {
            // Przyk�ad: ustaw domy�ln� normaln� jako jednostkowy wektor Z
            return Vector3.UnitZ;
        }


        private Vector3 ObliczNormalnaDlaPunktu(PunktKontrolny punkt)
        {
            // Obliczenie UV dla punktu
            (float u, float v) = ObliczUV(punkt.Pozycja);

            // Pobranie wektora normalnego z mapy
            Vector3 normalFromTexture = GetTextureNormal(u, v);

            // Wyznaczenie macierzy przekszta�cenia
            Matrix4x4 transformationMatrix = CalculateTransformationMatrix(punkt.TangentU, punkt.TangentV, punkt.Normalna);

            // Przekszta�cenie wektora normalnego
            Vector3 transformedNormal = Vector3.TransformNormal(normalFromTexture, transformationMatrix);

            // Normalizacja wynikowego wektora
            return Vector3.Normalize(transformedNormal);
        }

        private Vector3 GetTextureNormal(float u, float v)
        {
            if (textureBitmap == null)
                return Vector3.UnitZ; // Domy�lny wektor normalny, je�li brak mapy normalnych

            // Zap�tlenie UV
            u = u % 1;
            if (u < 0) u += 1;
            v = v % 1;
            if (v < 0) v += 1;

            // Przekszta�cenie UV na wsp�rz�dne tekstury
            int x = (int)(u * (textureBitmap.Width - 1));
            int y = (int)(v * (textureBitmap.Height - 1));

            // Pobranie koloru pikselu
            Color pixelColor = textureBitmap.GetPixel(x, y);

            // Przekszta�cenie koloru na zakres <-1, 1> dla wektora normalnego
            float nx = (pixelColor.R / 255f) * 2 - 1;
            float ny = (pixelColor.G / 255f) * 2 - 1;
            float nz = (pixelColor.B / 255f) * 2 - 1;

            // Normalizacja wektora
            Vector3 normal = new Vector3(nx, ny, nz);
            return Vector3.Normalize(normal);
        }

        private Matrix4x4 CalculateTransformationMatrix(Vector3 tangentU, Vector3 tangentV, Vector3 normal)
        {
            return new Matrix4x4(
                tangentU.X, tangentV.X, normal.X, 0,
                tangentU.Y, tangentV.Y, normal.Y, 0,
                tangentU.Z, tangentV.Z, normal.Z, 0,
                0, 0, 0, 1
            );
        }

        private void NormalMapButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Pliki obraz�w (*.png;*.jpg;*.bmp)|*.png;*.jpg;*.bmp",
                Title = "Wybierz map� normalnych"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Wczytanie bitmapy
                    textureBitmap = new Bitmap(openFileDialog.FileName);
                    MessageBox.Show("Mapa normalnych zosta�a za�adowana pomy�lnie.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Wyst�pi� b��d podczas wczytywania mapy: {ex.Message}");
                }
            }
        }
    }
}
