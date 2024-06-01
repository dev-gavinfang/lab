using Orleans;

namespace OrleansContracts
{
    public interface ICounterGrain : IGrainWithStringKey
    {
        ValueTask<int> Get();
        ValueTask<int> Increment();
    }
}
