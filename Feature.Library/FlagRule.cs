using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Feature.Library
{
    [DataContract]
    public class FlagRule
    {
        public FlagRule()
        {
            Users = new List<string>();
            Groups = new List<string>();
            Variants = new List<FlagVariant>();
            VariantName = string.Empty;
        }

        /// <summary>
        /// The feature is enabled.  Required
        /// </summary>
        [DataMember(Name="enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Indicates the feature is only on for a certain percentage of regular users  
        /// </summary>
        [DataMember(Name = "percentage")]
        public int Percentage { get; set; }

        /// <summary>
        /// The winning variant name
        /// </summary>
        [DataMember(Name = "variantname")]
        public string VariantName { get; set; }

        /// <summary>
        /// Indicates the feature is enabled for admin users
        /// </summary>
        [DataMember(Name = "admin")]
        public bool Admin { get; set; }

        /// <summary>
        /// Indicates the feature is enabled for internal users
        /// </summary>
        [DataMember(Name = "internal")]
        public bool Internal { get; set; }

        /// <summary>
        /// Indicates the set of users the feature is enabled for
        /// </summary>
        [DataMember(Name = "users")]
        public List<string> Users { get; set; }

        /// <summary>
        /// Indicates the groups the feature is enabled for
        /// </summary>
        [DataMember(Name = "groups")]
        public List<string> Groups { get; set; }

        /// <summary>
        /// The list of multivariant rules for the feature
        /// </summary>
        [DataMember(Name = "variants")]
        public List<FlagVariant> Variants { get; set; }
    }
}
