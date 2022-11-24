using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Talygen.Azure;
using TALYGEN.Subscription;

namespace TALYGEN.Helpers
{
    public class CloudHelpers
    {
        BlobHelper BlobHelper;


        public CloudHelpers()
        {
            string _azureStorageAccount = "", _azureStoragePrimaryKey = "", _azureBlobLink = "";
            //var list = TALYGEN.Models.ConfigurationRepository.GetConfigurationSettings(0, 0, "CloudStorage", null);
            //foreach (var keyValuePair in list)
            //{
            //    switch (keyValuePair.CoreConfigKey)
            //    {
            //        case "AzureStorageAccount":
            //            {
            //                _azureStorageAccount = keyValuePair.ConfigDataValue;
            //            }
            //            break;

            //        case "AzureStoragePrimaryKey":
            //            {
            //                _azureStoragePrimaryKey = keyValuePair.ConfigDataValue;
            //            }
            //            break;
            //        case "AzureBlobLink":
            //            {
            //                _azureBlobLink = keyValuePair.ConfigDataValue;
            //            }
            //            break;
            //    }
            //}
            BlobHelper = new BlobHelper(_azureStorageAccount, _azureStoragePrimaryKey, _azureBlobLink);
        }

        public CloudHelpers(string storageAccount, string storageKey, string storageLink)
        {
            BlobHelper = new BlobHelper(storageAccount, storageKey, storageLink);
        }
        /// <summary>
        /// Created By:Gourav Rampal
        /// Created On:6th Aug 2012
        /// Desc: Methord used for conversion of byte stream from file
        /// </summary>
        /// <param name="fullFilePath">string</param>
        /// <returns>byte</returns>
        public static byte[] GetBytesFromFile(string fullFilePath)
        {
            FileStream fs = File.OpenRead(fullFilePath);
            try
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                return bytes;
            }
            finally
            {
                fs.Close();
            }

        }

        /// <summary>
        /// Created By:Gourav Rampal
        /// Created On:6th Aug 2012
        /// Desc: Method used for Creating Container along with its permissions
        /// </summary>
        /// <param name="containerName">Container1</param>
        /// <param name="accesslevel">Container</param>
        /// <param name="startdate">Nullable also if exit it must be in UTC eg:2012-08-01T09:38:05Z</param>
        /// <param name="enddate">Nullable also if exit it must be in UTC eg:2012-12-31T09:38:05Z</param>
        /// <param name="permission">r=read,rw=readwrite,rwd=readwritedelete,rwdl=readwritedeletelist</param>
        /// <returns></returns>
        public bool CreateContainer(string containerName, string accesslevel, string startdate, string enddate, string permission, out string updatedcontainername)
        {
            //Shift container name to lower case replaces spaces with hifen "-"
            var updatecontainer = getreplaceconatinername(containerName);
            if (BlobHelper.CreateContainer(updatecontainer))
            {
                try
                {
                    updatedcontainername = updatecontainer;
                    return UpdateContainerAccessPolicy(updatedcontainername, accesslevel, startdate, enddate, permission);

                }
                catch
                {
                    updatedcontainername = "";
                    return false;
                }
            }
            else
            {
                updatedcontainername = "";
                return false;
            }
        }
        public bool CreateContainer(long companyId, string companyName, string accesslevel, string startdate, string enddate, string permission, out string updatedcontainername)
        {
            updatedcontainername = "";
            try
            {
                //Shift container name to lower case replaces spaces with hifen "-"
                var containerName = Convert.ToString(companyName);
                var strCompanyId = Convert.ToString(companyId);
                //replace wide spaces with "-" dash and make the length to 55
                containerName = companyName.ToLowerInvariant().Replace(" ", "-").Replace(".", "");
                //will add container for company in cloud
                if (companyName.Length > 55)
                    containerName = containerName.Substring(0, 55);
                // check lenght of containerName 
                if (containerName.Length == 0)
                    containerName = "ContainerOf";

                
                if ((containerName.Length + strCompanyId.Length) > 64)
                    containerName = containerName.Substring(0, 64 - (strCompanyId.Length + 2));

                containerName = containerName + "-" + strCompanyId;


                var updatecontainer = getreplaceconatinername(containerName);

                if (BlobHelper.CreateContainer(updatecontainer))
                {
                    try
                    {
                        updatedcontainername = updatecontainer;
                        var createStatus = UpdateContainerAccessPolicy(updatedcontainername, accesslevel, startdate, enddate, permission);
                        if (createStatus)
                        {
                            var objSub = new SubscriptionManager();
                            // Not in use. Now storage will be calculated on the bases of Subscription.
                            // var storage = _objpack.GetStorageSizeByUserCount(companyId, true);
                            // insert into company container table
                            return objSub.SaveConatinerName(companyId, updatedcontainername, "Azure", "", 0);
                        }
                    }
                    catch
                    {
                        updatedcontainername = "";
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                //ErrorHandlingClass.ErrorInsert("", null,
                //                             "Error to Create Container ... " + ex.Message + "..." +
                //                              (ex.InnerException == null ? "" : ex.InnerException.ToString()), "", "CreateContainer", "companyName--" + companyName);
            }
            return false;
        }

        /// <summary>
        /// Created By:Gourav Rampal
        /// Created On:6th Aug 2012
        /// Desc: List All Containers
        /// </summary>
        /// <returns></returns>
        public List<string> ListAllContainers()
        {
            return BlobHelper.ListContainers();
        }

        /// <summary>
        /// Created By:Gourav Rampal
        /// Created On:6th Aug 2012
        /// Desc: Method used for Saving File from full Filepath
        /// </summary>
        /// <param name="containername">string</param>
        /// <param name="filename">string</param>
        /// <param name="file">string</param>
        /// <returns>bool</returns>
        public bool Savefile(string containername, string filename, string filepath)
        {
            try
            {
                byte[] data = GetBytesFromFile(filepath);
                // By Surinderjit Singh.
                // If File size it more then 500KB will be uploaded to Async.
                // 2013-04-10
                //if (data.Length > (1024 * 500))
                //{
                //    return UploadDataToBlobAsync(data, filename, containername);
                //}
                return BlobHelper.PutBlob(containername, filename, data);
            }
            catch { return false; }
        }

        /// <summary>
        /// Created By:Hardeep Singh
        /// Created On:10th Aug 2012
        /// Desc: Method used for Saving File from Http Posted file
        /// </summary>
        /// <param name="containername">string</param>
        /// <param name="filename">string</param>
        /// <param name="file">string</param>
        /// <returns>bool</returns>
        public bool Savefile(string containername, string filename, HttpPostedFile file)
        {
            try
            {
                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    fileData = binaryReader.ReadBytes(file.ContentLength);
                }
                // By Surinderjit Singh.
                // If File size it more then 500KB will be uploaded to Async.
                // 2013-03-07
                //if (fileData.Length > (1024 * 500))
                //{
                //    return UploadDataToBlobAsync(fileData, filename, containername);
                //}
                return BlobHelper.PutBlob(containername, filename, fileData);
            }
            catch { return false; }
        }

        /// <summary>
        /// Created By:Gourav Rampal
        /// Created On:6th Aug 2012
        /// Desc: Method used for Saving File 
        /// </summary>
        /// <param name="containername"></param>
        /// <param name="filename"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool Savefile(string containername, string filename, HttpPostedFileBase file)
        {
            try
            {
                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    fileData = binaryReader.ReadBytes(file.ContentLength);
                }
                // By Surinderjit Singh.
                // If File size it more then 500KB will be uploaded to Async.
                // 2013-04-10
                var status = false;
                //if (fileData.Length > (1024 * 500))
                //{
                //    status = UploadDataToBlobAsync(fileData, filename, containername);
                //}
                //if (status)
                //    return status;
                //else
                    return BlobHelper.PutBlob(containername, filename, fileData);
            }
            catch(Exception ex) { return false; }
        }

        /// <summary>
        /// Created By:Gourav Rampal
        /// Created On:6th Aug 2012
        /// Desc: Methord used for saving file filesteam
        /// </summary>
        /// <param name="containername">string</param>
        /// <param name="filename">string</param>
        /// <param name="file">byte[]</param>
        /// <returns></returns>
        public bool Savefile(string containername, string filename, byte[] file)
        {
            try
            {
                // By Surinderjit Singh.
                // If File size it more then 500KB will be uploaded to Async.
                // 2013-04-10
                //if (file.Length > (1024 * 500))
                //{
                //    return UploadDataToBlobAsync(file, filename, containername);
                //}
                return BlobHelper.PutBlob(containername, filename, file);
            }
            catch { return false; }
        }

        /// <summary>
        /// Copy File From Cloud to Cloud.
        /// </summary>
        /// <CreatedBy>Surinderjit Singh</CreatedBy>
        /// <CreatedAt>2013-03-08</CreatedAt>
        /// <param name="containername"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="destFilePath"></param>
        /// <returns></returns>
        public bool CopyFile(string containername, string sourceFilePath, string destFilePath)
        {
            try
            {
                return BlobHelper.CopyBlob(containername, sourceFilePath, containername, destFilePath);
            }
            catch { return false; }
        }

        /// <summary>
        /// Created By:Gourav Rampal
        /// Created On:6th Aug 2012
        /// Desc: Methord used for deleting blob
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public bool DeleteFile(string containerName, string blobName)
        {
            return BlobHelper.DeleteBlob(containerName, blobName);
        }

        public bool DeleteFile(string blobName)
        {
            return BlobHelper.DeleteBlob(blobName);
        }

        /// <summary>
        /// Created By:Gourav Rampal
        /// Created On:6th Aug 2012
        /// Desc: List the files of container by container name
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public List<string> ListFiles(string containerName)
        {
            //list all files in a container           
            return BlobHelper.ListBlobs(containerName);
        }
        public List<BlobDetails> ListFilesNew(string containerName, string prefix)
        {
            //list all files in a container           
            return BlobHelper.ListBlobNew(containerName, prefix);
        }
        /// <summary>
        /// Created By:Gourav Rampal
        /// Created On:6th Aug 2012
        /// Desc: Get the permission of paticulare container
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public string Getpermission(string containerName)
        {
            return BlobHelper.GetContainerACL(containerName);
        }

        /// <summary>
        /// Created By:Gourav Rampal
        /// Created On:6th Aug 2012
        /// Methord used for updating permission of container
        /// </summary>
        /// <param name="containerName">string</param>
        /// <param name="accesslevel">string</param>
        /// <param name="startdate">Nullable also if exit it must be in UTC</param>
        /// <param name="enddate">Nullable also if exit it must be in UTC</param>
        /// <param name="permission">r= read,rw=readwrite,rwd=readwritedelete,rwdl=readwritedeletelist</param>
        /// <returns></returns>
        public bool UpdateContainerAccessPolicy(string containerName, string accesslevel, string startdate, string enddate, string permission)
        {
            string accessPolicyXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                                     "<SignedIdentifiers>\n" +
                                     "  <SignedIdentifier>\n" +
                                     "    <Id>acesspermission</Id>\n" +
                                     "    <AccessPolicy>\n" +
                                     "      <Start>" + startdate + "</Start>\n" +
                                     "      <Expiry>" + enddate + "</Expiry>\n" +
                                     "      <Permission>" + permission + "</Permission>\n" +
                                     "    </AccessPolicy>\n" +
                                     "  </SignedIdentifier>\n" +
                                     "</SignedIdentifiers>\n";


            if (BlobHelper.SetContainerACL(containerName, accesslevel.ToLowerInvariant()))
            {

                //if (BlobHelper.SetContainerAccessPolicy(containerName, accesslevel.ToLowerInvariant(), accessPolicyXml))
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}

                return true;
            }
            return false;

        }

        #region Download Files

        public byte[] downloadfilestream(string containerName, string blob)
        {
            return BlobHelper.GetBlobStream(containerName, blob);
        }

        public byte[] downloadfilestream(string blobPath)
        {
            return BlobHelper.GetBlobStream(blobPath);
        }

        #region Download Async
        public void DownloadBlobToFileAsync(string filePath, string container, string blobToDownload)
        {
            var worker = new Action<Stream, string, string>(ParallelDownloadFile);

            lock (_sync)
            {
                if (TaskIsRunning)
                    throw new InvalidOperationException("The control is currently busy.");

                AsyncOperation async = AsyncOperationManager.CreateOperation(null);
                var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                worker.BeginInvoke(fs, container, blobToDownload, TaskCompletedCallback, async);

                TaskIsRunning = true;

            }
            while (TaskIsRunning)
            {

            }
        }

        private void ParallelDownloadFile(Stream outputStream, string container, string blobToDownload)
        {
            int numThreads = 10;

            long blobLength = BlobHelper.GetBlobSize(container, blobToDownload);
            int bufferLength = GetBlockSize(blobLength);  // 4 * 1024 * 1024;
            long bytesDownloaded = 0;

            // Prepare a queue of chunks to be downloaded. Each queue item is a key-value pair 
            // where the 'key' is start offset in the blob and 'value' is the chunk length.
            Queue<KeyValuePair<long, int>> queue = new Queue<KeyValuePair<long, int>>();
            long offset = 0;
            while (blobLength > 0)
            {
                int chunkLength = (int)Math.Min(bufferLength, blobLength);
                queue.Enqueue(new KeyValuePair<long, int>(offset, chunkLength));
                offset += chunkLength;
                blobLength -= chunkLength;
            }

            int exceptionCount = 0;

            using (outputStream)
            {
                // Launch threads to download chunks.
                var tasks = new List<Task>();
                for (int idxThread = 0; idxThread < numThreads; idxThread++)
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        KeyValuePair<long, int> blockIdAndLength;

                        // A buffer to fill per read request.
                        byte[] buffer = new byte[bufferLength];

                        while (true)
                        {

                            // Dequeue block details.
                            lock (queue)
                            {
                                if (queue.Count == 0)
                                    break;

                                blockIdAndLength = queue.Dequeue();
                            }

                            try
                            {
                                using (Stream stream = BlobHelper.GetDownloadStream(container, blobToDownload, blockIdAndLength.Key, blockIdAndLength.Key + blockIdAndLength.Value - 1))
                                {
                                    int offsetInChunk = 0;
                                    int remaining = blockIdAndLength.Value;
                                    while (remaining > 0)
                                    {
                                        int read = stream.Read(buffer, offsetInChunk, remaining);
                                        lock (outputStream)
                                        {
                                            outputStream.Position = blockIdAndLength.Key + offsetInChunk;
                                            outputStream.Write(buffer, offsetInChunk, read);
                                        }
                                        offsetInChunk += read;
                                        remaining -= read;
                                        Interlocked.Add(ref bytesDownloaded, read);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // Add block back to queue
                                queue.Enqueue(blockIdAndLength);

                                exceptionCount++;
                                // If we have had more than 100 exceptions then break
                                if (exceptionCount == 100)
                                {
                                    throw new Exception("Received 100 exceptions while downloading." + ex.ToString());
                                }
                                if (exceptionCount >= 100)
                                {
                                    break;
                                }
                            }
                        }
                    }));
                }

                // Wait for all threads to complete downloading data.
                Task.WaitAll(tasks.ToArray());
            }
        }

        public async Task<bool> DownloadBlobToFile1Async(string filePath, string container, string blobToDownload)
        {
            try
            {
                var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await ParallelDownloadFileAsync(fs, container, blobToDownload);
                return true;
            }
            catch(Exception ex) {
                //ErrorHandlingClass.ErrorInsert("", null,
                //                                 "Error DownloadBlobToFile1Async ... " + ex.Message + "..." +
                //                                  (ex.InnerException == null ? "" : ex.InnerException.ToString()), "", "DownloadBlobToFile1Async", "companyName--" + container);
            }
            return false;
        }

        private async Task<bool> ParallelDownloadFileAsync(Stream outputStream, string container, string blobToDownload)
        {
            int numThreads = 10;

            long blobLength = BlobHelper.GetBlobSize(container, blobToDownload);
            int bufferLength = GetBlockSize(blobLength);  // 4 * 1024 * 1024;
            long bytesDownloaded = 0;

            // Prepare a queue of chunks to be downloaded. Each queue item is a key-value pair 
            // where the 'key' is start offset in the blob and 'value' is the chunk length.
            Queue<KeyValuePair<long, int>> queue = new Queue<KeyValuePair<long, int>>();
            long offset = 0;
            while (blobLength > 0)
            {
                int chunkLength = (int)Math.Min(bufferLength, blobLength);
                queue.Enqueue(new KeyValuePair<long, int>(offset, chunkLength));
                offset += chunkLength;
                blobLength -= chunkLength;
            }

            int exceptionCount = 0;

            using (outputStream)
            {
                // Launch threads to download chunks.
                var tasks = new List<Task>();
                for (int idxThread = 0; idxThread < numThreads; idxThread++)
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        KeyValuePair<long, int> blockIdAndLength;

                        // A buffer to fill per read request.
                        byte[] buffer = new byte[bufferLength];

                        while (true)
                        {

                            // Dequeue block details.
                            lock (queue)
                            {
                                if (queue.Count == 0)
                                    break;

                                blockIdAndLength = queue.Dequeue();
                            }

                            try
                            {
                                using (Stream stream = BlobHelper.GetDownloadStream(container, blobToDownload, blockIdAndLength.Key, blockIdAndLength.Key + blockIdAndLength.Value - 1))
                                {
                                    int offsetInChunk = 0;
                                    int remaining = blockIdAndLength.Value;
                                    while (remaining > 0)
                                    {
                                        int read = stream.Read(buffer, offsetInChunk, remaining);
                                        lock (outputStream)
                                        {
                                            outputStream.Position = blockIdAndLength.Key + offsetInChunk;
                                            outputStream.Write(buffer, offsetInChunk, read);
                                        }
                                        offsetInChunk += read;
                                        remaining -= read;
                                        Interlocked.Add(ref bytesDownloaded, read);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // Add block back to queue
                                queue.Enqueue(blockIdAndLength);

                                exceptionCount++;
                                // If we have had more than 100 exceptions then break
                                if (exceptionCount == 100)
                                {
                                    throw new Exception("Received 100 exceptions while downloading." + ex.ToString());
                                }
                                if (exceptionCount >= 100)
                                {
                                    break;
                                }
                            }
                        }
                    }));
                }
                

                // Wait for all threads to complete downloading data.
                //await Task.WaitAll(tasks.ToArray());
                await Task.WhenAll(tasks.ToArray());
                
            }
            return true;
        }

        #endregion
        #endregion

        public string getreplaceconatinername(string name)
        {
            // make it all lower case
            name = name.ToLowerInvariant();
            // remove entities
            name = Regex.Replace(name, @"&\/w+;,+=", "");
            // remove anything that is not letters, numbers, dash, or space
            name = Regex.Replace(name, @"[^a-z0-9\-\s]", "");
            // replace spaces
            name = name.Replace(" ", "");
            // collapse dashes
            name = Regex.Replace(name, @"-{2,}", "-");
            // trim excessive dashes at the beginning
            name = name.TrimStart(new[] { '-' });
            // remove trailing dashes
            name = name.TrimEnd(new[] { '-' });
            return name;
        }

        /// <summary>
        /// Created By:Gourav Rampal
        /// Created On:20/09/2012
        /// Desc: Methord used to return length of container in Mb's
        /// </summary>
        /// <param name="containername"></param>
        /// <returns></returns>
        public ContainerDetails GetContainerSizeDetails(string containername)
        {
            return BlobHelper.GetContainerSize(containername);
        }

        ///Created By:Gourav Rampal
        ///Created On:22/10/2012
        /// Checks the file exists or not.
        /// The URL of the remote file.
        /// True : If the file exits, False if file not exists
        public bool RemoteFileExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TURE if the Status code == 200
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

        #region Parallel Upload
        // Async events and properties
        public event EventHandler TransferCompleted;
        private bool TaskIsRunning = false;
        public bool IsBusy
        {
            get { return TaskIsRunning; }
        }
        private readonly object _sync = new object();

        /// <summary>
        /// Try to get close to 100K block size in order to offer good progress update response.
        /// Max blob size 200 GB
        /// Max block size 2 MB
        /// Max blocks 50,000    
        /// Created By: Gourav
        /// Created At: 2012-11-01
        /// </summary>
        /// <param name="fileSize">Size of file to upload</param>
        /// <returns>block size</returns>
        private int GetBlockSize(long fileSize)
        {
            const long KB = 1024;
            const long MB = 1024 * KB;
            const long GB = 1024 * MB;
            const long MAXBLOCKS = 50000;
            const long MAXBLOBSIZE = 200 * GB;
            const long MAXBLOCKSIZE = 2 * MB;

            long blocksize = 100 * KB;
            long blockCount;
            blockCount = ((int)Math.Floor((double)(fileSize / blocksize))) + 1;
            while (blockCount > MAXBLOCKS - 1)
            {
                blocksize += 100 * KB;
                blockCount = ((int)Math.Floor((double)(fileSize / blocksize))) + 1;
            }

            if (blocksize > MAXBLOCKSIZE)
            {
                throw new ArgumentException("Blob too big to upload.");
            }
            return (int)blocksize;
        }

        private void TaskCompletedCallback(IAsyncResult ar)
        {
            // get the original worker delegate and the AsyncOperation instance
            Action<Stream, string, string> worker = (Action<Stream, string, string>)((AsyncResult)ar).AsyncDelegate;
            AsyncOperation async = (AsyncOperation)ar.AsyncState;

            // finish the asynchronous operation
            worker.EndInvoke(ar);

            // clear the running task flag
            lock (_sync)
            {
                TaskIsRunning = false;
            }

            // raise the completed event
            async.PostOperationCompleted(state => OnTaskCompleted((EventArgs)state), new EventArgs());
        }

        protected virtual void OnTaskCompleted(EventArgs e)
        {
            if (TransferCompleted != null)
                TransferCompleted(this, e);
        }

        /// <summary>
        /// Uploads content to a blob using multiple threads.
        /// Created By: Surinderjit Singh
        /// Created At: 2012-11-30
        /// </summary>
        /// <param name="inputStream">Input Stream to Upload</param>
        /// <param name="blobName">Name of Blob</param>
        /// <param name="containername">Name of Container</param>
        private void ParallelUploadStream(Stream inputStream, string blobName, string containername)
        {

            // the optimal number of transfer threads
            int numThreads = 10;

            long fileSize = inputStream.Length;

            int maxBlockSize = GetBlockSize(fileSize);

            // Prepare a queue of blocks to be uploaded. Each queue item is a key-value pair where
            // the 'key' is block id and 'value' is the block length.
            var queue = new Queue<KeyValuePair<int, int>>();
            var blockList = new List<string>();
            int blockId = 0;
            while (fileSize > 0)
            {
                int blockLength = (int)Math.Min(maxBlockSize, fileSize);
                string blockIdString = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(
                string.Format("BlockId{0}", blockId.ToString("0000000"))));
                KeyValuePair<int, int> kvp = new KeyValuePair<int, int>(blockId++, blockLength);
                queue.Enqueue(kvp);
                blockList.Add(blockIdString);
                fileSize -= blockLength;
            }

            // Launch threads to upload blocks.
            var tasks = new List<Task>();

            for (int idxThread = 0; idxThread < numThreads; idxThread++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    KeyValuePair<int, int> blockIdAndLength;

                    using (inputStream)
                    {
                        while (true)
                        {
                            // Dequeue block details.
                            lock (queue)
                            {
                                if (queue.Count == 0)
                                    break;

                                blockIdAndLength = queue.Dequeue();
                            }

                            byte[] buff = new byte[blockIdAndLength.Value];
                            BinaryReader br = new BinaryReader(inputStream);

                            // move the file system reader to the proper position
                            inputStream.Seek(blockIdAndLength.Key * (long)maxBlockSize, SeekOrigin.Begin);
                            br.Read(buff, 0, blockIdAndLength.Value);

                            // Upload block.
                            string blockName = Convert.ToBase64String(BitConverter.GetBytes(blockIdAndLength.Key));
                            using (MemoryStream ms = new MemoryStream(buff, 0, blockIdAndLength.Value))
                            {
                                string blockIdString = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("BlockId{0}", blockIdAndLength.Key.ToString("0000000"))));
                                BlobHelper.PutBlock(containername, blobName, blockIdString, ms.ToArray());
                            }
                        }
                    }
                }));

            }

            // Wait for all threads to complete uploading data.
            Task.WaitAll(tasks.ToArray());

            // Commit the blocklist.
            BlobHelper.PutBlockList(containername, blobName, blockList.ToArray());

        }

        public bool UploadDataToBlobAsync(byte[] dataToUpload, string blobName, string containername)
        {
            try
            {
                var worker = new Action<Stream, string, string>(ParallelUploadStream);
                lock (_sync)
                {
                    if (TaskIsRunning)
                        throw new InvalidOperationException("The control is currently busy.");

                    AsyncOperation async = AsyncOperationManager.CreateOperation(null);
                    var ms = new MemoryStream(dataToUpload);
                    worker.BeginInvoke(ms, blobName, containername, TaskCompletedCallback, async);
                    TaskIsRunning = true;
                }
                while (TaskIsRunning)
                {

                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Created By:Surinderjit Singh
        /// Created On:2012-11-26
        /// Desc: Method used for saving file to cloud with parallel uploading.
        /// </summary>
        /// <param name="containername">Name of Container</param>
        /// <param name="filename">Name of File(Blob)</param>
        /// <param name="file">File in HttpPostedFileBase</param>
        /// <returns></returns>
        public bool SavefileAsync(string containername, string filename, HttpPostedFileBase file)
        {
            try
            {
                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    fileData = binaryReader.ReadBytes(file.ContentLength);
                }
                return UploadDataToBlobAsync(fileData, filename, containername);
            }
            catch { return false; }
        }

        /// <summary>
        /// Created By: Surinderjit Singh
        /// Created On: 2012-11-30
        /// Desc: Method used for saving file to cloud with parallel uploading.
        /// </summary>
        /// <param name="containername">Name of Container</param>
        /// <param name="filename">Name of File(Blob)</param>
        /// <param name="file">File in HttpPostedFile</param>
        /// <returns></returns>
        public bool SavefileAsync(string containername, string filename, HttpPostedFile file)
        {
            try
            {
                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    fileData = binaryReader.ReadBytes(file.ContentLength);
                }
                return UploadDataToBlobAsync(fileData, filename, containername);
            }
            catch { return false; }
        }

        /// <summary>
        /// Created By: Surinderjit Singh
        /// Created On: 2012-11-30
        /// Desc: Method used for saving file to cloud with parallel uploading from path of file.
        /// </summary>
        /// <param name="containername">Name of Container</param>
        /// <param name="filename">Name of File(Blob)</param>
        /// <param name="filepath">Path of file to upload</param>
        /// <returns></returns>
        public bool SavefileAsync(string containername, string filename, string filepath)
        {
            try
            {
                byte[] data = GetBytesFromFile(filepath);
                return UploadDataToBlobAsync(data, filename, containername);
            }
            catch { return false; }
        }

        /// <summary>
        /// Created By: Surinderjit Singh
        /// Created On: 2012-11-30
        /// Desc: Method used for saving file to cloud with parallel uploading from byte[].
        /// </summary>
        /// <param name="containername">Name of Container</param>
        /// <param name="filename">Name of File(Blob)</param>
        /// <param name="filepath">byte[] of file to upload</param>
        /// <returns></returns>
        public bool SavefileAsync(string containername, string filename, byte[] file)
        {
            try
            {
                return UploadDataToBlobAsync(file, filename, containername);
            }
            catch { return false; }
        }


        #endregion

        public long GetBlobSize(string container, string blob)
        {
            try
            {
                return BlobHelper.GetBlobSize(container, blob);
            }
            catch { return 0; }
        }

    }


}