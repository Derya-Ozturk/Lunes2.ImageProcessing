using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using FilterReferenceClass;
using System.Reflection;

namespace _02_BitmapPlayground.Filters
{
    public class MovingAverageFilter: FilterReferenceClass.FilterReference.IFilter //2.5
    {
        
        public Color[,] Apply(Color[,] input)
        {           
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            Color[,] result = new Color[width, height];

            //Moving average filter (2.3) and optimizing with Parallel.For (2.4)
            Parallel.For(0, width - 1, x =>
              {
                  Parallel.For(1, height - 1, y =>
                  {
                      var p = input[x, y];

                      //komşu pikseller alındı
                      var n1 = input[x, (y - 1)];
                      var n2 = input[(x + 1), y];
                      var n3 = input[x, (y + 1)];
                      var n4 = input[x, (y - 1)];

                      int avgR = (n1.R + n2.R + n3.R + n4.R) / 4;
                      int avgG = (n1.G + n2.G + n3.G + n4.G) / 4;
                      int avgB = (n1.B + n2.B + n3.B + n4.B) / 4;
                      result[x, y] = Color.FromArgb(p.A, avgR, avgG, avgB);
                  });

              });
              return result;
   
        }

        
        public string Name => "Moving Average Filter";

        public override string ToString()
            => Name;
    }
    
}
