using System;

namespace Hepsi.CommandProcessor.UnitTests.TestSupport
{
    internal class TestCommand : ICommand
    {
        public TestCommand()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string Value { get; set; }
    }
}
