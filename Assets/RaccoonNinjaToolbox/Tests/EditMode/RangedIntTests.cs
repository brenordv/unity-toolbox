using System.Collections.Generic;
using NUnit.Framework;
using RaccoonNinjaToolbox.Scripts.DataTypes;

namespace RaccoonNinjaToolbox.Tests.EditMode
{
    [TestFixture]
    public class RangedIntTests
    {
        [Test]
        public void Random_ReturnsValueWithinInclusiveRange()
        {
            var ranged = new RangedInt { MinValue = 1, MaxValue = 6 };

            for (var i = 0; i < 200; i++)
            {
                var value = ranged.Random();
                Assert.That(value, Is.GreaterThanOrEqualTo(1).And.LessThanOrEqualTo(6),
                    $"Iteration {i}: value {value} was out of range [1, 6]");
            }
        }

        [Test]
        public void Random_CanReturnMaxValue()
        {
            var ranged = new RangedInt { MinValue = 1, MaxValue = 2 };
            var seenValues = new HashSet<int>();

            for (var i = 0; i < 200; i++)
            {
                seenValues.Add(ranged.Random());
            }

            Assert.That(seenValues, Does.Contain(2),
                "MaxValue (2) was never returned in 200 iterations — the +1 inclusivity may be broken");
        }

        [Test]
        public void Random_CanReturnMinValue()
        {
            var ranged = new RangedInt { MinValue = 1, MaxValue = 2 };
            var seenValues = new HashSet<int>();

            for (var i = 0; i < 200; i++)
            {
                seenValues.Add(ranged.Random());
            }

            Assert.That(seenValues, Does.Contain(1),
                "MinValue (1) was never returned in 200 iterations");
        }

        [Test]
        public void Random_WhenMinEqualsMax_ReturnsThatValue()
        {
            var ranged = new RangedInt { MinValue = 5, MaxValue = 5 };

            for (var i = 0; i < 10; i++)
            {
                Assert.That(ranged.Random(), Is.EqualTo(5));
            }
        }

        [Test]
        public void Random_WithNegativeRange_ReturnsValueWithinRange()
        {
            var ranged = new RangedInt { MinValue = -10, MaxValue = -5 };

            for (var i = 0; i < 200; i++)
            {
                var value = ranged.Random();
                Assert.That(value, Is.GreaterThanOrEqualTo(-10).And.LessThanOrEqualTo(-5));
            }
        }

        [Test]
        public void Random_WithZeroRange_ReturnsZero()
        {
            var ranged = new RangedInt { MinValue = 0, MaxValue = 0 };

            Assert.That(ranged.Random(), Is.EqualTo(0));
        }
    }
}
