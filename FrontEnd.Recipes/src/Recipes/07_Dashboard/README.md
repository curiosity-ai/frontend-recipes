# 07 — Dashboards

A small dashboard that composes the three pieces every real Curiosity dashboard uses:

1. **Stat cards** — one number per important KPI, across the top.
2. **Charts** — Tesserae's `LineChart` and `BarChart`, which follow the workspace theme in light and dark mode.
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

## Chart tips

- A chart fills its container: give it a height (`.H(220)`) and `.WS()` for the width.
- `.Series(name, values)` adds a named series; `.XAxis(labels)` sets the categories; `.Tooltips()` and `.Legend()` turn those on.
- For data that changes, bind a series to an observable (`.Series(IObservable<double[]>)`) instead of rebuilding the chart.
- `AreaChart`, `PieChart` and `HeatMap` share the same API.

## See also

- [`Mosaik.FrontEnd.Admin/src/Hubs/UsageView.cs`](https://github.com/curiosity-ai/mosaik/blob/master/FrontEnd/Mosaik.FrontEnd.Admin/src/Hubs/UsageView.cs) — the workspace's own usage dashboard, in production.
- [Tesserae charts](https://docs.curiosity.ai/tesserae/charts/charts) — every chart type and option.
- [Connector Recipes](https://github.com/curiosity-ai/connector-recipes) — ingest the data the dashboard will visualise.
