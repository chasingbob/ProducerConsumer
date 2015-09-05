using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;

namespace Demo
{
    public class ImageFetcher 
    {
        int _batchSize;
        string[] _images;
        int _pointer = 0;
        int _size = 0;
        string _extension;

        public ImageFetcher()
        {
            _batchSize = 1;
        }

        public ImageFetcher(int batchSize, string location, string extension)
        {
            _batchSize = batchSize;
            _extension = extension;
            _images = Directory.GetFiles(location);
        }

        public async Task<List<ImageFilename>> FetchNextBatchAsync()
        {
            var images = new List<ImageFilename>();
            var count = 0;
            await Task.Run(() =>
            {
                for (int i = _pointer; i < _pointer + _batchSize && i < _images.Count(); i++)
                {
                    count++;
                    _size++;
                    var filename = _images[i];

                    images.Add(new ImageFilename() { Filename = filename, Img = Image.FromFile(filename) });
                }
            });
            _pointer += count;

            Debug.WriteLine("Total Fetch count: {0}", _size);

            return images;
        }
    }
}