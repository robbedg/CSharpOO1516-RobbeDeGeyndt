using LandscapeGeneratorInterface;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace LandscapeGeneratorImplementation
{
    public class LandscapeGenerator : ILandscapeGenerator
    {

        private List<Point> pointList = new List<Point>();
        private Random random = new Random(); 

        private List<Point> CalculateNextIteration(List<Point> points, int heightStep)
        {
            List<Point> newList = new List<Point>();
            newList.Add(points[0]);
            for (int i = 1; i < points.Count; i++)
            {
                Point startPoint = points[i - 1];
                Point endPoint = points[i];
                // calculate new midpoint                
                int newX = (startPoint.X + endPoint.X) / 2;
                int deltaY = random.Next(-heightStep, heightStep);
                int newY = ((startPoint.Y + endPoint.Y) / 2) + deltaY;
                Point midPoint = new Point(newX, newY);
                newList.Add(midPoint);
                newList.Add(endPoint);
            }
            return newList;
        }

        public List<Point> PointList {
            get { return pointList; }
        }

        public void ResetPointList(int width, int height)
        {
            pointList = new List<Point>();
            Point startPoint = new Point(0, height / 2);
            Point endPoint = new Point(width, height / 2);
            pointList.Add(startPoint);
            pointList.Add(endPoint);
        }

        public void CalculateLandscape(int nrOfIterations, int heightStep)
        {
            Point temp = pointList[0];
            for (int i = 0; i <= (nrOfIterations - 2); i++)
            {
                pointList.Add(temp);
            }
            CalculateNextIteration(pointList, heightStep);
        }
    }
}
