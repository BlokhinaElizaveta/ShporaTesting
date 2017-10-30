using System;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Challenge
{
	[TestFixture]
	public class WordsStatistics_Tests
	{
		public static string Authors = "Блохина"; // "Egorov Shagalina"

		public virtual IWordsStatistics CreateStatistics()
		{
			// меняется на разные реализации при запуске exe
			return new WordsStatistics();
		}

		private IWordsStatistics statistics;

		[SetUp]
		public void SetUp()
		{
			statistics = CreateStatistics();
		}

		[Test]
		public void GetStatistics_IsEmpty_AfterCreation()
		{
			statistics.GetStatistics().Should().BeEmpty();
		}

		[Test]
		public void GetStatistics_ContainsItem_AfterAddition()
		{
			statistics.AddWord("abc");
			statistics.GetStatistics().Should().Equal(Tuple.Create(1, "abc"));
		}

		[Test]
		public void GetStatistics_ContainsManyItems_AfterAdditionOfDifferentWords()
		{
			statistics.AddWord("abc");
			statistics.AddWord("def");
			statistics.GetStatistics().Should().HaveCount(2);
		}

	    [Test]
	    public void GetStatistics_IsNullOrWhiteSpace()
	    {
	        statistics.AddWord("    ");
	        statistics.GetStatistics().Should().BeNullOrEmpty();
	    }


	    [Test]
	    public void GetStatistics_CorrectOrderCount()
	    {
	        statistics.AddWord("1");
	        statistics.AddWord("2");
	        statistics.AddWord("2");
	        var expected = new List<Tuple<int, string>> {Tuple.Create(2, "2"), Tuple.Create(1, "1")};
            statistics.GetStatistics().Should().Equal(expected);
	    }

	    [Test]
	    public void GetStatistics_CorrectOrder()
	    {
	        statistics.AddWord("b");
	        statistics.AddWord("a");
	        var expected = new List<Tuple<int, string>> { Tuple.Create(1, "a"), Tuple.Create(1, "b") };
	        statistics.GetStatistics().Should().Equal(expected);
	    }

	    [Test]
	    public void GetStatistics_Length11()
	    {
	        statistics.AddWord("          a");
	        statistics.GetStatistics().Should().Equal(Tuple.Create(1, "          "));
	    }


        [Test]
	    public void GetStatistics_AddNull_ThrowArgumentNullException()
	    {
	        Action act = () => statistics.AddWord(null);
	        act.ShouldThrow<ArgumentNullException>();
        }


	    [Test]
	    public void GetStatistics_Repeatedly()
	    {
	        statistics.AddWord("a");
	        statistics.GetStatistics().Should().Equal(Tuple.Create(1, "a"));

            statistics.AddWord("a");
	        statistics.GetStatistics().Should().Equal(Tuple.Create(2, "a"));
        }

	    [Test]
	    public void GetStatistics_ToLower()
	    {
	        statistics.AddWord("Ё");
	        statistics.GetStatistics().Should().Equal(Tuple.Create(1, "ё"));
        }

        [Test, MaxTime(40)]
        public void GetStatistics_TimeOut()
        {
            var str = new[] {"a", "b"};
            for (var i = 0; i < 20000; i++)
            {
                statistics.AddWord(str[i%2]);
            }

            var expected = new List<Tuple<int, string>> { Tuple.Create(10000, "a"), Tuple.Create(10000, "b") };
            statistics.GetStatistics().Should().Equal(expected);
        }

        [Test]
	    public void GetStatistics_CreateNewStat()
	    {
            statistics.AddWord("a");
	        var stat = CreateStatistics();
            statistics.GetStatistics().Should().Equal(Tuple.Create(1, "a"));
        }

	    [Test]
	    public void GetStatistics_Hash()
	    {
	        statistics.AddWord("krumld");
            statistics.AddWord("xqzrbn");
	        

	        var expected = new List<Tuple<int, string>> { Tuple.Create(1, "krumld") , Tuple.Create(1, "xqzrbn") };
	        statistics.GetStatistics().Should().Equal(expected);
        }

	    // Документация по FluentAssertions с примерами : https://github.com/fluentassertions/fluentassertions/wiki
    }
}