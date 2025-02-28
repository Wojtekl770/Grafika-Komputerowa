using System;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace RuchObrazka
{
    public partial class Form1 : Form
    {
        private PointF[] controlPoints; // Punkty kontrolne
        private bool isDragging = false;
        private int draggedPointIndex = -1; // Indeks przeci¹ganego punktu
        private Bitmap loadedImage;

        private Timer animationTimer;
        private float t = 0f; // Wspó³czynnik t dla pozycji na krzywej Béziera
        private bool isMoving = false;
        private bool useFiltering = false; // Czy u¿ywaæ filtrowania k¹ta obrotu
        private bool isRotating = false;
        private float currentAngle = 0.0f;
        private float rotationSpeed = 2.0f;
        private PointF RotatingPosition;
        private float RotatingT;



        public Form1()
        {
            InitializeComponent();
            InitializeAnimation();
            loadedImage = new Bitmap("..\\..\\..\\checkerboard2.png");
        }

        private void InitializeAnimation()
        {
            // Inicjalizacja obrazka i timera
            loadedImage = new Bitmap("..\\..\\..\\checkerboard2.png");
            animationTimer = new Timer { Interval = 16 }; // 60 FPS
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (controlPoints == null || controlPoints.Length < 2 || !isMoving)
                return;

            // Aktualizacja wspó³czynnika t dla ruchu wzd³u¿ krzywej
            t += 0.002f;
            if (t > 1f) t = 0f;

            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(panel1.BackColor); // Czyszczenie panelu
            DrawBezierCurve(g);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (controlPoints != null)
            {
                for (int i = 0; i < controlPoints.Length; i++)
                {
                    float dx = e.X - controlPoints[i].X;
                    float dy = e.Y - controlPoints[i].Y;
                    if (Math.Sqrt(dx * dx + dy * dy) <= 10) // Jeœli klikniêto w promieniu 10px
                    {
                        isDragging = true;
                        draggedPointIndex = i;
                        break;
                    }
                }
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && draggedPointIndex != -1)
            {
                controlPoints[draggedPointIndex] = new PointF(e.X, e.Y);
                panel1.Invalidate();
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            draggedPointIndex = -1;
        }

        private PointF[] GenerateControlPoints(int number)
        {
            // Generowanie punktów kontrolnych z równymi odstêpami na osi X
            PointF[] points = new PointF[number];
            float xStep = panel1.Width / (number + 1);
            Random random = new Random();

            for (int i = 0; i < number; i++)
            {
                float x = (i + 1) * xStep;
                float y = random.Next(20, panel1.Height - 20);
                points[i] = new PointF(x, y);
            }

            return points;
        }

        private PointF BezierCurve(PointF[] points, float t)
        {
            // Rekurencyjne obliczanie punktów na krzywej Béziera
            if (points.Length == 1) return points[0];
            var nextPoints = new PointF[points.Length - 1];
            for (int i = 0; i < nextPoints.Length; i++)
                nextPoints[i] = new PointF(
                    (1 - t) * points[i].X + t * points[i + 1].X,
                    (1 - t) * points[i].Y + t * points[i + 1].Y
                );
            return BezierCurve(nextPoints, t);
        }

        private float GetAngle(PointF p1, PointF p2)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            return (float)(Math.Atan2(dy, dx) * (180 / Math.PI));
        }

        private void DrawBezierCurve(Graphics g)
        {
            if (controlPoints == null || controlPoints.Length < 2)
                return;

            // Rysowanie linii ³¹cz¹cych punkty kontrolne
            if (GlobalVariables.PolylineVisible)
            {
                for (int i = 0; i < controlPoints.Length - 1; i++)
                    g.DrawLine(Pens.Cyan, controlPoints[i], controlPoints[i + 1]);
            }

            // Rysowanie punktów kontrolnych
            if (GlobalVariables.ControlVisible)
            {
                foreach (var point in controlPoints)
                    g.FillEllipse(Brushes.Red, point.X - 5, point.Y - 5, 10, 10);
            }

            // Rysowanie krzywej Béziera
            const int steps = 100;
            PointF previousPoint = BezierCurve(controlPoints, 0);
            for (int i = 1; i <= steps; i++)
            {
                float t = i / (float)steps;
                PointF currentPoint = BezierCurve(controlPoints, t);
                g.DrawLine(Pens.Black, previousPoint, currentPoint);
                previousPoint = currentPoint;
            }

            PointF currentPosition = BezierCurve(controlPoints, t);
            PointF nextPosition = BezierCurve(controlPoints, t + 0.01f);

            if (isRotating && loadedImage != null)
            {

                // Inkrementacja k¹ta obrotu
                currentAngle += rotationSpeed;
                if (currentAngle >= 360) currentAngle -= 360;

                double rad = currentAngle * Math.PI / 180.0;

                // Obracanie obrazka
                Bitmap rotatedImage;
                if (!useFiltering)
                {
                    rotatedImage = RotateImageNaive(loadedImage, rad);
                }
                else
                {
                    if (currentAngle > 120 && currentAngle < 240)
                    {
                        rotatedImage = RotateImageWithShear(loadedImage, rad / 2);
                        rotatedImage = RotateImageWithShear(rotatedImage, rad / 2);
                    }
                    else
                    {
                        rotatedImage = RotateImageWithShear(loadedImage, rad);
                    }
                }
                g.DrawImage(rotatedImage, RotatingPosition.X - rotatedImage.Width / 2, RotatingPosition.Y - rotatedImage.Height / 2);
            }
            else if (loadedImage != null && isMoving)
            {
                // Obliczanie k¹ta obrotu
                float angle = GetAngle(currentPosition, nextPosition) + currentAngle;
                if (angle >= 360) angle -= 360;
                Bitmap rotatedImage;
                double rad = angle * Math.PI / 180.0;

                if (!useFiltering)
                {
                    rotatedImage = RotateImageNaive(loadedImage, rad);
                }
                else
                {
                    if (angle > 120 && angle < 240)
                    {
                        rotatedImage = RotateImageWithShear(loadedImage, rad / 2);
                        rotatedImage = RotateImageWithShear(rotatedImage, rad / 2);
                    }
                    else
                    {
                        rotatedImage = RotateImageWithShear(loadedImage, rad);
                    }
                }

                g.DrawImage(rotatedImage, currentPosition.X - rotatedImage.Width / 2, currentPosition.Y - rotatedImage.Height / 2);
            }
        }

        public Bitmap RotateImageNaive(Bitmap src, double rad)
        {
            // Tworzenie odwróconej macierzy R
            double[,] R = {
                    { Math.Cos(rad), Math.Sin(rad) },
                    { -Math.Sin(rad), Math.Cos(rad) }
                        };

            int width = src.Width;
            int height = src.Height;

            // Obliczenie nowych wymiarów obrazka wynikowego
            int newWidth = (int)(Math.Abs(width * R[0, 0]) + Math.Abs(height * R[0, 1]));
            int newHeight = (int)(Math.Abs(width * R[1, 0]) + Math.Abs(height * R[1, 1]));

            // Tworzymy bitmapê wynikow¹
            Bitmap dst = new Bitmap(newWidth, newHeight);

            // Œrodek obrazu Ÿród³owego i docelowego
            float cxSrc = width / 2.0f;
            float cySrc = height / 2.0f;
            float cxDst = newWidth / 2.0f;
            float cyDst = newHeight / 2.0f;

            // Iteracja po pikselach wynikowego obrazu
            for (int dy = 0; dy < newHeight; dy++)
            {
                for (int dx = 0; dx < newWidth; dx++)
                {
                    // Przesuniêcie wzglêdem œrodka obrazu wynikowego
                    float dxCenter = dx - cxDst;
                    float dyCenter = dy - cyDst;

                    double[] vector = [dxCenter, dyCenter];
                    double[] SxSy = MultiplyMatrixVector(R, vector);
                    double sx = SxSy[0] + cxSrc;
                    double sy = SxSy[1] + cySrc;

                    // Sprawdzamy, czy wspó³rzêdne Ÿród³owe mieszcz¹ siê w granicach obrazu Ÿród³owego
                    if (sx >= 0 && sx < width && sy >= 0 && sy < height)
                    {
                        // Zaokr¹glamy wspó³rzêdne Ÿród³owe
                        int srcX = (int)Math.Floor(sx);
                        int srcY = (int)Math.Floor(sy);

                        // Ustawiamy piksel w obrazie wynikowym
                        dst.SetPixel(dx, dy, src.GetPixel(srcX, srcY));
                    }
                    else
                    {
                        // Piksele poza granicami Ÿród³a ustawiamy na przezroczysto
                        dst.SetPixel(dx, dy, Color.Transparent);
                    }
                }
            }

            return dst;
        }

        public double[] MultiplyMatrixVector(double[,] matrix, double[] vector)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            if (cols != vector.Length)
                throw new ArgumentException("Liczba kolumn macierzy musi byæ równa d³ugoœci wektora.");

            double[] result = new double[rows];

            // Iteracja po wierszach macierzy
            for (int i = 0; i < rows; i++)
            {
                double sum = 0;

                // Iteracja po kolumnach macierzy
                for (int j = 0; j < cols; j++)
                {
                    sum += matrix[i, j] * vector[j];
                }

                result[i] = sum;
            }

            return result;
        }

        public Bitmap RotateImageWithShear(Bitmap src, double rad)
        {
            // Obliczenie parametrów Shear
            double tanPhiHalf = -Math.Tan(rad / 2);
            double sinPhi = Math.Sin(rad);

            // Wymiary obrazu Ÿród³owego
            int width = src.Width;
            int height = src.Height;

            int shearX1Width = (int)(width + Math.Abs(height * tanPhiHalf));

            // Wykonanie pierwszej operacji ShearX
            Bitmap shearX1Image = XShear(src, tanPhiHalf, shearX1Width, height);

            // Druga operacja (ShearY)
            int shearYHeight = (int)(height + Math.Abs(shearX1Width * sinPhi));
            Bitmap shearYImage = YShear(shearX1Image, sinPhi, shearX1Width, shearYHeight);

            // Trzecia operacja (ShearX_2)
            int shearX2Width = (int)(shearYHeight + Math.Abs(height * tanPhiHalf));
            Bitmap shearX2Image = XShear(shearYImage, tanPhiHalf, shearX2Width, shearYHeight);

            return CropTransparentRegions(shearX2Image);
        }

        public Bitmap CropTransparentRegions(Bitmap src)
        {
            int width = src.Width;
            int height = src.Height;

            int minX = width, minY = height;
            int maxX = -1, maxY = -1;

            // Przeszukiwanie obrazu w celu znalezienia granic aktywnego obszaru
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixel = src.GetPixel(x, y);
                    if (pixel.A > 0) // Piksel nie jest ca³kowicie przezroczysty
                    {
                        if (x < minX) minX = x;
                        if (x > maxX) maxX = x;
                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }
                }
            }

            // Jeœli ca³y obraz jest przezroczysty, zwracamy pust¹ bitmapê
            if (maxX == -1 || maxY == -1)
            {
                return new Bitmap(1, 1);
            }

            // Obliczenie nowych wymiarów
            int croppedWidth = maxX - minX + 1;
            int croppedHeight = maxY - minY + 1;

            // Tworzenie wykadrowanej bitmapy
            Bitmap croppedBitmap = new Bitmap(croppedWidth, croppedHeight);

            using (Graphics g = Graphics.FromImage(croppedBitmap))
            {
                g.Clear(Color.Transparent);
                g.DrawImage(src, 0, 0, new Rectangle(minX, minY, croppedWidth, croppedHeight), GraphicsUnit.Pixel);
            }

            return croppedBitmap;
        }


        public Bitmap XShear(Bitmap src, double shear, int newWidth, int height)
        {
            Bitmap dst = new Bitmap(newWidth, height);
            using (Graphics g = Graphics.FromImage(dst))
            {
                g.Clear(Color.Transparent);
            }

            // Przesuniêcie œrodka obrazu
            int cxSrc = src.Width / 2;
            int cxDst = newWidth / 2;

            for (int y = 0; y < height; y++)
            {
                int ii = (int)Math.Floor(shear * (y - height / 2.0)); // Przesuniêcie w wierszu
                double ff = shear * (y - height / 2.0) - ii;          // Czêœæ u³amkowa przesuniêcia

                Color prevLeft = Color.FromArgb(0, 0, 0, 0);

                for (int x = 0; x < src.Width; x++)
                {
                    Color pixel = src.GetPixel(x, y);

                    // Interpolacja
                    int rLeft = (int)(ff * pixel.R);
                    int gLeft = (int)(ff * pixel.G);
                    int bLeft = (int)(ff * pixel.B);
                    int aLeft = (int)(ff * pixel.A);
                    Color left = Color.FromArgb(aLeft, rLeft, gLeft, bLeft);

                    int r = Clamp(pixel.R - left.R + prevLeft.R, 0, 255);
                    int g = Clamp(pixel.G - left.G + prevLeft.G, 0, 255);
                    int b = Clamp(pixel.B - left.B + prevLeft.B, 0, 255);
                    int a = Clamp(pixel.A - left.A + prevLeft.A, 0, 255);

                    // Przesuniêcie wzglêdem œrodka
                    int newX = x + ii + cxDst - cxSrc;

                    if (newX >= 0 && newX < newWidth)
                    {
                        dst.SetPixel(newX, y, Color.FromArgb(a, r, g, b));
                    }

                    prevLeft = left;
                }

                // Obs³uga pozosta³ego piksela
                int remainingX = src.Width + ii + cxDst - cxSrc;
                if (remainingX >= 0 && remainingX < newWidth)
                {
                    dst.SetPixel(remainingX, y, prevLeft);
                }
            }

            return dst;
        }


        public Bitmap YShear(Bitmap src, double shear, int width, int newHeight)
        {
            Bitmap dst = new Bitmap(width, newHeight);
            using (Graphics g = Graphics.FromImage(dst))
            {
                g.Clear(Color.Transparent);
            }

            // Przesuniêcie œrodka obrazu
            int cySrc = src.Height / 2;
            int cyDst = newHeight / 2;

            for (int x = 0; x < width; x++)
            {
                int ii = (int)Math.Floor(shear * (x - width / 2.0)); // Przesuniêcie w kolumnie
                double ff = shear * (x - width / 2.0) - ii;          // Czêœæ u³amkowa przesuniêcia

                Color prevLeft = Color.FromArgb(0, 0, 0, 0);

                for (int y = 0; y < src.Height; y++)
                {
                    Color pixel = src.GetPixel(x, y);

                    // Interpolacja
                    int rLeft = (int)(ff * pixel.R);
                    int gLeft = (int)(ff * pixel.G);
                    int bLeft = (int)(ff * pixel.B);
                    int aLeft = (int)(ff * pixel.A);
                    Color left = Color.FromArgb(aLeft, rLeft, gLeft, bLeft);

                    int r = Clamp(pixel.R - left.R + prevLeft.R, 0, 255);
                    int g = Clamp(pixel.G - left.G + prevLeft.G, 0, 255);
                    int b = Clamp(pixel.B - left.B + prevLeft.B, 0, 255);
                    int a = Clamp(pixel.A - left.A + prevLeft.A, 0, 255);

                    // Przesuniêcie wzglêdem œrodka
                    int newY = y + ii + cyDst - cySrc;

                    if (newY >= 0 && newY < newHeight)
                    {
                        dst.SetPixel(x, newY, Color.FromArgb(a, r, g, b));
                    }

                    prevLeft = left;
                }

                // Obs³uga pozosta³ego piksela
                int remainingY = src.Height + ii + cyDst - cySrc;
                if (remainingY >= 0 && remainingY < newHeight)
                {
                    dst.SetPixel(x, remainingY, prevLeft);
                }
            }

            return dst;
        }




        private int Clamp(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }



        private void generateButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(numberTextBox.Text, out int parsedNumber) || parsedNumber <= 1)
            {
                MessageBox.Show("Please enter a valid number greater than 1.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            controlPoints = GenerateControlPoints(parsedNumber);
            t = 0f;
            panel1.Invalidate();
        }

        private void savePolylineButton_Click(object sender, EventArgs e)
        {
            if (controlPoints == null || controlPoints.Length == 0)
            {
                MessageBox.Show("No control points to save.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Wyœwietlenie okna dialogowego do zapisu
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                saveFileDialog.Title = "Save Control Points";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Serializacja punktów kontrolnych do pliku JSON
                        string json = JsonSerializer.Serialize(controlPoints);
                        File.WriteAllText(saveFileDialog.FileName, json);
                        MessageBox.Show("Control points saved successfully.", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving control points: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void loadPolylineButton_Click(object sender, EventArgs e)
        {
            // Wyœwietlenie okna dialogowego do wczytania pliku
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
                openFileDialog.Title = "Load Control Points";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Deserializacja punktów kontrolnych z pliku JSON
                        string json = File.ReadAllText(openFileDialog.FileName);
                        controlPoints = JsonSerializer.Deserialize<PointF[]>(json);

                        panel1.Invalidate();
                        MessageBox.Show("Control points loaded successfully.", "Load Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading control points: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void loadImageButton_Click(object sender, EventArgs e)
        {
            // Wyœwietlenie okna dialogowego do wczytania pliku
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All Files (*.*)|*.*";
                openFileDialog.Title = "Load Image";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Wczytanie obrazka
                        loadedImage = (Bitmap)Image.FromFile(openFileDialog.FileName);

                        ImagePanel.Invalidate();
                        MessageBox.Show("Image loaded successfully.", "Load Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void imagePreviewPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(ImagePanel.BackColor);

            if (loadedImage != null)
            {
                // Dopasowanie obrazka do rozmiaru panelu
                Rectangle imageRect = new Rectangle(0, 0, ImagePanel.Width, ImagePanel.Height);
                g.DrawImage(loadedImage, imageRect);
            }
        }

        private void visiblePolylineCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            GlobalVariables.PolylineVisible = visiblePolylineCheckBox.Checked;
            panel1.Invalidate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            GlobalVariables.ControlVisible = VisibleControlCheckBox.Checked;
            panel1.Invalidate();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            isMoving = true;
            animationTimer.Start();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            isMoving = false;
            animationTimer.Stop();
        }

        private void naiveRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (naiveRadioButton.Checked)
                useFiltering = false;
        }

        private void filteringRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (filteringRadioButton.Checked)
                useFiltering = true;
        }

        private void rotationRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (rotationRadioButton.Checked)
            {
                PointF currentPosition = BezierCurve(controlPoints, t);
                PointF nextPosition = BezierCurve(controlPoints, t + 0.01f);
                currentAngle = GetAngle(currentPosition, nextPosition) + currentAngle;
                if (currentAngle >= 360)
                    currentAngle -= 360;
                isRotating = true;
                RotatingPosition = BezierCurve(controlPoints, t);
                RotatingT = t;
            }
        }

        private void movingRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (movingRadioButton.Checked)
            {
                isRotating = false;
                t = RotatingT;
            }
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            Bitmap stripedBitmap = new Bitmap(100, 100);

            // Ustawianie pasków
            int totalWidth = 100;
            int currentStripeWidth = 7;
            int x = 0;

            using (Graphics g = Graphics.FromImage(stripedBitmap))
            {
                g.Clear(Color.White);

                // Rysowanie czarno-bia³ych pasków
                while (currentStripeWidth >= 1)
                {
                    g.FillRectangle(Brushes.Black, x, 0, currentStripeWidth, 100);

                    // Przesuniêcie i zmniejszenie szerokoœci
                    x += 2*currentStripeWidth;
                    currentStripeWidth -= 1;
                }

                int currentColorStripeWidth = 1;
                int colorX = x;
                double hue = 0; // Pocz¹tkowy k¹t koloru w HSV

                while (colorX < totalWidth)
                {
                    for (int y = 0; y < 100; y++)
                    {
                        // Wyliczanie nasycenia w zale¿noœci od pozycji y
                        double saturation = 1.0 - (y / 99.0);

                        // Konwersja koloru z HSV na RGB
                        Color color = ColorFromHSV(hue, saturation, 1.0);

                        // Rysowanie paska o szerokoœci 1 piksela w danym miejscu
                        using (Brush brush = new SolidBrush(color))
                        {
                            g.FillRectangle(brush, colorX, y, currentColorStripeWidth, 1);
                        }
                    }

                    // Przesuniêcie i zwiêkszenie szerokoœci paska
                    colorX += currentColorStripeWidth;
                    hue += 360.0 / (totalWidth/2);
                    if (hue > 360) hue -= 360;
                }
            }

            loadedImage = stripedBitmap;
            ImagePanel.Invalidate();
        }

        private Color ColorFromHSV(double hue, double saturation, double value)
        {
            int h = (int)(hue / 60) % 6;
            double f = hue / 60 - h;
            double p = value * (1 - saturation);
            double q = value * (1 - f * saturation);
            double t = value * (1 - (1 - f) * saturation);

            double r = 0, g = 0, b = 0;

            switch (h)
            {
                case 0:
                    r = value; g = t; b = p; break;
                case 1:
                    r = q; g = value; b = p; break;
                case 2:
                    r = p; g = value; b = t; break;
                case 3:
                    r = p; g = q; b = value; break;
                case 4:
                    r = t; g = p; b = value; break;
                case 5:
                    r = value; g = p; b = q; break;
            }

            return Color.FromArgb(
                255,
                (int)(r * 255),
                (int)(g * 255),
                (int)(b * 255)
            );
        }
    }
}
