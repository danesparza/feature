namespace FeatureFlags.Library
{
    /// <summary>
    /// Describes a flag variant
    /// </summary>
    public class FlagVariant
    {
        /// <summary>
        /// The name of the flag variant
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The percentage en
        /// </summary>
        public double Percentage { get; set; }
    }
}
