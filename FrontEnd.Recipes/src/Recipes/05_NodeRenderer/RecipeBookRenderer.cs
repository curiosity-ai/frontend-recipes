using System.Collections.Generic;
using Mosaik;
using Mosaik.Components;
using Mosaik.Schema;
using Tesserae;
using static Mosaik.UI;
using static Tesserae.UI;
using Node = Mosaik.Schema.Node;

namespace FrontEnd.Recipes.Recipes._05_NodeRenderer
{
    /// <summary>
    /// Renderer for the <c>RecipeBook</c> node type. It is discovered automatically: a public class that
    /// derives from <c>NodeRendererBase</c> and has a parameterless constructor is registered for the node
    /// type its style names. The base class draws every surface (search row, preview sheet, full page);
    /// override only the block you want to change.
    /// </summary>
    public sealed class RecipeBookRenderer : NodeRendererBase
    {
        public RecipeBookRenderer() : base(new SchemaStyleInfo
        {
            Name        = RecipeN.RecipeBook.Type,
            DisplayName = "Recipe Book",
            LabelField  = RecipeN.RecipeBook.Title,
            Color       = "#8e44ad",
            Icon        = UIcons.Book.ToString(),
        })
        {
        }

        // The dense row: search results, node lists. Start from the standard row and say what differs.
        public override OmniResult<Node> CompactView(Node node)
        {
            return NodeResult.For(this, node).SetText("by " + node.GetString(RecipeN.RecipeBook.Author));
        }

        // Which properties the Overview tab shows, and in which order.
        protected override IEnumerable<string> GetDisplayPropertyNames(Node node)
        {
            yield return RecipeN.RecipeBook.Title;
            yield return RecipeN.RecipeBook.Author;
            yield return RecipeN.RecipeBook.Year;
            yield return RecipeN.RecipeBook.Genre;
        }

        // The tabs of the preview sheet and the full page: keep the standard ones and add one of our own.
        protected override IEnumerable<NodeTab> BuildTabs(Node node, IReadOnlyList<RelationshipGroup> groups, NodeView nodeView)
        {
            foreach (var tab in base.BuildTabs(node, groups, nodeView))
            {
                yield return tab;
            }

            yield return new NodeTab
            {
                Id         = "metadata",
                Label      = "Metadata",
                TabTitle   = () => TextBlock("Metadata"),
                TabContent = () => RenderMetadata(nodeView.Node)
            };
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
