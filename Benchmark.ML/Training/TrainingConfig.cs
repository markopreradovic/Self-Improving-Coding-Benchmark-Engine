namespace Benchmark.ML.Training;

public class TrainingConfig
{
    public const string Section = "Training";

    public string PythonPath { get; set; } = "python3";
    public string ScriptPath { get; set; } = "scripts/finetune.py";
    public string ModelsDirectory { get; set; } = "models";
    public string DatasetDirectory { get; set; } = "datasets";
    public int MinSamplesRequired { get; set; } = 10;
    public int TimeoutMinutes { get; set; } = 60;
    public string BaseModelName { get; set; } = "microsoft/phi-2";
}
