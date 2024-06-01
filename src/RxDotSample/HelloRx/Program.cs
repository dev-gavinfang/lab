using Intro2RxDotnet;
using System.Reactive.Linq;

Console.WriteLine("Hello, Rx!");

HelloObservable();
HelloObserver();

static void HelloObservable()
{
    const int total = 10;
    var source = Observable.Empty<int>()
        .Concat(Observable.Interval(TimeSpan.FromSeconds(1))
        .Select((_, i) => i))
        .Take(total);

    using var disposable = source
        .Quiescent(TimeSpan.FromSeconds(1))
        .Subscribe(data =>
        {
            Console.WriteLine(string.Join(",", data));

            if (data.LastOrDefault() == total - 1)
            {
                Console.WriteLine("press any key to quit");
            }
        });
}

static void HelloObserver()
{
    var observer = new MyObserver();

    observer.Run();
}

Console.ReadKey();