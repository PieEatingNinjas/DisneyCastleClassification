using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Storage;
using Windows.AI.MachineLearning.Preview;

// 53979b10-4cb6-417f-9f57-a85d70709fbc_44105f29-1f46-48b2-b0ad-7d42d639cb20

namespace DisneyCastleClassification
{
    public sealed class _x0035_3979b10_x002D_4cb6_x002D_417f_x002D_9f57_x002D_a85d70709fbc_44105f29_x002D_1f46_x002D_48b2_x002D_b0ad_x002D_7d42d639cb20ModelInput
    {
        public VideoFrame data { get; set; }
    }

    public sealed class _x0035_3979b10_x002D_4cb6_x002D_417f_x002D_9f57_x002D_a85d70709fbc_44105f29_x002D_1f46_x002D_48b2_x002D_b0ad_x002D_7d42d639cb20ModelOutput
    {
        public IList<string> classLabel { get; set; }
        public IDictionary<string, float> loss { get; set; }
        public _x0035_3979b10_x002D_4cb6_x002D_417f_x002D_9f57_x002D_a85d70709fbc_44105f29_x002D_1f46_x002D_48b2_x002D_b0ad_x002D_7d42d639cb20ModelOutput()
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

    public sealed class _x0035_3979b10_x002D_4cb6_x002D_417f_x002D_9f57_x002D_a85d70709fbc_44105f29_x002D_1f46_x002D_48b2_x002D_b0ad_x002D_7d42d639cb20Model
    {
        private LearningModelPreview learningModel;
        public static async Task<_x0035_3979b10_x002D_4cb6_x002D_417f_x002D_9f57_x002D_a85d70709fbc_44105f29_x002D_1f46_x002D_48b2_x002D_b0ad_x002D_7d42d639cb20Model> Create_x0035_3979b10_x002D_4cb6_x002D_417f_x002D_9f57_x002D_a85d70709fbc_44105f29_x002D_1f46_x002D_48b2_x002D_b0ad_x002D_7d42d639cb20Model(StorageFile file)
        {
            LearningModelPreview learningModel = await LearningModelPreview.LoadModelFromStorageFileAsync(file);
            _x0035_3979b10_x002D_4cb6_x002D_417f_x002D_9f57_x002D_a85d70709fbc_44105f29_x002D_1f46_x002D_48b2_x002D_b0ad_x002D_7d42d639cb20Model model = new _x0035_3979b10_x002D_4cb6_x002D_417f_x002D_9f57_x002D_a85d70709fbc_44105f29_x002D_1f46_x002D_48b2_x002D_b0ad_x002D_7d42d639cb20Model();
            model.learningModel = learningModel;
            return model;
        }
        public async Task<_x0035_3979b10_x002D_4cb6_x002D_417f_x002D_9f57_x002D_a85d70709fbc_44105f29_x002D_1f46_x002D_48b2_x002D_b0ad_x002D_7d42d639cb20ModelOutput> EvaluateAsync(_x0035_3979b10_x002D_4cb6_x002D_417f_x002D_9f57_x002D_a85d70709fbc_44105f29_x002D_1f46_x002D_48b2_x002D_b0ad_x002D_7d42d639cb20ModelInput input) {
            _x0035_3979b10_x002D_4cb6_x002D_417f_x002D_9f57_x002D_a85d70709fbc_44105f29_x002D_1f46_x002D_48b2_x002D_b0ad_x002D_7d42d639cb20ModelOutput output = new _x0035_3979b10_x002D_4cb6_x002D_417f_x002D_9f57_x002D_a85d70709fbc_44105f29_x002D_1f46_x002D_48b2_x002D_b0ad_x002D_7d42d639cb20ModelOutput();
            LearningModelBindingPreview binding = new LearningModelBindingPreview(learningModel);
            binding.Bind("data", input.data);
            binding.Bind("classLabel", output.classLabel);
            binding.Bind("loss", output.loss);
            LearningModelEvaluationResultPreview evalResult = await learningModel.EvaluateAsync(binding, string.Empty);
            return output;
        }
    }
}
