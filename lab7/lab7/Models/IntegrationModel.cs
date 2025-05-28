using System.ComponentModel.DataAnnotations;

namespace lab7.Models
{
    public class IntegrationModel : IValidatableObject
    {
        [Required]
        [Display(Name = "Lower Limit (A)")]
        public double LowerLimit { get; init; }

        [Required]
        [Display(Name = "Upper Limit (B)")]
        public double UpperLimit { get; init; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Sub-intervals must be at least 1")]
        [Display(Name = "Number of Sub-intervals (N)")]
        public int SubIntervals { get; init; }

        public double Result { get; set; }

        public double CalculateTrapezoidalIntegral()
        {
            var h = (this.UpperLimit - this.LowerLimit) / this.SubIntervals;
            var sum = (Function(this.LowerLimit) + Function(this.UpperLimit)) / 2.0;

            for (var i = 1; i < this.SubIntervals; i++)
            {
                var x = this.LowerLimit + i * h;
                sum += Function(x);
            }

            return sum * h;
        }

        private static double Function(double x)
        {
            return Math.Log10(x) * x + 2 * x + 1;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UpperLimit <= LowerLimit)
            {
                yield return new ValidationResult(
                    "Upper Limit (B) must be greater than Lower Limit (A).",
                    new[] { nameof(UpperLimit) }
                );
            }
        }
    }
}