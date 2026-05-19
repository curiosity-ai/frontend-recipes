using System.Collections.Generic;
using System.Threading.Tasks;
using Curiosity.FrontEnd.Recipes.API;
using Curiosity.FrontEnd.Recipes.Schema;
using Mosaik;
using Mosaik.Components;
using Mosaik.Schema;
using Tesserae;
using static H5.Core.dom;
using static Mosaik.UI;
using static Tesserae.UI;
using UID;

namespace Curiosity.FrontEnd.Recipes.Recipes._08_CustomChat
{
    /// <summary>
    /// A drop-in replacement for the workspace's default chat view. We override:
    ///   - <see cref="CustomChatView.PostMessage"/> so the chat talks to our own backend endpoint;
    ///   - the header, with a custom topic selector next to the assistant-template dropdown;
    ///   - the empty-state examples;
    ///   - per-message commands (thumbs up / down).
    ///
    /// The endpoint call in <see cref="PostMessageAsync"/> currently returns a canned reply via
    /// <see cref="RecipeEndpoints.PostChatMessageAsync"/> — swap it for a real
    /// <c>Mosaik.API.Endpoints.CallAsync&lt;UID128&gt;("your-endpoint", request)</c> once you have a
    /// custom endpoint that drives an LLM.
    /// </summary>
    public sealed class CustomChatRecipeView : IComponent
    {
        private readonly ChatAIView _chatView;
        private readonly SettableObservable<string> _topic = new SettableObservable<string>("Any");

        public CustomChatRecipeView(Parameters state)
        {
            var endpoints = new CustomChatView
            {
                Title       = "Recipe Chat",
                PostMessage = PostMessageAsync
            };

            _chatView = ChatView(endpoints, state)
                          .WithCustomHeader(BuildHeader)
                          .WithCustomExamples(BuildExamples)
                          .WithMessageCommands(BuildMessageCommands);
        }

        // ------------------------------------------------------------------------------
        // Endpoint integration
        // ------------------------------------------------------------------------------

        private async Task<UID128> PostMessageAsync(CustomChatView.PostMessageRequest request)
        {
            // In a real chat you would either call the workspace's default chat endpoint OR your
            // own custom endpoint that drives the LLM and writes both the user message and the
            // assistant reply into the chat. The line below is what that real call looks like:
            //
            //     return await Mosaik.API.Endpoints.CallAsync<UID128>("recipes/chat/post-message",
            //         new RecipeChatRequest { Message = request.Message, Topic = _topic.Value });

            var reply = await RecipeEndpoints.PostChatMessageAsync(new RecipeChatRequest
            {
                Message = request.Message,
                Topic   = _topic.Value
            });

            Toast().Information(reply.Reply);

            // Return UID128.Empty when you are not posting the message to the workspace at all —
            // ChatAIView treats that as "you already handled this".
            return UID128.Empty;
        }

        // ------------------------------------------------------------------------------
        // Header + examples + per-message commands
        // ------------------------------------------------------------------------------

        private IComponent BuildHeader(SelectAIAssistantTemplateDropdown templateDropdown)
        {
            // The default header surfaces the assistant-template picker; we keep it but add a
            // small "topic" control next to it so the user can scope every chat to a topic.
            var topicDropdown = Dropdown().Items(
                ItemFor("Any"),
                ItemFor("Smartphones"),
                ItemFor("Laptops"),
                ItemFor("Cameras"));

            return HStack().AlignItemsCenter().PR(8).Children(
                templateDropdown,
                Label("Topic:").Inline().SetContent(topicDropdown).PL(8));

            Dropdown.Item ItemFor(string topic)
                => DropdownItem(topic).SelectedIf(_topic.Value == topic).OnSelected(_ => _topic.Value = topic);
        }

        private static bool BuildExamples(CurrentChat chat, Stack stack, TextArea area, ChatAISendStopButton sendButton, bool _)
        {
            stack.Add(TextBlock("Try one of these to get started:").Secondary());

            foreach (var example in new[]
                     {
                         "Summarise the most recent support cases for cameras.",
                         "Which laptop models had the most opened cases this week?",
                         "Show me my unresolved tickets sorted by age."
                     })
            {
                var captured = example;
                stack.Add(Button(captured).WS().TextLeft().M(4).OnClick(() =>
                {
                    area.Text = captured;
                    sendButton.TriggerSend(new MouseEvent(null));
                }));
            }

            return true; // we rendered our own examples, don't fall through to the default ones
        }

        private static IEnumerable<MessageCommand> BuildMessageCommands(CurrentChat chat, ChatMessage message)
        {
            if (message.Author == FixedUIDs.AssistantAuthor)
            {
                yield return new MessageCommand(UIcons.ThumbsUp,   "Helpful").OnClick(()    => Toast().Success("Thanks for the feedback."));
                yield return new MessageCommand(UIcons.ThumbsDown, "Not helpful").OnClick(() => Toast().Information("Logged as negative feedback."));
            }
        }

        public HTMLElement Render() => _chatView.Render();
    }
}
