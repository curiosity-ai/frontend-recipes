# 02 — SearchArea

Three flavours of `SearchArea` shown side-by-side in a `Pivot` so you can compare them.

## What it shows

- **Default** — `SearchArea()` with nothing customised. Hits the workspace's default search endpoint and renders the standard result cards.
- **With facets** — Pre-filter the query with `OnSearch(sr => sr.SetBeforeTypesFacet("..."))` and enable the facet sidebar with `.WithFacets()`. This is the right starting point for "search inside a single node type".
- **Custom card** — Replace the search-result card entirely with `Renderer(r => r.WithCustomizedRenderer((hit, rendered) => new ReplacedResult(...)))`. Use this when you need a denser list, custom row commands, or to surface fields the default card doesn't show.

The third example also adds a button next to the search box using `SearchBox(b => b.AppendToSearchBox(...))`.

## Key file

- [`SearchAreaView.cs`](./SearchAreaView.cs)

## See also

- [`Mosaik.FrontEnd.LicenseServer/src/CustomersView.cs`](https://github.com/curiosity-ai/mosaik/blob/master/FrontEnd/Mosaik.FrontEnd.LicenseServer/src/CustomersView.cs) — uses `WithCardCustomizer` to add per-row commands (open, copy link, delete).
- [`Mosaik.FrontEnd.Admin/src/Notifications/Settings/NotificationSettings.cs`](https://github.com/curiosity-ai/mosaik/blob/master/FrontEnd/Mosaik.FrontEnd.Admin/src/Notifications/Settings/NotificationSettings.cs) — uses `WithCustomizedRenderer` to fully take over rendering.
- The [Connector Recipes](https://github.com/curiosity-ai/connector-recipes) show how to ingest the data this search area will query.
