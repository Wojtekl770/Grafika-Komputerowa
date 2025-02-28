using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdytorWiel
{
    using System.Windows.Forms;

    public class BufferedPanel : Panel
    {
        public BufferedPanel()
        {
            // Włącz podwójne buforowanie
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

    }
}
