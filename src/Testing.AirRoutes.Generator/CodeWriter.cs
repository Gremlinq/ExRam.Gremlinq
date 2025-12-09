namespace ExRam.Gremlinq.Testing.AirRoutes.Generator
{
    internal readonly struct CodeWriter
    {
        private readonly int _level;
        private readonly int _lineLength;
        private readonly StringWriter? _stringWriter;

        private CodeWriter(StringWriter? stringBuilder, int level, int lineLength)
        {
            _level = level;
            _lineLength = lineLength;
            _stringWriter = stringBuilder;
        }

        public CodeWriter Write(string text) => WriteCore(text);

        public CodeWriter WriteLine() => this
            .WriteCore("\r\n");

        public CodeWriter WriteLine(string text) => this
            .WriteCore(text)
            .WriteCore("\r\n");

        private CodeWriter WriteCore(string text)
        {
            var writer = Writer;
            var lineLength = _lineLength;

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (c != '\r' && c != '\n')
                {
                    if (lineLength == 0 && _level != 0)
                        writer.Write("".PadRight(_level * 4));

                    lineLength++;
                }
                else
                    lineLength = 0;

                writer.Write(c);
            }

            return new(writer, _level, lineLength);
        }


        public CodeWriter Do(Func<CodeWriter, CodeWriter> continuation)
        {
            var continuedWriter = continuation(new CodeWriter(_stringWriter, _level, _lineLength));

            return new(continuedWriter._stringWriter, _level, continuedWriter._lineLength);
        }

        public CodeWriter Block<TState>(Func<CodeWriter, TState, CodeWriter> continuation, TState state)
        {
            return this
                .WriteLine("{")
                .Indent(continuation, state)
                .WriteLine("}");
        }

        public CodeWriter Block() => Block(_ => _);

        public CodeWriter Block(Func<CodeWriter, CodeWriter> continuation) => Block((writer, continuation) => continuation(writer), continuation);

        public CodeWriter Indent<TState>(Func<CodeWriter, TState, CodeWriter> continuation, TState state)
        {
            var continuedWriter = continuation(new CodeWriter(_stringWriter, _level + 1, _lineLength), state);

            return new(continuedWriter._stringWriter, _level, continuedWriter._lineLength);
        }

        public CodeWriter Indent(Func<CodeWriter, CodeWriter> continuation) => Indent((writer, continuation) => continuation(writer), continuation);

        public CodeWriter WriteList<TItem>(IEnumerable<TItem> items, Func<CodeWriter, TItem, int, CodeWriter> continuation)
        {
            var i = 0;
            var writer = this;

            foreach (var item in items)
            {
                if (i != 0)
                    writer = writer.Write(", ");

                writer = continuation(writer, item, i++);
            }

            return writer;
        }

        public CodeWriter If(bool @if, Func<CodeWriter, CodeWriter> then, Func<CodeWriter, CodeWriter>? @else = null)
        {
            if (@if)
                return then(this);

            if (@else is { } elseTransformation)
                return elseTransformation(this);

            return this;
        }

        public CodeWriter WriteArguments(Func<CodeWriter, CodeWriter> continuation) => continuation
            .Invoke(this
                .Write("("))
            .Write(")");

        public string Code() => Writer.ToString();

        private StringWriter Writer { get => _stringWriter ?? throw new InvalidOperationException(); }

        public static CodeWriter Create() => new(new StringWriter(), 0, 0);
    }
}
