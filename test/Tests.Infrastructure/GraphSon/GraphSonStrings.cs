using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public static class GraphSonStrings
    {
        [StringSyntax("Json")]
        public static readonly string UntypedEdge = """
            [
              {
                "id": 9,
                "label": "WorksFor",
                "type": "edge",
                "inVLabel": "Company",
                "outVLabel": "Person",
                "inV": "companyId",
                "outV": "personId",
                "properties": {
                  "Role": "Admin",
                  "ActiveFrom": 1521805004907
                }
              }
            ]
            """;

        [StringSyntax("Json")]
        public static readonly string ArrayOfLanguages = """
            [
              [
                {
                  "id": 1,
                  "label": "Language",
                  "type":  "vertex",
                  "properties": {
                    "IetfLanguageTag": [
                      {
                        "id": 1,
                        "value": "de"
                      }
                    ]
                  }
                },
                {
                  "id": 2,
                  "label": "Language",
                  "type": "vertex",
                  "properties": {
                    "IetfLanguageTag": [
                      {
                        "id": 2,
                        "value": "en"
                      }
                    ]
                  }
                }
              ]
            ]
            """;

    }
}
