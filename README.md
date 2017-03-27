# ExRam.Gremlinq
ExRam.Gremlinq is a simple ORM for Gremlin-enabled graph databases in a very early stage.

## Current Features

### Fluent Linq-style API:
Currently, the most common Gremlin steps are supported. Assuming an existing instance of an `IGremlinQueryProvider`, gremlin queries are fluently built starting with `CreateQuery()`:
    
    IGremlinQueryProvider provider = ...
    IGremlinQuery query = provider
        .CreateQuery()
        .V<SomeVertexType>()
        .Has(x => x.SomeProperty == 36);
     
### Basic definition of a graph schema:
To map graph element labels to CLR types, a basic fluent schema API is supported.

    IGremlinModel model = GremlinModel.Empty
        .AddVertexType<SomeVertexType>()
        .AddVertexType<SomeOtherVertexType>()
        .AddEdgeType<AnEgdeType>()
        .AddEdgeType<AnotherEdgeType>();

The model can then be attached to an existing `IGremlinQueryProvider`:

    var queryProviderWithModel = provider
        .WithModel(model);

Queries will use the class hierarchy from the model to automatically add the correct `hasLabel` steps to the gremlin-query. Automatic schema creation is vendor specific and currently not supported (help wanted!).
        
### Inheritance
Queries can use the class hierarchy information from an `IGremlinModel` to support inheritance:

    public class SomeBaseVertexType { }
    
    public class SomeInheritedVertexType { }
    
    IGremlinQuery query = provider
        .WithModel(GremlinModel.Empty
            .AddVertexType<SomeBaseVertexType>()
            .AddVertexType<SomeInheritedVertexType>())
        .CreateQuery()
        .V<SomeVertexBaseType>(); //Will include vertices of both `SomeBaseVertexType` and `SomeInheritedVertexType`.
 
 ### Object mapping from json-data
 Query results are typically provides as json documents. ExRam.Gremlinq supports mapping json-data to CLR types by adding `WithJsonSupport`:
 
     IGremlinQueryProvider jsonSupportingProvider = provider
         .WithJsonSupport();
      
      
 ### Customizable label-name to CLR-type mapping
 ExRam.Gremlinq enables you to define the graph element labels for CLR types by means of `IGraphElementNamingStrategy`.
 
 ### Binding to vendor-specific drivers
 Currently, a simple binding to the [DataStax C#-driver for DSE](https://github.com/datastax/csharp-driver-dse) is provided in a separate project. Given an instance of `IDseSession`, you can create an `IGremlinQueryProvider` from it using
 
     IDseSession session = ...
     IGremlinQueryProvider provider = session
         .CreateQueryProvider(someModel, someNamingStrategy);
         
 ## Getting involved
 This project, the repo, the documentation, just about everything is in an early stage. We at @ExRam are currently evaluating graph databases (more specifically DataStax Enterprise Graph). This work in progress reflects our progress regarding this. If you want to contribute to this, please we will happily accept pull requests!
