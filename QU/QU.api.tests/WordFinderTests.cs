using System;
using Xunit;

namespace QU.api.tests
{
    public class WordFinderTests
    {
        [Fact]
        public void FindMatrix8x8()
        {
            string[] matrix = { "cadgnaun", "zecautno", "numtrpno", "gatoperr", "perrorat", "gatorryu", "rperrowq", "xrgatovv" };
            string[] wordStream = { "perro", "gato", "loro" };

            WordFinder wordFinder = new WordFinder(matrix);

            var result = (string[]) wordFinder.Find(wordStream);

            Assert.True(result[0] == "gato");
            Assert.True(result[1] == "perro");
        }

        [Fact]
        public void FindMatrix5x5()
        {
            string[] matrix = { "abcdc", "fgwio", "chill", "pqnsd", "uvdxy"};
            string[] wordStream = { "cold", "wind", "snow", "chill" };

            WordFinder wordFinder = new WordFinder(matrix);

            var result = (string[])wordFinder.Find(wordStream);

            Assert.Equal(3, result.Length);
        }

        [Fact]
        public void FindMatrix10x10()
        {
            string[] matrix = { "qwertyuiop", "daydaydapd", "srhayyumar", "owmyslqnth", "psodhyodip", "nbsaebhiob", "iopycarnvo", "pobnmaselk", "dwinterhnt", "dvkcartodb" };
            string[] wordStream = { "day", "winter", "patio", "work", "car" };

            WordFinder wordFinder = new WordFinder(matrix);

            var result = (string[])wordFinder.Find(wordStream);

            Assert.True(result[0] == "day");
            Assert.True(result[1] == "car");
            Assert.Equal(4, result.Length);
        }
    }

}
