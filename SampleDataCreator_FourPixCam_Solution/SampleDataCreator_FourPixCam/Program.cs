using System;
using System.Threading.Tasks;

namespace SampleDataCreator_FourPixCam
{
    class Program
    {
        static int samplesCount = 100;
        static float distortionRange = .2f;
        static string fileName = string.Empty;

        static async Task Main(string[] args)
        {
            // Parse parameters

            if (!int.TryParse(args[0], out samplesCount))
            {
                Console.WriteLine($"Execution failed: Cannot read parameter 'samplesCount'.");
                Console.WriteLine($"\nPress any key to close this window.");
                Console.ReadKey();
                return;
            }
            if (!float.TryParse(args[1], out distortionRange))
            {
                Console.WriteLine($"Execution failed: Cannot read parameter 'distortionRange'.");
                Console.WriteLine($"\nPress any key to close this window.");
                Console.ReadKey();
                return;
            }

            // Create samples

            IFourPixCamSampleSetFactory sampleFactory = new FourPixCamSampleSetFactory();
            await sampleFactory.CreateSamplesAsync(samplesCount, distortionRange);

            // Save samples
            
            fileName = args[2]; // fileName = Path.Combine(Path.GetTempPath(), "FourPixCam_TestData");
            fileName = await sampleFactory.SaveSamplesAsCSVAsync(fileName, 5, true);

            // Load and show samples

            var loadedSamples = await sampleFactory.LoadSamplesAsync(fileName);
            Console.WriteLine("Creating samples for a Four-Pixel-Camera neural net.\n");
            Console.WriteLine(loadedSamples);
            Console.WriteLine($"\n{samplesCount} samples created and saved as \n{fileName}.");
            Console.WriteLine($"Distortion range is {distortionRange}");
            Console.WriteLine($"\nPress any key to close this window.");
            Console.ReadKey();
        }
    }
}