using System;
using System.Linq;
using Mosaik;
using Mosaik.Components;
using Tesserae;
using static H5.Core.dom;
using static Mosaik.UI;
using static Tesserae.UI;

namespace FrontEnd.Recipes.Recipes._03_Pivot
{
    /// <summary>
    /// Demonstrates the most useful Pivot configurations: default, justified, centered, cached vs
    /// not cached (note the clock value), max-height with scroll, and a tab strip with overflow.
    /// </summary>
    public sealed class PivotView : IComponent
    {
        private readonly IComponent _container;

        public PivotView(Parameters state)
        {
            var body = VStack().WS().Children(
                Section("Normal", "Tabs render in their natural width and align left.",
                    Threetab()),

                Section("Justified", "Tabs stretch to fill the available width.",
                    Threetab().Justified()),

                Section("Centered", "Tabs are centered horizontally inside the strip.",
                    Threetab().Centered()),

                Section("Cached vs. not cached",
                    "The first tab caches its rendered content — switching away and back keeps the same timestamp. The second rebuilds on every visit.",
                    Pivot()
                        .Pivot("c1", PivotTitle("Cached"),     () => TextBlock(DateTimeOffset.UtcNow.ToString()).P(16).Regular(), cached: true)
                        .Pivot("c2", PivotTitle("Not Cached"), () => TextBlock(DateTimeOffset.UtcNow.ToString()).P(16).Regular(), cached: false)),

                Section("Scroll with max height",
                    "Pin a height so long-content tabs scroll instead of pushing the page down.",
                    Pivot().MaxHeight(320.px())
                        .Pivot("5",   PivotTitle("5 items"),   () => Items(5),   cached: true)
                        .Pivot("20",  PivotTitle("20 items"),  () => Items(20),  cached: true)
                        .Pivot("100", PivotTitle("100 items"), () => Items(100), cached: true)),

                Section("Tab overflow",
                    "Many tabs collapse into a chevron / overflow menu — use this for settings panes.",
                    ManyTabs())
            ).P(16);

            _container = HubStack(HubTitle("Pivot", "#/recipe/pivot"), DefaultRoutes.Home)
                            .Section(body, grow: true);
        }

        private static IComponent Section(string title, string description, IComponent content)
        {
            return Card(VStack().WS().Children(
                TextBlock(title).SemiBold().Large(),
                TextBlock(description).Secondary(),
                content.MT(8)
            ).P(16)).WS().MB(16);
        }

        private static Pivot Threetab() =>
            Pivot()
                .Pivot("one",   PivotTitle("Overview"), () => TextBlock("Overview content").P(16))
                .Pivot("two",   PivotTitle("Activity"), () => TextBlock("Activity content").P(16))
                .Pivot("three", PivotTitle("Settings"), () => TextBlock("Settings content").P(16));

        private static IComponent Items(int count)
        {
            return VStack().WS().Children(
                Enumerable.Range(1, count)
                    .Select(i => Card(TextBlock("Item " + i)).MB(4).MinWidth(200.px()).Class("recipe-pivot-row"))
                    .ToArray<IComponent>()
            );
        }

        private static IComponent ManyTabs()
        {
            var titles = new[]
            {
                "Overview", "Activity", "Pull Requests", "Code Review",
                "Builds",   "Tests",    "Deployments",   "Issues",
                "Docs",     "Team",     "Metrics",       "Audits",
                "Releases", "Settings", "Integrations",  "Audit Log"
            };

            var pivot = Pivot().H(220);

            for (var i = 0; i < titles.Length; i++)
            {
                var t = titles[i];
                pivot = pivot.Pivot("many-" + i, PivotTitle(t), () => TextBlock("Content for: " + t).P(16), cached: true);
            }

            return pivot;
        }

        public HTMLElement Render() => _container.Render();
    }
}
