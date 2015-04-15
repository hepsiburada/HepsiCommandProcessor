using System;

namespace Hepsi.CommandProcessor.UnitTests.TestSupport
{
    internal class TestEvent : Event, IEquatable<TestEvent>
    {
        public int Data { get; private set; }

        public TestEvent()
        {
            Id = Guid.NewGuid();
            Data = 7;
        }

        public bool Equals(TestEvent other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Data == other.Data;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((TestEvent)obj);
        }

        public override int GetHashCode()
        {
            return Data;
        }

        public static bool operator ==(TestEvent left, TestEvent right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TestEvent left, TestEvent right)
        {
            return !Equals(left, right);
        }
    }
}
