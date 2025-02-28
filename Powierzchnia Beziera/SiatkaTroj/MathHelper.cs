using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiatkaTroj
{
    public static class MathHelper
    {
        public static float ToRadians(float degrees) => (float)(Math.PI / 180) * degrees;
    }
}
