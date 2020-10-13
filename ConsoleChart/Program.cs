using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Linq;
using System.Security;

namespace ConsoleChart
{
    class Program
    {
        static void Main(string[] args)
        {
            var groupIndex = -1;
            var numericIndex = -1;
            GetNumericAndGroupIndex(args, ref groupIndex, ref numericIndex);
            Dictionary<string, int> chart = ReadValuesAndSort(numericIndex, groupIndex);
            var numberOfLines = args.Length > 2 ? Int32.Parse(args[2]) : chart.Count;
            PrintChart(numberOfLines, chart);
        }

        static void GetNumericAndGroupIndex(string[] args, ref int groupIndex, ref int numericIndex)
        {
            var line = Console.ReadLine(); //First col is the desc
            var groups = line.Split('\t');
            for (int i = 0; i < groups.Length; i++)
            {
                if (groups[i] == args[0])
                {
                    groupIndex = i;
                }
                else if (groups[i] == args[1])
                {
                    numericIndex = i;
                }
            }
            if (numericIndex == -1 || groupIndex == -1 || args.Length < 2) { Console.WriteLine("Args are incorrect!"); Environment.Exit(-1); }
        }

        static Dictionary<string, int> ReadValuesAndSort(int numericIndex, int groupIndex)
        {
            var allValues = new Dictionary<string, int>();
            var line = String.Empty;
            while ((line = Console.ReadLine()) != null)
            {
                var splittedLine = line.Split('\t');
                if (allValues.ContainsKey(splittedLine[groupIndex]))
                {
                    allValues[splittedLine[groupIndex]] += Int32.Parse(splittedLine[numericIndex]);
                }
                else
                {
                    allValues.Add(splittedLine[groupIndex], Int32.Parse(splittedLine[numericIndex]));
                }
            }
            allValues = allValues.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); //Sort the value descending
            return allValues;
        }

        static void PrintChart(int numberOfLines, Dictionary<string, int> chart)
        {
            var maxValue = chart.Values.Max(); // The first item is the biggest value because the list is sorted descending
            for (int i = 0; i < numberOfLines; i++)
            {
                var characters = Convert.ToInt32((double)chart.ElementAt(i).Value / maxValue * 100); // currentValue / maxValue * 100
                var outChart = new char[characters];
                Console.Write($"{chart.ElementAt(i).Key,-70} |\t");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(outChart);
                Console.ResetColor();
                Console.WriteLine();
            }
        }
    }
}
