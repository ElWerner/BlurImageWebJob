using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Accord;
using Accord.Imaging.Filters;
using System.Drawing;
using System.Drawing.Imaging;

namespace BlurImageWebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("messages-queue")] string message,
            [Blob("images-container/{queueTrigger}", FileAccess.Read)] Stream readStream,
            [Blob("images-container/{queueTrigger}", FileAccess.Write)] Stream writeStream, TextWriter log)
        {
            Bitmap bitmap = new Bitmap(readStream);

            Blur filter = new Blur();

            ImageFormat imageFormat = bitmap.RawFormat;
            filter.ApplyInPlace(bitmap);

            bitmap.Save(writeStream, imageFormat);

            log.WriteLine(message);
        }
    }
}
