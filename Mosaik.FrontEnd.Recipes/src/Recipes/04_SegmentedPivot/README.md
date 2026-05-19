# 04 — SegmentedPivot

`SegmentedPivot` is `Pivot`'s compact cousin — same API, but styled as a segmented control. It reads as "pick exactly one", so reach for it when the choices are mutually-exclusive filters or short toggles.

## When to use which

| Use `Pivot` when… | Use `SegmentedPivot` when… |
|---|---|
| Tabs are *page sections* (Overview / Activity / Settings). | Tabs are *filters or toggles* (All / Open / Closed). |
| Labels are long or vary in length. | Labels are short and roughly the same length. |
| You may have many tabs (overflow matters). | You have 2–5 tightly related options. |
| Tabs each render a substantial view. | Tabs render the same kind of thing, filtered. |

## Key file

- [`SegmentedPivotView.cs`](./SegmentedPivotView.cs)

## See also

- [Pivot recipe](../03_Pivot/) for the full tab-strip story.
- [`Tesserae.Tests/src/Samples/Surfaces/SegmentedPivotSample.cs`](https://github.com/curiosity-ai/tesserae/blob/main/Tesserae.Tests/src/Samples/Surfaces/SegmentedPivotSample.cs).
