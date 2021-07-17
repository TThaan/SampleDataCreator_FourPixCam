using System;
using System.IO;
using System.Threading.Tasks;

namespace SampleDataCreator_FourPixCam
{
    class Program
    {

        // Example usage
        static async Task Main(string[] args)
        {
            // Create samples

            IFourPixCamSampleSetFactory sampleFactory = new FourPixCamSampleSetFactory();
            await sampleFactory.CreateSamplesAsync(10, .2f); //, .3f

            // Save samples

            string fileName = Path.Combine(Path.GetTempPath(), "FourPixCamSamples");
            fileName = await sampleFactory.SaveSamplesAsCSVAsync(fileName, 5, true);

            // Load and show samples

            var loadedSamples = await sampleFactory.LoadSamplesAsync(fileName);
            Console.WriteLine(loadedSamples);
            Console.ReadKey();
        }
    }
}