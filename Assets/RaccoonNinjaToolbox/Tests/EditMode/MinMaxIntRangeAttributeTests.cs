using NUnit.Framework;
using RaccoonNinjaToolbox.Scripts.Attributes;
using RaccoonNinjaToolbox.Scripts.Interfaces;

namespace RaccoonNinjaToolbox.Tests.EditMode
{
    [TestFixture]
    public class MinMaxIntRangeAttributeTests
    {
        [Test]
        public void Constructor_WithValues_StoresMinMax()
        {
            var attr = new MinMaxIntRangeAttribute(3, 12);

            Assert.That(attr.Min, Is.EqualTo(3));
            Assert.That(attr.Max, Is.EqualTo(12));
        }

        [Test]
        public void Constructor_WithDefaults_UsesZeroAndOne()
        {
            var attr = new MinMaxIntRangeAttribute();

            Assert.That(attr.Min, Is.EqualTo(0));
            Assert.That(attr.Max, Is.EqualTo(1));
        }

        [Test]
        public void Constructor_WithNegativeValues_StoresCorrectly()
        {
            var attr = new MinMaxIntRangeAttribute(-100, -1);

            Assert.That(attr.Min, Is.EqualTo(-100));
            Assert.That(attr.Max, Is.EqualTo(-1));
        }

        [Test]
        public void ImplementsIMinMaxRangeAttribute()
        {
            var attr = new MinMaxIntRangeAttribute(0, 10);

            Assert.That(attr, Is.InstanceOf<IMinMaxRangeAttribute<int>>());
        }
    }
}
