namespace SIS.Framework.Attributes.Properties
{
    using System.ComponentModel.DataAnnotations;

    public class NumberRangeAttribute : ValidationAttribute
    {
        private readonly double minValue;
        private readonly double maxValue;

        public NumberRangeAttribute(double minValue = double.MinValue, double maxValue = double.MaxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            return this.minValue <= (double)value && this.maxValue >= (double)value;
        }
    }
}
