using System;
using FrontEnd.Recipes.Recipes._05_NodeRenderer;
using FrontEnd.Recipes.Recipes._10_UserPreferences;
using Mosaik;
using Mosaik.Components;
using Tesserae;
using static H5.Core.dom;
using static Tesserae.UI;

namespace FrontEnd.Recipes
{
    internal static class RecipesApp
    {
        private static void Main()
        {
            // Lazy-load the admin assembly so admin-only screens (and INodeRenderer auto-discovery)
            // still work when running this front-end against a workspace.
            Mosaik.Admin.LazyLoad();

            App.Name = "Mosaik Front-End Recipes";

            // Routes must be registered BEFORE App.Initialize is called, otherwise the router will
            // already be running and won't recognise our paths on the first hash change.
            foreach (var recipe in RecipeCatalog.All)
            {
                var captured = recipe;
                Router.Register(captured.Route, state => App.ShowDefault(captured.ViewFactory(state)));
            }

            // Recipe 10 contributes its own preferences page on the user-preferences route.
            Router.Register(UserPreferencesRecipeView.PreferencesRoute, state => App.ShowDefault(new UserPreferencesRecipeView(state)));

            App.Initialize(Configure, OnLoad).FireAndForget();
        }

        private static void Configure(App.DefaultSettings settings)
        {
            // Recipe — replace the default home view with the recipe catalog landing page.
            settings.HomeView = state => new RecipesHomeView(state);

            // Recipes 09 and 10 — wire sidebar customizations for the default mode AND for the
            // user-preferences mode. We do everything in this single subscription so the order in
            // which recipes are added stays explicit and easy to read.
            App.Sidebar.OnSidebarRebuild_BeforeFooter += (sidebar, mode, tracker) =>
            {
                switch (mode)
                {
                    case App.Sidebar.Mode.Default:
                        {
                            // Pin a button per recipe under the standard sidebar — see Recipes/09_Sidebar.
                            foreach (var recipe in RecipeCatalog.All)
                            {
                                var captured = recipe;
                                var btn      = new SidebarButton(captured.Id, captured.Icon, captured.Title)
                                                  .OnClick(() => Router.Navigate(captured.Route));

                                tracker.Add(() => btn.IsSelected = window.location.hash.Contains(captured.Route));
                                sidebar.AddContent(btn);
                            }

                            break;
                        }

                    case App.Sidebar.Mode.UserPreferences:
                        {
                            // Recipe 10 — surface a custom preferences page under the user-preferences sidebar mode.
                            var btn = new SidebarButton("recipe-prefs", UIcons.SlidersHSquare, "Recipe Preferences")
                                         .OnClick(() => Router.Navigate(UserPreferencesRecipeView.PreferencesRoute));

                            tracker.Add(() => btn.IsSelected = window.location.hash.Contains(UserPreferencesRecipeView.PreferencesRoute));
                            sidebar.AddContent(btn);
                            break;
                        }
                }
            };
        }

        private static void OnLoad()
        {
            // Recipe 05 — register the demo node schema so INodeRenderer auto-discovery has something
            // to render against. Only admins can write the schema, so the call short-circuits otherwise.
            NodeRendererSchema.EnsureSchemaAsync().FireAndForget();
        }
    }
}
