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
    /// <summary>
    /// Filters the red component from an image.
    /// </summary>
    /// 
    
   
    public class RedFilter: FilterReferenceClass.FilterReference.IFilter //2.5
    {
        public Color[,] Apply(Color[,] input)
        {
            int width = input.GetLength(0);
            int height = input.GetLength(1);
            Color[,] result = new Color[width, height];

            //Optimizing with Parallel.For (2.4)
            Parallel.For(0, width, x =>
            {
                Parallel.For(0, height, y =>
                {
                    var p = input[x, y];
                    result[x, y] = Color.FromArgb(p.A, 0, p.G, p.B);
                });
            });

            return result;
        }

        public string Name => "Red Filter";

        public override string ToString()
            => Name;
    }
}
