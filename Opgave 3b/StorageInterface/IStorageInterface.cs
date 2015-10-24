using System.Drawing;
using System.Collections.Generic;

namespace StorageInterface
{
    public interface IStorageInterface
    {
        void SaveLandscape(string name, List<Point> landscape);

        List<Point> LoadLandscape(string landscape);
    }
}
