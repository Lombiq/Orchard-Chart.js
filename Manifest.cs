using OrchardCore.Modules.Manifest;
using static Lombiq.ChartJs.Constants.FeatureIds;

[assembly: Module(
    Name = "Lombiq Chart.js",
    Author = "Lombiq Technologies",
    Version = "1.0",
    Description = "Module for displaying data using Chart.js.",
    Website = "https://github.com/Lombiq/Orchard-Chart.js"
)]

[assembly: Feature(
    Id = Default,
    Name = "Lombiq Chart.js",
    Category = "Content",
    Description = "Module for displaying data using Chart.js.",
    Dependencies = new[]
    {
        "OrchardCore.Contents",
        "OrchardCore.ResourceManagement",
    }
)]
