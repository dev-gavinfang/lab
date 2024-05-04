using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Intro2RxDotnet;

static class RxExt
{
    public static IObservable<IList<T>> Quiescent<T>(this IObservable<T> src, TimeSpan minimumInactivityPeriod, IScheduler? scheduler = null)
    {
        scheduler ??= Scheduler.Default;

        var onoffs = src.SelectMany(_ => EmitOnOff(minimumInactivityPeriod, scheduler));

        var outstanding = onoffs.Scan(0, (total, delta) => total + delta);
        var zeroCrossings = outstanding.Where(total => total == 0);

        return src.Buffer(zeroCrossings);

        static IObservable<int> EmitOnOff(TimeSpan minimumInactivityPeriod, IScheduler scheduler)
        {
            return Observable.Return(1, scheduler)
                .Concat(Observable.Return(-1, scheduler).Delay(minimumInactivityPeriod, scheduler));
        }
    }
}