using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QU.api
{
    public class WordFinder
    {
        private char[] [] _wordMatrix;
        private ConcurrentBag<Tuple<string, int>> _wordsOccurrencies = new ConcurrentBag<Tuple<string, int>>();

        public WordFinder(IEnumerable<string> matrix)
        {
            var lineas = new List<char[]>();
            foreach (var item in matrix)
            {
                char[] linea = item.ToCharArray();
                lineas.Add(linea);
            }
            _wordMatrix = lineas.ToArray();
            ValidateSquareMatrix(_wordMatrix);
        }

        

        public IEnumerable<string> Find(IEnumerable<string> wordStream) {

            Task[] taskArray = new Task[2];
            taskArray[0] = Task.Factory.StartNew(() => CountWordsInMatrix(_wordMatrix, wordStream));
            var verticalMatrix = TransposeRowsAndColumns(_wordMatrix);
            taskArray[1] = Task.Factory.StartNew(() => CountWordsInMatrix(verticalMatrix, wordStream));

            Task.WaitAll(taskArray);

            var orderedResult = _wordsOccurrencies.Where(x => x.Item2 > 0)
                .GroupBy(x => x.Item1).Select(t => new { Word = t.Key, Count = t.Sum(z => z.Item2) })
                .OrderByDescending(x => x.Count)
                .Select(x => x.Word).Take(10).ToArray();

            return orderedResult;
        }

        private void ValidateSquareMatrix(char[][] wordMatrix)
        {
            int rowCount = _wordMatrix.Length;
            int columnCount = _wordMatrix.Length;

            if (rowCount != columnCount)
            {
                throw new Exception("The input is not correct");
            }
        }

        private void CountWordsInMatrix(char[][] wordMatrix, IEnumerable<string> wordStream)
        {
            foreach (var wordToFind in wordStream)
            {
                int count = 0;
                for (int i = 0; i < wordMatrix.Length; i++)
                {
                    count = CountOccurrencies(new string(wordMatrix[i]), wordToFind) + count;
                }
                _wordsOccurrencies.Add(new Tuple<string, int>(wordToFind, count));
            }
        }

        private int CountOccurrencies(string line, string wordToFind)
        {
            return Regex.Matches(line, wordToFind).Count;
        }

        private static char[][] TransposeRowsAndColumns(char[][] arr)
        {
            int rowCount = arr.Length;
            int columnCount = arr[0].Length;
            char[][] transposed = new char[columnCount][];
            for (int i = 1; i < rowCount; i++)
            {
                char[] tempRow = new char[rowCount];
                for (int j = 0; j < columnCount; j++)
                { 
                    char temp = arr[j][i];
                    tempRow[j] = temp;
                }
                transposed[i] = tempRow;
            }
            return transposed;
        }

    }
}
