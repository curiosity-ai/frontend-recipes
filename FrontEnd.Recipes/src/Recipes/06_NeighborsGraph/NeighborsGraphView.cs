using Mosaik;
using Mosaik.Components;
using Tesserae;
using static Transpose.Core.dom;
using static Mosaik.UI;
using static Tesserae.UI;

namespace FrontEnd.Recipes.Recipes._06_NeighborsGraph
{
    /// <summary>
    /// Three ways to traverse the graph from the front-end: query helpers, the Neighbors component
    /// (a search-area scoped to a query), and the interactive GraphExplorerView.
    ///
    /// All snippets are wrapped in <c>Defer(...)</c> so they only run when their tab is shown.
    /// </summary>
    public sealed class NeighborsGraphView : IComponent
    {
        private readonly IComponent _container;

        public NeighborsGraphView(Parameters state)
        {
            var pivot = Pivot().S()
                .Pivot("query",     PivotTitle("Direct query"),      () => QuerySnippet().S(),     cached: true)
                .Pivot("neighbors", PivotTitle("Neighbors"),         () => NeighborsExample().S(), cached: true)
                .Pivot("explorer",  PivotTitle("Graph explorer"),    () => GraphExample().S(),    cached: true);

            _container = Hub("Neighbors & Graph", "#/recipe/neighbors-graph")
                            .Section(pivot, grow: true);
        }

        private static IComponent QuerySnippet()
        {
            return Card(VStack().WS().Children(
                TextBlock("Direct query").SemiBold().Large(),
                TextBlock("Call Mosaik.API.Query from anywhere — the same fluent API is used by Neighbors and SearchArea internally.").Secondary(),
                CodeBlock(@"// Get the documents linked to a given user
var docs = await Mosaik.API.Query
    .StartAt(new UID128(""... user uid ...""))
    .Out(""_Document"", ""HasDocument"")
    .Take(50)
    .GetAsync();

// Same query, but only return UIDs (cheaper)
var uids = await Mosaik.API.Query
    .StartAt(new UID128(""... user uid ...""))
    .Out(""_Document"", ""HasDocument"")
    .TakeAll()
    .GetUIDsAsync();")
            ).P(16));
        }

        private static IComponent NeighborsExample()
        {
            return Card(VStack().WS().Children(
                TextBlock("Neighbors component").SemiBold().Large(),
                TextBlock("Neighbors wraps a SearchArea with a query that materialises only when the component is mounted.").Secondary(),
                CodeBlock(@"// Inside a node detail view:
return Neighbors(
    () => Mosaik.API.Query
              .StartAt(node.UID)
              .Out(""_Document"", ""HasDocument"")
              .TakeAll()
              .GetUIDsAsync(),
    new[] { ""_Document"" },
    showSearchBox: true,
    facetDisplay: FacetDisplayOptions.Visible).S();"),
                TextBlock("The same pattern works inside a node renderer, to list a node's related nodes.").Secondary().PT(8)
            ).P(16));
        }

        private static IComponent GraphExample()
        {
            return Card(VStack().WS().Children(
                TextBlock("Graph Explorer view").SemiBold().Large(),
                TextBlock("Render an interactive node-link diagram for a list of UIDs. Use it inside a node detail tab to show what the node is connected to.").Secondary(),
                CodeBlock(@"// Inside a node renderer's view: the node and everything it is connected to
return GraphExplorerView.ViewFor(expandNeighbors: true, node.UID).S();

// Or exactly the nodes a query returns
return Defer(async () =>
{
    var result = await Mosaik.API.Query
                    .StartAt(node.UID)
                    .Out(""_Document"")
                    .TakeAll()
                    .GetUIDsAsync();

    return GraphExplorerView.ViewFor(result.UIDs.ToArray()).S();
}).S();")
            ).P(16));
        }

        private static IComponent CodeBlock(string code)
        {
            var pre = TextBlock(code).BreakSpaces().Class("recipe-code");
            return Card(pre).WS().P(8);
        }

        public HTMLElement Render() => _container.Render();
    }
}
