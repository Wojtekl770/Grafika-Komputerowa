using System.Drawing.Drawing2D;
using System.Text.Json;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static EdytorWiel.Global;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EdytorWiel
{
    public partial class Form1 : Form
    {
        private List<Punkt> punkty = new List<Punkt>();
        private List<Krawedz> krawedzie = new List<Krawedz>();
        private bool isPolygonClosed = false;
        public bool alreadyHighlighted = false;
        object? currentlyHighlighted = null;
        private bool isDraggingPolygon = false;
        private bool isDragging = false;
        private int controlPointsSelected = 0;
        private Krawedz? selectedEdge = null;
        private Point? dragStartPoint = null;

        private ContextMenuStrip contextMenu_Kraw;
        private ContextMenuStrip contextMenu_Wierz;
        private ContextMenuStrip contextMenu_Bezier;
        private ToolStripMenuItem AddPointMenu;
        private ToolStripMenuItem DeletePointMenu;
        private ToolStripMenuItem setHorizontalConstraint;
        private ToolStripMenuItem setVerticalConstraint;
        private ToolStripMenuItem setLengthConstraint;
        private ToolStripMenuItem removeConstraint;
        private ToolStripMenuItem setBezierCurve;
        private ToolStripMenuItem removeBezierCurve;
        private ToolStripMenuItem G0Contingency;
        private ToolStripMenuItem G1Contingency;
        private ToolStripMenuItem C1Contingency;

        public Form1()
        {
            InitializeComponent();
            panel1.BackColor = Color.White;
            panel1.MouseClick += Panel1_MouseClick;
            panel1.Paint += Panel1_Paint;
            panel1.MouseMove += Panel1_MouseMove;
            panel1.MouseDown += Panel1_MouseDown;
            panel1.MouseUp += Panel1_MouseUp;
            AddDefaultShape();


            #region Menu kontekstowe
            contextMenu_Kraw = new ContextMenuStrip();
            contextMenu_Wierz = new ContextMenuStrip();
            contextMenu_Bezier = new ContextMenuStrip();

            AddPointMenu = new ToolStripMenuItem("Dodaj wierzcho³ek", null, AddNewPoint);
            DeletePointMenu = new ToolStripMenuItem("Usuñ wierzcho³ek", null, DeletePoint);
            setHorizontalConstraint = new ToolStripMenuItem("Ograniczenie: KrawêdŸ pozioma", null, SetHorizontalConstraint_Click);
            setVerticalConstraint = new ToolStripMenuItem("Ograniczenie: KrawêdŸ pionowa", null, SetVerticalConstraint_Click);
            setLengthConstraint = new ToolStripMenuItem("Ograniczenie: Zadana d³ugoœæ", null, SetLengthConstraint_Click);
            removeConstraint = new ToolStripMenuItem("Usuñ ograniczenie", null, RemoveConstraint_Click);
            setBezierCurve = new ToolStripMenuItem("Prze³¹cz na krzyw¹ Béziera", null, SetBezierCurve_Click);
            removeBezierCurve = new ToolStripMenuItem("Wy³¹cz krzyw¹ Beziera", null, RemoveBezierCurve_Click);
            G0Contingency = new ToolStripMenuItem("Zmieñ ci¹g³oœæ wierzcho³ka na G0", null, SetContingencyG0);
            G1Contingency = new ToolStripMenuItem("Zmieñ ci¹g³oœæ wierzcho³ka na G1", null, SetContingencyG1);
            C1Contingency = new ToolStripMenuItem("Zmieñ ci¹g³oœæ wierzcho³ka na C1", null, SetContingencyC1);

            // Dodawanie elementów do Menu
            contextMenu_Kraw.Items.AddRange(new ToolStripItem[] { AddPointMenu, setHorizontalConstraint, setVerticalConstraint, setLengthConstraint, setBezierCurve, removeConstraint });
            contextMenu_Wierz.Items.AddRange(new ToolStripItem[] { DeletePointMenu, G0Contingency, G1Contingency, C1Contingency });
            contextMenu_Bezier.Items.AddRange(new ToolStripItem[] { removeBezierCurve });
            #endregion
        }

        #region Obsluga Panelu
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using (GraphicsPath fillPath = new GraphicsPath())
            {
                foreach (var krawedz in krawedzie)
                {
                    if (krawedz.IsBezier && krawedz.ControlPoint1 != null && krawedz.ControlPoint2 != null)
                    {
                        // Dodajemy krzyw¹ Béziera do œcie¿ki wype³nienia
                        fillPath.AddBezier(
                            krawedz.Start.Position,
                            krawedz.ControlPoint1.Position,
                            krawedz.ControlPoint2.Position,
                            krawedz.End.Position);
                    }
                    else
                    {
                        // Dodajemy prost¹ liniê do œcie¿ki wype³nienia
                        fillPath.AddLine(krawedz.Start.Position, krawedz.End.Position);
                    }
                }

                if (isPolygonClosed && punkty.Count > 2)
                {
                    fillPath.CloseFigure();
                    using (Brush brush = new SolidBrush(Color.FromArgb(128, Color.LightBlue)))
                    {
                        g.FillPath(brush, fillPath);
                    }
                }
            }

            foreach (var krawedz in krawedzie)
            {
                krawedz.DrawWithBezier(g, null);
            }

            foreach (var punkt2 in punkty)
            {
                punkt2.Draw(g);
            }

            if (currentlyHighlighted is Punkt punkt)
            {
                Krawedz edgeWithStartingPoint = krawedzie.FirstOrDefault(k => k.Start == punkt);
                if (edgeWithStartingPoint != null)
                {
                    ApplyConstraintsRecursively(edgeWithStartingPoint);
                }
                Krawedz edgeWithEndingPoint = krawedzie.FirstOrDefault(k => k.End == punkt);
                if (edgeWithEndingPoint != null)
                {
                    ApplyConstraintsRecursively(edgeWithEndingPoint);
                }
                Krawedz? edgeWithControl = FindEdgeWithControlPoint(punkt);
                if (edgeWithControl != null)
                {
                    ApplyConstraintsRecursively(edgeWithControl);
                }
            }
            else if (currentlyHighlighted is Krawedz edge)
            {
                ApplyConstraintsRecursively(edge);
                Krawedz edgeWithEndingPoint = krawedzie.FirstOrDefault(k => k.End == edge.Start);
                ApplyConstraintsRecursively(edgeWithEndingPoint);
            }
        }

        private void Panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ShowContextMenu(e.Location);
                return;
            }

            Punkt selectedPunkt = null;

            foreach (var punkt in punkty)
            {
                if (punkt.IsHighlighted && punkt.IsMouseOver(e.Location))
                {
                    selectedPunkt = punkt;
                    break;
                }
            }

            // Klikniêto na podœwietlony punkt
            Punkt? first = null;
            if (punkty.Count > 0) first = punkty.First();
            if (selectedPunkt == first && punkty.Count > 2 && !isPolygonClosed)
            {
                Krawedz newKrawedz = new Krawedz(punkty.Last(), punkty.First());
                krawedzie.Add(newKrawedz);
                isPolygonClosed = true;
                panel1.Invalidate();
                return;
            }

            if (!isPolygonClosed && !alreadyHighlighted)
            {
                Punkt nowyPunkt = new Punkt(e.X, e.Y);
                punkty.Add(nowyPunkt);

                if (punkty.Count > 1)
                {
                    Krawedz nowaKrawedz = new Krawedz(punkty[punkty.Count - 2], nowyPunkt);
                    krawedzie.Add(nowaKrawedz);
                }

                panel1.Invalidate();
            }
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            bool needsRedraw = false;

            if ((isDraggingPolygon) && e.Button == MouseButtons.Left)
            {
                MovePolygon(e.Location);
                needsRedraw = true;
                UpdateContingency();
            }

            // Sprawdzanie, czy przycisk myszy jest wciœniêty
            if (e.Button == MouseButtons.Left && currentlyHighlighted != null)
            {
                if (currentlyHighlighted is Punkt punkt)
                {
                    if (punkt.isControl == true)
                    {
                        needsRedraw = MoveControlPoint(punkt, e.Location);
                    }
                    else
                    {
                        needsRedraw = MovePoint(punkt, e.Location);
                        UpdateContingency();
                    }
                    if (isPolygonClosed)
                    {
                        Krawedz edgeWithStartingPoint = krawedzie.FirstOrDefault(k => k.Start == punkt);
                        if (edgeWithStartingPoint != null)
                        {
                            ApplyConstraintsRecursively(edgeWithStartingPoint);
                        }
                        Krawedz edgeWithEndingPoint = krawedzie.FirstOrDefault(k => k.End == punkt);
                        if (edgeWithEndingPoint != null)
                        {
                            ApplyConstraintsRecursively(edgeWithEndingPoint);
                        }
                        Krawedz? edgeWithControl = FindEdgeWithControlPoint(punkt);
                        if (edgeWithControl != null)
                        {
                            ApplyConstraintsRecursively(edgeWithControl);
                        }
                    }
                }
                else if (currentlyHighlighted is Krawedz edge)
                {
                    needsRedraw = MoveEdge(edge, e.Location);
                    if (isPolygonClosed)
                    {
                        ApplyConstraintsRecursively(edge);
                        Krawedz edgeWithEndingPoint = krawedzie.FirstOrDefault(k => k.End == edge.Start);
                        ApplyConstraintsRecursively(edgeWithEndingPoint);
                    }
                    UpdateContingency();
                }
            }

            if (currentlyHighlighted == null && !isDraggingPolygon)
                setDrag(null);

            // Sprawdzanie podœwietlenia punktów i krawêdzi
            needsRedraw |= HighlightObjects(e.Location);

            if (needsRedraw)
            {
                panel1.SuspendLayout();
                panel1.Invalidate();
                panel1.ResumeLayout();
            }
            UpdateMoving();
        }

        private void ApplyConstraintsRecursively(Krawedz edge, HashSet<Krawedz> visitedEdges = null)
        {
            if (edge == null) return;

            if (visitedEdges == null)
                visitedEdges = new HashSet<Krawedz>();

            if (visitedEdges.Contains(edge))
                return;

            visitedEdges.Add(edge);
            edge.ApplyConstraints();
            int index = krawedzie.IndexOf(edge);

            if (index != -1)
            {
                // Popraw nastêpne krawêdzie w kolejnoœci
                Krawedz nextEdge = krawedzie[(index + 1) % krawedzie.Count];
                if (nextEdge != null && nextEdge.Constraint != ConstraintType.None)
                {
                    ApplyConstraintsRecursively(nextEdge, visitedEdges);
                }

                // Popraw poprzednie krawêdzie
                Krawedz previousEdge = krawedzie[index == 0 ? krawedzie.Count - 1 : index - 1];
                if (previousEdge != null && previousEdge.Constraint != ConstraintType.None)
                {
                    ApplyConstraintsRecursively(previousEdge, visitedEdges);
                }
            }
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            // Sprawdzamy, czy u¿ytkownik klikn¹³ wewn¹trz wype³nionego wielok¹ta
            if (isPolygonClosed && IsInsidePolygon(e.Location) && currentlyHighlighted == null)
            {
                isDraggingPolygon = true;
                isDragging = true;
                setDrag(e.Location);
            }
            if (e.Button == MouseButtons.Left && currentlyHighlighted != null)
            {
                isDragging = true;  // Ustawienie flagi wciœniêcia lewego przycisku
            }
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isDraggingPolygon = false;
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
            UpdateMoving();
            UpdateContingency();
            alreadyHighlighted = false;
            panel1.Invalidate();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            punkty.Clear();
            krawedzie.Clear();
            panel1.Invalidate();
            isPolygonClosed = false;
            currentlyHighlighted = null;
            alreadyHighlighted = false;
            isDragging = false;
            isDraggingPolygon = false;
        }

        private void AddDefaultShape()
        {
            int centerX = panel1.Width / 2;
            int centerY = panel1.Height / 2;
            int radius = 100;

            List<Punkt> vertices = new List<Punkt>();
            for (int i = 0; i < 5; i++)
            {
                double angle = 2 * Math.PI / 5 * i;
                int x = (int)(centerX + radius * Math.Cos(angle));
                int y = (int)(centerY + radius * Math.Sin(angle));
                Punkt punkt = new Punkt(x, y);
                punkt.contingency = ContingencyType.C1;
                vertices.Add(punkt);
            }

            punkty.AddRange(vertices);

            for (int i = 0; i < vertices.Count; i++)
            {
                Krawedz edge = new Krawedz(vertices[i], vertices[(i + 1) % vertices.Count]);
                krawedzie.Add(edge);
            }

            if (krawedzie.Count > 0)
            {
                krawedzie[0].Constraint = ConstraintType.Vertical;
            }

            if (krawedzie.Count > 4)
            {
                krawedzie[4].Constraint = ConstraintType.Length;
                krawedzie[4].Length = 100;
            }

            if (krawedzie.Count > 2)
            {
                Krawedz bezierEdge = krawedzie[2];
                bezierEdge.IsBezier = true;

                bezierEdge.ControlPoint1 = new Punkt((bezierEdge.Start.Position.X + bezierEdge.End.Position.X) / 2,
                                                     bezierEdge.Start.Position.Y - 50);
                bezierEdge.ControlPoint2 = new Punkt((bezierEdge.Start.Position.X + bezierEdge.End.Position.X) / 2,
                                                     bezierEdge.End.Position.Y - 50);
                bezierEdge.ControlPoint1.isControl = true;
                bezierEdge.ControlPoint2.isControl = true;
                bezierEdge.ApplyContingency(krawedzie[1], krawedzie[3]);
            }

            foreach (var edge in krawedzie)
            {
                edge.ApplyConstraints();
            }

            // Close the polygon
            isPolygonClosed = true;

            UpdateContingency();

            // Refresh the panel to show the newly added shape
            panel1.SuspendLayout();
            panel1.Invalidate();
            panel1.ResumeLayout();
        }

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            punkty.Clear();
            krawedzie.Clear();
            panel1.Invalidate();
            isPolygonClosed = false;
            currentlyHighlighted = null;
            alreadyHighlighted = false;
            isDragging = false;
            isDraggingPolygon = false;
            AddDefaultShape();
        }

        private void ControlButton_Click(object sender, EventArgs e)
        {
            var kontrol = new Kontrolki();
            kontrol.Show();
        }

        private void AlgorithmButton_Click(object sender, EventArgs e)
        {
            var algorytm = new AlgorithmExplenation();
            algorytm.Show();
        }

        private void DrawLineButton_Click(object sender, EventArgs e)
        {
            currentLineMethod = LineDrawingMethod.Standard;
            panel1.Invalidate();
        }

        private void BresenhamButton_Click(object sender, EventArgs e)
        {
            currentLineMethod = LineDrawingMethod.Bresenham;
            panel1.Invalidate();
        }

        private void WUButton_Click(object sender, EventArgs e)
        {
            currentLineMethod = LineDrawingMethod.Wu;
            panel1.Invalidate();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            ShapeData shapeData = new ShapeData();

            foreach (var punkt in punkty)
            {
                shapeData.Punkty.Add(ToPunktData(punkt));
            }

            foreach (var krawedz in krawedzie)
            {
                shapeData.Krawedzie.Add(ToKrawedzData(krawedz));
            }

            string json = JsonSerializer.Serialize(shapeData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("figura.json", json);
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists("figura.json"))
            {
                MessageBox.Show("File not found!");
                return;
            }

            string json = File.ReadAllText("figura.json");
            ShapeData shapeData = JsonSerializer.Deserialize<ShapeData>(json);

            // Resetowanie obecnych danych
            punkty.Clear();
            krawedzie.Clear();
            currentlyHighlighted = null;
            alreadyHighlighted = false;
            isDragging = false;
            isDraggingPolygon = false;

            // Mapowanie PunktData na Punkt
            Dictionary<PunktData, Punkt> punktMap = new Dictionary<PunktData, Punkt>();

            // Tworzenie i mapowanie punktów
            foreach (var punktData in shapeData.Punkty)
            {
                Punkt punkt = FromPunktData(punktData);
                punkty.Add(punkt);
                punktMap[punktData] = punkt;
                if (punkty.Count > 1)
                {
                    Krawedz nowaKrawedz = new Krawedz(punkty[punkty.Count - 2], punkt);
                    krawedzie.Add(nowaKrawedz);
                }
            }

            Krawedz LastKrawedz = new Krawedz(punkty.Last(), punkty.First());
            krawedzie.Add(LastKrawedz);
            int i = 0;

            foreach (var krawedzData in shapeData.Krawedzie)
            {
                if (!punktMap.TryGetValue(krawedzData.Start, out Punkt start))
                {
                    start = FromPunktData(krawedzData.Start);
                }

                if (!punktMap.TryGetValue(krawedzData.End, out Punkt end))
                {
                    end = FromPunktData(krawedzData.End);
                }

                // Tworzenie krawêdzi z odpowiednimi punktami
                Krawedz krawedz = FromKrawedzData(krawedzData, start, end);
                if (krawedz.IsBezier && krawedzData.ControlPoint1 != null && krawedzData.ControlPoint2 != null)
                {
                    krawedzie[i].ControlPoint1 = krawedz.ControlPoint1;
                    krawedzie[i].ControlPoint2 = krawedz.ControlPoint2;
                    krawedzie[i].IsBezier = true;
                }
                krawedzie[i].Constraint = krawedz.Constraint;
                krawedzie[i].Length = krawedz.Length;
                krawedzie[i].useBresenham = krawedz.useBresenham;
                i++;
            }



            // Stosowanie ograniczeñ
            foreach (var edge in krawedzie)
            {
                edge.ApplyConstraints();
            }

            UpdateContingency();

            isPolygonClosed = true;
            panel1.Invalidate();
        }

        public PunktData ToPunktData(Punkt punkt)
        {
            return new PunktData
            {
                X = punkt.Position.X,
                Y = punkt.Position.Y,
                IsControl = punkt.isControl,
                Contingency = punkt.contingency
            };
        }

        public Punkt FromPunktData(PunktData data)
        {
            Punkt punkt = new Punkt(data.X, data.Y);
            punkt.isControl = data.IsControl;
            punkt.contingency = data.Contingency;
            return punkt;
        }

        public KrawedzData ToKrawedzData(Krawedz krawedz)
        {
            return new KrawedzData
            {
                Start = ToPunktData(krawedz.Start),
                End = ToPunktData(krawedz.End),
                IsBezier = krawedz.IsBezier,
                ControlPoint1 = krawedz.ControlPoint1 != null ? ToPunktData(krawedz.ControlPoint1) : null,
                ControlPoint2 = krawedz.ControlPoint2 != null ? ToPunktData(krawedz.ControlPoint2) : null,
                Constraint = krawedz.Constraint,
                Length = krawedz.Length,
                UseBresenham = krawedz.useBresenham
            };
        }

        public Krawedz FromKrawedzData(KrawedzData data, Punkt start, Punkt end)
        {
            Krawedz krawedz = new Krawedz(start, end);
            krawedz.IsBezier = data.IsBezier;
            krawedz.Constraint = data.Constraint;
            krawedz.Length = data.Length;
            krawedz.useBresenham = data.UseBresenham;

            // Inicjalizacja punktów kontrolnych dla Béziera
            if (data.ControlPoint1 != null)
                krawedz.ControlPoint1 = FromPunktData(data.ControlPoint1);

            if (data.ControlPoint2 != null)
                krawedz.ControlPoint2 = FromPunktData(data.ControlPoint2);

            return krawedz;
        }


        #endregion


        #region Poruszanie
        public void setDrag(Point? dragStartPoint)
        {
            this.dragStartPoint = dragStartPoint;
        }

        public bool MovePoint(Punkt punkt, Point newLocation)
        {
            punkt.Position = newLocation;
            punkt.isMoving = true;
            return true;
        }

        public bool MoveControlPoint(Punkt controlPoint, Point newLocation)
        {
            Point OldPosition = controlPoint.Position;
            controlPoint.Position = newLocation;
            controlPoint.isMoving = true;
            Krawedz? currentEdge = FindEdgeWithControlPoint(controlPoint);
            int index = krawedzie.IndexOf(currentEdge);
            Krawedz nextEdge = krawedzie[(index + 1) % krawedzie.Count];
            Krawedz prevEdge = krawedzie[(index - 1 + krawedzie.Count) % krawedzie.Count];
            if (currentEdge == null)
            {
                return false;
            }
            if (controlPoint == currentEdge.ControlPoint1)
            {
                if (currentEdge.Start.contingency == ContingencyType.G0) return false;
                if (prevEdge.IsBezier && prevEdge.ControlPoint1 != null && prevEdge.ControlPoint2 != null)
                {
                    if (currentEdge.Start.contingency == ContingencyType.G1)
                    {
                        prevEdge.ControlPoint2.Position = NearestPointOnLine(controlPoint.Position, prevEdge.End.Position, prevEdge.ControlPoint2.Position);
                    }
                    else
                    {
                        prevEdge.ControlPoint2.Position = new Point(
                            prevEdge.End.Position.X + (prevEdge.End.Position.X - controlPoint.Position.X),
                            prevEdge.End.Position.Y + (prevEdge.End.Position.Y - controlPoint.Position.Y)
                        );
                    }
                }
                else
                {
                    if (prevEdge.Constraint == ConstraintType.Horizontal)
                    {
                        prevEdge.End.Position = new Point(prevEdge.End.Position.X, newLocation.Y);
                    }
                    else if (prevEdge.Constraint == ConstraintType.Vertical)
                    {
                        prevEdge.End.Position = new Point(newLocation.X, prevEdge.End.Position.Y);
                    }

                    if (currentEdge.Start.contingency == ContingencyType.G1)
                    {
                        prevEdge.Start.Position = NearestPointOnLine(controlPoint.Position, prevEdge.End.Position, prevEdge.Start.Position);
                    }
                    else
                    {
                        if (prevEdge.Constraint == ConstraintType.Length)
                        {
                            int XChange = newLocation.X - OldPosition.X;
                            int YChange = newLocation.Y - OldPosition.Y;
                            prevEdge.Start.Position = new Point(prevEdge.Start.Position.X + XChange, prevEdge.Start.Position.Y + YChange);
                            prevEdge.End.Position = new Point(prevEdge.End.Position.X + XChange, prevEdge.End.Position.Y + YChange);
                        }
                        else
                        {
                            prevEdge.Start.Position = new Point(
                                prevEdge.End.Position.X + (prevEdge.End.Position.X - controlPoint.Position.X) * 3,
                                prevEdge.End.Position.Y + (prevEdge.End.Position.Y - controlPoint.Position.Y) * 3
                            );
                        }
                    }
                }
                prevEdge.Start.isMoving = true;
                ApplyConstraintsRecursively(prevEdge);
            }

            else if (controlPoint == currentEdge.ControlPoint2)
            {
                if (currentEdge.End.contingency == ContingencyType.G0) return false;
                if (nextEdge.IsBezier && nextEdge.ControlPoint1 != null && nextEdge.ControlPoint2 != null)
                {
                    if (currentEdge.End.contingency == ContingencyType.G1)
                    {
                        nextEdge.ControlPoint1.Position = NearestPointOnLine(controlPoint.Position, nextEdge.Start.Position, nextEdge.ControlPoint1.Position);
                    }
                    else
                    {
                        nextEdge.ControlPoint1.Position = new Point(
                            nextEdge.Start.Position.X + (nextEdge.Start.Position.X - controlPoint.Position.X),
                            nextEdge.Start.Position.Y + (nextEdge.Start.Position.Y - controlPoint.Position.Y)
                        );
                    }
                }
                else
                {
                    if (nextEdge.Constraint == ConstraintType.Horizontal)
                    {
                        nextEdge.Start.Position = new Point(nextEdge.Start.Position.X, newLocation.Y);
                    }
                    else if (nextEdge.Constraint == ConstraintType.Vertical)
                    {
                        nextEdge.Start.Position = new Point(newLocation.X, nextEdge.Start.Position.Y);
                    }

                    if (currentEdge.End.contingency == ContingencyType.G1)
                    {
                        nextEdge.End.Position = NearestPointOnLine(controlPoint.Position, nextEdge.Start.Position, nextEdge.End.Position);
                    }
                    else
                    {
                        if (nextEdge.Constraint == ConstraintType.Length)
                        {
                            int XChange = newLocation.X - OldPosition.X;
                            int YChange = newLocation.Y - OldPosition.Y;
                            nextEdge.Start.Position = new Point(nextEdge.Start.Position.X + XChange, nextEdge.Start.Position.Y + YChange);
                            nextEdge.End.Position = new Point(nextEdge.End.Position.X + XChange, nextEdge.End.Position.Y + YChange);
                        }
                        else
                        {
                            nextEdge.End.Position = new Point(
                                nextEdge.Start.Position.X + (nextEdge.Start.Position.X - controlPoint.Position.X) * 3,
                                nextEdge.Start.Position.Y + (nextEdge.Start.Position.Y - controlPoint.Position.Y) * 3
                            );
                        }
                    }
                }
                nextEdge.End.isMoving = true;
                ApplyConstraintsRecursively(nextEdge);
            }
            return true;
        }

        public bool MoveEdge(Krawedz edge, Point newLocation)
        {
            if (dragStartPoint == null)
                dragStartPoint = newLocation;

            int Xchange = newLocation.X - dragStartPoint.Value.X;
            int YChange = newLocation.Y - dragStartPoint.Value.Y;

            edge.Start.Position = new Point(edge.Start.Position.X + Xchange, edge.Start.Position.Y + YChange);
            edge.End.Position = new Point(edge.End.Position.X + Xchange, edge.End.Position.Y + YChange);

            edge.End.isMoving = true;
            edge.Start.isMoving = true;

            dragStartPoint = newLocation;

            return true;
        }

        public void MovePolygon(Point newLocation)
        {
            if (dragStartPoint == null)
                return;

            int Xchange = newLocation.X - dragStartPoint.Value.X;
            int YChange = newLocation.Y - dragStartPoint.Value.Y;

            foreach (var punkt in punkty)
            {
                punkt.Position = new Point(punkt.Position.X + Xchange, punkt.Position.Y + YChange);
            }

            foreach (var krawedz in krawedzie)
            {
                if (krawedz.IsBezier && krawedz.ControlPoint1 != null && krawedz.ControlPoint2 != null)
                {
                    krawedz.ControlPoint1.Position = new Point(krawedz.ControlPoint1.Position.X + Xchange, krawedz.ControlPoint1.Position.Y + YChange);
                    krawedz.ControlPoint2.Position = new Point(krawedz.ControlPoint2.Position.X + Xchange, krawedz.ControlPoint2.Position.Y + YChange);
                }
            }

            dragStartPoint = newLocation;
        }

        public bool IsInsidePolygon(Point p)
        {
            if (punkty.Count < 3) return false;

            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(punkty.Select(point => point.Position).ToArray());
            return path.IsVisible(p);
        }

        #endregion


        #region Funkcje pomocnicze
        private bool HighlightObjects(Point mouseLocation)
        {
            if (isDragging)
            {
                return true;
            }

            bool needsRedraw = false;
            Graphics g = panel1.CreateGraphics();

            foreach (var punkt in punkty)
            {
                bool isMouseOver = punkt.IsMouseOver(mouseLocation);
                if (isMouseOver && !alreadyHighlighted)
                {
                    punkt.IsHighlighted = true;
                    alreadyHighlighted = true;
                    currentlyHighlighted = punkt;
                }
                else if (!isMouseOver && punkt.IsHighlighted)
                {
                    punkt.IsHighlighted = false;
                    alreadyHighlighted = false;
                    currentlyHighlighted = null;
                    needsRedraw = true;
                }
                punkt.Draw(g);
            }

            foreach (var edge in krawedzie)
            {
                bool isMouseOver = edge.IsMouseOver(mouseLocation);

                if (isMouseOver && !alreadyHighlighted)
                {
                    edge.IsHighlighted = true;
                    alreadyHighlighted = true;
                    currentlyHighlighted = edge;
                }
                else if (!isMouseOver && edge.IsHighlighted)
                {
                    edge.IsHighlighted = false;
                    alreadyHighlighted = false;
                    currentlyHighlighted = null;
                    needsRedraw = true;
                }

                if (edge.IsBezier && edge.ControlPoint1 != null && edge.ControlPoint2 != null)
                {
                    bool isMouseOverControl1 = edge.IsMouseOverControlPoint(mouseLocation, edge.ControlPoint1);
                    if (isMouseOverControl1 && !alreadyHighlighted)
                    {
                        edge.ControlPoint1.IsHighlighted = true;
                        currentlyHighlighted = edge.ControlPoint1;
                        alreadyHighlighted = true;
                    }
                    else if (!isMouseOverControl1 && edge.ControlPoint1.IsHighlighted)
                    {
                        edge.ControlPoint1.IsHighlighted = false;
                        currentlyHighlighted = null;
                        alreadyHighlighted = false;
                        needsRedraw = true;
                    }

                    bool isMouseOverControl2 = edge.IsMouseOverControlPoint(mouseLocation, edge.ControlPoint2);
                    if (isMouseOverControl2 && !alreadyHighlighted)
                    {
                        edge.ControlPoint2.IsHighlighted = true;
                        currentlyHighlighted = edge.ControlPoint2;
                        alreadyHighlighted = true;
                    }
                    else if (!isMouseOverControl2 && edge.ControlPoint2.IsHighlighted)
                    {
                        edge.ControlPoint2.IsHighlighted = false;
                        currentlyHighlighted = null;
                        alreadyHighlighted = false;
                        needsRedraw |= true;
                    }
                }

                edge.Draw(g);
            }

            return needsRedraw;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            foreach (var edge in krawedzie)
            {
                edge.Draw(e.Graphics);
            }
        }

        private Point NearestPointOnLine(Point lineStart, Point lineEnd, Point point)
        {
            float dx = lineEnd.X - lineStart.X;
            float dy = lineEnd.Y - lineStart.Y;
            float lengthSquared = dx * dx + dy * dy;

            if (lengthSquared == 0)
                return new Point(lineStart.X, lineStart.Y);

            float t = ((point.X - lineStart.X) * dx + (point.Y - lineStart.Y) * dy) / lengthSquared;

            return new Point(
                (int)(lineStart.X + t * dx),
                (int)(lineStart.Y + t * dy)
            );
        }

        public Krawedz? FindEdgeWithControlPoint(Punkt controlPoint)
        {
            foreach (var edge in krawedzie)
            {
                if (edge.IsBezier)
                {
                    if (edge.ControlPoint1 == controlPoint || edge.ControlPoint2 == controlPoint)
                    {
                        return edge;
                    }
                }
            }
            return null;
        }

        public void UpdateMoving()
        {
            foreach (var point in punkty)
            {
                point.isMoving = false;
            }
        }

        #endregion


        #region Ograniczenia
        private void ShowContextMenu(Point location)
        {
            if (!isPolygonClosed) return;
            currentlyHighlighted = null;

            // Wierzcholek
            foreach (var punkt in punkty)
            {
                if (punkt.IsMouseOver(location))
                {
                    currentlyHighlighted = punkt;
                    contextMenu_Wierz.Show(panel1, location);
                    return;
                }
            }

            // Krawedz
            foreach (var edge in krawedzie)
            {
                if (edge.IsMouseOver(location))
                {
                    currentlyHighlighted = edge;
                    if (edge.IsBezier)
                    {
                        contextMenu_Bezier.Show(panel1, location);
                    }
                    else
                    {
                        contextMenu_Kraw.Show(panel1, location);
                    }
                    return;
                }
            }
        }

        private void AddNewPoint(object? sender, EventArgs e)
        {
            if (currentlyHighlighted is Krawedz edge)
            {
                Point midpoint = new Point(
                    (edge.Start.Position.X + edge.End.Position.X) / 2,
                    (edge.Start.Position.Y + edge.End.Position.Y) / 2
                );

                Punkt nowyPunkt = new Punkt(midpoint.X, midpoint.Y);
                punkty.Insert(punkty.IndexOf(edge.End), nowyPunkt);

                krawedzie.Insert(krawedzie.IndexOf(edge), new Krawedz(edge.Start, nowyPunkt));
                edge.Start = nowyPunkt;
                edge.Constraint = ConstraintType.None;

                panel1.Invalidate();
            }
        }

        private void DeletePoint(object? sender, EventArgs e)
        {
            if (currentlyHighlighted is Punkt punkt && punkty.Count > 3)
            {
                int punktIndex = punkty.IndexOf(punkt);

                // ZnajdŸ s¹siadów
                Punkt poprzedni = punkty[punktIndex == 0 ? punkty.Count - 1 : punktIndex - 1];
                Punkt nastepny = punkty[(punktIndex + 1) % punkty.Count];

                int krawedzIndex = krawedzie.FindIndex(k => k.Start == punkt || k.End == punkt);

                krawedzie.RemoveAll(k => k.Start == punkt || k.End == punkt);

                // Dodaj now¹ krawêdŸ miêdzy poprzednim a nastêpnym punktem
                if (krawedzIndex >= 0)
                {
                    krawedzie.Insert(krawedzIndex, new Krawedz(poprzedni, nastepny));
                }

                punkty.Remove(punkt);

                alreadyHighlighted = false;
                currentlyHighlighted = null;
                panel1.Invalidate();
            }
            else
            {
                // Jeœli nie mo¿na usun¹æ punktu, wyczyœæ wszystkie dane
                punkty.Clear();
                krawedzie.Clear();
                panel1.Invalidate();
                isPolygonClosed = false;
                currentlyHighlighted = null;
                alreadyHighlighted = false;
            }
        }

        private void SetHorizontalConstraint_Click(object? sender, EventArgs e)
        {
            if (currentlyHighlighted is Krawedz edge)
            {
                if (CanSetConstraint(edge, ConstraintType.Horizontal))
                {
                    edge.Constraint = ConstraintType.Horizontal;
                    edge.Length = null;
                    panel1.Invalidate();
                    edge.ApplyConstraints();
                    UpdateContingency();
                }
                else
                {
                    MessageBox.Show("Nie mo¿na ustawiæ obu s¹siednich krawêdzi jako poziome.");
                }
            }
        }

        private void SetVerticalConstraint_Click(object? sender, EventArgs e)
        {
            if (currentlyHighlighted is Krawedz edge)
            {
                if (CanSetConstraint(edge, ConstraintType.Vertical))
                {
                    edge.Constraint = ConstraintType.Vertical;
                    edge.Length = null;
                    panel1.Invalidate();
                    edge.ApplyConstraints();
                    UpdateContingency();
                }
                else
                {
                    MessageBox.Show("Nie mo¿na ustawiæ obu s¹siednich krawêdzi jako pionowe.");
                }
            }
        }

        private void SetLengthConstraint_Click(object? sender, EventArgs e)
        {
            if (currentlyHighlighted is Krawedz edge)
            {
                using (var lengthDialog = new LengthInputDialog(edge.GetLength()))
                {
                    if (lengthDialog.ShowDialog() == DialogResult.OK)
                    {
                        double newLength = lengthDialog.EnteredLength;
                        edge.Constraint = ConstraintType.Length;
                        edge.Length = newLength;
                        edge.ApplyConstraints();
                        UpdateContingency();
                        panel1.Invalidate();
                    }
                }
            }
        }

        private void RemoveConstraint_Click(object? sender, EventArgs e)
        {
            if (currentlyHighlighted is Krawedz edge)
            {
                edge.Constraint = ConstraintType.None;
                edge.Length = null;
                panel1.Invalidate();
            }
        }

        private bool CanSetConstraint(Krawedz edge, ConstraintType constraint)
        {
            int edgeIndex = krawedzie.IndexOf(edge);
            Krawedz previousEdge = krawedzie[edgeIndex == 0 ? krawedzie.Count - 1 : edgeIndex - 1];
            Krawedz nextEdge = krawedzie[(edgeIndex + 1) % krawedzie.Count];

            return !(constraint == ConstraintType.Horizontal && (previousEdge.Constraint == ConstraintType.Horizontal || nextEdge.Constraint == ConstraintType.Horizontal))
                && !(constraint == ConstraintType.Vertical && (previousEdge.Constraint == ConstraintType.Vertical || nextEdge.Constraint == ConstraintType.Vertical));
        }

        private void SetBezierCurve_Click(object? sender, EventArgs e)
        {
            if (currentlyHighlighted is Krawedz edge)
            {
                selectedEdge = edge;
                selectedEdge.IsBezier = true;

                controlPointsSelected = 0;

                panel1.MouseClick += SelectControlPoints;
            }
        }

        public void RemoveBezierCurve_Click(object? sender, EventArgs e)
        {
            if (currentlyHighlighted is Krawedz edge)
            {
                selectedEdge = edge;
                selectedEdge.IsBezier = false;

                edge.ControlPoint1 = null;
                edge.ControlPoint2 = null;
            }
        }

        private void SelectControlPoints(object? sender, MouseEventArgs e)
        {
            if (selectedEdge == null || !selectedEdge.IsBezier)
                return;

            if (controlPointsSelected == 0)
            {
                // Pierwszy Punkt Kontrolny
                if (selectedEdge.ControlPoint1 == null)
                {
                    selectedEdge.ControlPoint1 = new Punkt(e.X, e.Y);
                    selectedEdge.ControlPoint1.isControl = true;
                }
                else
                {
                    selectedEdge.ControlPoint1.Position = new Point(e.X, e.Y);
                }

                controlPointsSelected++;
            }
            else if (controlPointsSelected == 1)
            {
                // Drugi Punkt Kontrolny
                if (selectedEdge.ControlPoint2 == null)
                {
                    selectedEdge.ControlPoint2 = new Punkt(e.X, e.Y);
                    selectedEdge.ControlPoint2.isControl = true;
                }
                else
                {
                    selectedEdge.ControlPoint2.Position = new Point(e.X, e.Y);
                }
                controlPointsSelected = 0;

                UpdateContingency();

                panel1.MouseClick -= SelectControlPoints;

                panel1.Invalidate();
            }
        }

        public void UpdateContingency()
        {
            if (krawedzie.Count > 2)
            {
                if (krawedzie[0].IsBezier && krawedzie[0].ControlPoint1 != null && krawedzie[0].ControlPoint2 != null)
                {
                    krawedzie[0].ApplyContingency(krawedzie[krawedzie.Count - 1], krawedzie[1]);
                }
                for (int i = 1; i < krawedzie.Count - 1; i++)
                {
                    if (i < krawedzie.Count - 1)
                    {
                        // Zastosowanie ci¹g³oœci miêdzy t¹ krawêdzi¹ a nastêpn¹
                        if (krawedzie[i].IsBezier && krawedzie[i].ControlPoint1 != null && krawedzie[i].ControlPoint2 != null)
                            krawedzie[i].ApplyContingency(krawedzie[i - 1], krawedzie[i + 1]);
                    }
                }
                if (krawedzie[krawedzie.Count - 1].IsBezier && krawedzie[krawedzie.Count - 1].ControlPoint1 != null && krawedzie[krawedzie.Count - 1].ControlPoint2 != null)
                {
                    krawedzie[krawedzie.Count - 1].ApplyContingency(krawedzie[krawedzie.Count - 2], krawedzie[0]);
                }
            }
        }

        public void SetContingencyG0(object? sender, EventArgs e)
        {
            if (currentlyHighlighted is Punkt punkt)
            {
                punkt.contingency = ContingencyType.G0;
                UpdateContingency();
                panel1.Invalidate();
            }
        }

        public void SetContingencyG1(object? sender, EventArgs e)
        {
            if (currentlyHighlighted is Punkt punkt)
            {
                punkt.contingency = ContingencyType.G1;
                UpdateContingency();
                panel1.Invalidate();
            }
        }

        public void SetContingencyC1(object? sender, EventArgs e)
        {
            if (currentlyHighlighted is Punkt punkt)
            {
                punkt.contingency = ContingencyType.C1;
                UpdateContingency();
                panel1.Invalidate();
            }
        }

        #endregion

    }
}