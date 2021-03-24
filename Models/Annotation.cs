namespace Lombiq.ChartJs.Models
{
    // There are line and box annotations but we have no use for the latter right now. Also additional properties like
    // dashed borders or line labels. Those will be implemented as demand arises.
    public abstract class Annotation
    {
        public string Id { get; set; }
        public abstract string Type { get; }

        public string BorderColor { get; set; }
        public double BorderWidth { get; set; }
    }
}
