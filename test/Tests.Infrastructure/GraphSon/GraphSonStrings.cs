namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public static class GraphSonStrings
    {
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

        public static readonly string Country_with_meta_properties = """
            [
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
            ]
            """;

        public static readonly string Graphson2_Paths = """
            [
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
            ]
            """;

        public static readonly string Graphson3_Paths = """
            [
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
            ]
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
            [
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
            ]
            """;

        public static readonly string Named_tuple_of_Person_Language = """
            [
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
            ]
            """;

        public static readonly string Nested_array_of_Languages = """
            [
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
            [
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
            ]
            """;

        public static readonly string Single_Language = """
            [
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
            ]
            """;

        public static readonly string Single_Person = """
            [
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
            ]
            """;

        public static readonly string Single_Person_lowercase_properties = """
            [
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
            ]
            """;

        public static readonly string Single_Person_String_Id = """
            [
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
            ]
            """;

        public static readonly string Single_Person_with_null = """
            [
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
            ]
            """;

        public static readonly string Single_Person_without_PhoneNumbers = """
            [
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
            ]
            """;

        public static readonly string Single_TimeFrame = """
            [
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
            ]
            """;

        public static readonly string Single_TimeFrame_with_numbers = """
            [
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
            ]
            """;

        public static readonly string Traverser = """
            [
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
            ]
            """;

        public static readonly string Tuple_of_Person_Language = """
            [
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
            ]
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
    }
}
