using _02_BitmapPlayground.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using FilterReferenceClass;
using System.Reflection;

namespace _02_BitmapPlayground
{

    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();

            PopulateFilterPicker();

            PopulateListBox();


        }

        private void PopulateFilterPicker()
        {
            FilterPickerBox.Items.Add(new RedFilter());
            FilterPickerBox.Items.Add(new GrayScaleFilter());
            FilterPickerBox.Items.Add(new MovingAverageFilter());

        }

        public object GetTheType(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);

            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }
            return null;
        }

        //2.6
        private void PopulateListBox()
        {

            foreach (Type t in Assembly.GetCallingAssembly().GetTypes())
            {

                if (t.GetInterface("IFilter") != null)
                {

                    FilterPickerBox.Items.Add(t.Name);

                    //object objMyClass = Activator.CreateInstance(t);
                    //label1.Text += objMyClass.ToString();
                    //MethodInfo mInfo = objMyClass.GetType().GetMethods().FirstOrDefault
                    //                   (method => method.Name == "RedFilterSystem.Reflection.MethodInfo[]"
                    //                    && method.GetParameters().Count() == 0);
                    //MovingAverageFilter myObject = new MovingAverageFilter();
                    //object result = mInfo.Invoke(myObject, null);

                }
            }
        }

        /// <summary>
        /// Applies a filter to an image.
        /// </summary>
        /// <param name="filter">The filter to apply. Must not be null.</param>
        /// <param name="image">The image to which the filter is applied.</param>
        /// <returns>A new image with the filter applied.</returns>
        private Image ApplyFilter(FilterReferenceClass.FilterReference.IFilter filter, Image image)
        {
            // Sanity check input
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            if (image == null)
                throw new ArgumentNullException(nameof(image));

            // Create a new bitmap from the existing image
            Bitmap result = new Bitmap(image);

            // Copy the pixel colors of the existing bitmap to a new 2D - color array for further processing.

            Color[,] colors = new Color[result.Width, result.Height];
            for (int x = 0; x < result.Width; x++)
                for (int y = 0; y < result.Height; y++)
                    colors[x, y] = result.GetPixel(x, y);

            // Apply the user defined filter.
            var filteredImageData = filter.Apply(colors);

            // Copy the resulting array back to the bitmap
            for (int x = 0; x < result.Width; x++)
                for (int y = 0; y < result.Height; y++)
                    result.SetPixel(x, y, filteredImageData[x, y]);

            return result;
        }

        private void ApplyFilterButton_Click(object sender, EventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            if (FilterPickerBox.SelectedItem is FilterReferenceClass.FilterReference.IFilter selectedFilter)
                PictureBoxFiltered.Image = ApplyFilter(selectedFilter, PictureBoxOriginal.Image);
            sw.Stop();
            //label1.Text = string.Format("{0:0,0} ms.", sw.ElapsedMilliseconds);
        }

    }

}
