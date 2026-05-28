namespace Benchmark.Domain.Dataset;

public enum SampleType
{
    Positive,  // Accepted – used for SFT
    Negative   // Failed attempt – used for DPO / RLHF
}
