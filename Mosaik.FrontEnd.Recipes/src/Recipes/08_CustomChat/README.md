# 08 — Custom Chat

A drop-in replacement for the workspace's default chat experience that:

- Calls a **custom backend endpoint** for `PostMessage`.
- Renders a **custom header** with a topic selector beside the assistant-template dropdown.
- Shows **custom empty-state examples** that pre-fill the input box on click.
- Adds **per-message commands** (thumbs up / thumbs down) for assistant replies.

## Code shape

```csharp
var endpoints = new CustomChatView
{
    Title       = "Recipe Chat",
    PostMessage = PostMessageAsync   // ← the only required override for a basic custom chat
};

_chatView = ChatView(endpoints, state)
              .WithCustomHeader(BuildHeader)
              .WithCustomExamples(BuildExamples)
              .WithMessageCommands(BuildMessageCommands);
```

Other customization points on `CustomChatView` you'll reach for eventually:

| Property | Use when |
|---|---|
| `NewChat`        | You want to create the chat row yourself (e.g. attach pre-set context). |
| `ReplaceMessage` | You support message editing and need to re-trigger your own pipeline. |
| `ListTools`      | The chat should show a curated set of tools instead of every one in the workspace. |
| `ListChats`      | You're filtering the chat list (per-user, per-project, archived view). |

## How the backend call works

```csharp
private async Task<UID128> PostMessageAsync(CustomChatView.PostMessageRequest request)
{
    // In a real chat you'd call:
    //     return await Mosaik.API.Endpoints.CallAsync<UID128>(
    //         "recipes/chat/post-message",
    //         new RecipeChatRequest { Message = request.Message, Topic = _topic.Value });

    var reply = await RecipeEndpoints.PostChatMessageAsync(new RecipeChatRequest { ... });

    Toast().Information(reply.Reply);
    return UID128.Empty;   // "I handled it myself"
}
```

The recipe's `RecipeEndpoints.PostChatMessageAsync` lives in `src/API/Endpoints.cs` and currently returns a hard-coded reply. Once you have a real workspace endpoint, uncomment the `Mosaik.API.Endpoints.CallAsync<T>` line in there and delete the canned fallback.

## Key file

- [`CustomChatRecipeView.cs`](./CustomChatRecipeView.cs)

## See also

- [`TechnicalSupport.FrontEnd/src/SupportChat.cs`](https://github.com/curiosity-ai/technical-support/blob/main/custom-front-end/src/SupportChat.cs) — fuller example with chat-scoped context state attached to `ChatMetadata`.
- The chat reference in `INSTRUCTIONS.md`: [`custom-front-end/INSTRUCTIONS.md#custom-chat-interface`](https://github.com/curiosity-ai/technical-support/blob/main/custom-front-end/INSTRUCTIONS.md#custom-chat-interface).
- `Mosaik.FrontEnd/src/Desktop/ChatAIView/CustomChatView.cs` — the full surface of `CustomChatView` and its request DTOs.
