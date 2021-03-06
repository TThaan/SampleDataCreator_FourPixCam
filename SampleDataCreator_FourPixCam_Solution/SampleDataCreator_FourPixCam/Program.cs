using System;
using System.IO;
using System.Threading.Tasks;

namespace SampleDataCreator_FourPixCam
{
    class Program
    {
        static int samplesCount = 2000;
        static float distortionRange = .2f;
        static string path = Path.GetTempPath();
        static string fileName = "FourPixCam_Samples";

        /// <summary>
        /// args[0] = amount of samples to create
        /// args[1] = distortion range of the samples (i.e. deviation from idealized values, here 1 and -1)
        /// args[2] = filename (incl full path) to save samples to
        /// </summary>
        static async Task Main(string[] args)
        {
            // Set parameters

            if(args == null)
                Console.WriteLine("Cannot read the 'Target' value (appended parameters) of the short cut to your exe file.");

            samplesCount = GetSamplesCount(args.Length > 0 ? args[0] : null);
            distortionRange = GetDistortionRange(args.Length > 1 ? args[1] : null);
            path = GetPathName(args.Length > 2 ? args[2] : null);

            // Create samples

            IFourPixCamSampleSetFactory sampleFactory = new FourPixCamSampleSetFactory();
            await sampleFactory.CreateSamplesAsync(samplesCount, distortionRange);

            // Save samples

            string fullFileName = Path.Combine(path, fileName);
            fullFileName = await sampleFactory.SaveSamplesAsCSVAsync(fullFileName, 5, true);

            // Load and show samples

            var loadedSamples = await sampleFactory.LoadSamplesAsync(fullFileName);
            Console.WriteLine("Creating samples for a Four-Pixel-Camera neural net.\n");
            Console.WriteLine(loadedSamples);
            Console.WriteLine($"\n{samplesCount} samples created and saved as \n{path}.");
            Console.WriteLine($"\nPress any key to close this window.");
            Console.ReadKey();
        }

        #region helpers

        private static int GetSamplesCount(string args0)
        {
            Console.WriteLine();

            if (!int.TryParse(args0, out int samplesCount))
            {
                Console.WriteLine("Cannot read parameter 'samplesCount'.");
                Console.WriteLine("Change the 'Target' value of the short cut to your exe file \n" +
                    $"or enter how many samples you want to create (default = 2000) :\n");

                var input = Console.ReadLine();
                return GetSamplesCount(input);
            }
            else
            {
                Console.WriteLine($"Amount of samples set to {samplesCount}.");
                return samplesCount;
            }
        }
        private static float GetDistortionRange(string args1)
        {
            Console.WriteLine();

            if (!float.TryParse(args1, out float distortionRange))
            {
                Console.WriteLine($"Cannot read parameter 'distortionRange'.");
                Console.WriteLine($"Change the 'Target' value of the short cut to your exe file \n" +
                    $"or enter the distortion range of the samples (default = 0,2) :\n");

                var input = Console.ReadLine();
                return GetDistortionRange(input);
            }
            else
            {
                Console.WriteLine($"Distortion range set to {distortionRange}.");
                return distortionRange;

            }
        }
        private static string GetPathName(string args2)
        {
            string result;
            Console.WriteLine();

            if (args2 == null)
            {
                Console.WriteLine($"Cannot read parameter 'path'.");
                result = path;
            }
            else
            {
                result = args2;
            }

            Console.WriteLine($"Samples will be saved into folder {result}.");
            Console.WriteLine($"\nPress any key to create samples.");
            Console.ReadKey();
            return result;
        }

        #endregion
    }
}