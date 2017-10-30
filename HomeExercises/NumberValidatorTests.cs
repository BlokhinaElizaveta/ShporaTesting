using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercises
{
	public class NumberValidatorTests
	{
	    [TestCase(-3, 2, true, TestName = "Precision must be a positive number")]
	    [TestCase(3, -2, true, TestName = "Scale must be a non-negative number")]
	    [TestCase(3, 5, true, TestName = "Scale less or equal than precision")]
	    public void IncorrectArgs_Expect_ArgumentException(int precision, int scale, bool onlyPositive)
	    {
	        Action act = () => new NumberValidator(precision, scale, onlyPositive);
	        act.ShouldThrow<ArgumentException>();
	    }

        [TestCase(4, 2, true, "0", ExpectedResult = true, TestName = "Correct work without fracPart")]
        [TestCase(4, 2, true, "0,3", ExpectedResult = true, TestName = "Correct work with comma")]
        [TestCase(4, 2, true, "+0.3", ExpectedResult = true, TestName = "'+' in intPart")]
        [TestCase(3, 2, true, "00.00", ExpectedResult = false, TestName = "Long number")]
        [TestCase(4, 2, false, "-0.3", ExpectedResult = true, TestName = "Negative number")]
        [TestCase(4, 2, true, "-0.3", ExpectedResult = false, TestName = "Negative number with onlyPositive flag")]
        [TestCase(6, 2, true, "0.3333", ExpectedResult = false, TestName = "Long fracPart")]
        [TestCase(3, 2, true, "a.sd", ExpectedResult = false, TestName = "Incorrect value")]
        [TestCase(3, 2, true, "", ExpectedResult = false, TestName = "Value is null")]
        [TestCase(3, 2, true, null, ExpectedResult = false, TestName = "Value is empty")]
        public bool CorrectArgs_Expect_CorrectWork(int precision, int scale, bool onlyPositive, string value)
        {
            var validator = new NumberValidator(precision, scale, onlyPositive);
            return validator.IsValidNumber(value);
	    }
	}

	public class NumberValidator
	{
		private readonly Regex numberRegex;
		private readonly bool onlyPositive;
		private readonly int precision;
		private readonly int scale;

		public NumberValidator(int precision, int scale = 0, bool onlyPositive = false)
		{
			this.precision = precision;
			this.scale = scale;
			this.onlyPositive = onlyPositive;
			if (precision <= 0)
				throw new ArgumentException("precision must be a positive number");
			if (scale < 0 || scale >= precision)
				throw new ArgumentException("precision must be a non-negative number less or equal than precision");
			numberRegex = new Regex(@"^([+-]?)(\d+)([.,](\d+))?$", RegexOptions.IgnoreCase);
		}

		public bool IsValidNumber(string value)
		{
			// Проверяем соответствие входного значения формату N(m,k), в соответствии с правилом, 
			// описанным в Формате описи документов, направляемых в налоговый орган в электронном виде по телекоммуникационным каналам связи:
			// Формат числового значения указывается в виде N(m.к), где m – максимальное количество знаков в числе, включая знак (для отрицательного числа), 
			// целую и дробную часть числа без разделяющей десятичной точки, k – максимальное число знаков дробной части числа. 
			// Если число знаков дробной части числа равно 0 (т.е. число целое), то формат числового значения имеет вид N(m).

			if (string.IsNullOrEmpty(value))
				return false;

			var match = numberRegex.Match(value);
			if (!match.Success)
				return false;

			// Знак и целая часть
			var intPart = match.Groups[1].Value.Length + match.Groups[2].Value.Length;
			// Дробная часть
			var fracPart = match.Groups[4].Value.Length;

			if (intPart + fracPart > precision || fracPart > scale)
				return false;

			if (onlyPositive && match.Groups[1].Value == "-")
				return false;
			return true;
		}
	}
}