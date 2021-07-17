using DeepLearningDataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleDataCreator_FourPixCam
{
    public class FourPixCamSampleSetFactory : IFourPixCamSampleSetFactory
    {
        #region fields

        private enum Label { Undefined, AllBlack, AllWhite, LeftBlack, LeftWhite, SlashBlack, SlashWhite, TopBlack, TopWhite }
        private static Random rnd;

        #endregion

        #region IFourPixCamSampleSetFactory

        public List<KeyValuePair<string, float[]>> SamplesList { get; set; }
        public string SamplesText { get; set; }

        public async Task<List<KeyValuePair<string, float[]>>> CreateSamplesAsync(int samplesCount, float inputDistortion) //, float targetTolerance
        {
            return await Task.Run(() =>
            {
                rnd = RandomProvider.GetThreadRandom();
                // Sample.Tolerance = targetTolerance;

                // Create 'natural' input (2x2 matrix) with values 0 or 1 only.
                // The Keys represent the labels.
                Dictionary<string, float[,]> idealizedInputs = GetIdealizedInputs();

                // Multiply up to 'samplesCount' inputs are reached.
                // Since multiple identical keys are not allowed in a dictionary use IEnumerable<KeyValuePair>.
                IEnumerable<KeyValuePair<string, float[,]>> multipliedIdealizedInputs = GetMultipliedInput(idealizedInputs, samplesCount);

                // Flatten each 'natural' input into a one-dimensional array.
                IEnumerable<KeyValuePair<string, float[]>> flattenedInputs = GetFlattenedInputs(multipliedIdealizedInputs);

                // Distort Inputs
                IEnumerable<KeyValuePair<string, float[]>> distortedInputs = GetDistortedInputs(flattenedInputs, inputDistortion);

                // Transform the string-typed labels into one-hot vectors.
                //Dictionary<float[], float[]> oneHotEncodedOutputs = GetOneHotEncodedOutputs(flattenedInputs.Values);

                return SamplesList = distortedInputs.ToList();
            });
        }
        public async Task<string> LoadSamplesAsync(string fileName, bool overWriteExistingFile = false)
        {
            return SamplesText = await ImpEx.Import.LoadAsOriginalFileTextAsync(fileName);
        }
        public async Task<string> SaveSamplesAsCSVAsync(string fileName, int columns, bool overWriteExistingFile = false)
        {
            return await ImpEx.Export.SaveAsCSVAsync(SamplesList, fileName, columns, overWriteExistingFile);
        }
        public async Task<string> SaveSamplesAsTSVAsync(string fileName, int columns, bool overWriteExistingFile = false)
        {
            return await ImpEx.Export.SaveAsTSVAsync(SamplesList, fileName, columns, overWriteExistingFile);
        }

        #region helpers for 'CreateSamplesAsync(..)'

        static Dictionary<string, float[,]> GetIdealizedInputs()
        {
            return new Dictionary<string, float[,]>
            {
                [Label.AllBlack.ToString()] = new float[,] {
                    { -1, -1 },
                    { -1, -1 } },

                [Label.AllWhite.ToString()] = new float[,] {
                    { 1, 1 },
                    { 1, 1 } },

                [Label.TopBlack.ToString()] = new float[,] {
                    { -1, -1 },
                    { 1, 1 } },

                [Label.TopWhite.ToString()] = new float[,] {
                    { 1, 1 },
                    { -1, -1 } },

                [Label.LeftBlack.ToString()] = new float[,] {
                    { -1, 1 },
                    { -1, 1 } },

                [Label.LeftWhite.ToString()] = new float[,] {
                    { 1, -1 },
                    { 1, -1 } },

                [Label.SlashBlack.ToString()] = new float[,] {
                    { 1, -1 },
                    { -1, 1 } },

                [Label.SlashWhite.ToString()] = new float[,] {
                    { -1, 1 },
                    { 1, -1 } }
            };
        }
        /// <summary>
        /// Flattening two-dimensional raw input into one dimensional input.
        /// </summary>
        static IEnumerable<KeyValuePair<string, float[,]>> GetMultipliedInput(Dictionary<string, float[,]> data, int samplesCount)
        {
            IEnumerable<KeyValuePair<string, float[,]>> result = new List<KeyValuePair<string, float[,]>>();
            int multiplicationFactor = (int)Math.Round((double)samplesCount / data.Values.Count, 0); // Check: idealizedInputs.Values.Count = 8?

            for (int i = 0; i < multiplicationFactor; i++)
            {
                result = result.Concat(data);
            }

            return result;
        }
        static IEnumerable<KeyValuePair<string, float[]>> GetFlattenedInputs(IEnumerable<KeyValuePair<string, float[,]>> data)
        {
            return data
                .Select(x => new KeyValuePair<string, float[]>(x.Key, x.Value.ToList<float>().ToArray()));
        }
        static IEnumerable<KeyValuePair<string, float[]>> GetDistortedInputs(IEnumerable<KeyValuePair<string, float[]>> data, float inputDistortion)
        {
            return data.Select(x => new KeyValuePair<string, float[]>(x.Key, x.Value.Select(y => GetDistortedValue(y, inputDistortion)).ToArray()));
        }
        static float GetDistortedValue(float value, float inputDistortion)
        {
            return (float)(value * (1f - rnd.NextDouble() * inputDistortion));
        }

        #endregion

        #endregion
    }
}
