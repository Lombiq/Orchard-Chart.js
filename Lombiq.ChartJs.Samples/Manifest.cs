using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "Lombiq Chart.js - Samples",
    Author = "Lombiq Technologies",
    Website = "https://github.com/Lombiq/Orchard-Chart.js",
    Version = "0.0.1",
    Description = "Samples for Lombiq Chart.js.",
    Category = "Chart.js",
    Dependencies = new[]
    {
        "OrchardCore.Alias",
        "OrchardCore.Autoroute",
        "OrchardCore.ContentFields",
        "OrchardCore.ContentFields.Indexing.SQL",
        "OrchardCore.Taxonomies",
        "OrchardCore.Title",
        Lombiq.ChartJs.Constants.FeatureIds.Area,
    }
)]
