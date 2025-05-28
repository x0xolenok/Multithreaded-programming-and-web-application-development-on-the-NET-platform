using System;
using System.Threading.Tasks;

namespace lab4
{
    static class ParallelQuickSort
    {
        // Create a ParallelOptions instance with MaxDegreeOfParallelism set to the number of processors.
        private static readonly ParallelOptions ParallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        private static void QuickSort(int[] arr, int left, int right)
        {
            if (left >= right) return;

            int pivot = Partition(arr, left, right);

            // Use Parallel.Invoke with specified ParallelOptions
            Parallel.Invoke(ParallelOptions,
                () => QuickSort(arr, left, pivot - 1),
                () => QuickSort(arr, pivot + 1, right)
            );
        }

        private static int Partition(int[] arr, int left, int right)
        {
            int pivot = arr[right];
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                if (arr[j] < pivot)
                {
                    i++;
                    Swap(arr, i, j);
                }
            }
            return i + 1;
        }

        private static void Swap(int[] arr, int i, int j)
        {
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }

        private static void Main()
        {
            Console.Write("Enter the length of the array: ");
            int length = int.Parse(Console.ReadLine() ?? "10");

            int[] arr = new int[length];
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                arr[i] = random.Next(0, 101);
            }

            Console.WriteLine("Generated array: " + string.Join(", ", arr));

            QuickSort(arr, 0, arr.Length - 1);

            Console.WriteLine("Sorted array: " + string.Join(", ", arr));
        }
    }
}