using RaccoonNinjaToolbox.Scripts.Abstractions.Controllers;

namespace RaccoonNinjaToolbox.Tests.PlayMode.Helpers
{
    public class TestSingleton : BaseSingletonController<TestSingleton>
    {
        public bool PostAwakeCalled { get; private set; }
        public int PostAwakeCallCount { get; private set; }

        protected override void PostAwake()
        {
            PostAwakeCalled = true;
            PostAwakeCallCount++;
        }
    }
}
