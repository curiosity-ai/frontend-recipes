# 10 — User Preferences

How to add a **custom page under the user-preferences sidebar mode** and persist its settings.

## Two halves

1. **In [`src/App.cs`](../../App.cs)**, we subscribe to `App.Sidebar.OnSidebarRebuild_BeforeFooter` and add a button when `mode == App.Sidebar.Mode.UserPreferences`:

    ```csharp
    case App.Sidebar.Mode.UserPreferences:
    {
        var btn = new SidebarButton("recipe-prefs", UIcons.SlidersHSquare, "Recipe Preferences")
                     .OnClick(() => Router.Navigate(UserPreferencesRecipeView.PreferencesRoute));

        tracker.Add(() => btn.IsSelected = window.location.hash.Contains(UserPreferencesRecipeView.PreferencesRoute));
        sidebar.AddContent(btn);
        break;
    }
    ```

2. **In [`UserPreferencesRecipeView.cs`](./UserPreferencesRecipeView.cs)** we render the page itself — a dropdown for density, a dropdown for default view, and a toggle for onboarding tips. Each control persists immediately to `LocalStorage` on change.

The route `#/recipe/preferences` is registered in `App.cs` next to the rest of the recipe routes.

## Where to put the data

| Storage | Use when | API |
|---|---|---|
| `LocalStorage` (used here) | The setting is per-browser. | `LocalStorage.Set("key", value)` / `LocalStorage.Get("key")`. |
| Workspace user preferences | The setting should follow the user across devices. | `Mosaik.API.User.SettingSaverForUserPreferences()` — see `Mosaik.FrontEnd.Admin/src/Notifications/Settings/...` for usage. |
| A custom backend endpoint   | The setting affects backend behaviour. | `Mosaik.API.Endpoints.CallAsync<T>("recipes/save-preferences", body)` — see [`src/API/Endpoints.cs`](../../API/Endpoints.cs). |

## Why a `Mode` switch matters

The sidebar rebuild callback fires for **every** sidebar variant (default, admin, user preferences, …). Switching on `mode` lets you put the right buttons in the right context — admin-only items in admin mode, user-scoped settings in `UserPreferences`, navigation in `Default`. See [Recipe 09](../09_Sidebar/) for the default-mode story.

## See also

- [Sidebar recipe](../09_Sidebar/)
- [`Mosaik.FrontEnd/src/Settings/Desktop/PreferencesView.cs`](https://github.com/curiosity-ai/mosaik/blob/master/FrontEnd/Mosaik.FrontEnd/src/Settings/Desktop/PreferencesView.cs) — the workspace's own preferences view, much larger but follows exactly this shape.
