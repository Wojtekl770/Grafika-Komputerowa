using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiatkaTroj
{
    public class Edge
    {
        public float x; // współrzędna x punktu przecięcia z aktualną scan-linią
        public float dx; // odwrotność nachylenia krawędzi (1/m)
        public int ymax; // najwyższy punkt krawędzi
    }
}
