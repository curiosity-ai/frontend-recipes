using System;
using System.Threading.Tasks;
using H5.Core;
using Mosaik;
using Mosaik.Schema;

namespace Curiosity.FrontEnd.Recipes.Recipes._05_NodeRenderer
{
    /// <summary>
    /// Registers the RecipeBook schema with the workspace on startup so the INodeRenderer recipe
    /// has something concrete to render. Only admins may write schemas, so the call short-circuits
    /// for any other user.
    ///
    /// This wires into <c>App.cs</c> via:
    ///   <c>NodeRendererSchema.EnsureSchemaAsync().FireAndForget();</c>
    ///
    /// The actual HTTP call lives in <c>Mosaik.API.Schemas.Create(...)</c>, which was moved out of
    /// <c>SchemaEditor.cs</c> in the Mosaik repo (see the recipe README for the rationale).
    /// </summary>
    public static class NodeRendererSchema
    {
        private static bool _registered;

        public static async Task EnsureSchemaAsync()
        {
            if (_registered) return;
            _registered = true;

            if (!CurrentUser.IsAdmin)
            {
                // Schema creation is admin-only. Non-admins can still render existing RecipeBook
                // nodes — we just don't auto-provision the schema for them.
                return;
            }

            try
            {
                var schema = new SchemaDefinition(
                    name:   RecipeN.RecipeBook.Type,
                    type:   "Node",
                    key:    RecipeN.RecipeBook.ISBN,
                    fields: new[]
                    {
                        new FieldDefinition(FieldSchemaType.Field.ToString(), RecipeN.RecipeBook.Title,  "String"),
                        new FieldDefinition(FieldSchemaType.Field.ToString(), RecipeN.RecipeBook.Author, "String"),
                        new FieldDefinition(FieldSchemaType.Field.ToString(), RecipeN.RecipeBook.Year,   "Int"),
                        new FieldDefinition(FieldSchemaType.Field.ToString(), RecipeN.RecipeBook.Genre,  "String")
                    });

                await Mosaik.API.Schemas.Create(schema);
            }
            catch (Exception e)
            {
                // The workspace returns an error when the schema already exists — that's expected
                // on every run after the first, so just log instead of bothering the user.
                dom.console.warn("RecipeBook schema not created (already exists?)", e);
            }
        }
    }
}
