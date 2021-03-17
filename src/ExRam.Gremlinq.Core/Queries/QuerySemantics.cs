namespace ExRam.Gremlinq.Core
{
    public enum QuerySemantics
    {
        Value = 1,
        Element = Value | 2,
        EdgeOrVertex = Element | 4,
        Vertex = EdgeOrVertex | 8,
        Edge = EdgeOrVertex | 16,
        Property = Value | 32,
        VertexProperty = Property | Element | 64
    }

    /*                       1       2          4          8      16      32           64
     *                     Value  Element  EdgeOrVertex  Vertex  Edge  Property  VertexProperty 
     *  Value                1
     *  Element              1       1
     *  EdgeOrVertex         1       1          1
     *  Vertex               1       1          1          1
     *  Edge                 1       1          1          0       1
     *  Property             1       0          0          0       0       1            0
     *  VertexProperty       1       1          0          0       0       1            1
     */
}
