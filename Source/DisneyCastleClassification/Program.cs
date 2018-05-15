using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Graphics.Imaging;
using Windows.Media;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using System.Linq;

namespace DisneyCastleClassification
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running...");

            Run().ContinueWith((_) => Console.WriteLine("Done!"));

            Console.ReadKey();
        }

        private static async Task Run()
        {
            StorageFile modelFile =
                await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/DisneyCastles.onnx"));
            Model model = await Model.CreateModel(modelFile);

            foreach (StorageFile image in await GetImagesFromExecutingFolder())
            {
                Console.Write($"Processing {image.Name}... ");
                var videoFrame = await GetVideoFrame(image);

                ModelInput ModelInput = new ModelInput();
                ModelInput.data = videoFrame;

                var ModelOutput = await model.EvaluateAsync(ModelInput);
                var topCategory = ModelOutput.loss.OrderByDescending(kvp => kvp.Value).FirstOrDefault();

                Console.Write($"DONE ({topCategory.Key} {topCategory.Value:P2})\n");

                await UpdateImageTagMetadata(await image.Properties.GetImagePropertiesAsync(), topCategory.Key);
            }
        }

        private static async Task<IEnumerable<StorageFile>> GetImagesFromExecutingFolder()
        {
            var folder = await StorageFolder.GetFolderFromPathAsync(Environment.CurrentDirectory);

            var queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery, new List<string>() { ".jpg", ".png" });

            return await folder.CreateFileQueryWithOptions(queryOptions)?.GetFilesAsync();
        }

        private static async Task UpdateImageTagMetadata(ImageProperties imageProperties, params string[] tags)
        {
            var propertiesToSave = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("System.Keywords", String.Join(';', tags))
            };
            await imageProperties.SavePropertiesAsync(propertiesToSave);
        }

        private static async Task<VideoFrame> GetVideoFrame(StorageFile file)
        {
            SoftwareBitmap softwareBitmap;
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                // Create the decoder from the stream 
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                // Get the SoftwareBitmap representation of the file in BGRA8 format
                softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

                return VideoFrame.CreateWithSoftwareBitmap(softwareBitmap);
            }
        }
    }
}