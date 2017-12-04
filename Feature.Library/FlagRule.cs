using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Feature.Library
{
    /// <summary>
    /// Feature Flag rule definition
    /// </summary>
    [DataContract]
    public class FlagRule
    {
        public FlagRule()
        {
            Users = new List<string>();
            Groups = new List<string>();
            Variants = new List<FlagVariant>();
            VariantName = string.Empty;
            Url = string.Empty;
        }

        /// <summary>
        /// Indicates the feature is completely enabled or disabled for everyone.
        /// </summary>
        [DataMember(Name="enabled")]
        public bool? Enabled { get; set; }

        /// <summary>
        /// Indicates the feature is only on for a certain percentage of logged in users
        /// </summary>
        [DataMember(Name = "percent_loggedin")]
        public int PercentLoggedIn { get; set; }

        /// <summary>
        /// The winning variant name
        /// </summary>
        [DataMember(Name = "variant_name")]
        public string VariantName { get; set; }

        /// <summary>
        /// Indicates the feature is enabled for admin users
        /// </summary>
        [DataMember(Name = "admin")]
        public bool Admin { get; set; }

        /// <summary>
        /// Indicates the feature is for internal users
        /// </summary>
        [DataMember(Name = "internal")]
        public bool Internal { get; set; }

        /// <summary>
        /// Indicates the feature should be on by url
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }

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
