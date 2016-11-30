
using System;
using Xunit;

namespace CouchbaseHealth.Test
{
    public class MemoryCheckTest
    {
        [Fact]
        public void ParameterToPass_IsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new MemoryCheck(null));
        }
    }
}
