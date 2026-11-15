using System.Linq;
using FrontEnd.Recipes.API;
using FrontEnd.Recipes.Schema;
using Mosaik;
using Mosaik.Components;
using Tesserae;
using static Transpose.Core.dom;
using static Mosaik.UI;
using static Tesserae.UI;

namespace FrontEnd.Recipes.Recipes._07_Dashboard
{
    /// <summary>
    /// A small dashboard built from three pieces every Curiosity dashboard ends up using:
    ///   1. Metric "stat" cards across the top.
    ///   2. Tesserae line and bar charts for the time-series.
    ///   3. A leaderboard / facet card on the side.
    ///
    /// All the data is pulled from <c>RecipeEndpoints</c>, which currently returns hard-coded
    /// values — swap the commented <c>Mosaik.API.Endpoints.CallAsync&lt;T&gt;</c> calls in there to hit
    /// a real workspace endpoint.
    /// </summary>
    public sealed class DashboardView : IComponent
    {
        private readonly IComponent _container;

        public DashboardView(Parameters state)
        {
            var body = Defer(async () =>
            {
                var series     = await RecipeEndpoints.GetDashboardSeriesAsync();
                var categories = await RecipeEndpoints.GetTopCategoriesAsync();

                return VStack().WS().Children(
                    StatRow(series, categories),
                    HStack().WS().Wrap().Children(
                        ChartCard("Opened vs Resolved", LineChart(series.Days, series.Opened, series.Resolved)).Grow(),
                        ChartCard("Median resolution (h)", BarChart(series.Days, series.MedianHours)).Grow()
                    ),
                    HStack().WS().Wrap().Children(
                        CategoriesCard(categories).Grow()
                    )
                ).P(16);
            });

            _container = Hub("Dashboard", "#/recipe/dashboard")
                            .Section(body.S(), grow: true);
        }

        private static IComponent StatRow(DashboardSeriesResponse series, TopCategoriesResponse categories)
        {
            var opened    = (int)series.Opened.Sum();
            var resolved  = (int)series.Resolved.Sum();
            var openCases = opened - resolved;
            var avgHours  = series.MedianHours.Average();

            return HStack().WS().Wrap().Children(
                Stat("Tickets opened",   opened.ToString("n0"),   UIcons.PaperPlaneTop),
                Stat("Tickets resolved", resolved.ToString("n0"), UIcons.CheckCircle),
                Stat("Currently open",   openCases.ToString("n0"), UIcons.MessageQuestion),
                Stat("Median resolution", avgHours.ToString("n1") + "h", UIcons.Clock),
                Stat("Top category",     categories.Categories[0].Label, UIcons.Tags)
            ).PB(16);
        }

        private static IComponent Stat(string label, string value, UIcons icon)
        {
            return Card(HStack().AlignItemsCenter().Children(
                Icon(icon).XXLarge().PR(12),
                VStack().Children(
                    TextBlock(value).XLarge().SemiBold(),
                    TextBlock(label).Secondary().Small()
                )
            ).P(12)).MinWidth(180.px()).M(8);
        }

        private static IComponent ChartCard(string title, IComponent chart) =>
            Card(VStack().WS().Children(
                TextBlock(title).SemiBold(),
                chart.PT(8)
            ).P(16)).MinWidth(360.px()).M(8);

        private static IComponent LineChart(string[] x, double[] series1, double[] series2)
        {
            return Tesserae.UI.LineChart()
               .XAxis(x)
               .Series("Opened",   series1)
               .Series("Resolved", series2)
               .Points()
               .Tooltips()
               .Legend()
               .H(220)
               .WS();
        }

        private static IComponent BarChart(string[] x, double[] y)
        {
            return Tesserae.UI.BarChart()
               .XAxis(x)
               .Data(y)
               .Rounded()
               .Tooltips()
               .H(220)
               .WS();
        }

        private static IComponent CategoriesCard(TopCategoriesResponse categories)
        {
            var rows = categories.Categories.Select(c =>
                HStack().AlignItemsCenter().WS().Children(
                    TextBlock(c.Label).Grow(),
                    TextBlock(c.Count.ToString("n0")).SemiBold()
                ).PB(6)
            ).ToArray<IComponent>();

            return Card(VStack().WS().Children(
                TextBlock("Top categories").SemiBold(),
                VStack().WS().PT(8).Children(rows)
            ).P(16)).MinWidth(360.px()).M(8);
        }

        public HTMLElement Render() => _container.Render();
    }
}
