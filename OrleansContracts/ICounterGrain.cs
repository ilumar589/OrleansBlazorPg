using Orleans;

namespace OrleansContracts;

[Alias("OrleansContracts.ICounterGrain")]
public interface ICounterGrain : IGrainWithStringKey
{
    [Alias("Get")]
    ValueTask<int> Get();
    [Alias("Increment")]
    ValueTask<int> Increment();
}
