using Basket.Basket.Modules;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basket.Data.JsonConverters
{
    public class ShoppingCartConverter : JsonConverter<ShoppingCart>
    {
        public override ShoppingCart? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var jsonDocument = JsonDocument.ParseValue(ref reader);
            var rootElement = jsonDocument.RootElement;

            var id = rootElement.GetProperty("id").GetGuid();
            var userName = rootElement.GetProperty("userName").GetString() ?? string.Empty;
            var itemsElement = rootElement.GetProperty("items");

            var shoppingCart = ShoppingCart.Create(id, userName);

            var items = itemsElement.Deserialize<List<ShoppingCart>>(options);
            if (items != null)
            {
                var itemField = typeof(ShoppingCart).GetField("_items", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                itemField?.SetValue(shoppingCart, items);
            }

            return shoppingCart;
        }

        public override void Write(Utf8JsonWriter writer, ShoppingCart value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("id", value.Id.ToString());
            writer.WriteString("userName", value.UserName);
            writer.WritePropertyName("items");
            JsonSerializer.Serialize(writer, value.Items);

            writer.WriteEndObject();
        }
    }
}
