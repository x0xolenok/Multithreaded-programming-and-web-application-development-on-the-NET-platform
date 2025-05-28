using System;
using System.Threading;
using System.Threading.Tasks;

namespace lab3
{
    static class ParallelQuickSort
    {
        // Maximum allowed threads
        private static readonly int MaxThreads = Environment.ProcessorCount;
        // Counter for active threads
        private static int _activeThreads;
        // Lock object to synchronize access to _activeThreads
        private static readonly object LockObject = new();
        // TaskFactory instance to create tasks
        private static readonly TaskFactory TaskFactory = new TaskFactory();

        private static void QuickSort(int[] arr, int left, int right)
        {
            if (left >= right) return; // Base case of recursion

            int pivot = Partition(arr, left, right);

            bool leftTaskCreated = false;
            bool rightTaskCreated = false;
            Task leftTask = null;
            Task rightTask = null;

            // Try to sort the left partition in parallel if allowed
            lock (LockObject)
            {
                if (_activeThreads < MaxThreads)
                {
                    _activeThreads++;
                    leftTaskCreated = true;
                    leftTask = TaskFactory.StartNew(() => QuickSort(arr, left, pivot - 1));
                }
            }

            // Try to sort the right partition in parallel if allowed
            lock (LockObject)
            {
                if (_activeThreads < MaxThreads)
                {
                    _activeThreads++;
                    rightTaskCreated = true;
                    rightTask = TaskFactory.StartNew(() => QuickSort(arr, pivot + 1, right));
                }
            }

            // If left branch is running in a task, wait for its completion and decrement thread counter
            if (leftTaskCreated)
            {
                leftTask.Wait();
                lock (LockObject)
                {
                    _activeThreads--;
                }
            }
            else
            {
                QuickSort(arr, left, pivot - 1);
            }

            // If right branch is running in a task, wait for its completion and decrement thread counter
            if (rightTaskCreated)
            {
                rightTask.Wait();
                lock (LockObject)
                {
                    _activeThreads--;
                }
            }
            else
            {
                QuickSort(arr, pivot + 1, right);
            }
        }

        private static int Partition(int[] arr, int left, int right)
        {
            int pivot = arr[right]; // Use the last element as pivot
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                if (arr[j] >= pivot) continue;
                i++;
                Swap(arr, i, j);
            }

            Swap(arr, i + 1, right);
            return i + 1; // Return new pivot position
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
                arr[i] = random.Next(0, 101); // Random numbers from 0 to 100
            }

            Console.WriteLine("Initial array: " + string.Join(", ", arr));

            QuickSort(arr, 0, arr.Length - 1);

            Console.WriteLine("Sorted array: " + string.Join(", ", arr));
        }
    }
}