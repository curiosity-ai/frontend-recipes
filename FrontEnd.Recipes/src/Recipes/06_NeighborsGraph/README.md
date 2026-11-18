# 06 — Neighbors & Graph

Three ways to walk the graph from the front-end.

## What it shows

| Tab | Component | When to reach for it |
|---|---|---|
| Direct query | `Mosaik.API.Query.StartAt(...).Out(...).GetAsync()` | When you need the data in a `Defer` block or to feed something other than a list. |
| Neighbors    | `Neighbors(() => query.GetUIDsAsync(), ...)` | When you want a list with search + facets, materialised lazily when the tab is opened. |
| Graph explorer | `GraphExplorerView.ViewFor(uids)` | When the relationships *are* the story — neighbours-of-neighbours, dependencies, social graphs. |

The recipe page itself is presentation-only (live data needs real UIDs you have in your workspace) — copy the snippets into a node renderer to see them work.

## Key file

- [`NeighborsGraphView.cs`](./NeighborsGraphView.cs)

## See also

- [Node Renderer recipe](../05_NodeRenderer/) — a renderer's tabs are where these patterns usually end up.
- [Connector Recipes](https://github.com/curiosity-ai/connector-recipes) — the schemas and `[Node]` / `[Key]` / `[Property]` shapes define what `Mosaik.API.Query` traverses.
- [`TechnicalSupport.FrontEnd/src/Views/DeviceRenderer.cs`](https://github.com/curiosity-ai/technical-support/blob/main/custom-front-end/src/Views/DeviceRenderer.cs) — production-ish example with all three patterns.
