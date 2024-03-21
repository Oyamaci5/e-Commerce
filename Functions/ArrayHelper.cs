namespace ParanumusTask.Functions
{
	public class ArrayHelper
	{
		public static double CalculateAverage(int[] numbers)
		{
			if (numbers == null)
			{
				throw new ArgumentNullException(nameof(numbers), "The input array is null.");
			}

			if (numbers.Length == 0)
			{
				throw new ArgumentException("The input array is empty.", nameof(numbers));
			}

			int sum = 0;
			foreach (int num in numbers)
			{
				sum += num;
			}

			return (double)sum / numbers.Length;
		}
	}

}
