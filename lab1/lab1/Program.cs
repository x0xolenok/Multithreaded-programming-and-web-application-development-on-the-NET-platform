namespace lab1
{
    static class ParallelQuickSort
    {
        // Maximum allowed threads 
        private static readonly int MaxThreads = Environment.ProcessorCount;
        // Counter for active threads
        private static int _activeThreads;
        // Lock object to synchronize access to ActiveThreads
        private static readonly Lock LockObject = new ();

        private static void QuickSort(int[] arr, int left, int right)
        {
            if (left >= right) return; // Base case of recursion

            var pivot = Partition(arr, left, right);

            var leftThreadCreated = false;
            var rightThreadCreated = false;
            Thread? leftThread = null;
            Thread? rightThread = null;

            // Try to sort the left partition in parallel if allowed
            lock (LockObject)
            {
                if (_activeThreads < MaxThreads)
                {
                    _activeThreads++;
                    leftThreadCreated = true;
                    leftThread = new Thread(() => QuickSort(arr, left, pivot - 1));
                    leftThread.Start();
                }
            }

            // Try to sort the right partition in parallel if allowed
            lock (LockObject)
            {
                if (_activeThreads < MaxThreads)
                {
                    _activeThreads++;
                    rightThreadCreated = true;
                    rightThread = new Thread(() => QuickSort(arr, pivot + 1, right));
                    rightThread.Start();
                }
            }

            // If left partition was processed in a separate thread, wait for it and decrement the counter
            if (leftThreadCreated)
            {
                leftThread?.Join();
                lock (LockObject)
                {
                    _activeThreads--;
                }
            }
            else
            {
                // Otherwise, process it sequentially
                QuickSort(arr, left, pivot - 1);
            }

            // If right partition was processed in a separate thread, wait for it and decrement the counter
            if (rightThreadCreated)
            {
                rightThread?.Join();
                lock (LockObject)
                {
                    _activeThreads--;
                }
            }
            else
            {
                // Otherwise, process it sequentially
                QuickSort(arr, pivot + 1, right);
            }
        }

        // Partition method: rearranges elements and returns the pivot index
        private static int Partition(int[] arr, int left, int right)
        {
            var pivot = arr[right]; // Choose the pivot element (last element in subarray)
            var i = left - 1;

            for (var j = left; j < right; j++)
            {
                if (arr[j] >= pivot) continue;
                i++;
                Swap(arr, i, j);
            }

            Swap(arr, i + 1, right);
            return i + 1; // Return the pivot position
        }

        private static void Swap(int[] arr, int i, int j)
        {
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }

        private static void Main()
        {
            Console.Write("Enter the length of the array: ");
            var length = int.Parse(Console.ReadLine() ?? "10");

            var arr = new int[length];
            var random = new Random();
            for (var i = 0; i < length; i++)
            {
                arr[i] = random.Next(0, 101);
            }

            Console.WriteLine("Initial array: " + string.Join(", ", arr));

            QuickSort(arr, 0, arr.Length - 1);

            Console.WriteLine("Sorted array: " + string.Join(", ", arr));
        }
    }
}
