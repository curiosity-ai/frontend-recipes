using H5;

namespace FrontEnd.Recipes.Schema
{
    // DTOs that mirror the JSON shape the (mock) workspace endpoints in Endpoints.cs return.
    // They use [ObjectLiteral] so H5 can serialize / deserialize them as plain JS objects when
    // they cross the network boundary.

    [ObjectLiteral]
    public class DashboardSeriesResponse
    {
        public string[] Days        { get; set; }
        public double[] Opened      { get; set; }
        public double[] Resolved    { get; set; }
        public double[] MedianHours { get; set; }
    }

    [ObjectLiteral]
    public class TopCategoriesResponse
    {
        public TopCategoryEntry[] Categories { get; set; }
    }

    [ObjectLiteral]
    public class TopCategoryEntry
    {
        public string Label { get; set; }
        public int    Count { get; set; }
    }

    [ObjectLiteral]
    public class RecipeChatRequest
    {
        public string Message { get; set; }
        public string Topic   { get; set; }
    }

    [ObjectLiteral]
    public class RecipeChatReply
    {
        public string Reply { get; set; }
    }
}
