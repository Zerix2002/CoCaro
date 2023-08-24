using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NguyenNgocThach_Tuan1
{
    internal class NuocDi
    {
        int x, y;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public NuocDi()
        {
            X = 0; Y = 0;
        }

        public NuocDi(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public NuocDi(NuocDi td)
        {
            this.X = td.X;
            this.Y = td.Y;
        }


    }
}
