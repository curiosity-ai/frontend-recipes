using Mosaik;
using Mosaik.Components;
using Mosaik.Components.Nodes;
using Mosaik.Schema;
using Tesserae;
using static Transpose.Core.dom;
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

            _container = Hub("SearchArea", "#/recipe/search-area")
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

        // Change the row every result is drawn as. CustomizeResult hands you the row the node's
        // registered renderer produced: return it changed (as here) or return a different one.
        // It is also the place for per-row commands (open, copy URL, delete, ...).
        private IComponent SearchWithCustomCard()
        {
            return SearchArea()
                .SearchBox(b => b.AppendToSearchBox(Button("New").SetIcon(UIcons.Plus).OnClick(() => Toast().Information("Hook up a 'new item' modal here."))))
                .Renderer(r => r.CustomizeResult(result => result
                    .SetIcon(UIcons.Box)
                    .SetBadge(result.Result.Type)
                    .OpenWith(() => NodePreview.For(result.Result))));
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
