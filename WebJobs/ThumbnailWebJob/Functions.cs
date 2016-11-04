using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using DevEvent.Data.ViewModels;
using Newtonsoft.Json;
using DevEvent.Data.Services;
using DevEvent.Data.Models;
using System.Drawing;
using System.Data.Entity;

namespace ThumbnailWebJob
{
    public class Functions
    {

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static async void ProcessQueueMessage([QueueTrigger("thumbrequestqueue")] string message, TextWriter log)
        {
            log.WriteLine(message);

            ApplicationDbContext dbContext = new ApplicationDbContext();
            IStorageService storageService =new AzureStorageService();
            IThumbnailService thumbnailService = new ThumbnailService();

            try
            {
                var item = JsonConvert.DeserializeObject<ThumbnailQueueItem>(message);

                var evt = dbContext.Events.Where(x => x.EventId == item.EventId).FirstOrDefault();
                if (evt == null) return;

                // Make thumbnail and save itto the blob
                var stream = new MemoryStream();
                var filestream = await storageService.DownloadBlobAsStreamAsync(stream, "images", item.Guid + "/" + item.FileName);

                Image srcimg = Image.FromStream(filestream);
                Image thumbimg = thumbnailService.CreateThumbnailImage(150, srcimg, true);

                thumbimg.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                await storageService.UploadBlobAsync(stream, item.Guid + "/" + item.FileName, "thumbs");
                var thumburl = Path.Combine(storageService.StorageBaseUrl, "thumbs/" + item.Guid + "/", item.FileName);
                evt.ThumbnailImageUrl = thumburl;

                dbContext.SaveChanges();

            }
            catch(Exception ex)
            {
                dbContext.Dispose();
            }

            dbContext.Dispose();

        }
    }
}
