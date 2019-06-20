using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ZooFantasy.Stage
{
    class StageManager
    {
        public static async Task CreatCacheMode(string mode)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync("Cache") == null)
                await folder.CreateFolderAsync("Cache");
            folder = await folder.GetFolderAsync("Cache");
            if (await folder.TryGetItemAsync("CacheMode.txt") == null)
                await folder.CreateFileAsync("CacheMode.txt");
            StorageFile file = await folder.GetFileAsync("CacheMode.txt");
            using (Stream fileStream = await file.OpenStreamForWriteAsync())
            {
                using (StreamWriter write = new StreamWriter(fileStream))
                {
                    write.WriteLine(mode);
                }
            }
        }

        public static async Task CreatCacheStage(string stage)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync("Cache") == null)
                await folder.CreateFolderAsync("Cache");
            folder = await folder.GetFolderAsync("Cache");
            if (await folder.TryGetItemAsync("CacheStage.txt") == null)
                await folder.CreateFileAsync("CacheStage.txt");
            StorageFile file = await folder.GetFileAsync("CacheStage.txt");
            using (Stream fileStream = await file.OpenStreamForWriteAsync())
            {
                using (StreamWriter write = new StreamWriter(fileStream))
                {
                    write.WriteLine(stage);
                }
            }
        }

    }
}
