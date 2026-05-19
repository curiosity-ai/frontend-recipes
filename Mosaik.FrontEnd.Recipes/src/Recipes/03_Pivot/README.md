# 03 — Pivot

Tabbed navigation built from Tesserae's `Pivot()` component. Pivots organise content into related views within the same context — a single workspace page can switch between Overview / Activity / Settings without changing routes.

## What it shows

- Default, `.Justified()`, and `.Centered()` styles.
- `cached: true` vs `cached: false` — the cached tab keeps its rendered tree (and its state) when you switch away; the uncached one rebuilds on every visit. Watch the timestamp.
- `.MaxHeight(...)` to bound a tab's height and let it scroll.
- A long list of titles to trigger the overflow chevron / "all tabs" menu.

## Key file

- [`PivotView.cs`](./PivotView.cs)

## See also

- [INodeRenderer recipe](../05_NodeRenderer/) — every node detail page uses a Pivot to organise its tabs.
- [Mosaik's PivotSample](https://github.com/curiosity-ai/tesserae/blob/main/Tesserae.Tests/src/Samples/Surfaces/PivotSample.cs) — the canonical reference in Tesserae's own test app.
