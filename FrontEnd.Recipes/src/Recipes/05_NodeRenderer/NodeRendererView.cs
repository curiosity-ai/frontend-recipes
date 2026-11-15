using Mosaik;
using Mosaik.Components;
using Tesserae;
using static Transpose.Core.dom;
using static Mosaik.UI;
using static Tesserae.UI;

namespace FrontEnd.Recipes.Recipes._05_NodeRenderer
{
    /// <summary>
    /// Landing page for the node renderer recipe — explains the moving parts and gives a live
    /// SearchArea pre-filtered to the <c>RecipeBook</c> node type so the renderer is visible.
    /// </summary>
    public sealed class NodeRendererView : IComponent
    {
        private readonly IComponent _container;

        public NodeRendererView(Parameters state)
        {
            var intro = Card(VStack().WS().Children(
                TextBlock("How this recipe works").SemiBold().Large(),
                Bullet("Schema lives in RecipeBookSchema.cs — strongly-typed constants for the node type and its fields."),
                Bullet("The RecipeBook schema itself belongs to the workspace: a data connector or the workspace definitions create it, not the front end. The README shows both."),
                Bullet("RecipeBookRenderer derives from NodeRendererBase; the workspace discovers it at startup and uses it for every RecipeBook node: the search row, the preview sheet and the full page."),
                Bullet("It overrides only what differs: the row's text, the properties shown, and one extra tab.")
            ).P(16)).WS().MB(16);

            var demo = Card(VStack().WS().Children(
                TextBlock("Live preview").SemiBold().Large(),
                TextBlock("Every RecipeBook node in the workspace is drawn by RecipeBookRenderer: open one to see its preview sheet.").Secondary(),
                SearchArea()
                    .OnSearch(s => s.SetBeforeTypesFacet(RecipeN.RecipeBook.Type))
                    .WithFacets()
                    .S()
                    .H(500)
            ).P(16)).WS();

            _container = Hub("Node Renderer", "#/recipe/node-renderer")
                            .Section(VStack().WS().Children(intro, demo).P(16), grow: true);
        }

        private static IComponent Bullet(string text) =>
            HStack().AlignItems(ItemAlign.Start).Children(Icon(UIcons.Check).PR(8).PT(4), TextBlock(text).BreakSpaces()).PB(4);

        public HTMLElement Render() => _container.Render();
    }
}
