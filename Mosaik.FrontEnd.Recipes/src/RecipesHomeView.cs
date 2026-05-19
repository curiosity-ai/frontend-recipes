using Mosaik;
using Mosaik.Components;
using Tesserae;
using static H5.Core.dom;
using static Mosaik.UI;
using static Tesserae.UI;

namespace Curiosity.FrontEnd.Recipes
{
    /// <summary>
    /// Landing page wired up via <c>settings.HomeView = state =&gt; new RecipesHomeView(state);</c>.
    /// Shows one card per recipe so a new developer can land on the home view and immediately
    /// pick which sample to explore. This replaces the workspace's default home view.
    /// </summary>
    public sealed class RecipesHomeView : IComponent
    {
        private readonly IComponent _container;

        public RecipesHomeView(Parameters state)
        {
            var intro = Card(VStack().WS().Children(
                TextBlock("Mosaik Front-End Recipes").XLarge().SemiBold(),
                TextBlock("A self-contained tour of the most useful building blocks for a custom Curiosity Workspace UI.").Secondary(),
                TextBlock("Pick a recipe below — each one is a single folder with its own README and the smallest amount of code that demonstrates the feature.")
            ).PB(8)).WS();

            var grid = HStack().WS().Wrap();

            foreach (var recipe in RecipeCatalog.All)
            {
                grid.Add(BuildRecipeCard(recipe));
            }

            _container = HubStack(HubTitle("Recipes", DefaultRoutes.Home), DefaultRoutes.Home)
                            .Section(VStack().WS().Children(intro, grid).P(16), grow: true);
        }

        private static IComponent BuildRecipeCard(RecipeInfo recipe)
        {
            var icon  = Icon(recipe.Icon).XXLarge();
            var title = TextBlock(recipe.Title).SemiBold().Large();
            var body  = TextBlock(recipe.Summary).Secondary().BreakSpaces();

            return Card(VStack().Children(
                       HStack().AlignItemsCenter().Children(icon.PR(8), title),
                       body.PT(8)
                   ).P(8))
                .W(320)
                .M(8)
                .Class("recipe-card")
                .OnClick(() => Router.Navigate(recipe.Route));
        }

        public HTMLElement Render() => _container.Render();
    }
}
