using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static System.Int32;

namespace TestStringCalculator
{
    [TestFixture]
    public class TestStringCalculator
    {
        [Test]
        public void Add_Given_EmptyString_ShouldReturn0()
        {
            //---------------Set up test pack-------------------
            const string input = "";
            const int expected = 0;
            var calculate = new Calculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Add_Given_StringOfSingleNumber_ShouldReturnItself()
        {
            //---------------Set up test pack-------------------
            const string input = "1";
            const int expected = 1;
            var calculate = new Calculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Add_Given_CommaDelimitedString_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "1,2";
            const int expected = 3;
            var calculate = new Calculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Add_Given_NewLineDelimitedString_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "1\n2,3";
            const int expected = 6;
            var calculate = new Calculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Add_Given_CustomDelimiter_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "//;\n1;2,3";
            const int expected = 6;
            var calculate = new Calculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Add_Given_NegativeNumbers_ShouldThrow()
        {
            //---------------Set up test pack-------------------
            const string input = "//;\n1;2,-3";
            const string expected = "Negative Numbers Not Allowed";
            var calculate = new Calculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = Assert.Throws<Exception>(() => calculate.Add(input));
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result.Message);
        }

        [Test]
        public void Add_Given_NumbersGreaterThanOneThousand_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "2000,3";
            const int expected = 3;
            var calculate = new Calculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Add_Given_CustomerDelimiterStringGreaterThan1Character_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "//[***]\n1***2***3";
            const int expected = 6;
            var calculate = new Calculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void Add_Given_MultipleCustomDelimiterString_ShouldReturnSum()
        {
            //---------------Set up test pack-------------------
            const string input = "//[*][%]\n1*2%3";
            const int expected = 6;
            var calculate = new Calculate();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = calculate.Add(input);
            //---------------Test Result -----------------------
            Assert.AreEqual(expected, result);
        }
    }

    public class Calculate
    {
        public int Add(string input)
        {
            var defaultDelimiters = DefaultDelimiters();
            if (!defaultDelimiters.Any(input.Contains)) return string.IsNullOrEmpty(input) ? 0 : ConvertToInt(input);
            if (input.StartsWith("//"))
            {
                var splitStrings = GetStringParts(input);
                var customDelimiter = GetCustomDelimiter(splitStrings[0]);
                defaultDelimiters.Add(customDelimiter);
                if (input.Contains("[") & input.Contains("]"))
                {
                    
                        var customDelimiterString = GetCustomDelimiterString(splitStrings[0]);
                        defaultDelimiters.AddRange(customDelimiterString);
                    
                   
                }
                input = splitStrings[1];
            }

            var listOfNumbers = SplitString(input,defaultDelimiters);
            CheckForNegativeNumbers(listOfNumbers);
            var checkForNumbersGreaterThanOneThousand = CheckForNumbersGreaterThanOneThousand(listOfNumbers);
            listOfNumbers = checkForNumbersGreaterThanOneThousand;
            var sum = Sum(listOfNumbers);
            return sum;
        }

        private static string[] GetStringParts(string input)
        {
            return input.Split('\n');
        }
        
        private static List<string> GetCustomDelimiterString(string splitString)
        {
            var trimStart = splitString.Substring(2,splitString.Length-2);
            var delimiter = trimStart.Split('[', ']').ToList();  
            return delimiter;
        }

        private static List<int> CheckForNumbersGreaterThanOneThousand(List<int> listOfNumbers)
        {
            for (var i = 0; i < listOfNumbers.Count; i++)
            {
                if (listOfNumbers[i] > 1000)
                {
                    listOfNumbers[i] = 0;
                }
            }
            return listOfNumbers;
        }

      
        private void CheckForNegativeNumbers(List<int> listOfNumbers)
        {
            if (listOfNumbers.Any(t => t < 0))
            {
                throw new Exception("Negative Numbers Not Allowed");
            }
        }

        private string GetCustomDelimiter(string splitString)
        {
            return splitString.Last().ToString();
        }

        private static List<int> SplitString(string input, List<string> DefaultDelimiters)
        {
            var Numbers= input.Split(DefaultDelimiters.ToArray(), StringSplitOptions.None);

            var list = ConvertToIntAndList(Numbers);
            return list;
        }

        private static List<int> ConvertToIntAndList(string[] Numbers)
        {
            return Numbers.Select(Parse).ToList();
        }

        private static List<string> DefaultDelimiters()
        {
            return new List<string> {",","\n"};
        }

        private static int Sum(List<int> listOfNumbers)
        {
            return listOfNumbers.Sum();
        }

        private static int ConvertToInt(string input)
        {
            return int.Parse(input);
        }
    }

}   