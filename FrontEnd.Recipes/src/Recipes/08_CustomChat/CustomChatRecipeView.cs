using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaik;
using Mosaik.Components;
using Mosaik.Schema;
using Tesserae;
using UID;
using static Transpose.Core.dom;
using static Mosaik.UI;
using static Tesserae.UI;

namespace FrontEnd.Recipes.Recipes._08_CustomChat
{
    /// <summary>
    /// A chat of our own, built from the workspace's <c>ChatView</c> with a <c>ChatViewConfiguration</c>:
    ///   - a title, a welcome line and fixed example prompts;
    ///   - a topic picker next to the assistant selector in the header;
    ///   - <c>PostMessage</c>, so every turn tells the model which topic the user picked;
    ///   - per-message commands (thumbs up / down) on the assistant's replies.
    ///
    /// The same class replaces the product's assistant page when it is set as
    /// <c>settings.AssistantView</c> in <c>App.cs</c>; here it is one recipe page.
    /// </summary>
    public sealed class CustomChatRecipeView : IComponent
    {
        private readonly IComponent                 _chatView;
        private readonly SettableObservable<string> _topic = new SettableObservable<string>("Any");

        public CustomChatRecipeView(Parameters state)
        {
            var configuration = new ChatViewConfiguration
            {
                Title                  = "Recipe Chat",
                WelcomeTitle           = "Ask about the support cases",
                ChatBoxPlaceholderText = "Ask a question about a product or a case",
                WelcomePrompts         = new[]
                {
                    "Summarise the most recent support cases for cameras.",
                    "Which laptop models had the most opened cases this week?",
                    "Show me my unresolved tickets sorted by age."
                },

                // Chats this page starts are stamped with the category, and its history lists only those.
                Category = "recipes",

                CustomHeader    = BuildHeader,
                PostMessage     = PostMessageAsync,
                ReplaceMessage  = ReplaceMessageAsync,
                MessageCommands = BuildMessageCommands,
            };

            _chatView = new ChatView(configuration, state);
        }

        // ------------------------------------------------------------------------------
        // Sending a turn
        // ------------------------------------------------------------------------------

        // The topic travels as uiContext: the model reads it, and the transcript shows only what the
        // user typed. To answer from an endpoint of your own instead, call it here with
        // Mosaik.API.Endpoints.CallAsync and return the UID of the message it wrote.
        private Task<UID128> PostMessageAsync(ChatViewConfiguration.PostMessageRequest request)
            => Mosaik.API.ChatAI.PostMessage(
                chatUID:           request.ActiveChat.UID,
                message:           request.Message,
                useTools:          request.ActiveTools,
                useAgents:         request.ActiveAgents,
                useSkills:         request.ActiveSkills,
                attachments:       request.AttachmentUIDs,
                uiContext:         TopicContext(),
                cancellationToken: request.CancellationToken);

        // "Edit" and "Regenerate" go through ReplaceMessage: give it the same context, or a regenerated
        // answer forgets the topic.
        private Task<UID128> ReplaceMessageAsync(ChatViewConfiguration.ReplaceMessageRequest request)
            => Mosaik.API.ChatAI.ReplaceMessage(
                chatUID:           request.ActiveChat.UID,
                messageUID:        request.MessageUID,
                message:           request.Message,
                useTools:          request.ActiveTools,
                useAgents:         request.ActiveAgents,
                useSkills:         request.ActiveSkills,
                uiContext:         TopicContext(),
                cancellationToken: request.CancellationToken);

        private string TopicContext()
            => _topic.Value == "Any" ? null : "[UI context]\nThe user scoped this conversation to the topic: " + _topic.Value + ".";

        // ------------------------------------------------------------------------------
        // Header + per-message commands
        // ------------------------------------------------------------------------------

        private IComponent BuildHeader(SelectAIAssistantTemplateDropdown templateDropdown)
        {
            // Keep the assistant picker and add a topic control beside it.
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

        private static IEnumerable<MessageCommand> BuildMessageCommands(CurrentChat chat, Mosaik.Schema.ChatMessage message)
        {
            if (message.Author == FixedUIDs.AssistantAuthor)
            {
                yield return new MessageCommand(UIcons.ThumbsUp,   "Helpful").OnClick(()     => Toast().Success("Thanks for the feedback."));
                yield return new MessageCommand(UIcons.ThumbsDown, "Not helpful").OnClick(() => Toast().Information("Logged as negative feedback."));
            }
        }

        public HTMLElement Render() => _chatView.Render();
    }
}
