using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.AxHost;

namespace EdytorWiel
{
    public enum ConstraintType { None, Horizontal, Vertical, Length }

    public static class Global
    {
        public enum LineDrawingMethod { Standard, Bresenham, Wu }
        public static LineDrawingMethod currentLineMethod = LineDrawingMethod.Standard;
    }
    public class Krawedz
    {
        public Punkt Start { get; set; }
        public Punkt End { get; set; }
        public bool IsBezier { get; set; }
        public Punkt? ControlPoint1 { get; set; }
        public Punkt? ControlPoint2 { get; set; }
        public bool IsHighlighted { get; set; }
        public ConstraintType Constraint { get; set; }
        public Form1 form;
        public double? Length { get; set; }
        private int tolerance = 7;
        public bool useBresenham = false;

        public Krawedz(Punkt start, Punkt end)
        {
            Start = start;
            End = end;
            IsHighlighted = false;
            Constraint = ConstraintType.None;
            Length = null;
            IsBezier = false;
        }

        public void Draw(Graphics g)
        {
            Pen pen = IsHighlighted ? new Pen(Color.Blue, 3) : new Pen(Color.Black, 1);

            if (IsBezier && ControlPoint1 != null && ControlPoint2 != null)
            {
                // Rysowanie krzywej Béziera 3-go stopnia
                DrawBezierCurve(g, pen, Start, ControlPoint1, ControlPoint2, End);

                // Rysowanie punktów kontrolnych
                ControlPoint1.Draw(g);
                ControlPoint2.Draw(g);
                g.FillEllipse(Brushes.Green, ControlPoint1.Position.X - 3, ControlPoint1.Position.Y - 3, 6, 6);
                g.FillEllipse(Brushes.Green, ControlPoint2.Position.X - 3, ControlPoint2.Position.Y - 3, 6, 6);
                DrawDashedLine(g, Start, ControlPoint1, 5, 5);
                DrawDashedLine(g, ControlPoint1, ControlPoint2, 5, 5);
                DrawDashedLine(g, ControlPoint2, End, 5, 5);
            }
            else
            {
                // Rysowanie linii
                if (Global.currentLineMethod == Global.LineDrawingMethod.Bresenham)
                {
                    DrawLineBresenham(g, pen, Start.Position, End.Position);
                }
                else if (Global.currentLineMethod == Global.LineDrawingMethod.Standard)
                {
                    g.DrawLine(pen, Start.Position, End.Position);
                }
                else
                {
                    DrawLineWu(g, pen, Start.Position, End.Position);
                }

                if (ControlPoint1 != null) ControlPoint1.Draw(g);
                if (ControlPoint2 != null) ControlPoint2.Draw(g);

                // Rysowanie wskaźnika ograniczenia
                if (Constraint != ConstraintType.None)
                {
                    string symbol = Constraint == ConstraintType.Horizontal ? "H" :
                                    Constraint == ConstraintType.Vertical ? "V" : "L";
                    DrawConstraintSymbol(g, symbol);
                }
            }
        }

        public void DrawLineBresenham(Graphics g, Pen pen, Point start, Point end)
        {
            int x = start.X;
            int y = start.Y;
            int dx = Math.Abs(end.X - start.X);
            int dy = Math.Abs(end.Y - start.Y);
            int sx = start.X < end.X ? 1 : -1;
            int sy = start.Y < end.Y ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                g.DrawRectangle(pen, x, y, 1, 1); // Rysowanie pojedynczego piksela

                if (x == end.X && y == end.Y) break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y += sy;
                }
            }
        }

        private void DrawLineWu(Graphics g, Pen pen, Point start, Point end)
        {
            float dx = end.X - start.X;
            float dy = end.Y - start.Y;

            // Check if the line is steep
            bool isSteep = Math.Abs(dy) > Math.Abs(dx);
            if (isSteep)
            {
                // Swap coordinates
                int tempX = start.X;
                int tempY = start.Y;
                start.X = end.X;
                start.Y = end.Y;
                end.X = tempX;
                end.Y = tempY;

                dx = end.X - start.X;
                dy = end.Y - start.Y;
            }

            if (start.X > end.X)
            {
                int tempX2 = start.X;
                int tempY2 = start.Y;
                start.X = end.X;
                start.Y = end.Y;
                end.X = tempX2;
                end.Y = tempY2;
            }

            float gradient = dy / dx;

            int xPixel1 = (int)MathF.Round(start.X);
            int yPixel1 = (int)MathF.Round(start.Y);
            float intery = start.Y + gradient;

            // Draw the first pixel
            g.FillRectangle(new SolidBrush(pen.Color), xPixel1, yPixel1, 1, 1);

            for (int x = (int)start.X + 1; x <= end.X; x++)
            {
                yPixel1 = (int)MathF.Floor(intery);
                float alpha = intery - yPixel1; // Calculate alpha for anti-aliasing

                g.FillRectangle(new SolidBrush(Color.FromArgb((int)(pen.Color.A * (1 - alpha)), pen.Color)), x, yPixel1, 1, 1);
                g.FillRectangle(new SolidBrush(Color.FromArgb((int)(pen.Color.A * alpha), pen.Color)), x, yPixel1 + 1, 1, 1);

                intery += gradient;
            }
        }



        public void SetBresenhamUsage(bool value)
        {
            useBresenham = value;
        }

        private void DrawConstraintSymbol(Graphics g, string symbol)
        {
            Point midpoint = new Point((Start.Position.X + End.Position.X) / 2, (Start.Position.Y + End.Position.Y) / 2);
            g.DrawString(symbol, new Font("Arial", 10), Brushes.Red, midpoint);
        }

        public bool IsMouseOver(Point mousePosition)
        {
            if (IsBezier && ControlPoint1 != null && ControlPoint2 != null)
            {
                const int steps = 100;
                PointF previousPoint = Start.Position;

                for (int i = 1; i <= steps; i++)
                {
                    float t = (float)i / steps;
                    PointF currentPoint = CalculateBezierPoint(t, Start.Position, ControlPoint1.Position, ControlPoint2.Position, End.Position);

                    double distance = Math.Sqrt(Math.Pow(mousePosition.X - currentPoint.X, 2) + Math.Pow(mousePosition.Y - currentPoint.Y, 2));

                    if (distance <= tolerance)
                    {
                        return true;
                    }

                    previousPoint = currentPoint;
                }

                return false;
            }
            else
            {
                return DistanceFromPointToLineSegment(mousePosition, Start.Position, End.Position) <= tolerance;
            }
        }

        public double DistanceFromPointToLineSegment(Point p, Point v, Point w)
        {
            double l2 = Math.Pow(v.X - w.X, 2) + Math.Pow(v.Y - w.Y, 2);
            if (l2 == 0) return Math.Sqrt(Math.Pow(p.X - v.X, 2) + Math.Pow(p.Y - v.Y, 2));
            double t = Math.Max(0, Math.Min(1, ((p.X - v.X) * (w.X - v.X) + (p.Y - v.Y) * (w.Y - v.Y)) / l2));
            Point projection = new Point((int)(v.X + t * (w.X - v.X)), (int)(v.Y + t * (w.Y - v.Y)));
            return Math.Sqrt(Math.Pow(p.X - projection.X, 2) + Math.Pow(p.Y - projection.Y, 2));
        }

        public double GetLength()
        {
            return Math.Sqrt(Math.Pow(End.Position.X - Start.Position.X, 2) + Math.Pow(End.Position.Y - Start.Position.Y, 2));
        }

        public void ApplyConstraints()
        {
            if (Constraint == ConstraintType.Horizontal)
            {
                if (End.isMoving)
                {
                    if (Start.Position.Y != End.Position.Y)
                    {
                        Start.Position = new Point(Start.Position.X, End.Position.Y);
                        Start.isMoving = true;
                    }
                    else
                    {
                        Start.isMoving = false;
                    }
                }
                else
                {
                    if (End.Position.Y != Start.Position.Y)
                    {
                        End.Position = new Point(End.Position.X, Start.Position.Y);
                        End.isMoving = true;
                    }
                    else
                    {
                        End.isMoving = false;
                    }
                }
            }
            else if (Constraint == ConstraintType.Vertical)
            {
                if (End.isMoving)
                {
                    if (Start.Position.X != End.Position.X)
                    {
                        Start.Position = new Point(End.Position.X, Start.Position.Y);
                        Start.isMoving = true;
                    }
                    else
                    {
                        Start.isMoving= false;
                    }
                }
                else
                {
                    if (End.Position.X != Start.Position.X)
                    {
                        End.Position = new Point(Start.Position.X, End.Position.Y);
                        End.isMoving = true;
                    }
                    else
                    {
                        End.isMoving= false;
                    }
                }
            }
            else if (Constraint == ConstraintType.Length && Length.HasValue)
            {
                AdjustLengthTo(Length.Value);
            }
        }

        private void AdjustLengthTo(double targetLength)
        {
            double currentLength = GetLength();
            if (currentLength == 0) return;

            double scaleFactor = targetLength / currentLength;

            int newEndX, newEndY;
            if (Start.isMoving)
            {
                int deltaX = End.Position.X - Start.Position.X;
                int deltaY = End.Position.Y - Start.Position.Y;

                newEndX = Start.Position.X + (int)(deltaX * scaleFactor);
                newEndY = Start.Position.Y + (int)(deltaY * scaleFactor);
                if (End.Position.X != newEndX || End.Position.Y != newEndY)
                {
                    End.Position = new Point(newEndX, newEndY);
                    End.isMoving = true;
                }
                else
                {
                    End.isMoving = false;
                }
            }
            else
            {
                int deltaX = Start.Position.X - End.Position.X;
                int deltaY = Start.Position.Y - End.Position.Y;

                newEndX = End.Position.X + (int)(deltaX * scaleFactor);
                newEndY = End.Position.Y + (int)(deltaY * scaleFactor);
                if (Start.Position.X != newEndX || Start.Position.Y != newEndY)
                {
                    Start.Position = new Point(newEndX, newEndY);
                    Start.isMoving = true;
                }
                else
                {
                    Start.isMoving = false;
                }
            }
        }

        private void DrawBezierCurve(Graphics g, Pen pen, Punkt start, Punkt controlPoint1, Punkt controlPoint2, Punkt end)
        {
            const int steps = 100; // Liczba kroków iteracji
            PointF previousPoint = start.Position;

            for (int i = 1; i <= steps; i++)
            {
                float t = (float)i / steps;
                // Obliczanie kolejnych współrzędnych na krzywej Béziera
                PointF currentPoint = CalculateBezierPoint(t, start.Position, controlPoint1.Position, controlPoint2.Position, end.Position);

                g.DrawLine(pen, previousPoint, currentPoint);

                if (Global.currentLineMethod == Global.LineDrawingMethod.Bresenham)
                {
                    DrawLineBresenham(g, pen, new Point((int)previousPoint.X, (int)previousPoint.Y), new Point((int)currentPoint.X, (int)currentPoint.Y));
                }
                else if (Global.currentLineMethod == Global.LineDrawingMethod.Standard)
                {
                    g.DrawLine(pen, previousPoint, currentPoint);
                }
                else
                {
                    DrawLineWu(g, pen, new Point((int)previousPoint.X, (int)previousPoint.Y), new Point((int)currentPoint.X, (int)currentPoint.Y));
                }

                // Ustawiamy bieżący punkt jako poprzedni dla kolejnej iteracji
                previousPoint = currentPoint;
            }
        }

        public void DrawWithBezier(Graphics g, GraphicsPath? fillPath)
        {
            Pen pen = IsHighlighted ? new Pen(Color.Blue, 3) : new Pen(Color.Black, 1);

            if (IsBezier && ControlPoint1 != null && ControlPoint2 != null)
            {
                DrawBezierCurve(g, pen, Start, ControlPoint1, ControlPoint2, End);

                if (fillPath != null)
                {
                    fillPath.AddBezier(Start.Position, ControlPoint1.Position, ControlPoint2.Position, End.Position);
                }

                ControlPoint1.Draw(g);
                ControlPoint2.Draw(g);
                g.FillEllipse(Brushes.Green, ControlPoint1.Position.X - 3, ControlPoint1.Position.Y - 3, 6, 6);
                g.FillEllipse(Brushes.Green, ControlPoint2.Position.X - 3, ControlPoint2.Position.Y - 3, 6, 6);
            }
            else
            {
                if (Global.currentLineMethod == Global.LineDrawingMethod.Bresenham)
                {
                    DrawLineBresenham(g, pen, Start.Position, End.Position);
                }
                else if (Global.currentLineMethod == Global.LineDrawingMethod.Standard)
                {
                    g.DrawLine(pen, Start.Position, End.Position);
                }
                else
                {
                    DrawLineWu(g, pen, Start.Position, End.Position);
                }
            }
        }

        private PointF CalculateBezierPoint(float t, PointF p0, PointF p1, PointF p2, PointF p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            // Wzór na punkt na krzywej Beziera
            float x = (uuu * p0.X) + (3 * uu * t * p1.X) + (3 * u * tt * p2.X) + (ttt * p3.X);
            float y = (uuu * p0.Y) + (3 * uu * t * p1.Y) + (3 * u * tt * p2.Y) + (ttt * p3.Y);

            return new PointF(x, y);
        }

        public void DrawDashedLine(Graphics g, Punkt start, Punkt end, float dashLength, float gapLength)
        {
            float totalDistance = Distance(start, end);

            float deltaX = end.Position.X - start.Position.X;
            float deltaY = end.Position.Y - start.Position.Y;

            float directionX = deltaX / totalDistance;
            float directionY = deltaY / totalDistance;

            float currentLength = 0.0f;
            bool drawSegment = true;

            while (currentLength < totalDistance)
            {
                float segmentLength = drawSegment ? dashLength : gapLength;

                if (currentLength + segmentLength > totalDistance)
                {
                    segmentLength = totalDistance - currentLength;
                }

                float startX = start.Position.X + directionX * currentLength;
                float startY = start.Position.Y + directionY * currentLength;
                float endX = start.Position.X + directionX * (currentLength + segmentLength);
                float endY = start.Position.Y + directionY * (currentLength + segmentLength);

                if (drawSegment)
                {
                    if (Global.currentLineMethod == Global.LineDrawingMethod.Bresenham)
                    {
                        DrawLineBresenham(g, Pens.Black, new Point((int)startX, (int)startY), new Point((int)endX, (int)endY));
                    }
                    else if (Global.currentLineMethod == Global.LineDrawingMethod.Standard)
                    {
                        g.DrawLine(Pens.Black, new Point((int)startX, (int)startY), new Point((int)endX, (int)endY));
                    }
                    else
                    {
                        DrawLineWu(g, Pens.Black, new Point((int)startX, (int)startY), new Point((int)endX, (int)endY));
                    }
                }

                currentLength += segmentLength;
                drawSegment = !drawSegment;
            }
        }

        float Distance(Punkt p1, Punkt p2)
        {
            return (float)Math.Sqrt(Math.Pow(p2.Position.X - p1.Position.X, 2) + Math.Pow(p2.Position.Y - p1.Position.Y, 2));
        }

        public void ApplyContingency(Krawedz previousEdge, Krawedz nextEdge)
        {
            if (previousEdge != null)
            {
                if (previousEdge.IsBezier && previousEdge.ControlPoint2 != null)
                {
                    if (Start.contingency == ContingencyType.G1)
                    {
                        Punkt Control1 = NearestPointOnLine(Start.Position, previousEdge.ControlPoint2.Position, ControlPoint1.Position);
                        double maxDistance = Math.Max(Distance(Control1, Start), Distance(Start, previousEdge.ControlPoint2));
                        if (Distance(Control1, previousEdge.ControlPoint2) < maxDistance)
                        {
                            ControlPoint1 = new Punkt(2*Start.Position.X - Control1.Position.X,
                                2*Start.Position.Y - Control1.Position.Y);
                        }
                        else
                        {
                            ControlPoint1 = Control1;
                        }
                    }
                    else if (Start.contingency == ContingencyType.C1)
                    {
                        float distance = Distance(Start, previousEdge.ControlPoint2);
                        var normalizedVector = Normalize(new PointF(
                            Start.Position.X - previousEdge.ControlPoint2.Position.X,
                            Start.Position.Y - previousEdge.ControlPoint2.Position.Y
                        ));

                        ControlPoint1 = new Punkt(
                            Start.Position.X + (int)(normalizedVector.X * distance),
                            Start.Position.Y + (int)(normalizedVector.Y * distance)
                        );
                    }
                }
                else
                {
                    float prevEdgeLength = Distance(previousEdge.Start, previousEdge.End);
                    if (Start.contingency == ContingencyType.G1 || Start.contingency == ContingencyType.C1)
                    {
                        ControlPoint1 = NearestPointOnLine(previousEdge.Start.Position, previousEdge.End.Position, ControlPoint1.Position);

                        if (Start.contingency == ContingencyType.C1)
                        {
                            int prevDeltaX = previousEdge.End.Position.X - previousEdge.Start.Position.X;
                            int prevDeltaY = previousEdge.End.Position.Y - previousEdge.Start.Position.Y;
                            var normalizedPrevVector = Normalize(new PointF(prevDeltaX, prevDeltaY));
                            ControlPoint1 = new Punkt(
                                Start.Position.X + (int)(normalizedPrevVector.X * prevEdgeLength / 3),
                                Start.Position.Y + (int)(normalizedPrevVector.Y * prevEdgeLength / 3)
                            );
                        }
                    }
                }
            }

            if (nextEdge != null)
            {
                if (nextEdge.IsBezier && nextEdge.ControlPoint1 != null)
                {
                    if (End.contingency == ContingencyType.G1)
                    {
                        Punkt Control2 = NearestPointOnLine(End.Position, nextEdge.ControlPoint1.Position, ControlPoint2.Position);
                        double maxDistance = Math.Max(Distance(Control2, End), Distance(End, nextEdge.ControlPoint1));
                        if (Distance(Control2, nextEdge.ControlPoint1) <  maxDistance)
                        {
                            ControlPoint2 = new Punkt(2 * End.Position.X - Control2.Position.X,
                                2 * End.Position.Y - Control2.Position.Y);
                        }
                        else
                        {
                            ControlPoint2 = Control2;
                        }
                    }
                    else if (End.contingency == ContingencyType.C1)
                    {
                        float distance = Distance(End, nextEdge.ControlPoint1);

                        var normalizedVector = Normalize(new PointF(
                            End.Position.X - nextEdge.ControlPoint1.Position.X,
                            End.Position.Y - nextEdge.ControlPoint1.Position.Y
                        ));

                        ControlPoint2 = new Punkt(
                            End.Position.X + (int)(normalizedVector.X * distance),
                            End.Position.Y + (int)(normalizedVector.Y * distance)
                        );
                    }
                }
                else
                {
                    float nextEdgeLength = Distance(nextEdge.Start, nextEdge.End);
                    if (End.contingency == ContingencyType.G1 || End.contingency == ContingencyType.C1)
                    {
                        ControlPoint2 = NearestPointOnLine(nextEdge.Start.Position, nextEdge.End.Position, ControlPoint2.Position);
                        if (End.contingency == ContingencyType.C1)
                        {
                            int nextDeltaX = nextEdge.Start.Position.X - nextEdge.End.Position.X;
                            int nextDeltaY = nextEdge.Start.Position.Y - nextEdge.End.Position.Y;
                            var normalizedNextVector = Normalize(new PointF(nextDeltaX, nextDeltaY));
                            ControlPoint2 = new Punkt(
                                End.Position.X + (int)(normalizedNextVector.X * nextEdgeLength / 3),
                                End.Position.Y + (int)(normalizedNextVector.Y * nextEdgeLength / 3)
                            );
                        }
                    }
                }
            }
            ControlPoint1.isControl = true;
            ControlPoint2.isControl = true;
        }

        private Punkt NearestPointOnLine(Point lineStart, Point lineEnd, Point point)
        {
            float dx = lineEnd.X - lineStart.X;
            float dy = lineEnd.Y - lineStart.Y;
            float lengthSquared = dx * dx + dy * dy;

            if (lengthSquared == 0)
                return new Punkt(lineStart.X, lineStart.Y);

            float t = ((point.X - lineStart.X) * dx + (point.Y - lineStart.Y) * dy) / lengthSquared;

            return new Punkt(
                (int)(lineStart.X + t * dx),
                (int)(lineStart.Y + t * dy)
            );
        }

        private PointF Normalize(PointF vector)
        {
            float length = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            return new PointF(vector.X / length, vector.Y / length);
        }

        public bool IsMouseOverControlPoint(Point mouseLocation, Punkt controlPoint)
        {
            int tolerance = 7;
            return Math.Abs(mouseLocation.X - controlPoint.Position.X) <= tolerance &&
                   Math.Abs(mouseLocation.Y - controlPoint.Position.Y) <= tolerance;
        }

    }

}

