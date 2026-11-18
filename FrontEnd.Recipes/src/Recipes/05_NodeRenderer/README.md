# 05 — Node Renderer

How to draw a node type your way: one class that derives from `NodeRendererBase`, discovered at startup and used wherever a `RecipeBook` node appears — the search row, the preview sheet and the full page.

## Files

| File | What it does |
|---|---|
| [`RecipeBookSchema.cs`](./RecipeBookSchema.cs) | Constants for the node type and its field names, so a renamed field is a compile error rather than an empty cell. |
| [`RecipeBookRenderer.cs`](./RecipeBookRenderer.cs) | The renderer. A public class deriving from `NodeRendererBase` with a parameterless constructor is registered automatically. |
| [`NodeRendererView.cs`](./NodeRendererView.cs) | The recipe page: an explanation and a `SearchArea` scoped to `RecipeBook`, so you can open a node and see the renderer. |

## Override the block you want to change

The base class draws every surface from the style it is given (`SchemaStyleInfo`: name, display name, label field, colour, icon). Override only what differs:

```csharp
public sealed class RecipeBookRenderer : NodeRendererBase
{
    public RecipeBookRenderer() : base(new SchemaStyleInfo
    {
        Name        = RecipeN.RecipeBook.Type,
        DisplayName = "Recipe Book",
        LabelField  = RecipeN.RecipeBook.Title,
        Color       = "#8e44ad",
        Icon        = UIcons.Book.ToString(),
    }) { }

    // The dense row (search results, lists): start from the standard row and change it.
    public override OmniResult<Node> CompactView(Node node)
        => NodeResult.For(this, node).SetText("by " + node.GetString(RecipeN.RecipeBook.Author));

    // Which properties the Overview tab shows, in this order.
    protected override IEnumerable<string> GetDisplayPropertyNames(Node node) { ... }

    // The tabs of the preview sheet and the page: the standard ones plus one of our own.
    protected override IEnumerable<NodeTab> BuildTabs(Node node, IReadOnlyList<RelationshipGroup> groups, NodeView nodeView) { ... }
}
```

| Method | Where it shows up |
|---|---|
| `CompactView` | Search results, dense lists, tables. |
| `PreviewAsync` | The preview sheet that opens when a result is clicked. The base builds it from `BuildTabs`. |
| `ViewAsync` | The full page when the user navigates to the node. The base builds it from `PreviewAsync`. |
| `GetDisplayPropertyNames`, `BuildTabs`, `RenderPropertyValue`, … | One part of the preview and the page. |

`LabelField` also takes a fallback list (`"Title|Name"`) or an interpolated label (`"$Book {Title} ({Year})"`), which is often enough without overriding `CompactView` at all.

## Where the data comes from

The renderer draws `RecipeBook` nodes, so the workspace needs that node type and some nodes. A front end does not create schemas: they belong to the workspace. Create them from a data connector with [`Curiosity.Library`](https://www.nuget.org/packages/Curiosity.Library):

```csharp
[Node]
public class RecipeBook
{
    [Key]      public string ISBN   { get; set; }
    [Property] public string Title  { get; set; }
    [Property] public string Author { get; set; }
    [Property] public int    Year   { get; set; }
    [Property] public string Genre  { get; set; }
}

using var graph = Graph.Connect("http://localhost:8080", token, "Recipe books");

await graph.CreateNodeSchemaAsync<RecipeBook>();

graph.AddOrUpdate(new RecipeBook { ISBN = "978-1594204616", Title = "Salt, Fat, Acid, Heat", Author = "Samin Nosrat", Year = 2017, Genre = "Technique" });

await graph.CommitPendingAsync();
```

The [Connector Recipes](https://github.com/curiosity-ai/connector-recipes) cover the same from CSV, JSON and other sources. A workspace can also declare the schema in its definitions, as `config/schemas/nodes/RecipeBook.cs`.

## See also

- [`NodeRendererBase.cs`](https://github.com/curiosity-ai/mosaik/blob/master/FrontEnd/Mosaik.FrontEnd/src/Components/Data/NodeRendering/NodeRendererBase.cs) — every block you can override.
- [`INodeRenderer.cs`](https://github.com/curiosity-ai/mosaik/blob/master/FrontEnd/Mosaik.FrontEnd/src/INodeRenderer.cs) — the interface the base class implements.
- A larger example: [`TechnicalSupport.FrontEnd/src/Views/DeviceRenderer.cs`](https://github.com/curiosity-ai/technical-support/blob/main/custom-front-end/src/Views/DeviceRenderer.cs).
