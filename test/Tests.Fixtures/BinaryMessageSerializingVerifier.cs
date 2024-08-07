﻿using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Tests.Infrastructure;

using Gremlin.Net.Driver.Messages;

namespace ExRam.Gremlinq.Tests.Fixtures
{
    public class BinaryMessageSerializingVerifier<TBinaryMessage> : GremlinQueryVerifier
        where TBinaryMessage : IMemoryOwner<byte>
    {
        public BinaryMessageSerializingVerifier([CallerFilePath] string sourceFile = "") : base(sourceFile)
        {
        }

        public override Task Verify<TElement>(IGremlinQueryBase<TElement> query)
        {
            var env = query.AsAdmin().Environment;

            var requestMessage = env.Serializer
                .TransformTo<RequestMessage>()
                .From(query, env);

            var binaryMessage = env.Serializer
                .TransformTo<TBinaryMessage>()
                .From(requestMessage, env);

            return this
                .InnerVerify(Encoding.UTF8.GetString(binaryMessage.Memory.Span));
        }

        protected override SettingsTask ModifySettingsTask(SettingsTask task) => base.ModifySettingsTask(task)
            .DontScrubGuids()
            .ScrubGuids();
    }
}
