using Mosaik;
using Mosaik.Components;
using Tesserae;
using static H5.Core.dom;
using static Mosaik.UI;
using static Tesserae.UI;

namespace FrontEnd.Recipes.Recipes._01_HelloWorld
{
    /// <summary>
    /// Smallest meaningful Curiosity page — a <c>HubStack</c> with a title and a section of content.
    /// Compare to <c>App.ShowDefault(TextBlock("Hello World!"))</c>, which works but skips the
    /// title bar and the standard back/forward navigation a real workspace page should have.
    /// </summary>
    public sealed class HelloWorldView : IComponent
    {
        private readonly IComponent _container;

        public HelloWorldView(Parameters state)
        {
            var body = VStack().WS().Children(
                TextBlock("Welcome to the recipes").Large().SemiBold(),
                TextBlock("Each page in this front-end is a single component returned from a route. " +
                          "This one wraps everything in HubStack + HubTitle so it inherits the standard " +
                          "title bar and back-navigation behaviour.").BreakSpaces().Secondary(),
                HStack().Children(
                    Button("Open SearchArea recipe").Primary().SetIcon(UIcons.Search).OnClick(() => Router.Navigate("#/recipe/search-area")),
                    Button("Open Pivot recipe").SetIcon(UIcons.TableLayout).OnClick(() => Router.Navigate("#/recipe/pivot")),
                    Button("Back to recipe catalog").SetIcon(UIcons.Home).OnClick(() => Router.Navigate(DefaultRoutes.Home))
                ).PT(16)
            ).P(16);

            _container = HubStack(HubTitle("Hello World", "#/recipe/hello-world"), DefaultRoutes.Home)
                            .Section(body, grow: true);
        }

        public HTMLElement Render() => _container.Render();
    }
}
