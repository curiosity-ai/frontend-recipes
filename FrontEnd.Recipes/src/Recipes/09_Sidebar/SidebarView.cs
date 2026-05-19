using Mosaik;
using Mosaik.Components;
using Tesserae;
using static H5.Core.dom;
using static Mosaik.UI;
using static Tesserae.UI;

namespace FrontEnd.Recipes.Recipes._09_Sidebar
{
    /// <summary>
    /// Documentation page for the sidebar recipe. The actual sidebar code is in App.cs — this view
    /// just explains the wiring and points the reader at it.
    /// </summary>
    public sealed class SidebarView : IComponent
    {
        private readonly IComponent _container;

        public SidebarView(Parameters state)
        {
            var explanation = Card(VStack().WS().Children(
                TextBlock("How custom sidebar buttons get there").SemiBold().Large(),
                TextBlock(@"In App.cs we subscribe to App.Sidebar.OnSidebarRebuild_BeforeFooter and add a SidebarButton for every recipe in the catalog when the sidebar is in its default mode. The same callback fires every time the sidebar rebuilds, so the buttons survive workspace switches, theme changes and similar events.

The tracker.Add(...) lines bind the IsSelected state of each button to the current URL hash — that is what makes the active button highlight as the user navigates.").BreakSpaces().Secondary()
            ).P(16)).WS().MB(16);

            const string code = @"App.Sidebar.OnSidebarRebuild_BeforeFooter += (sidebar, mode, tracker) =>
{
    switch (mode)
    {
        case App.Sidebar.Mode.Default:
            foreach (var recipe in RecipeCatalog.All)
            {
                var captured = recipe;
                var btn = new SidebarButton(captured.Id, captured.Icon, captured.Title)
                            .OnClick(() => Router.Navigate(captured.Route));

                tracker.Add(() => btn.IsSelected = window.location.hash.Contains(captured.Route));
                sidebar.AddContent(btn);
            }
            break;
    }
};";

            var snippet = Card(VStack().WS().Children(
                TextBlock("Excerpt from App.cs").SemiBold(),
                TextBlock(code).BreakSpaces().Class("recipe-code")
            ).P(16)).WS();

            _container = HubStack(HubTitle("Sidebar", "#/recipe/sidebar"), DefaultRoutes.Home)
                            .Section(VStack().WS().Children(explanation, snippet).P(16), grow: true);
        }

        public HTMLElement Render() => _container.Render();
    }
}
