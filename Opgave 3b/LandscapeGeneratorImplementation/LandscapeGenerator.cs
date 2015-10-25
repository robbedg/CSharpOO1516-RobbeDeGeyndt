using LandscapeGeneratorInterface;
using StorageInterface;
//using StorageImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using StorageTestImplementation;

namespace LandscapeGeneratorImplementation
{
    public class LandscapeGenerator : ILandscapeGenerator
    {
        private IStorageInterface storageInterface = new StorageTest();

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
            int tempHeightStep = heightStep;
            for (int i = 0; i < nrOfIterations; i++)
            {
                List<Point> temp = CalculateNextIteration(pointList, tempHeightStep);
                pointList = temp;
                tempHeightStep = tempHeightStep / 2;
            }
        }

        public void SaveLandscape(string name)
        {
            storageInterface.SaveLandscape(name, pointList);
        }

        public void LoadLandscape(string name)
        {
            pointList = storageInterface.LoadLandscape(name);
        }
    }
}
