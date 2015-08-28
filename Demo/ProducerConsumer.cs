using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace Demo
{
    public class ProducerConsumer
    {
        public bool IsRunning { get; set; }
        private readonly BlockingCollection<ImageFilename> _queue = new BlockingCollection<ImageFilename>();
        private ImageFetcher fetcher = new ImageFetcher(2, @"c:\temp\images", "*.jpg");

        public void Start()
        {
            IsRunning = true;

            //  Fetch
            Task.Run(async () =>
            {
                while (IsRunning)
                {
                    if (_queue.Count() < 2)
                    {
                        var imgs = await fetcher.FetchNextBatchAsync();

                        if (!imgs.Any())
                        {
                            _queue.CompleteAdding();
                            IsRunning = false;
                        }
                        else
                            imgs.ForEach(img => _queue.Add(img));
                    }

                    Thread.Sleep(10);
                }
            });

            Action process = () =>
            {
                while (IsRunning)
                {
                    foreach (var item in _queue.GetConsumingEnumerable())
                    {
                        var grayImg = ImageManipulater.ToGrayScale(item.Img);
                        grayImg.Save(item.Filename + ".gray.jpg");
                    }
                }
            };

            //  Single Process
            //  Parallel.Invoke(process);
            //  Concurrent
            Parallel.Invoke(process, process,process,process);

            Console.ReadKey();
        }


        public void Stop()
        {
            IsRunning = false;
        }
    }

    public class ImageFilename
    {
        public string Filename { get; set; }
        public Image Img { get; set; }
    }
}
