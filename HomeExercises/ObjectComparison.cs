﻿using FluentAssertions;
using NUnit.Framework;

namespace HomeExercises
{
	public class ObjectComparison
    {
	    [Test]
	    [Description("Проверка текущего царя")]
	    [Category("ToRefactor")]
	    public void CheckCurrentTsar()
	    {
	        var actualTsar = TsarRegistry.GetCurrentTsar();
	        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
	            new Person("Vasili III of Russia", 28, 170, 60, null));

            actualTsar.ShouldBeEquivalentTo(expectedTsar, options => options
	                  .Excluding(s => s.SelectedMemberInfo.DeclaringType == typeof(Person) &&
                                      s.SelectedMemberInfo.Name == "Id"));
	    }

	    [Test]
		[Description("Альтернативное решение. Какие у него недостатки?")]
		public void CheckCurrentTsar_WithCustomEquality()
		{
			var actualTsar = TsarRegistry.GetCurrentTsar();
			var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
			new Person("Vasili III of Russia", 28, 170, 60, null));

			
			Assert.True(AreEqual(actualTsar, expectedTsar));
		    // Какие недостатки у такого подхода? 
            //
            // 1. При добавлении новых полей или свойств в класс Person
            // в альтернативном решении нужно будет дописать сравнения
            // в метод AreEqual.
            //
            // 2. При использовании FluentAssertions выводится понятное 
            // сообщение об ошибке (например: 'Expected member Age to be
            // 55, but found 54.'), а в альтернативном решении не будет 
            // понятно какое именно поле не соответствует.
        }

        private bool AreEqual(Person actual, Person expected)
		{
			if (actual == expected) return true;
			if (actual == null || expected == null) return false;
			return
			actual.Name == expected.Name
			&& actual.Age == expected.Age
			&& actual.Height == expected.Height
			&& actual.Weight == expected.Weight
			&& AreEqual(actual.Parent, expected.Parent);
		}
	}

	public class TsarRegistry
	{
		public static Person GetCurrentTsar()
		{
			return new Person(
				"Ivan IV The Terrible", 54, 170, 70,
				new Person("Vasili III of Russia", 28, 170, 60, null));
		}
	}

	public class Person
	{
		public static int IdCounter = 0;
		public int Age, Height, Weight;
		public string Name;
		public Person Parent;
		public int Id;

		public Person(string name, int age, int height, int weight, Person parent)
		{
			Id = IdCounter++;
			Name = name;
			Age = age;
			Height = height;
			Weight = weight;
			Parent = parent;
		}
	}
}
