using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace lab8.Models
{
    public class SortModel
    {
    
        [Required(ErrorMessage = "Please enter numbers separated by commas.")]
        [Display(Name = "Enter numbers (comma-separated)")]
        public string InputNumbers { get; set; } = string.Empty;

        public string OriginalNumbers { get; set; } = string.Empty;
        public float[] SortedNumbers { get; set; } = [];

        public float[] BubbleSort()
        {
            var arr = this.InputNumbers.Split(',')
                .Select(n => float.Parse(n.Trim(), CultureInfo.InvariantCulture))
                .ToArray();

            var n = arr.Length;
            for (var i = 0; i < n - 1; i++)
            {
                var swapped = false;
                for (var j = 0; j < n - i - 1; j++)
                {
                    if (arr[j] <= arr[j + 1]) continue;
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]); // Swap
                    swapped = true;
                }

                if (!swapped) break; // Optimized: Stop if already sorted
            }

            return arr;
        }
    }

}