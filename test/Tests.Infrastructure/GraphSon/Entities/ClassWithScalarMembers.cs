using System.Numerics;

namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    public class ClassWithScalarMembers
    {
        public bool Bool { get; set; }

        public long Long { get; set; }

        public double Double { get; set; }

        public BigInteger BigInteger { get; set; }

        public byte[]? Bytes { get; set; }
    }
}
