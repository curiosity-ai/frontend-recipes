using System;
using System.Collections.Generic;
using FrontEnd.Recipes.Recipes._01_HelloWorld;
using FrontEnd.Recipes.Recipes._02_SearchArea;
using FrontEnd.Recipes.Recipes._03_Pivot;
using FrontEnd.Recipes.Recipes._04_SegmentedPivot;
using FrontEnd.Recipes.Recipes._05_NodeRenderer;
using FrontEnd.Recipes.Recipes._06_NeighborsGraph;
using FrontEnd.Recipes.Recipes._07_Dashboard;
using FrontEnd.Recipes.Recipes._08_CustomChat;
using FrontEnd.Recipes.Recipes._09_Sidebar;
using FrontEnd.Recipes.Recipes._10_UserPreferences;
using Tesserae;

namespace FrontEnd.Recipes
{
    public sealed class RecipeInfo
    {
        public string                       Id          { get; set; }
        public string                       Title       { get; set; }
        public string                       Summary     { get; set; }
        public string                       Route       { get; set; }
        public UIcons                       Icon        { get; set; }
        public Func<Parameters, IComponent> ViewFactory { get; set; }
    }

    public static class RecipeCatalog
    {
        public static readonly IReadOnlyList<RecipeInfo> All = new[]
        {
            new RecipeInfo
            {
                Id          = "hello-world",
                Title       = "Hello World",
                Summary     = "Components, HubStack, HubTitle and routing — the bare minimum to render a page.",
                Route       = "#/recipe/hello-world",
                Icon        = UIcons.HandWave,
                ViewFactory = state => new HelloWorldView(state)
            },
            new RecipeInfo
            {
                Id          = "search-area",
                Title       = "SearchArea",
                Summary     = "Wire the workspace search box, facet filters and a custom search-result renderer.",
                Route       = "#/recipe/search-area",
                Icon        = UIcons.Search,
                ViewFactory = state => new SearchAreaView(state)
            },
            new RecipeInfo
            {
                Id          = "pivot",
                Title       = "Pivot",
                Summary     = "Tabbed navigation with cached vs. lazy tabs, justified, centered and overflow styles.",
                Route       = "#/recipe/pivot",
                Icon        = UIcons.TableLayout,
                ViewFactory = state => new PivotView(state)
            },
            new RecipeInfo
            {
                Id          = "segmented-pivot",
                Title       = "SegmentedPivot",
                Summary     = "Segmented-control style tabs for compact filter / view toggles.",
                Route       = "#/recipe/segmented-pivot",
                Icon        = UIcons.MenuDots,
                ViewFactory = state => new SegmentedPivotView(state)
            },
            new RecipeInfo
            {
                Id          = "node-renderer",
                Title       = "INodeRenderer",
                Summary     = "Define a custom node schema and render it in cards, previews and full pages.",
                Route       = "#/recipe/node-renderer",
                Icon        = UIcons.Box,
                ViewFactory = state => new NodeRendererView(state)
            },
            new RecipeInfo
            {
                Id          = "neighbors-graph",
                Title       = "Neighbors & Graph",
                Summary     = "Traverse the graph with Mosaik.API.Query and visualize neighbours with GraphExplorerView.",
                Route       = "#/recipe/neighbors-graph",
                Icon        = UIcons.ChartNetwork,
                ViewFactory = state => new NeighborsGraphView(state)
            },
            new RecipeInfo
            {
                Id          = "dashboard",
                Title       = "Dashboards",
                Summary     = "Build a dashboard from cards, Plotly charts and endpoint-backed metrics.",
                Route       = "#/recipe/dashboard",
                Icon        = UIcons.Dashboard,
                ViewFactory = state => new DashboardView(state)
            },
            new RecipeInfo
            {
                Id          = "custom-chat",
                Title       = "Custom Chat",
                Summary     = "Replace the default chat with a custom PostMessage, header, examples and tool rendering.",
                Route       = "#/recipe/custom-chat",
                Icon        = UIcons.ChatbotSpeechBubble,
                ViewFactory = state => new CustomChatRecipeView(state)
            },
            new RecipeInfo
            {
                Id          = "sidebar",
                Title       = "Sidebar",
                Summary     = "Inject custom buttons into the default sidebar and react to the current route.",
                Route       = "#/recipe/sidebar",
                Icon        = UIcons.Menu,
                ViewFactory = state => new SidebarView(state)
            },
            new RecipeInfo
            {
                Id          = "user-preferences",
                Title       = "User Preferences",
                Summary     = "Add a custom page to the user-preferences sidebar mode and persist settings.",
                Route       = "#/recipe/user-preferences",
                Icon        = UIcons.UserGear,
                ViewFactory = state => new UserPreferencesRecipeView(state)
            }
        };
    }
}
