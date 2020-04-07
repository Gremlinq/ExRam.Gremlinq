using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ExRam.Gremlinq.Core.Tests
{
    public class SideEffectTestCaseOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            int GetIndex(string str)
            {
                if (str.StartsWith("Drop"))
                    return 0;

                if (str.StartsWith("Add"))
                    return 1;

                return 2;
            }

            var result = testCases.ToList();

            result.Sort((x, y) =>
            {
                var comparison = GetIndex(x.TestMethod.Method.Name).CompareTo(GetIndex(y.TestMethod.Method.Name));

                return comparison != 0
                    ? comparison
                    : StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name, y.TestMethod.Method.Name);
            });

            return result;
        }
    }
}