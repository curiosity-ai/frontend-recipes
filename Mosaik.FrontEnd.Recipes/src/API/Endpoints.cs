using System.Threading.Tasks;
using Curiosity.FrontEnd.Recipes.Schema;
using Mosaik;

namespace Curiosity.FrontEnd.Recipes.API
{
    /// <summary>
    /// All HTTP calls the recipes make to a (real or hypothetical) workspace endpoint live here.
    /// Every method has the production-style call commented out and returns hard-coded data
    /// instead, so this repo can be cloned and explored without any backend running.
    ///
    /// To make these calls hit a real workspace:
    ///   1. Implement matching custom endpoints in your workspace (Manage → Endpoints).
    ///   2. Uncomment the <c>Mosaik.API.Endpoints.CallAsync&lt;T&gt;(...)</c> line.
    ///   3. Delete the hard-coded <c>return</c> below it.
    /// </summary>
    public static class RecipeEndpoints
    {
        /// <summary>
        /// Returns three time-series for a dashboard widget: tickets opened, tickets resolved, and
        /// median resolution time over the last 14 days.
        /// </summary>
        public static async Task<DashboardSeriesResponse> GetDashboardSeriesAsync()
        {
            // return await Mosaik.API.Endpoints.CallAsync<DashboardSeriesResponse>("recipes/dashboard-series");

            await Task.Delay(150); // simulate network latency

            return new DashboardSeriesResponse
            {
                Days        = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
                Opened      = new[] { 18.0, 24, 22, 31, 29, 12, 9,  21, 27, 24, 33, 30, 14, 11 },
                Resolved    = new[] { 14.0, 19, 25, 28, 32, 10, 8,  18, 23, 22, 30, 31, 12, 10 },
                MedianHours = new[] { 4.2,  3.9, 4.6, 5.1, 4.8, 4.3, 4.7, 4.1, 3.8, 4.2, 4.9, 4.6, 4.4, 4.0 }
            };
        }

        /// <summary>
        /// Returns the top facets used to populate the dashboard's "By category" widget.
        /// </summary>
        public static async Task<TopCategoriesResponse> GetTopCategoriesAsync()
        {
            // return await Mosaik.API.Endpoints.CallAsync<TopCategoriesResponse>("recipes/top-categories");

            await Task.Delay(100);

            return new TopCategoriesResponse
            {
                Categories = new[]
                {
                    new TopCategoryEntry { Label = "Billing",     Count = 142 },
                    new TopCategoryEntry { Label = "Account",     Count = 97  },
                    new TopCategoryEntry { Label = "Performance", Count = 64  },
                    new TopCategoryEntry { Label = "Integration", Count = 51  },
                    new TopCategoryEntry { Label = "Onboarding",  Count = 33  }
                }
            };
        }

        /// <summary>
        /// Returns a deterministic, pre-baked LLM-style answer for the custom chat recipe so it can
        /// run without an actual model. The real endpoint would post a message and return the UID
        /// of the new assistant message.
        /// </summary>
        public static async Task<RecipeChatReply> PostChatMessageAsync(RecipeChatRequest request)
        {
            // return await Mosaik.API.Endpoints.CallAsync<RecipeChatReply>("recipes/chat/post-message", request);

            await Task.Delay(400);

            var topic = string.IsNullOrEmpty(request.Topic) ? "any topic" : request.Topic;

            return new RecipeChatReply
            {
                Reply = "This is a canned reply from the recipes endpoint. You asked about '" +
                        request.Message + "' on topic '" + topic + "'. Wire this method to a real " +
                        "Curiosity custom endpoint to get a live model response."
            };
        }
    }
}
