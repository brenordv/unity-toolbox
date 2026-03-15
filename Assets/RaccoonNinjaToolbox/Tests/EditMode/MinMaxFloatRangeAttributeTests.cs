using NUnit.Framework;
using RaccoonNinjaToolbox.Scripts.Attributes;
using RaccoonNinjaToolbox.Scripts.Interfaces;

namespace RaccoonNinjaToolbox.Tests.EditMode
{
    [TestFixture]
    public class MinMaxFloatRangeAttributeTests
    {
        [Test]
        public void Constructor_WithValues_StoresMinMax()
        {
            var attr = new MinMaxFloatRangeAttribute(2.5f, 7.5f);

            Assert.That(attr.Min, Is.EqualTo(2.5f));
            Assert.That(attr.Max, Is.EqualTo(7.5f));
        }

        [Test]
        public void Constructor_WithDefaults_UsesZeroAndOne()
        {
            var attr = new MinMaxFloatRangeAttribute();

            Assert.That(attr.Min, Is.EqualTo(0f));
            Assert.That(attr.Max, Is.EqualTo(1f));
        }

        [Test]
        public void Constructor_WithNegativeValues_StoresCorrectly()
        {
            var attr = new MinMaxFloatRangeAttribute(-10f, -0.5f);

            Assert.That(attr.Min, Is.EqualTo(-10f));
            Assert.That(attr.Max, Is.EqualTo(-0.5f));
        }

        [Test]
        public void ImplementsIMinMaxRangeAttribute()
        {
            var attr = new MinMaxFloatRangeAttribute(1f, 5f);

            Assert.That(attr, Is.InstanceOf<IMinMaxRangeAttribute<float>>());
        }
    }
}
