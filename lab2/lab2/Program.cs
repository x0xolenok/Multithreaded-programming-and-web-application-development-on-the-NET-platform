namespace lab2;

static class ParallelQuickSort
{
    private static readonly int MaxThreads = Environment.ProcessorCount;
    private static int _activeThreads;
    private static readonly object LockObject = new();

    private static async Task QuickSort(int[] arr, int left, int right)
    {
        if (left >= right) return; // Base case

        int pivot = Partition(arr, left, right);

        bool leftTaskCreated = false;
        bool rightTaskCreated = false;
        Task? leftTask = null;
        Task? rightTask = null;

        // Try to process left partition in a new Task if allowed
        lock (LockObject)
        {
            if (_activeThreads < MaxThreads)
            {
                _activeThreads++;
                leftTaskCreated = true;
                leftTask = Task.Run(() => QuickSort(arr, left, pivot - 1));
            }
        }

        // Try to process right partition in a new Task if allowed
        lock (LockObject)
        {
            if (_activeThreads < MaxThreads)
            {
                _activeThreads++;
                rightTaskCreated = true;
                rightTask = Task.Run(() => QuickSort(arr, pivot + 1, right));
            }
        }

        // Wait for the left Task if it was created; otherwise, process sequentially
        if (leftTaskCreated)
        {
            await leftTask!;
            lock (LockObject)
            {
                _activeThreads--;
            }
        }
        else
        {
            await QuickSort(arr, left, pivot - 1);
        }

        // Wait for the right Task if it was created; otherwise, process sequentially
        if (rightTaskCreated)
        {
            await rightTask!;
            lock (LockObject)
            {
                _activeThreads--;
            }
        }
        else
        {
            await QuickSort(arr, pivot + 1, right);
        }
    }

    private static int Partition(int[] arr, int left, int right)
    {
        int pivotValue = arr[right]; // choose pivot from end
        int i = left - 1;
        for (int j = left; j < right; j++)
        {
            if (arr[j] >= pivotValue) continue;
            i++;
            Swap(arr, i, j);
        }
        Swap(arr, i + 1, right);
        return i + 1;
    }

    private static void Swap(int[] arr, int i, int j)
    {
        (arr[i], arr[j]) = (arr[j], arr[i]);
    }

    private static async Task Main()
    {
        Console.Write("Enter the length of the array: ");
        var length = int.Parse(Console.ReadLine() ?? "10");

        var arr = new int[length];
        var random = new Random();
        for (int i = 0; i < length; i++)
        {
            arr[i] = random.Next(0, 101);
        }

        Console.WriteLine("Initial array: " + string.Join(", ", arr));
        await QuickSort(arr, 0, arr.Length - 1);
        Console.WriteLine("Sorted array: " + string.Join(", ", arr));
    }
}