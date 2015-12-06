using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using LogicInterface;

namespace LogicImplementation
{
    public class ImageFilter : IImageFilter
    {
        private Bitmap image;
        private Bitmap output;

        public Image FilteredImage
        {
            get
            {
                return output;
            }
        }

        private delegate Color filterOperation(Color pixel);

        public void Load(string file)
        {
            // Laadt de opgegeven afbeelding in, 
            // stockeer het origineel in een private veld
            // en kopieer het naar een tweede bitmap waarin je de bewerkte versie zal opslaan 
            image = (Bitmap)Image.FromFile(file);
            output = (Bitmap)image.Clone();

        }

        public void ApplyFilter(Filter filterMode)
        {
            // Roep ExecuteFilter aan met een geschikte filterOperation 
            // op basis van de doorgegeven filtermode.
            // declareer die filterOperation telkens als een lambda expression
            filterOperation filteroperation;

            if (filterMode == Filter.Original)
            {
                filteroperation = pixel =>
                {
                    return Color.FromArgb(pixel.R, pixel.G, pixel.B);
                };
                ExecuteFilter(filteroperation);
            }
            else if (filterMode == Filter.GreyScale)
            {
                filteroperation = pixel =>
                {
                    byte x = (byte)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                    //http://www.had2know.com/technology/rgb-to-gray-scale-converter.html
                    return Color.FromArgb(x, x, x);
                };
                ExecuteFilter(filteroperation);
            }
            else if (filterMode == Filter.Threshold)
            {
                filteroperation = pixel =>
                {
                    byte x = (byte)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);
                    //http://www.had2know.com/technology/rgb-to-gray-scale-converter.html
                    x = (byte)(x < 128 ? 0 : 255);
                    return Color.FromArgb(x, x, x);
                };
                ExecuteFilter(filteroperation);
            }
            else if (filterMode == Filter.Invert)
            {
                filteroperation = pixel =>
                {
                    return Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                };
                ExecuteFilter(filteroperation);
            }
        }

        private void ExecuteFilter(filterOperation operation)
        {
            // calculate the new image by looping through all the pixels 
            // in the (original) image & apply the passed operation to them  
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    output.SetPixel(i, j, operation(image.GetPixel(i, j)));
                }
            }
        }

    }
}
