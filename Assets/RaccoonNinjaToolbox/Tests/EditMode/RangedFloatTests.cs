using NUnit.Framework;
using RaccoonNinjaToolbox.Scripts.DataTypes;

namespace RaccoonNinjaToolbox.Tests.EditMode
{
    [TestFixture]
    public class RangedFloatTests
    {
        [Test]
        public void Random_ReturnsValueWithinRange()
        {
            var ranged = new RangedFloat { MinValue = 2f, MaxValue = 8f };

            for (var i = 0; i < 100; i++)
            {
                var value = ranged.Random();
                Assert.That(value, Is.GreaterThanOrEqualTo(2f).And.LessThanOrEqualTo(8f),
                    $"Iteration {i}: value {value} was out of range [2, 8]");
            }
        }

        [Test]
        public void Random_WhenMinEqualsMax_ReturnsThatValue()
        {
            var ranged = new RangedFloat { MinValue = 5f, MaxValue = 5f };

            for (var i = 0; i < 10; i++)
            {
                Assert.That(ranged.Random(), Is.EqualTo(5f));
            }
        }

        [Test]
        public void Random_WithNegativeRange_ReturnsValueWithinRange()
        {
            var ranged = new RangedFloat { MinValue = -10f, MaxValue = -5f };

            for (var i = 0; i < 100; i++)
            {
                var value = ranged.Random();
                Assert.That(value, Is.GreaterThanOrEqualTo(-10f).And.LessThanOrEqualTo(-5f));
            }
        }

        [Test]
        public void Random_WithZeroRange_ReturnsZero()
        {
            var ranged = new RangedFloat { MinValue = 0f, MaxValue = 0f };

            Assert.That(ranged.Random(), Is.EqualTo(0f));
        }

        [Test]
        public void Random_WithLargeRange_ReturnsValueWithinRange()
        {
            var ranged = new RangedFloat { MinValue = -1000f, MaxValue = 1000f };

            for (var i = 0; i < 100; i++)
            {
                var value = ranged.Random();
                Assert.That(value, Is.GreaterThanOrEqualTo(-1000f).And.LessThanOrEqualTo(1000f));
            }
        }
    }
}
