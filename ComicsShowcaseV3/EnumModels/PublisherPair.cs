using System.ComponentModel;
using ComicsShowcaseV3.Models;

namespace ComicsShowcaseV3.EnumModels
{
    public class PublisherPair
    {
        [DisplayName("enumValue")]
        public Publisher EnumValue { get; set; }

        [DisplayName("name")]
        public string Name { get; set; }
    }
}
