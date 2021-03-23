using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilterReferenceClass;
using System.Reflection;

namespace _02_BitmapPlayground.Filters
{
    public class GrayScaleFilter: FilterReferenceClass.FilterReference.IFilter //2.5
    {
        //Greyscale filter (2.2) and optimizing with Parallel.For (2.4)
        public Color[,] Apply(Color[,] input)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            Color[,] result = new Color[width, height];

            Parallel.For(0, width, x =>
            {
                Parallel.For(0, height, y =>
                {
                    var p = input[x, y];
                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;
                    int avg = (r + g + b) / 3;
                    result[x, y] = Color.FromArgb(a, avg, avg, avg);
                });
            });

            return result;
        }

        public string Name => "Greyscale Filter";

        public override string ToString()
            => Name;
       
    }

}


