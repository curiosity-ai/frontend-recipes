# 07 — Dashboards

A small dashboard that composes the three pieces every real Curiosity dashboard uses:

1. **Stat cards** — one number per important KPI, across the top.
2. **Plotly charts** — time-series, bars, and any of the [Plotly.js trace types](https://plotly.com/javascript/).
3. **A leaderboard** — top categories / users / facet values, on the side.

## How the data flows

- The view calls `RecipeEndpoints.GetDashboardSeriesAsync()` and `RecipeEndpoints.GetTopCategoriesAsync()` from inside a `Defer(async () => ...)` block, so the network calls only run when the tab is mounted.
- Both methods currently return hard-coded values from `src/API/Endpoints.cs`. Each one has the production-style call commented out — uncomment it (and delete the canned `return`) once you have a matching workspace endpoint:

```csharp
return await Mosaik.API.Endpoints.CallAsync<DashboardSeriesResponse>("recipes/dashboard-series");
```

- The DTOs live next door in `src/Schema/DTOs.cs`. They are `[ObjectLiteral]` so they round-trip cleanly through the JSON layer.

## Key file

- [`DashboardView.cs`](./DashboardView.cs)

## Plotly tips

- Always wrap charts in `Defer(...)` so they only build when visible — Plotly is a relatively heavy library to instantiate.
- Use `PlotlyConfig.Background()`, `PlotlyConfig.Font()`, `PlotlyConfig.PaperBackground()` to inherit the workspace theme. They handle light/dark mode for you.
- `Layout.autosize(true)` + `.WS()` on the wrapping component is the standard recipe for charts that need to fill their container.

## See also

- [`Mosaik.FrontEnd.Admin/src/Hubs/UsageView.cs`](https://github.com/curiosity-ai/mosaik/blob/master/FrontEnd/Mosaik.FrontEnd.Admin/src/Hubs/UsageView.cs) — the workspace's own usage dashboard, in production.
- [Plotly.H5 reference](https://www.nuget.org/packages/Plotly.H5/) — the strongly-typed binding used here.
- [Connector Recipes](https://github.com/curiosity-ai/connector-recipes) — ingest the data the dashboard will visualise.
