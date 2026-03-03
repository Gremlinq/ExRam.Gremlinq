using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ExRam.Gremlinq.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class NullCheckAnalyzer : DiagnosticAnalyzer
    {
        public static readonly DiagnosticDescriptor GQ0001 = new DiagnosticDescriptor("GQ0001", "Reference-type parameter lacks null check", "Parameter '{0}' should be checked for null via ArgumentNullException.ThrowIfNull", "Reliability", DiagnosticSeverity.Warning, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [GQ0001];

        public override void Initialize(AnalysisContext context)
        {
            context
                .ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            context
                .EnableConcurrentExecution();

            context
                .RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration, SyntaxKind.ConstructorDeclaration);
        }

        private static void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is BaseMethodDeclarationSyntax methodDeclaration)
            {
                if (methodDeclaration.Body != null || methodDeclaration.ExpressionBody != null)
                {
                    if (context.SemanticModel.GetDeclaredSymbol(methodDeclaration, context.CancellationToken) is { IsAbstract: false } methodSymbol)
                    {
                        if (IsPublicFacingMethod(methodSymbol))
                            AnalyzeParameters(context, methodSymbol, methodDeclaration.Body, methodDeclaration.ExpressionBody);
                    }
                }
            }
        }

        private static bool IsPublicFacingMethod(IMethodSymbol method)
        {
            if (method.MethodKind == MethodKind.ExplicitInterfaceImplementation)
            {
                foreach (var interfaceMethod in method.ExplicitInterfaceImplementations)
                {
                    if (interfaceMethod.ContainingType.DeclaredAccessibility == Accessibility.Public)
                        return true;
                }

                return false;
            }

            return method.DeclaredAccessibility == Accessibility.Public && IsPublicOrNestedInPublic(method.ContainingType);
        }

        private static bool IsPublicOrNestedInPublic(INamedTypeSymbol type)
        {
            while (type != null)
            {
                if (type.DeclaredAccessibility != Accessibility.Public)
                    return false;

                type = type.ContainingType;
            }

            return true;
        }

        private static void AnalyzeParameters(SyntaxNodeAnalysisContext context, IMethodSymbol method, BlockSyntax? body, ArrowExpressionClauseSyntax? expressionBody)
        {
            if (method.Parameters.Length > 0)
            {
                var throwIfNullCalls = GetThrowIfNullParameterNames(context, body, expressionBody);

                foreach (var parameter in method.Parameters)
                {
                    if (parameter.RefKind != RefKind.Out)
                    {
                        if (IsNullableReferenceType(parameter))
                        {
                            if (!throwIfNullCalls.Contains(parameter.Name))
                                context.ReportDiagnostic(Diagnostic.Create(GQ0001, parameter.Locations.Length > 0 ? parameter.Locations[0] : method.Locations[0], parameter.Name));
                        }
                    }
                }
            }
        }

        private static bool IsNullableReferenceType(IParameterSymbol parameter)
        {
            var type = parameter.Type;

            if (type.TypeKind == TypeKind.TypeParameter)
            {
                var typeParam = (ITypeParameterSymbol)type;

                return !typeParam.HasValueTypeConstraint && (typeParam.HasNotNullConstraint || typeParam.HasReferenceTypeConstraint);
            }

            return !type.IsValueType && parameter.NullableAnnotation != NullableAnnotation.Annotated;
        }

        private static HashSet<string> GetThrowIfNullParameterNames(SyntaxNodeAnalysisContext context, BlockSyntax? body, ArrowExpressionClauseSyntax? expressionBody)
        {
            var set = new HashSet<string>(StringComparer.Ordinal);

            if (((SyntaxNode?)body ?? expressionBody) is { } nodeToSearch)
            {
                foreach (var invocation in nodeToSearch.DescendantNodes().OfType<InvocationExpressionSyntax>())
                {
                    if (IsThrowIfNullCall(invocation, context.SemanticModel, context.CancellationToken))
                    {
                        var arguments = invocation.ArgumentList.Arguments;

                        if (arguments.Count >= 1 && arguments[0].Expression is IdentifierNameSyntax identifier)
                            set.Add(identifier.Identifier.Text);
                    }
                }
            }

            return set;
        }

        private static bool IsThrowIfNullCall(InvocationExpressionSyntax invocation, SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            if (invocation.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                if (memberAccess.Name.Identifier.Text != "ThrowIfNull")
                    return false;

                if (semanticModel.GetSymbolInfo(invocation, cancellationToken).Symbol is IMethodSymbol { ContainingType: { Name: "ArgumentNullException" } containingType } && containingType.ContainingNamespace?.ToDisplayString() == "System")
                    return true;
            }

            return false;
        }
    }
}
