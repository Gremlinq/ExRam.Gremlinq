namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public static class GraphSonStrings
    {
        public static readonly string UntypedEdge = """
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
            """;

        public static readonly string Array_With_Traverser_With_Ints = """
            [
              {
                "@type" : "g:Traverser",
                "@value" : {
                  "bulk" : {
                    "@type" : "g:Int64",
                    "@value" : 7
                  },
                  "value" : 42
                }
              }
            ]
            """;

        public static readonly string ArrayOfLanguages = """
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
            """;

        public static readonly string BulkSet = """
          {
            "@type" : "g:BulkSet",
            "@value" : 
            [
              "one", 
              {
                "@type" : "g:Int64",
                "@value" : 1
              },
              "two",
              {
                "@type" : "g:Int64",
                "@value" : 2
              },
              "three",
              {
                "@type" : "g:Int64",
                "@value" : 3
              }
            ]
          }
          """;

        public static readonly string Country_with_meta_properties = """
            {
              "id": 3,
              "type": "vertex",
              "label": "Country",
              "properties": {
                "Name": [
                  {
                    "id": 1,
                    "value": "GER",
                    "properties": {
                      "de": "Deutschland",
                      "en": "Germany"
                    }
                  }
                ]
              }
            }
            """;

        public static readonly string Graphson2_Paths = """
            {
              "labels": [
                ["edge"]
              ],
              "objects": [
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
            }
            """;

        public static readonly string Graphson3_Paths = """
            {
              "@type":"g:Path",
              "@value":{
                "labels":[ [],[],[] ],
                "objects":[
                 {
                    "@type":"g:Vertex",
                    "@value":{
                      "id":{
                        "@type":"g:Int32",
                        "@value":1
                      },
                      "label":"person",
                      "properties":{
                        "name":[
                         {
                           "@type":"g:VertexProperty",
                           "@value":{
                             "id":{
                               "@type":"g:Int64",
                               "@value":0
                             },
                             "value":"marko",
                             "label":"name"
                           }
                         }
                        ],
                        "location":[
                          {
                             "@type":"g:VertexProperty",
                             "@value":{
                                "id":{
                                  "@type":"g:Int64",
                                  "@value":6
                                },
                                "value":"san diego",
                                "label":"location",
                                "properties":{
                                  "startTime":{
                                    "@type":"g:Int32",
                                    "@value":1997
                                  },
                                  "endTime":{
                                    "@type":"g:Int32",
                                    "@value":2001
                                  }
                                }
                             }
                          },
                          {
                             "@type":"g:VertexProperty",
                             "@value":{
                                "id":{
                                  "@type":"g:Int64",
                                  "@value":7
                                },
                                "value":"santa cruz",
                                "label":"location",
                                "properties":{
                                  "startTime":{
                                     "@type":"g:Int32",
                                     "@value":2001
                                  },
                                  "endTime":{
                                     "@type":"g:Int32",
                                     "@value":2004
                                  }
                                }
                             }
                          },
                          {
                            "@type":"g:VertexProperty",
                            "@value":{
                              "id":{
                                "@type":"g:Int64",
                                "@value":8
                              },
                              "value":"brussels",
                              "label":"location",
                              "properties":{
                                "startTime":{
                                  "@type":"g:Int32",
                                  "@value":2004
                                },
                                "endTime":{
                                  "@type":"g:Int32",
                                  "@value":2005
                                }
                              }
                            }
                          },
                          {
                            "@type":"g:VertexProperty",
                            "@value":{
                              "id":{
                                "@type":"g:Int64",
                                "@value":9
                              },
                              "value":"santa fe",
                              "label":"location",
                              "properties":{
                                "startTime":{
                                   "@type":"g:Int32",
                                   "@value":2005
                                }
                              }
                            }
                          }
                        ]
                      }
                    }
                  }
                ]
              }
            }
            """;

        public static readonly string Graphson3_Tuple_of_Person_Language = """
            {
              "@type": "g:List",
              "@value": [
                {
                  "@type": "g:Map",
                  "@value": [
                    "Item1",
                    {
                      "@type": "g:Vertex",
                      "@value": {
                        "id": 4,
                        "label": "User",
                        "properties": {
                          "Name": [
                            {
                              "@type": "g:VertexProperty",
                              "@value": {
                                "id": {
                                  "@type": "g:Int64",
                                  "@value": 1
                                },
                                "value": "Name of some base entity"
                              }
                            }
                          ],
                          "Age": [
                            {
                              "@type": "g:VertexProperty",
                              "@value": {
                                "id": {
                                  "@type": "g:Int64",
                                  "@value": 2
                                },
                                "value": {
                                  "@type": "g:Int32",
                                  "@value": 36
                                }
                              }
                            }
                          ]
                        }
                      }
                    },
                    "Item2",
                    {
                      "@type": "g:Vertex",
                      "@value": {
                        "id": 5,
                        "label": "Language",
                        "properties": {
                          "IetfLanguageTag": [
                            {
                              "@type": "g:VertexProperty",
                              "@value": {
                                "id": {
                                  "@type": "g:Int32",
                                  "@value": 3
                                },
                                "value": "de"
                              }
                            }
                          ]
                        }
                      }
                    }
                  ]
                }
              ]
            }
            """;

        public static readonly string Graphson3ReferenceVertex = """
            {
              "@type": "g:Vertex",
              "@value": {
                "id": {
                  "@type": "g:Int32",
                  "@value": 1
                },
                "label": "person",
                "properties": {
                  "name": [
                    {
                      "@type": "g:VertexProperty",
                      "@value": {
                        "id": {
                          "@type": "g:Int64",
                          "@value": 0
                        },
                        "value": "marko",
                        "label": "name"
                      }
                    }
                  ],
                  "location": [
                    {
                      "@type": "g:VertexProperty",
                      "@value": {
                        "id": {
                          "@type": "g:Int64",
                          "@value": 6
                        },
                        "value": "san diego",
                        "label": "location",
                        "properties": {
                          "startTime": {
                            "@type": "g:Int32",
                            "@value": 1997
                          },
                          "endTime": {
                            "@type": "g:Int32",
                            "@value": 2001
                          }
                        }
                      }
                    },
                    {
                      "@type": "g:VertexProperty",
                      "@value": {
                        "id": {
                          "@type": "g:Int64",
                          "@value": 7
                        },
                        "value": "santa cruz",
                        "label": "location",
                        "properties": {
                          "startTime": {
                            "@type": "g:Int32",
                            "@value": 2001
                          },
                          "endTime": {
                            "@type": "g:Int32",
                            "@value": 2004
                          }
                        }
                      }
                    },
                    {
                      "@type": "g:VertexProperty",
                      "@value": {
                        "id": {
                          "@type": "g:Int64",
                          "@value": 8
                        },
                        "value": "brussels",
                        "label": "location",
                        "properties": {
                          "startTime": {
                            "@type": "g:Int32",
                            "@value": 2004
                          },
                          "endTime": {
                            "@type": "g:Int32",
                            "@value": 2005
                          }
                        }
                      }
                    },
                    {
                      "@type": "g:VertexProperty",
                      "@value": {
                        "id": {
                          "@type": "g:Int64",
                          "@value": 9
                        },
                        "value": "santa fe",
                        "label": "location",
                        "properties": {
                          "startTime": {
                            "@type": "g:Int32",
                            "@value": 2005
                          }
                        }
                      }
                    }
                  ]
                }
              }
            }
            """;

        public static readonly string Named_tuple_of_Person_Language = """
            {
              "key": {
                "id": 16,
                "label": "Person",
                "type": "vertex",
                "properties": {
                  "Name": [
                    {
                      "id": 1,
                      "value": "Name of some base entity"
                    }
                  ],
                  "Age": [
                    {
                      "id": 2,
                      "value": "36"
                    }
                  ]
                }
              },
              "value": {
                "id": 17,
                "label": "Language",
                "type": "vertex",
                "properties": {
                  "IetfLanguageTag": [
                    {
                      "id": 3,
                      "value": "de"
                    }
                  ]
                }
              }
            }
            """;

        public static readonly string Nested_array_of_Languages = """
            [
              [
                {
                  "id": 6,
                  "label": "Language",
                  "type": "vertex",
                  "properties": {
                    "IetfLanguageTag": [
                      {
                        "id": 1,
                        "value": "en"
                      }
                    ]
                  }
                }
              ],
              [
                {
                  "id": 7,
                  "label": "Language",
                  "type": "vertex",
                  "properties": {
                    "IetfLanguageTag": [
                      {
                        "id": 2,
                        "value": "de"
                      }
                    ]
                  }
                },
                {
                  "id": 8,
                  "label": "Language",
                  "type": "vertex",
                  "properties": {
                    "IetfLanguageTag": [
                      {
                        "id": 1,
                        "value": "en"
                      }
                    ]
                  }
                }
              ]
            ]
            """;

        public static readonly string Properties = """
            [
              {
                "key": "metaKey1",
                "value": "metaValue1"
              },
              {
                "key": "metaKey2",
                "value": 36
              }
            ]
            """;

        public static readonly string Single_Company = """
            {
              "id": "b9b89d7f-9313-4eed-b354-2760ba7a3fbe",
              "label": "Company",
              "type": "vertex",
              "properties": {
                "FoundingDate": [
                  {
                    "id": "87c82d45-623d-45fc-91f6-5757d2010403",
                    "value": "2018-12-17T08:00:00Z"
                  }
                ],
                "Name": [
                  {
                    "id": "af8d9d94-3814-400a-91b8-1a6bef584a9a",
                    "value": "Company!"
                  }
                ]
              }
            }
            """;

        public static readonly string Single_Language = """
            {
              "id": 10,
              "label": "Language",
              "type": "vertex",
              "properties": {
                "IetfLanguageTag": [
                  {
                    "id": 1,
                    "value": "de"
                  }
                ]
              }
            }
            """;

        public static readonly string Single_Person = """
            {
              "id": 13,
              "label": "Person",
              "type": "vertex",
              "properties": {
                "Age": [
                  {
                    "id": 1,
                    "value": "36"
                  }
                ],
                "RegistrationDate": [
                  {
                    "id": 2,
                    "value": 1481750076295
                  }
                ],
                "Gender": [
                  {
                    "id": 3,
                    "value": 1
                  }
                ],
                "PhoneNumbers": [
                  {
                    "id": 4,
                    "value": "+123456"
                  },
                  {
                    "id": 5,
                    "value": "+234567"
                  }
                ]
              }
            }
            """;

        public static readonly string Single_Person_lowercase_properties = """
            {
              "id": 14,
              "label": "Person",
              "type": "vertex",
              "properties": {
                "age": [
                  {
                    "id": 1,
                    "value": "36"
                  }
                ],
                "registrationDate": [
                  {
                    "id": 2,
                    "value": 1481750076295
                  }
                ],
                "phoneNumbers": [
                  {
                    "id": 3,
                    "value": "+123456"
                  },
                  {
                    "id": 4,
                    "value": "+234567"
                  }
                ]
              }
            }
            """;

        public static readonly string Single_Person_String_Id = """
            {
              "id": "13",
              "label": "Person",
              "type": "vertex",
              "properties": {
                "Age": [
                  {
                    "id": 1,
                    "value": "36"
                  }
                ],
                "RegistrationDate": [
                  {
                    "id": 2,
                    "value": 1481750076295
                  }
                ],
                "Gender": [
                  {
                    "id": 3,
                    "value": 1
                  }
                ],
                "PhoneNumbers": [
                  {
                    "id": 4,
                    "value": "+123456"
                  },
                  {
                    "id": 5,
                    "value": "+234567"
                  }
                ]
              }
            }
            """;

        public static readonly string Single_Person_with_null = """
            {
              "id": 13,
              "label": "Person",
              "type": "vertex",
              "properties": {
                "Age": [
                  {
                    "id": 1,
                    "value": "36"
                  }
                ],
                "RegistrationDate": null,
                "Gender": [
                  {
                    "id": 3,
                    "value": 1
                  }
                ],
                "PhoneNumbers": [
                  {
                    "id": 4,
                    "value": "+123456"
                  },
                  {
                    "id": 5,
                    "value": "+234567"
                  }
                ]
              }
            }
            """;

        public static readonly string Single_Person_without_PhoneNumbers = """
            {
              "id": 15,
              "label": "Person",
              "type": "vertex",
              "properties": {
                "Age": [
                  {
                    "id": 1,
                    "value": "36"
                  }
                ],
                "RegistrationDate": [
                  {
                    "id": 2,
                    "value": 1481750076295
                  }
                ]
              }
            }
            """;

        public static readonly string Single_TimeFrame = """
            {
              "id": 11,
              "label": "TimeFrame",
              "type": "vertex",
              "properties": {
                "WeekDay": [
                  {
                    "id": 1,
                    "value": 1
                  }
                ],
                "StartTime": [
                  {
                    "id": 2,
                    "value": "PT6H"
                  }
                ],
                "Duration": [
                  {
                    "id": 3,
                    "value": "PT16H"
                  }
                ]
              }
            }
            """;

        public static readonly string Single_TimeFrame_with_numbers = """
            {
              "id": 12,
              "label": "TimeFrame",
              "type": "vertex",
              "properties": {
                "WeekDay": [
                  {
                    "id": 1,
                    "value": 1
                  }
                ],
                "StartTime": [
                  {
                    "id": 2,
                    "value": 21600000
                  }
                ],
                "Duration": [
                  {
                    "id": 3,
                    "value": 57600000
                  }
                ]
              }
            }
            """;

        public static readonly string Traverser = """
            {
              "@type": "g:Traverser",
              "@value": {
                "bulk": {
                  "@type": "g:Int64",
                  "@value": 3

                },
                "value": {
                  "id": "b9b89d7f-9313-4eed-b354-2760ba7a3fbe",
                  "label": "Company",
                  "type": "vertex",
                  "properties": {
                    "FoundingDate": [
                      {
                        "id": "87c82d45-623d-45fc-91f6-5757d2010403",
                        "value": "2018-12-17T08:00:00Z"
                      }
                    ],
                    "Name": [
                      {
                        "id": "af8d9d94-3814-400a-91b8-1a6bef584a9a",
                        "value": "Company!"
                      }
                    ]
                  }
                }
              }
            }
            """;

        public static readonly string Tuple_of_Person_Language = """
            {
              "Item1": {
                "id": 16,
                "label": "Person",
                "type": "vertex",
                "properties": {
                  "Name": [
                    {
                      "id": 1,
                      "value": "Name of some base entity"
                    }
                  ],
                  "Age": [
                    {
                      "id": 2,
                      "value": "36"
                    }
                  ]
                }
              },
              "Item2": {
                "id": 17,
                "label": "Language",
                "type": "vertex",
                "properties": {
                  "IetfLanguageTag": [
                    {
                      "id": 3,
                      "value": "de"
                    }
                  ]
                }
              }
            }
            """;

        public static readonly string Vertex_Properties = """
            [
              {
                "value": 1540202009475,
                "label": "Property1",
                "properties": {
                  "metaKey": "MetaValue"
                }
              },
              {
                "value": "Some string",
                "label": "Property2"
              },
              {
                "value": 36,
                "label": "Property3"
              }
            ]
            """;

        public static readonly string String_Keys_Int_Values = """
            {
              "key1": 1,
              "key2": 2
            }
            """;

        public static readonly string String_Keys_Typed_Int_Values = """
            {
              "key1": { "@type": "g:Int32", "@value": 1 },
              "key2": { "@type": "g:Int32", "@value": 2 }
            }
            """;

        public static readonly string Map_of_String_Keys_Typed_Int_Values = """
            {
              "@type": "g:Map",
              "@value":
              [
                "key1", { "@type": "g:Int32", "@value": 1 },
                "key2", { "@type": "g:Int32", "@value": 2 }
              ]
            }
            """;

        public static readonly string Map_of_Typed_Int_Keys_Typed_String_Values = """
            {
              "@type": "g:Map",
              "@value":
              [
                { "@type": "g:Int32", "@value": 1 }, "value1", 
                { "@type": "g:Int32", "@value": 2 }, "value2"
              ]
            }
            """;

        public static readonly string Ints = "[ 1, 3, 5 ]";

        public static readonly string Typed_Ints = """
            [
              { "@type": "g:Int32", "@value": 1 },
              { "@type": "g:Int32", "@value": 3 },
              { "@type": "g:Int32", "@value": 5 }
            ]
            """;

        public static readonly string Empty_typed_tree = """
            {
              "@type": "g:Tree",
              "@value": [
              ]
            }
            """;

        public static readonly string RootOnly_string_tree = """
            {
              "@type": "g:Tree",
              "@value": [
                {
                  "key": "3",
                  "value": {
                    "@type": "g:Tree",
                    "@value": []
                  }
                }
              ]
            }
            """;

        public static readonly string RootOnly_int_tree = """
            {
              "@type": "g:Tree",
              "@value": [
                {
                  "key": {
                    "@type": "g:Int32",
                    "@value": 1
                  },
                  "value": {
                    "@type": "g:Tree",
                    "@value": []
                  }
                }
              ]
            }
            """;

        public static readonly string RootOnly_int_tree_CosmosDb = """
            {
              "1": {
                "key": 1,
                "value": {
                  "2": {
                    "key": "2",
                    "value": {}
                  },
                  "3": {
                    "key": "3",
                    "value": {}
                  }
                }
              }
            }
            """;

        public static readonly string Branching_scalar_tree = """
            {
              "@type": "g:Tree",
              "@value": [
                {
                  "key": {
                    "@type": "g:Int32",
                    "@value": 1
                  },
                  "value": {
                    "@type": "g:Tree",
                    "@value": [
                      {
                        "key": "2",
                        "value": {
                          "@type": "g:Tree",
                          "@value": []
                        }
                      },
                      {
                        "key": "3",
                        "value": {
                          "@type": "g:Tree",
                          "@value": []
                        }
                      }
                    ]
                  }
                }
              ]
            }
            """;

        public static readonly string Linear_string_tree = """
            {
              "@type": "g:Tree",
              "@value": [
                {
                  "key": "1",
                  "value": {
                    "@type": "g:Tree",
                    "@value": [
                      {
                        "key": "2",
                        "value": {
                          "@type": "g:Tree",
                          "@value": [
                            {
                              "key": "3",
                              "value": {
                                "@type": "g:Tree",
                                "@value": []
                              }
                            }
                          ]
                        }
                      }
                    ]
                  }
                }
              ]
            }
            """;

        public static readonly string Mixed_entity_and_scalar_tree_CosmosDb = """
            {
              "9c64eed3-cdfe-4bfb-8e44-769146ace9e0": {
                "key": {
                  "id": "9c64eed3-cdfe-4bfb-8e44-769146ace9e0",
                  "label": "Person",
                  "type": "vertex",
                  "properties": {
                    "Age": [
                      {
                        "id": "d9490da8-835e-4ccb-b0d2-1ff38d18b996",
                        "value": 0
                      }
                    ],
                    "PartitionKey": [
                      {
                        "id": "9c64eed3-cdfe-4bfb-8e44-769146ace9e0|PartitionKey",
                        "value": "PartitionKey"
                      }
                    ]
                  }
                },
                "value": {
                  "1": {
                    "key": 1,
                    "value": {}
                  },
                  "2": {
                    "key": 2,
                    "value": {}
                  }
                }
              }
            }
            """;

        public static readonly string Mixed_entity_and_scalar_tree = """
            [
              {
                "key": {
                  "@type": "g:Vertex",
                  "@value": {
                    "id": {
                      "@type": "g:Int64",
                      "@value": 0
                    },
                    "label": "Person",
                    "properties": {
                      "PartitionKey": [
                        {
                          "@type": "g:VertexProperty",
                          "@value": {
                            "id": {
                              "@type": "g:Int64",
                              "@value": 2
                            },
                            "value": "PartitionKey",
                            "label": "PartitionKey"
                          }
                        }
                      ],
                      "Age": [
                        {
                          "@type": "g:VertexProperty",
                          "@value": {
                            "id": {
                              "@type": "g:Int64",
                              "@value": 1
                            },
                            "value": {
                              "@type": "g:Int32",
                              "@value": 0
                            },
                            "label": "Age"
                          }
                        }
                      ]
                    }
                  }
                },
                "value": {
                  "@type": "g:Tree",
                  "@value": [
                    {
                      "key": {
                        "@type": "g:Int32",
                        "@value": 1
                      },
                      "value": {
                        "@type": "g:Tree",
                        "@value": []
                      }
                    },
                    {
                      "key": {
                        "@type": "g:Int32",
                        "@value": 2
                      },
                      "value": {
                        "@type": "g:Tree",
                        "@value": []
                      }
                    }
                  ]
                }
              },
              {
                "key": {
                  "@type": "g:Vertex",
                  "@value": {
                    "id": {
                      "@type": "g:Int64",
                      "@value": 3
                    },
                    "label": "Person",
                    "properties": {
                      "PartitionKey": [
                        {
                          "@type": "g:VertexProperty",
                          "@value": {
                            "id": {
                              "@type": "g:Int64",
                              "@value": 5
                            },
                            "value": "PartitionKey",
                            "label": "PartitionKey"
                          }
                        }
                      ],
                      "Age": [
                        {
                          "@type": "g:VertexProperty",
                          "@value": {
                            "id": {
                              "@type": "g:Int64",
                              "@value": 4
                            },
                            "value": {
                              "@type": "g:Int32",
                              "@value": 0
                            },
                            "label": "Age"
                          }
                        }
                      ]
                    }
                  }
                },
                "value": {
                  "@type": "g:Tree",
                  "@value": [
                    {
                      "key": {
                        "@type": "g:Int32",
                        "@value": 1
                      },
                      "value": {
                        "@type": "g:Tree",
                        "@value": []
                      }
                    },
                    {
                      "key": {
                        "@type": "g:Int32",
                        "@value": 2
                      },
                      "value": {
                        "@type": "g:Tree",
                        "@value": []
                      }
                    }
                  ]
                }
              }
            ]
            """;

        public static readonly string EverythingAllAtOnceData = $$"""
            {
              "Int_from_double": 4.2,
              "IImmutableDictionary_string_keys_typed_int_values": {{String_Keys_Typed_Int_Values}},
              "Dictionary_typed_int_keys_string_values": {{Map_of_Typed_Int_Keys_Typed_String_Values}},
              "IUntypedDictionary_string_keys_typed_int_values": {{String_Keys_Typed_Int_Values}},
              "IEnumerable_from_Typed_Ints": {{Typed_Ints}},
              "Untyped_IEnumerable_from_Typed_Ints": {{Typed_Ints}},
              "ISet_Typed_Ints": {{Typed_Ints}},
              "IList_Typed_Ints": {{Typed_Ints}},
              "IImmutableList_Ints": {{Typed_Ints}},
              "IImmutableQueue_Ints": {{Typed_Ints}},
              "IImmutableSet_Ints": {{Typed_Ints}},
              "IImmutableStack_Ints": {{Typed_Ints}},
              "ConcurrentQueue_from_typed_Ints": {{Typed_Ints}},
              "ConcurrentStack_from_typed_Ints": {{Typed_Ints}},
              "ImmutableQueue_from_typed_Ints": {{Typed_Ints}},
              "ImmutableStack_from_typed_Ints": {{Typed_Ints}},
              "IReadOnlyList_from_Ints": {{Ints}},
              "IReadOnlyList_from_Typed_Ints": {{Typed_Ints}},
              "Queue_from_typed_Ints": {{Typed_Ints}},
              "Stack_from_typed_Ints": {{Typed_Ints}},
              "Array": {{ArrayOfLanguages}},
              "Bulk_set": {{BulkSet}},
              "DateTime_from_double": 123456789.2,
              "DateTime_from_number": 123456789,
              "DateTime_from_string": "2018-12-17T08:00:00Z",
              "DateTime_is_UTC": {{Single_Company}},
              "DateTimeOffset_from_number": 123456789,
              "DateTimeOffset_from_string": "2018-12-17T08:00:00Z",
              "DynamicData": { "values": [ ], "count": { "@type": "g:Int32", "@value": 36 } },
              "Edge": {{UntypedEdge}},
              "Empty_to_ints": { "Item1": [], "Item2": [] },
              "Empty1": [],
              "Empty2": [],
              "Graphson2Path": {{Graphson2_Paths}},
              "GraphSon3_Tuple": {{Graphson3_Tuple_of_Person_Language}},
              "Graphson3Path": {{Graphson3_Paths}},
              "GraphSon3ReferenceVertex": {{Graphson3ReferenceVertex}},
              "Guid": "FCE0765A-454F-4D00-83DA-D76790156E29",
              "IDictionary_string_keys_typed_int_values": {{String_Keys_Typed_Int_Values}},
              "IReadOnlyDictionary_string_keys_typed_int_values": {{String_Keys_Typed_Int_Values}},
              "IUntypedList_Typed_Ints": {{Typed_Ints}},
              "IUntypedCollection_from_typed_ints": {{Typed_Ints}},
              "ICollection_from_typed_ints": {{Typed_Ints}},
              "ImmutableArray": [ 1, 3, 5 ],
              "ImmutableArray_typed_ints": {{Typed_Ints}},
              "ImmutableDictionary_map_of_string_keys_typed_int_values": {{Map_of_String_Keys_Typed_Int_Values}},
              "ImmutableDictionary_string_keys_int_values": {{String_Keys_Int_Values}},
              "ImmutableDictionary_string_keys_typed_int_values": {{String_Keys_Typed_Int_Values}},
              "ImmutableList_ints": {{Ints}},
              "ImmutableList_typed_ints": {{Typed_Ints}},
              "Int_Ids": [ 1, 2 ],
              "Ints_from_Traverser": {{Array_With_Traverser_With_Ints}},
              "List_Of_Ints_from_Traverser": {{Array_With_Traverser_With_Ints}},
              "IList_Of_Ints_from_Traverser": {{Array_With_Traverser_With_Ints}},
              "Language_by_vertex_inheritance": {{Single_Language}},
              "Language_strongly_typed": {{Single_Language}},
              "Language_to_generic_vertex": {{Single_Language}},
              "Language_unknown_type": {{Single_Language}},
              "Languages_to_object": {{ArrayOfLanguages}},
              "List_ints": [ 1, 2, 3 ],
              "Meta_Properties": {{Country_with_meta_properties}},
              "MetaProperties": {{Properties}},
              "Mixed_Ids": [ 1, "id2" ],
              "NamedTuple": {{Named_tuple_of_Person_Language}},
              "Nested_Array": {{Nested_array_of_Languages}},
              "Nullable": 42,
              "Nullable_null": [ 42, null ],
              "Object_from_double": 1.2,
              "Object_from_true": true,
              "Person_lowercase_strongly_typed": {{Single_Person_lowercase_properties}},
              "Person_StringId": {{Single_Person_String_Id}},
              "Person_strongly_typed": {{Single_Person}},
              "Person_with_null": {{Single_Person_with_null}},
              "Person_without_PhoneNumbers_strongly_typed": {{Single_Person_without_PhoneNumbers}},
              "Property_as_object": { "value": 1540202009475, "key": "Property1" },
              "Property_from_Scalar": 36,
              "Scalar": 36,
              "Scalar_as_object": 36,
              "String_Ids": [ "id1", "id2" ],
              "String_Ids2": [ "1", "2" ],
              "TimeFrame_strongly_typed": {{Single_TimeFrame}},
              "TimeSpan_from_double": 123456789.2,
              "TimeSpan_from_integer": 123456789,
              "Tuple": {{Tuple_of_Person_Language}},
              "Tuple_vertex_vertex": {{Tuple_of_Person_Language}},
              "VertexProperties": {{Vertex_Properties}},
              "VertexProperties_with_model": {{Vertex_Properties}},
              "VertexProperty_as_object": { "value": 1540202009475, "id": 1, "label": "Property1", "properties": { "metaKey": "MetaValue" } },
              "VertexPropertyWithDateTimeOffset": { "id": 166, "value": "bob", "label": "Name", "properties": { "ValidFrom": 1548112365431 } },
              "VertexPropertyWithoutProperties": { "id": 166, "value": "bob", "label": "Name" }
            }
            """;
    }
}
