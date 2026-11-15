# 01 — Hello World

The smallest useful Curiosity Workspace page: a single route, a Tesserae `Hub` with its header, and some content inside.

## What it shows

- Registering a route via `Router.Register("#/recipe/hello-world", state => App.ShowDefault(new HelloWorldView(state)))` — see [`App.cs`](../../App.cs).
- Putting the content in a `Hub("Hello World", "#/recipe/hello-world")`, so the page gets the workspace's breadcrumb header with the page name linking back to its route.
- Stacking Tesserae components (`VStack`, `HStack`, `TextBlock`, `Button`, `Icon`) with the fluent style.
- Triggering navigation between recipes with `Router.Navigate(...)`.

## Key file

- [`HelloWorldView.cs`](./HelloWorldView.cs)

## Going further

- The same idea is used by every other recipe in this project — they all return an `IComponent` from a factory registered in `App.cs`.
- For data-driven pages, swap the static body for a `Defer(async () => ...)` block — see the **Dashboards** recipe.
- For richer page chrome, add crumbs with `.Crumb(...)` and buttons on the right with `.Commands(...)`, or see [`Mosaik.FrontEnd.LicenseServer/src/LicensesView.cs`](https://github.com/curiosity-ai/mosaik/blob/master/FrontEnd/Mosaik.FrontEnd.LicenseServer/src/LicensesView.cs) in the Mosaik repo, and the [Connector Recipes](https://github.com/curiosity-ai/connector-recipes) for the equivalent on the data-ingestion side.
