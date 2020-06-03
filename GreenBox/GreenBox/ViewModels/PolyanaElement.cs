using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox.ViewModels
{
    class PolyanaElement
    {
        public int ElementId { get; set; }
        public byte[] Image {get;set;}

        public double X { get; set; }

        public double Y { get; set; }

        public int Size { get; set; }

        public int Rotate { get; set; }

        public double CenterX { get; set; }

        public double CenterY { get; set; }

        public int BorderThinckessProperty { get; set; }

        public PolyanaElement(int id, byte[] image, double x, double y, int size, int rotate)
        {
            if (image == null || size == 0)
                return;
            ElementId = id;
            Image = image;
            X = x - size/2;
            Y = y - size/2;
            Size = size;
            Rotate = rotate;
            BorderThinckessProperty = 0;
            CenterX = size/2;
            CenterY = size/2;
        }

    }
}
