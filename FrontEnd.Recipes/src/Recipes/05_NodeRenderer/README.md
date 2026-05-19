# 05 — INodeRenderer

How to take over node rendering for a custom node type: define a schema, register it with the workspace, and implement `INodeRenderer` for the cards / preview / full view.

## What it shows

Three files, one responsibility each:

| File | Responsibility |
|---|---|
| [`RecipeBookSchema.cs`](./RecipeBookSchema.cs) | Compile-time constants for the node type and its fields. Mirrors the auto-generated `Schema.cs` you'd get from `Manage → Interface → Download template`. |
| [`NodeRendererSchema.cs`](./NodeRendererSchema.cs) | Creates the schema on the workspace on startup via `Mosaik.API.Schemas.Create(...)`. Admin only — the call is a no-op for everyone else. |
| [`RecipeBookRenderer.cs`](./RecipeBookRenderer.cs) | The `INodeRenderer` implementation. Mosaik discovers it automatically via `App.AutoDiscoverViews()`. |
| [`NodeRendererView.cs`](./NodeRendererView.cs) | The recipe's landing page — explains the wiring and shows a live `SearchArea` filtered to `RecipeBook`. |

## The schema-creation flow

The HTTP call that creates a schema used to live inline inside `Mosaik.FrontEnd.Admin/.../SchemaEditor.cs`:

```csharp
await REQ.New("schema").WithBody(schema).PutAsync();
```

For this recipe (and for any other code that needs to create a schema outside of the admin UI), the call was moved into `Mosaik.FrontEnd.API`:

```csharp
// Mosaik.FrontEnd.API/API.Schemas.cs
public static async Task Create(SchemaDefinition schema)
    => await REQ.New("schema").WithBody(schema).PutAsync();
```

`SchemaEditor.cs` now calls `API.Schemas.Create(schema)` directly, and so does this recipe.

## The renderer contract

`INodeRenderer` is three methods:

```csharp
public interface INodeRenderer : INodeStyle
{
    CardContent           CompactView(Node node);
    Task<CardContent>     PreviewAsync(Node node, Parameters parameters);
    Task<IComponent>      ViewAsync(Node node, Parameters parameters);
}
```

| Method | Where it shows up |
|---|---|
| `CompactView` | Search results, dense lists, the side panel. Keep this tight. |
| `PreviewAsync` | The modal that opens when a user clicks "Quick look" on a card. |
| `ViewAsync` | The dedicated detail page when the user navigates to the node directly. |

In this recipe, `PreviewAsync` and `ViewAsync` share a body — the only difference is whether the header and body get merged into a single page (`.Merge()`).

## Where the data comes from

To see your custom renderer in action you need a few `RecipeBook` nodes in the workspace. Ingest them with any of the connector samples in [Connector Recipes](https://github.com/curiosity-ai/connector-recipes) — point a CSV / JSON connector at a book dataset and map columns to the `Title / Author / Year / Genre / ISBN` fields.

## See also

- The `INodeRenderer` reference: [`Mosaik.FrontEnd/src/INodeRenderer.cs`](https://github.com/curiosity-ai/mosaik/blob/master/FrontEnd/Mosaik.FrontEnd/src/INodeRenderer.cs).
- Renderer auto-discovery: [`Mosaik.FrontEnd/src/App.cs`](https://github.com/curiosity-ai/mosaik/blob/master/FrontEnd/Mosaik.FrontEnd/src/App.cs) (`AutoDiscoverViews`).
- A larger example: [`TechnicalSupport.FrontEnd/src/Views/DeviceRenderer.cs`](https://github.com/curiosity-ai/technical-support/blob/main/custom-front-end/src/Views/DeviceRenderer.cs).
