# 08 — Custom Chat

A chat page of your own, built from the workspace's `ChatView` and a `ChatViewConfiguration`:

- a title, a welcome line and fixed **example prompts**;
- a **custom header** with a topic selector beside the assistant picker;
- a **`PostMessage`** hook that tells the model which topic the user picked, on every turn;
- **per-message commands** (thumbs up / thumbs down) on the assistant's replies.

The same view replaces the product's assistant page when it is set as `settings.AssistantView` in `App.cs`.

## Code shape

```csharp
var configuration = new ChatViewConfiguration
{
    Title          = "Recipe Chat",
    WelcomeTitle   = "Ask about the support cases",
    WelcomePrompts = new[] { "Summarise the most recent support cases for cameras.", ... },
    Category       = "recipes",          // this page's history lists only the chats it started

    CustomHeader    = BuildHeader,
    PostMessage     = PostMessageAsync,
    ReplaceMessage  = ReplaceMessageAsync,
    MessageCommands = BuildMessageCommands,
};

_chatView = new ChatView(configuration, state);
```

## Passing context with a turn

What the model should know about the page — here, the topic — goes in `uiContext`, not in the message. The model reads it; the transcript shows only what the user typed.

```csharp
private Task<UID128> PostMessageAsync(ChatViewConfiguration.PostMessageRequest request)
    => Mosaik.API.ChatAI.PostMessage(
        chatUID:           request.ActiveChat.UID,
        message:           request.Message,
        useTools:          request.ActiveTools,
        useAgents:         request.ActiveAgents,
        useSkills:         request.ActiveSkills,
        attachments:       request.AttachmentUIDs,
        uiContext:         "[UI context]\nThe user scoped this conversation to the topic: " + _topic.Value + ".",
        cancellationToken: request.CancellationToken);
```

`ReplaceMessage` is what **Edit** and **Regenerate** call. Give it the same context (and, if you pin tools, the same tools), or a regenerated answer runs without them.

To answer from an endpoint of your own instead of the workspace's assistant, call it in `PostMessage` with `Mosaik.API.Endpoints.CallAsync<T>(...)` (with a small response class carrying the UID of the message it wrote) and return that UID.

Other hooks on `ChatViewConfiguration` you'll reach for eventually:

| Property | Use when |
|---|---|
| `NewChat`        | You decide which assistant a new chat uses. |
| `ListChats`      | You filter the chat history yourself. |
| `ListToolsAndAgents` | The composer should offer a curated set of tools and agents. |
| `DisableAssistantSelector`, `DisableTools` | The page answers as one assistant with one capability set. Pin them in `NewChat`, `PostMessage` and `ReplaceMessage`. |
| `CustomExamples` | The welcome area needs more than a list of prompts. |

## Key file

- [`CustomChatRecipeView.cs`](./CustomChatRecipeView.cs)

## See also

- [`ChatViewConfiguration.cs`](https://github.com/curiosity-ai/mosaik/blob/master/FrontEnd/Mosaik.FrontEnd/src/Desktop/ChatView/ChatViewConfiguration.cs) — every option and hook.
- [`TechnicalSupport.FrontEnd/src/SupportChat.cs`](https://github.com/curiosity-ai/technical-support/blob/main/custom-front-end/src/SupportChat.cs) — a fuller example.
