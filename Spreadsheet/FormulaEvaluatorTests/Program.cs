using System;
using static FormulaEvaluator.Evaluator;

namespace FormulaEvaluatorTests
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 1;
            int expected, actual;
            //Test 1
            actual = Evaluate("1 + 2 * 3 / 3", BasicLookup);
            expected = 3;
            Console.WriteLine("Test " + i++ + ": " + (actual == expected));

            //Test 2
            actual = Evaluate("1 - 2 * 3", BasicLookup);
            expected = -5;
            Console.WriteLine("Test " + i++ + ": " + (actual == expected));

            //Test 3
            actual = Evaluate("5 * 4 / 2", BasicLookup);
            expected = 10;
            Console.WriteLine("Test " + i++ + ": " + (actual == expected));

            //Test 4
            actual = Evaluate("10 * (2 - 3)", BasicLookup);
            expected = -10;
            Console.WriteLine("Test " + i++ + ": " + (actual == expected));

            //Test 5
            actual = Evaluate("1 + 3 / (7 - 5 + (13 - 4 * 3)) / 2", BasicLookup);
            expected = 1;
            Console.WriteLine("Test " + i++ + ": " + (actual == expected));

            //Test 6
            actual = Evaluate("8 + a1", BasicLookup);
            expected = 8;
            Console.WriteLine("Test " + i++ + ": " + (actual == expected));

            //Test 7
            actual = Evaluate("8 + a1", SimpleLookup);
            expected = 13;
            Console.WriteLine("Test " + i++ + ": " + (actual == expected));

            //Test 8
            actual = Evaluate("8 + 7 + 5 * 2 * (3 + 2 * (4 - 1) / 6) + 2", BasicLookup);
            expected = 57;
            Console.WriteLine("Test " + i++ + ": " + (actual == expected));

            //Test 9
            actual = Evaluate("8 + 7 + a1 * 2 * (3 + 2 * (4 - 1) / (b4 / 2)) + 2", SimpleLookup);
            expected = 57;
            Console.WriteLine("Test " + i++ + ": " + (actual == expected));

            //Test 10
            actual = Evaluate("1(+1)", BasicLookup);
            expected = 2;
            Console.WriteLine("Test " + i++ + ": " + (actual == expected));

            //Test 11
            try
            {
                Evaluate("1/0", BasicLookup);
                Console.WriteLine("Test " + i++ + ": Failed, exception not thrown");
            }
            catch(ArgumentException e)
            {
                Console.WriteLine("Test " + i++ + ": Passed, exception thrown succesfully");
            }

            //Test 11
            try
            {
                Evaluate("4 / (a1 / 2)", BasicLookup);
                Console.WriteLine("Test " + i++ + ": Failed, exception not thrown");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Test " + i++ + ": Passed, exception thrown succesfully");
            }

            //Test 11
            try
            {
                Evaluate("4 / (0 / 2)", BasicLookup);
                Console.WriteLine("Test " + i++ + ": Failed, exception not thrown");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Test " + i++ + ": Passed, exception thrown succesfully");
            }

            //Test 12
            try
            {
                Evaluate("4 /", BasicLookup);
                Console.WriteLine("Test " + i++ + ": Failed, exception not thrown");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Test " + i++ + ": Passed, exception thrown succesfully");
            }

            //Test 13
            try
            {
                Evaluate("+ 4", BasicLookup);
                Console.WriteLine("Test " + i++ + ": Failed, exception not thrown");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Test " + i++ + ": Passed, exception thrown succesfully");
            }

            //Test 14
            try
            {
                Evaluate("2 + 2 + )", BasicLookup);
                Console.WriteLine("Test " + i++ + ": Failed, exception not thrown");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Test " + i++ + ": Passed, exception thrown succesfully");
            }

            //Test 15
            try
            {
                Evaluate("-", BasicLookup);
                Console.WriteLine("Test " + i++ + ": Failed, exception not thrown");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Test " + i++ + ": Passed, exception thrown succesfully");
            }

            //Test 16
            try
            {
                Evaluate("a", BasicLookup);
                Console.WriteLine("Test " + i++ + ": Failed, exception not thrown");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Test " + i++ + ": Passed, exception thrown succesfully");
            }

            //Test 17
            try
            {
                Evaluate("a1 % 6", BasicLookup);
                Console.WriteLine("Test " + i++ + ": Failed, exception not thrown");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Test " + i++ + ": Passed, exception thrown succesfully");
            }

            //Test 18
            try
            {
                Evaluate("c1 /()", BasicLookup);
                Console.WriteLine("Test " + i++ + ": Failed, exception not thrown");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Test " + i++ + ": Passed, exception thrown succesfully");
            }

            //Test 20
            actual = Evaluate("(1)(1) +", BasicLookup);
            expected = 2;
            Console.WriteLine("Test " + i++ + ": " + (actual == expected));
            

            Console.Read();


        }

        static int BasicLookup(string var)
        {
            return 0;
        }

        static int SimpleLookup(string var)
        {
            if (var == "a1")
                return 5;
            else if (var == "b4")
                return 12;
            else
                return 0;
        }
    }
}
