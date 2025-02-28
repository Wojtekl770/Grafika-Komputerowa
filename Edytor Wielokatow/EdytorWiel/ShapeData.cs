using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdytorWiel
{
    [Serializable]
    public class PunktData
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsControl { get; set; }
        public ContingencyType Contingency { get; set; }
    }

    [Serializable]
    public class KrawedzData
    {
        public PunktData Start { get; set; }
        public PunktData End { get; set; }
        public bool IsBezier { get; set; }
        public PunktData? ControlPoint1 { get; set; }
        public PunktData? ControlPoint2 { get; set; }
        public ConstraintType Constraint { get; set; }
        public double? Length { get; set; }
        public bool UseBresenham { get; set; }
    }

    [Serializable]
    public class ShapeData
    {
        public List<PunktData> Punkty { get; set; } = new List<PunktData>();
        public List<KrawedzData> Krawedzie { get; set; } = new List<KrawedzData>();
    }
}
