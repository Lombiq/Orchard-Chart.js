# Lombiq Chart.js for Orchard Core

[![Lombiq.ChartJs NuGet](https://img.shields.io/nuget/v/Lombiq.ChartJs?label=Lombiq.ChartJs)](https://www.nuget.org/packages/Lombiq.ChartJs/) [![Lombiq.ChartJs.Samples NuGet](https://img.shields.io/nuget/v/Lombiq.ChartJs.Samples?label=Lombiq.ChartJs.Samples)](https://www.nuget.org/packages/Lombiq.ChartJs.Samples/) [![Lombiq.ChartJs.Tests.UI NuGet](https://img.shields.io/nuget/v/Lombiq.ChartJs.Tests.UI?label=Lombiq.ChartJs.Tests.UI)](https://www.nuget.org/packages/Lombiq.ChartJs.Tests.UI/)

## About

An Orchard Core wrapper around the [Chart.js](https://www.chartjs.org/) library for displaying datasets as various charts.

Do you want to quickly try out this project and see it in action? Check it out in our [Open-Source Orchard Core Extensions](https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions) full Orchard Core solution and also see our other useful Orchard Core-related open-source projects!

## Documentation

Display the "Chart" shape like this:

```html
<chart labels="@viewModel.ChartLabels"
       datasets="@viewModel.ChartDataSets"
       options="@viewModel.ChartOptions"
       datalabels="@dataLabelsConfiguration"></chart>
```

The properties are:

- ChartType ("type"): String indicating the [chart type](https://www.chartjs.org/docs/latest/charts/).
- Labels ("labels"): An array of strings for the series lables.
- DataSets ("datasets"): An array of `ChartJsDataSet` objects, each representing a series.
- Options ("options"): An object that gets serialized (and property names converted to camelCase) to become the `options` property of the Chart.js configuration object.
- BackgroundColor ("background"): A CSS style color string. Default value is "white".
- WithDataLabels ("datalabels"): If not `null`, it sets up the Datalabels plugin and its display behavior.

Labels and DataSets are required, the rest are options..

## Contributing and support

Bug reports, feature requests, comments, questions, code contributions and love letters are warmly welcome. Please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.
