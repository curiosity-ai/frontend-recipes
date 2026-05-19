using Mosaik;
using Mosaik.Components;
using Mosaik.Components.Nodes;
using Mosaik.Schema;
using Tesserae;
using static H5.Core.dom;
using static Mosaik.UI;
using static Tesserae.UI;

namespace FrontEnd.Recipes.Recipes._02_SearchArea
{
    /// <summary>
    /// Three SearchArea configurations on a single page — defaults, type-filtered with facets,
    /// and a fully customised renderer that replaces the search result card with a compact button.
    /// </summary>
    public sealed class SearchAreaView : IComponent
    {
        private readonly IComponent _container;

        public SearchAreaView(Parameters state)
        {
            var pivot = Pivot().S()
                .Pivot("default",    PivotTitle("Default"),    () => DefaultSearch().S(),    cached: true)
                .Pivot("with-facets", PivotTitle("With facets"), () => SearchWithFacets().S(), cached: true)
                .Pivot("custom",     PivotTitle("Custom card"), () => SearchWithCustomCard().S(), cached: true);

            _container = HubStack(HubTitle("SearchArea", "#/recipe/search-area"), DefaultRoutes.Home)
                            .Section(pivot, grow: true);
        }

        // No options: just a regular workspace search. Useful baseline.
        private IComponent DefaultSearch() => SearchArea();

        // Pre-filter to a single node type (replace with your own type to scope the search) and
        // turn on facets so users can drill into the results.
        private IComponent SearchWithFacets()
        {
            return SearchArea()
                .OnSearch(sr => sr.SetBeforeTypesFacet(N.Document.Type))
                .WithFacets();
        }

        // Replace the default search-result card with a tight button row. The "card customizer"
        // hook is also a good place to add per-row commands (open, copy URL, delete, etc.).
        private IComponent SearchWithCustomCard()
        {
            return SearchArea()
                .SearchBox(b => b.AppendToSearchBox(Button("New").SetIcon(UIcons.Plus).OnClick(() => Toast().Information("Hook up a 'new item' modal here."))))
                .Renderer(r => r.WithCustomizedRenderer((hit, rendered) =>
                {
                    var title = TextBlock(NodeRenderer.GetDisplayName(hit.Node)).SemiBold().Ellipsis().Grow();
                    var type  = TextBlock(hit.Node.Type).Secondary().Tiny().PR(8);
                    var row   = HStack().AlignItemsCenter().WS().Children(Icon(UIcons.Box).PR(8), title, type);

                    var btn = Button().WS().ReplaceContent(row).OnClick(() => NodePreview.For(hit.Node));
                    return new ReplacedResult(btn, rendered);
                }));
        }

        public HTMLElement Render() => _container.Render();
    }

    // Local schema constants — the SearchArea cares about node-type names as strings. Production
    // code typically auto-generates this from the workspace; here we just pin a built-in type.
    internal static class N
    {
        public sealed class Document
        {
            public const string Type = "_Document";
        }
    }
}
