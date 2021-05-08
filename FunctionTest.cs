using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using LastAssignmentLoad;

namespace LastAssignmentLoad.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TestRandomFox()
        {
            RandomFox foxie = new RandomFox("This Image", "This Link");
            Assert.NotNull(foxie);
        }
    }
}
