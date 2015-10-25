using System;
using System.Collections.Generic;
using System.Drawing;
using StorageInterface;

namespace StorageTestImplementation
{
    public class StorageTest : IStorageInterface
    {
        private Dictionary<string, List<Point>> landscapeStore = new Dictionary<string, List<Point>>();

        public List<Point> LoadLandscape(string name)
        {
            if (landscapeStore.ContainsKey(name))
            {
                return landscapeStore[name];
            }
            else
            {
                throw new ArgumentException("Image does not exist");
            }
        }

        public void SaveLandscape(string name, List<Point> landscape)
        {
            if (landscapeStore.ContainsKey(name))
            {
                landscapeStore[name] = new List<Point>(landscape);
            }
            else
            {
                landscapeStore.Add(name, new List<Point>(landscape));                
            }
        }
    }
}
