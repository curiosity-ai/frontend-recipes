using System.Threading.Tasks;
using Mosaik;
using Mosaik.Components;
using Tesserae;
using static Mosaik.UI;
using static Tesserae.UI;
using Node = Mosaik.Schema.Node;

namespace FrontEnd.Recipes.Recipes._05_NodeRenderer
{
    /// <summary>
    /// INodeRenderer for the <c>RecipeBook</c> node type. The class is discovered automatically by
    /// Mosaik's <c>AutoDiscoverViews</c> step — no manual registration needed, just implement the
    /// interface and provide a parameter-less constructor.
    /// </summary>
    public sealed class RecipeBookRenderer : INodeRenderer
    {
        public string NodeType    => RecipeN.RecipeBook.Type;
        public string DisplayName => "Recipe Book";
        public string LabelField  => RecipeN.RecipeBook.Title;
        public string Color       => "#8e44ad";
        public UIcons Icon        => UIcons.Book;

        // Compact view: what shows up inside search results and dense lists. Keep it tight — a
        // header with the icon, label and a one-line summary is usually enough.
        public CardContent CompactView(Node node)
        {
            return CardContent(Header(this, node), TextBlock("by " + node.GetString(RecipeN.RecipeBook.Author)).Secondary());
        }

        // Preview: shown inside a modal when the user clicks "Quick look" on a card. Same shape as
        // the full view, just usually less dense.
        public Task<CardContent> PreviewAsync(Node node, Parameters state)
        {
            return Task.FromResult(CardContent(Header(this, node), BuildBody(node)));
        }

        // Full view: the dedicated detail page when the user opens the node directly.
        public async Task<IComponent> ViewAsync(Node node, Parameters state)
        {
            return (await PreviewAsync(node, state)).Merge();
        }

        private static IComponent BuildBody(Node node)
        {
            return Pivot().S()
                .Pivot("overview", PivotTitle("Overview"), () => RenderOverview(node))
                .Pivot("metadata", PivotTitle("Metadata"), () => RenderMetadata(node));
        }

        private static IComponent RenderOverview(Node node)
        {
            return VStack().S().Children(
                Label("Title").Inline().AutoWidth().SetContent(TextBlock(node.GetString(RecipeN.RecipeBook.Title)).SemiBold()),
                Label("Author").Inline().AutoWidth().SetContent(TextBlock(node.GetString(RecipeN.RecipeBook.Author))),
                Label("Year").Inline().AutoWidth().SetContent(TextBlock(node.GetInt(RecipeN.RecipeBook.Year).ToString())),
                Label("Genre").Inline().AutoWidth().SetContent(TextBlock(node.GetString(RecipeN.RecipeBook.Genre)))
            ).P(16);
        }

        private static IComponent RenderMetadata(Node node)
        {
            return VStack().S().Children(
                Label("ISBN (key)").Inline().AutoWidth().SetContent(TextBlock(node.GetString(RecipeN.RecipeBook.ISBN))),
                Label("Node UID").Inline().AutoWidth().SetContent(TextBlock(node.UID.ToString()))
            ).P(16);
        }
    }
}
