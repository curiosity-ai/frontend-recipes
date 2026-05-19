using Mosaik;
using Mosaik.Components;
using Mosaik.Helpers;
using Tesserae;
using static H5.Core.dom;
using static Mosaik.UI;
using static Tesserae.UI;

namespace FrontEnd.Recipes.Recipes._10_UserPreferences
{
    /// <summary>
    /// Custom preferences page surfaced under the user-preferences sidebar mode. Combine this with
    /// the <c>App.Sidebar.OnSidebarRebuild_BeforeFooter</c> hook in <c>App.cs</c> (which adds the
    /// matching SidebarButton when the sidebar is in <c>Mode.UserPreferences</c>).
    ///
    /// The recipe persists settings to <c>LocalStorage</c> so they survive reloads. For settings
    /// that need to follow the user across devices, use
    /// <c>Mosaik.API.User.SettingSaverForUserPreferences()</c> instead — see the README.
    /// </summary>
    public sealed class UserPreferencesRecipeView : IComponent
    {
        public const string PreferencesRoute = "#/recipe/preferences";

        private const string DensityKey      = "recipes.preferences.density";
        private const string ShowTipsKey     = "recipes.preferences.showTips";
        private const string DefaultViewKey  = "recipes.preferences.defaultView";

        private readonly IComponent _container;

        public UserPreferencesRecipeView(Parameters state)
        {
            // Read existing values (or defaults if nothing's stored yet).
            var density     = LocalStorage.Get(DensityKey)     ?? "Comfortable";
            var defaultView = LocalStorage.Get(DefaultViewKey) ?? "Cards";
            var showTips    = LocalStorage.GetBool(ShowTipsKey);

            var densityDropdown = Dropdown().Items(
                ItemFor("Compact",     density),
                ItemFor("Comfortable", density),
                ItemFor("Spacious",    density));

            densityDropdown.OnChange((d, _) =>
            {
                LocalStorage.Set(DensityKey, d.SelectedText);
                Toast().Success("Saved");
            });

            var defaultViewDropdown = Dropdown().Items(
                ItemFor("Cards", defaultView),
                ItemFor("List",  defaultView),
                ItemFor("Table", defaultView));

            defaultViewDropdown.OnChange((d, _) =>
            {
                LocalStorage.Set(DefaultViewKey, d.SelectedText);
                Toast().Success("Saved");
            });

            var showTipsToggle = Toggle(offText: "Tips hidden", onText: "Tips visible").Checked(showTips);

            showTipsToggle.OnChange((t, _) =>
            {
                LocalStorage.Set(ShowTipsKey, t.IsChecked.ToString());
                Toast().Success("Saved");
            });

            var body = VStack().WS().Children(
                Card(VStack().WS().Children(
                    TextBlock("Recipe Preferences").SemiBold().Large(),
                    TextBlock("Settings stored locally per browser. The matching sidebar entry is wired up in App.cs under the UserPreferences mode.").Secondary().BreakSpaces()
                ).P(16)).WS().MB(16),

                Card(VStack().WS().Children(
                    Label("Display density").Inline().AutoWidth().SetContent(densityDropdown),
                    Label("Default view").Inline().AutoWidth().SetContent(defaultViewDropdown),
                    Label("Onboarding tips").Inline().AutoWidth().SetContent(showTipsToggle)
                ).P(16)).WS()
            ).P(16);

            _container = HubStack(HubTitle("Recipe Preferences", PreferencesRoute), DefaultRoutes.Home)
                            .Section(body, grow: true);
        }

        private static Dropdown.Item ItemFor(string label, string current) =>
            DropdownItem(label).SelectedIf(current == label);

        public HTMLElement Render() => _container.Render();
    }
}
