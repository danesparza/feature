using System.Collections.Generic;

namespace Feature.Library
{
    public class FlagRule
    {
        public FlagRule()
        {
            Users = new List<string>();
            Groups = new List<string>();
            Variants = new List<FlagVariant>();
        }

        /// <summary>
        /// The feature is enabled.  Required
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Indicates the feature is only on for a certain percentage of regular users  
        /// </summary>
        public int Percentage { get; set; }

        /// <summary>
        /// The winning variant name
        /// </summary>
        public string VariantName { get; set; }

        /// <summary>
        /// Indicates the feature is enabled for admin users
        /// </summary>
        public bool Admin { get; set; }

        /// <summary>
        /// Indicates the feature is enabled for internal users
        /// </summary>
        public bool Internal { get; set; }

        /// <summary>
        /// Indicates the set of users the feature is enabled for
        /// </summary>
        public List<string> Users { get; set; }

        /// <summary>
        /// Indicates the groups the feature is enabled for
        /// </summary>
        public List<string> Groups { get; set; }

        /// <summary>
        /// The list of multivariant rules for the feature
        /// </summary>
        public List<FlagVariant> Variants { get; set; }

    }
}
