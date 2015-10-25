using StorageInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace StorageImplementation
{
    public class Storage : IStorageInterface
    {
        public void SaveLandscape(string name, List<Point> landscape)
        {
            String filename = name + ".bin";
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);

            w.Write((int)2000);

            w.Close();
            fs.Close();
        }

        public List<Point> LoadLandscape(string landscape)
        {
            String filename = landscape + ".bin";
            if (File.Exists(filename))
            {
                var formatter = new BinaryFormatter();
                using (var bestand = File.OpenRead(filename))
                {
                    return (List<Point>)formatter.Deserialize(bestand);
                }
            }
            return new List<Point>();
        }

    }
    
}
