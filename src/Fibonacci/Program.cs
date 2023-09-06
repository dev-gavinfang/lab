class Program
{
  static async Task Main(string[] args)
  {
    if (args?.Length == 0 || !int.TryParse(args![0], out var n))
    {
      Console.WriteLine("Please provide a number");
      return;
    }

    var task1 = Fib(n);
    var task2 = Fib(n);

    var completedTask = await Task.WhenAny(task1, task2);

    if (completedTask == task1)
    {
      Console.WriteLine("Task 1 finished first");
      await task2;
    }
    else
    {
      Console.WriteLine("Task 2 finished first");
      await task1;
    }

    Console.WriteLine($"Fib({n}) = {completedTask.Result}");
  }

  static async Task<int> Fib(int n)
  {
    var radomMilliseconds = new Random().Next(1000);
    await Task.Delay(radomMilliseconds);

    if (n <= 1)
      return n;

    var minus1 = Fib(n - 1);
    var minus2 = Fib(n - 2);

    return await minus1 + await minus2;
  }
}
