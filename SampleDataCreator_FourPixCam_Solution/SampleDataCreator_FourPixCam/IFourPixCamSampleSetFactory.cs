using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleDataCreator_FourPixCam
{
    public interface IFourPixCamSampleSetFactory
    {
        List<KeyValuePair<string, float[]>> SamplesList { get; set; }
        string SamplesText { get; set; }

        Task<List<KeyValuePair<string, float[]>>> CreateSamplesAsync(int samplesCount, float inputDistortion);
        Task<string> LoadSamplesAsync(string fileName, bool overWriteExistingFile = false);
        Task<string> SaveSamplesAsCSVAsync(string fileName, int columns, bool overWriteExistingFile = false);
        Task<string> SaveSamplesAsTSVAsync(string fileName, int columns, bool overWriteExistingFile = false);
    }
}