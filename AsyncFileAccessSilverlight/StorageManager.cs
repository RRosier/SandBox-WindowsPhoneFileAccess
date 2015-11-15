using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncFileAccessSilverlight
{
    public class StorageManager : IDisposable
    {
        private IsolatedStorageFile isolatedStorageFile;

        public StorageManager()
        {
            this.isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
        }

        public async Task CreateFile(string fileName)
        {
            byte[] data = new byte[5 * 1024 * 1024];
            Random rng = new Random();
            rng.NextBytes(data);
            var memoryStream = new MemoryStream(data);

            using (var fileStream = this.isolatedStorageFile.OpenFile(fileName, FileMode.Create, FileAccess.ReadWrite))
            //using (var writer = new FileStream( new BinaryWriter(fileStream))
            {
                await fileStream.WriteAsync(data, 0, data.Length);
            }
        }

        public void Dispose()
        {
            this.isolatedStorageFile = null;
        }

        public static async Task CreateFileStatic(string fileName)
        {
            byte[] data = new byte[5 * 1024 * 1024];
            Random rng = new Random();
            rng.NextBytes(data);
            var memoryStream = new MemoryStream(data);

            //using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
            using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
            using (var fileStream = isolatedStorageFile.OpenFile(fileName, FileMode.Create, FileAccess.ReadWrite))
            //using (var writer = new FileStream( new BinaryWriter(fileStream))
            {
                await fileStream.WriteAsync(data, 0, data.Length);
            }
        }

        public static Stream ReadFileStatic(string fileName)
        {
            using (var isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isolatedStorageFile.FileExists(fileName))
                {
                    using (var isolatedStream = isolatedStorageFile.OpenFile(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        return isolatedStream;
                        ////// copy in a memory stream because no 2 threads can access isolated storage at the same time.
                        //var memoryStream = new MemoryStream();
                        //isolatedStream.CopyTo(memoryStream);
                        //return memoryStream;
                    }
                }

                return null;
            }
        }
    }
}
