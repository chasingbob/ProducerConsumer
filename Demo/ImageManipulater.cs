using AForge.Imaging.Filters;
using System.Drawing;

namespace Demo
{
    public class ImageManipulater
    {
        public static Image ToGrayScale(Image img)
        {
            var filterGreyScale = new Grayscale(0.2125, 0.7154, 0.0721);

            return filterGreyScale.Apply((Bitmap)img);
        }
    }
}
