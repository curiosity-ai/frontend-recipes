# Front-End Recipes

A library of self-contained, runnable examples for building **custom front-ends** on top of a [Curiosity](https://curiosity.ai) workspace using the Mosaik front-end framework, the Tesserae UI library, and the `h5` C# → JavaScript compiler.

Where the [Connector Recipes](https://github.com/curiosity-ai/connector-recipes) cover ingesting data **into** a Curiosity workspace, these recipes cover putting a custom UI **on top of one**.

All ten recipes live inside a single front-end project — [`Mosaik.FrontEnd.Recipes`](./Mosaik.FrontEnd.Recipes/) — so they build together and can be deployed as a single front-end bundle to your workspace.

---

## Table of contents

1. [What's in here](#whats-in-here)
2. [The recipes](#the-recipes)
3. [Prerequisites](#prerequisites)
4. [Running locally](#running-locally)
5. [Deploying to a workspace](#deploying-to-a-workspace)
6. [Code shape](#code-shape)
7. [Reusing a recipe](#reusing-a-recipe)
8. [License](#license)

---

## What's in here

```
Mosaik.FrontEnd.Recipes/
├── Mosaik.FrontEnd.Recipes.csproj
├── h5.json
├── h5.Release.json
└── src/
    ├── App.cs                    ← main entry: routes + sidebar + HomeView replacement
    ├── Recipes.cs                ← catalog used by App.cs + the landing page
    ├── RecipesHomeView.cs        ← replaces the workspace's default home view
    ├── API/
    │   └── Endpoints.cs          ← Mosaik.API.Endpoints.CallAsync wrappers (currently mocked)
    ├── Schema/
    │   └── DTOs.cs               ← [ObjectLiteral] DTOs returned by Endpoints.cs
    └── Recipes/
        ├── 01_HelloWorld/
        ├── 02_SearchArea/
        ├── 03_Pivot/
        ├── 04_SegmentedPivot/
        ├── 05_NodeRenderer/
        ├── 06_NeighborsGraph/
        ├── 07_Dashboard/
        ├── 08_CustomChat/
        ├── 09_Sidebar/
        └── 10_UserPreferences/
```

Each recipe folder is **self-contained**: a `README.md` explaining the idea and a single (or small handful of) `.cs` file(s) showing the smallest amount of code that makes the feature work.

## The recipes

| # | Recipe | What it covers |
|---|---|---|
| 01 | [Hello World](./Mosaik.FrontEnd.Recipes/src/Recipes/01_HelloWorld/) | The minimal Curiosity page — routing, `HubStack`, `HubTitle`, basic Tesserae components. |
| 02 | [SearchArea](./Mosaik.FrontEnd.Recipes/src/Recipes/02_SearchArea/) | Wiring the workspace search box, facets, and custom search-result renderers. |
| 03 | [Pivot](./Mosaik.FrontEnd.Recipes/src/Recipes/03_Pivot/) | Tabbed pages — cached vs lazy, justified, centered, overflow. |
| 04 | [SegmentedPivot](./Mosaik.FrontEnd.Recipes/src/Recipes/04_SegmentedPivot/) | Compact segmented-control toggles for filters and views. |
| 05 | [INodeRenderer](./Mosaik.FrontEnd.Recipes/src/Recipes/05_NodeRenderer/) | Custom node schema + custom cards / previews / detail pages, with admin-side schema bootstrap. |
| 06 | [Neighbors & Graph](./Mosaik.FrontEnd.Recipes/src/Recipes/06_NeighborsGraph/) | Traversal via `Mosaik.API.Query`, the `Neighbors` list component, and the interactive `GraphExplorerView`. |
| 07 | [Dashboards](./Mosaik.FrontEnd.Recipes/src/Recipes/07_Dashboard/) | Stat cards + Plotly line / bar charts + a top-categories card, driven by endpoints. |
| 08 | [Custom Chat](./Mosaik.FrontEnd.Recipes/src/Recipes/08_CustomChat/) | Replace `PostMessage`, customise the chat header, examples and per-message actions. |
| 09 | [Sidebar](./Mosaik.FrontEnd.Recipes/src/Recipes/09_Sidebar/) | Add custom buttons to the default sidebar and react to the active route. |
| 10 | [User Preferences](./Mosaik.FrontEnd.Recipes/src/Recipes/10_UserPreferences/) | Custom preferences page surfaced under the `UserPreferences` sidebar mode. |

The home view (`settings.HomeView = state => new RecipesHomeView(state);`) is replaced once for the whole project and shows the catalog as a card grid — the same `RecipeCatalog` table drives the sidebar entries and the routes.

## Prerequisites

- **.NET SDK** that matches the H5 toolchain bundled in the csproj.
- The **h5 compiler** as a global dotnet tool:

  ```bash
  dotnet tool update --global h5-compiler
  ```

- The **Curiosity CLI** if you want to push the front-end to a workspace from the command line:

  ```bash
  dotnet tool update --global Curiosity.CLI
  ```

- A **Curiosity workspace** to connect to. Local workspaces typically run at `http://localhost:8080/`.

## Running locally

From the repo root:

```bash
cd Mosaik.FrontEnd.Recipes
dotnet build

# Serve the compiled bundle against a workspace
curiosity-cli serve \
    -s    http://localhost:8080 \
    -p    bin/Debug/netstandard2.0/h5 \
    -port 5000
```

Add the serving URL to your workspace's CORS allow-list:

```bash
export MSK_CORS=http://localhost:5000
```

Then open `http://localhost:5000` in a browser. The first thing you should see is the catalog landing page (the replaced `HomeView`).

## Deploying to a workspace

Zip the `h5` output folder and upload it via **Manage → Interface → Upload Front End**, or push it directly with the CLI:

```bash
curiosity-cli upload-front-end \
    -s http://localhost:8080/ \
    -t $CURIOSITY_INTERFACE_TOKEN \
    -p bin/Release/netstandard2.0/h5/
```

## Code shape

Every recipe shares the same skeleton:

```csharp
public sealed class XyzView : IComponent
{
    private readonly IComponent _container;

    public XyzView(Parameters state)
    {
        _container = HubStack(HubTitle("Title", "#/recipe/xyz"), DefaultRoutes.Home)
                        .Section(BuildBody(state), grow: true);
    }

    private IComponent BuildBody(Parameters state) { /* ... */ }

    public HTMLElement Render() => _container.Render();
}
```

`App.cs` registers it as a route and adds a `SidebarButton` for it — both driven from `Recipes.cs`, so adding an eleventh recipe is a matter of dropping a new entry into `RecipeCatalog.All` and creating a folder under `src/Recipes/`.

### Talking to the workspace

Anywhere a recipe would hit a custom workspace endpoint, the call goes through `src/API/Endpoints.cs`. Each method has the production-style call commented out and returns hard-coded data so this repo runs without any backend:

```csharp
public static async Task<DashboardSeriesResponse> GetDashboardSeriesAsync()
{
    // return await Mosaik.API.Endpoints.CallAsync<DashboardSeriesResponse>("recipes/dashboard-series");

    return new DashboardSeriesResponse { /* canned data */ };
}
```

DTOs returned by those endpoints live in `src/Schema/DTOs.cs` and use `[ObjectLiteral]` so they round-trip cleanly through H5.

### Creating a node schema from the front-end

The INodeRenderer recipe needs a schema in the workspace to render against. The HTTP call that creates a schema used to live inline inside `Mosaik.FrontEnd.Admin/.../SchemaEditor.cs` (`await REQ.New("schema").WithBody(schema).PutAsync();`). It now lives in `Mosaik.FrontEnd.API/API.Schemas.cs`:

```csharp
public static async Task Create(SchemaDefinition schema)
    => await REQ.New("schema").WithBody(schema).PutAsync();
```

so any front-end can register a schema with a single call:

```csharp
await Mosaik.API.Schemas.Create(new SchemaDefinition(name, type, key, fields));
```

The INodeRenderer recipe gates this behind `CurrentUser.IsAdmin` — non-admins still see existing nodes rendered, the schema just isn't auto-provisioned for them.

## Reusing a recipe

1. **Copy the folder** under `src/Recipes/` to your own project (or a new folder here).
2. **Rename the view class** and adjust its namespace.
3. **Register the route** in your `App.cs` (or add an entry to `RecipeCatalog.All` if you're staying inside this project — the route, sidebar entry and home-page card all light up automatically).
4. **Wire the data** — replace any call into `RecipeEndpoints` with a real `Mosaik.API.Endpoints.CallAsync<T>` to your workspace endpoint.

## License

MIT — see [LICENSE](./LICENSE).
