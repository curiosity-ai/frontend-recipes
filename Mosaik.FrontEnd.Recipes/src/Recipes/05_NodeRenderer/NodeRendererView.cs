using Mosaik;
using Mosaik.Components;
using Tesserae;
using static H5.Core.dom;
using static Mosaik.UI;
using static Tesserae.UI;

namespace Curiosity.FrontEnd.Recipes.Recipes._05_NodeRenderer
{
    /// <summary>
    /// Landing page for the INodeRenderer recipe — explains the moving parts and gives a live
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
                Bullet("NodeRendererSchema.EnsureSchemaAsync() (called from App.cs OnLoad) writes the schema to the workspace via Mosaik.API.Schemas.Create — admin only."),
                Bullet("RecipeBookRenderer implements INodeRenderer; Mosaik auto-discovers it at startup and uses it for every RecipeBook node it finds.")
            ).P(16)).WS().MB(16);

            var demo = Card(VStack().WS().Children(
                TextBlock("Live preview").SemiBold().Large(),
                TextBlock("Anything indexed as a RecipeBook node will be rendered with our custom card / preview / view.").Secondary(),
                SearchArea()
                    .OnSearch(s => s.SetBeforeTypesFacet(RecipeN.RecipeBook.Type))
                    .WithFacets()
                    .S()
                    .H(500)
            ).P(16)).WS();

            _container = HubStack(HubTitle("INodeRenderer", "#/recipe/node-renderer"), DefaultRoutes.Home)
                            .Section(VStack().WS().Children(intro, demo).P(16), grow: true);
        }

        private static IComponent Bullet(string text) =>
            HStack().AlignItemsStart().Children(Icon(UIcons.Check).PR(8).PT(4), TextBlock(text).BreakSpaces()).PB(4);

        public HTMLElement Render() => _container.Render();
    }
}
