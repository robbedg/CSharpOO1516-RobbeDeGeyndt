using System.Drawing;
using System.Collections.Generic;

namespace DummyBackend
{
    public interface IStorageInterface
    {
        void SaveLandscape(string name, List<Point> landscape);

        List<Point> LoadLandscape(string landscape);
    }
}
