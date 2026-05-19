using Mosaik;
using Mosaik.Components;
using Tesserae;
using static H5.Core.dom;
using static Mosaik.UI;
using static Tesserae.UI;

namespace FrontEnd.Recipes.Recipes._04_SegmentedPivot
{
    /// <summary>
    /// A SegmentedPivot is the right pick when you have a small, mutually-exclusive set of views
    /// or filters — All / Open / Closed, Day / Week / Month, List / Grid. Visually it reads as a
    /// segmented control, not a tab strip, so users understand "pick exactly one of these".
    /// </summary>
    public sealed class SegmentedPivotView : IComponent
    {
        private readonly IComponent _container;

        public SegmentedPivotView(Parameters state)
        {
            var basicExample = Card(VStack().WS().Children(
                TextBlock("Filter the same content by status").SemiBold(),
                SegmentedPivot()
                    .SegmentedPivot("all",    SegmentTitle("All"),    () => Banner("Showing all 128 cases"))
                    .SegmentedPivot("open",   SegmentTitle("Open"),   () => Banner("Showing 24 open cases"))
                    .SegmentedPivot("closed", SegmentTitle("Closed"), () => Banner("Showing 104 closed cases"))
            ).P(16)).WS().MB(16);

            var rangeExample = Card(VStack().WS().Children(
                TextBlock("Time-range toggle").SemiBold(),
                SegmentedPivot()
                    .SegmentedPivot("day",   SegmentTitle("Day"),   () => Banner("Showing today"))
                    .SegmentedPivot("week",  SegmentTitle("Week"),  () => Banner("Showing the last 7 days"))
                    .SegmentedPivot("month", SegmentTitle("Month"), () => Banner("Showing the last 30 days"))
                    .SegmentedPivot("year",  SegmentTitle("Year"),  () => Banner("Showing the last 12 months"))
            ).P(16)).WS().MB(16);

            var compactExample = Card(VStack().WS().Children(
                TextBlock("Side-by-side with content").SemiBold(),
                HStack().AlignItemsCenter().WS().Children(
                    Label("View").Inline().AutoWidth(),
                    SegmentedPivot()
                        .SegmentedPivot("list", SegmentTitle("List"), () => TextBlock("List view"))
                        .SegmentedPivot("grid", SegmentTitle("Grid"), () => TextBlock("Grid view"))
                )
            ).P(16)).WS();

            var body = VStack().WS().Children(basicExample, rangeExample, compactExample).P(16);

            _container = HubStack(HubTitle("SegmentedPivot", "#/recipe/segmented-pivot"), DefaultRoutes.Home)
                            .Section(body, grow: true);
        }

        private static IComponent Banner(string text) =>
            Card(TextBlock(text).TextCenter()).WS().P(32);

        public HTMLElement Render() => _container.Render();
    }
}
