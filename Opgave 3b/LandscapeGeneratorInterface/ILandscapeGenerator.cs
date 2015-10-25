using System.Collections.Generic;
using System.Drawing;

namespace LandscapeGeneratorInterface
{
    public interface ILandscapeGenerator
    {
        List<Point> PointList { get;}

        void ResetPointList(int width, int height);

        void CalculateLandscape(int nrOfIterations, int heightStep);

        void SaveLandscape(string name);

        void LoadLandscape(string name);

    }
}
