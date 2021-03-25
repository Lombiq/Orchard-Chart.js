namespace Lombiq.ChartJs.Models
{
    /// <summary>
    /// Identifies where to put the data label in relation to the chart data visualizer (e.g. point or bar). The default
    /// value in the library is <see cref="Center"/> <see
    /// href="https://chartjs-plugin-datalabels.netlify.app/guide/positioning.html#alignment-and-offset)">(see docs
    /// here)</see> which is why it's the first member.
    /// </summary>
    public enum DataLabelAlignment
    {
        Center,
        Start,
        End,
    }
}
