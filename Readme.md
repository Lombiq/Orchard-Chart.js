# Lombiq Chart.js for Orchard Core



## About

An Orchard Core wrapper around the [Chart.js](https://www.chartjs.org/) library for displaying datasets as various charts. 


## Documentation

Display the "Chart" shape like this:

```html
<chart labels="@viewModel.ChartLabels"
       datasets="@viewModel.ChartDataSets"
       options="@viewModel.ChartOptions"
       with-data-labels="@true"></chart>
```

The properties are:
- ChartType ("type"): String indicating the [chart type](https://www.chartjs.org/docs/latest/charts/).
- Labels ("labels"): An array of strings for the series lables.
- DataSets ("datasets"): An array of `ChartJsDataSet` objects, each representing a series.
- Options ("options"): An object that gets serialized (and property names converted to camelCase) to become the `options` property of the Chart.js configuration object.
- BackgroundColor ("background"): A CSS style color string. Default value is "white".
- WithDataLabels ("with-data-labels"): Default is false, if set to true the [data labels plugin](https://chartjs-plugin-datalabels.netlify.app/) is loaded for the page.

Labels and DataSets are required, the rest are options..


## Contributing and support

Bug reports, feature requests, comments, questions, code contributions, and love letters are warmly welcome, please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.
