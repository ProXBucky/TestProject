using Xunit;

namespace TestProject
{
    public class Calculator
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.True(true);
        }

        [Fact]
        public void Add_TwoPositiveNumbers_ReturnsSum()
        {
            var calculator = new Calculator();
            int result = calculator.Add(2, 3);
            Assert.Equal(5, result);
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(-1, -1, -2)]
        public void Add_VariousNumbers_ReturnsCorrectSum(int a, int b, int expected)
        {
            var calculator = new Calculator();
            int result = calculator.Add(a, b);
            Assert.Equal(expected, result);
        }
    }
}