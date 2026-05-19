# 09 — Sidebar

How to inject custom buttons into the workspace's default sidebar.

## What it shows

The actual code lives in [`src/App.cs`](../../App.cs) — it's a single subscription to `App.Sidebar.OnSidebarRebuild_BeforeFooter` that runs every time the sidebar is rebuilt. For each recipe in [`Recipes.cs`](../../Recipes.cs) we:

1. Create a `SidebarButton` with an id, icon and label.
2. Wire `OnClick` to `Router.Navigate(...)`.
3. Use `tracker.Add(...)` so the button's `IsSelected` state stays in sync with the URL hash.

The four `OnSidebarRebuild_*` hooks (`BeforeHeader`, `AfterHeader`, `BeforeFooter`, `AfterFooter`) give you four anchor points inside the sidebar. `BeforeFooter` is the right one for "add a navigation item to the existing list".

## When to react to mode

The `mode` argument tells you which sidebar variant is being rebuilt:

| Mode | Sidebar context |
|---|---|
| `Default`         | Standard navigation sidebar. |
| `UserPreferences` | The sidebar of the user-preferences page — covered in [Recipe 10](../10_UserPreferences/). |
| `AdminSettings`*  | The sidebar of the admin section (and its per-category siblings). |

`*` See `App.Sidebar.IsAdminMode(mode)` for the full list.

## See also

- [User Preferences recipe](../10_UserPreferences/) — adds a button to the `UserPreferences` sidebar mode and a custom page to back it.
- [`Mosaik.FrontEnd.LicenseServer/src/LicenseApp.cs`](https://github.com/curiosity-ai/mosaik/blob/master/FrontEnd/Mosaik.FrontEnd.LicenseServer/src/LicenseApp.cs) — production app that uses the same pattern, plus role-gating (`if (CurrentUser.IsAdmin)`).
- [`Mosaik.FrontEnd/src/App.Sidebar.cs`](https://github.com/curiosity-ai/mosaik/blob/master/FrontEnd/Mosaik.FrontEnd/src/App.Sidebar.cs) — the full sidebar implementation if you need to look behind the curtain.
