using System;
using Xunit;

namespace CouchbaseHealth.Test
{
    public class DocumentCheckTest
    {
        [Fact]
        public void ParameterToPass_IsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(()=> new DocumentCheck(null));
        }
    }
}
