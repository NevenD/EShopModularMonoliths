namespace Catalog.Seed
{
    public static class InitialData
    {
        public static IEnumerable<Product> Products => new List<Product>
            {
                Product.Create(
                    Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    "Apple iPhone 14",
                    ["category1"],
                    "iPhone 14 with A15 Bionic chip, dual 12MP cameras, and 6.1-inch Super Retina XDR display.",
                    "iphone14.jpg",
                    799.00m
                ),

                Product.Create(
                    Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    "Samsung Galaxy S23",
                        ["category1"],
                    "Samsung Galaxy S23 with Snapdragon processor, 50MP main camera, and Dynamic AMOLED 2X display.",
                    "galaxy_s23.jpg",
                    749.00m
                ),

                Product.Create(
                    Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    "Google Pixel 7",
                        ["category2"],
                    "Google Pixel 7 featuring Google's Tensor chip, excellent computational photography, and clean Android experience.",
                    "pixel7.jpg",
                    599.00m
                ),

                Product.Create(
                    Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    "OnePlus 11",
                        ["category2"],
                    "OnePlus 11 with flagship performance, fast charging, and Hasselblad-tuned cameras.",
                    "oneplus11.jpg",
                    699.00m
                ),

                Product.Create(
                    Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    "Xiaomi 13",
                       ["category2"],
                    "Xiaomi 13 offering high-end specs, premium cameras, and AMOLED display at a competitive price.",
                    "xiaomi13.jpg",
                    649.00m
                ),

                Product.Create(
                    Guid.Parse("66666666-6666-6666-6666-666666666666"),
                    "Sony Xperia 5 IV",
                    new List<string> { "Smartphones", "Sony", "Android" },
                    "Sony Xperia 5 IV with compact form factor, pro-grade camera features, and OLED 21:9 display.",
                    "xperia5iv.jpg",
                    899.00m
                ),

                Product.Create(
                    Guid.Parse("77777777-7777-7777-7777-777777777777"),
                    "Motorola Edge 40",
                    new List<string> { "Smartphones", "Motorola", "Android" },
                    "Motorola Edge 40 featuring sleek design, reliable performance, and solid battery life.",
                    "edge40.jpg",
                    499.00m
                )
            };
    }
}
