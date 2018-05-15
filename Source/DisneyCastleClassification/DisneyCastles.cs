using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.AI.MachineLearning.Preview;

// 53979b10-4cb6-417f-9f57-a85d70709fbc_44105f29-1f46-48b2-b0ad-7d42d639cb20

namespace DisneyCastleClassification
{
    public sealed class ModelInput
    {
        public VideoFrame data { get; set; }
    }

    public sealed class ModelOutput
    {
        public IList<string> classLabel { get; set; }
        public IDictionary<string, float> loss { get; set; }
        public ModelOutput()
        {
            this.classLabel = new List<string>();
            this.loss = new Dictionary<string, float>()
            {
                { "Anaheim", float.NaN },
                { "Paris", float.NaN },
                { "Shanghai", float.NaN },
            };
        }
    }

    public sealed class Model
    {
        private LearningModelPreview learningModel;
        public static async Task<Model> CreateModel(StorageFile file)
        {
            LearningModelPreview learningModel = await LearningModelPreview.LoadModelFromStorageFileAsync(file);
            Model model = new Model();
            model.learningModel = learningModel;
            return model;
        }
        public async Task<ModelOutput> EvaluateAsync(ModelInput input)
        {
            ModelOutput output = new ModelOutput();
            LearningModelBindingPreview binding = new LearningModelBindingPreview(learningModel);
            binding.Bind("data", input.data);
            binding.Bind("classLabel", output.classLabel);
            binding.Bind("loss", output.loss);
            LearningModelEvaluationResultPreview evalResult = await learningModel.EvaluateAsync(binding, string.Empty);
            return output;
        }
    }
}
