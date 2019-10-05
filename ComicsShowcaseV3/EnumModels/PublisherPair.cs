using System.ComponentModel;

namespace ComicsShowcaseV3.EnumModels
{
    public class PublisherPair
    {
        [DisplayName("enumValue")]
        public string EnumValue { get; set; }

        [DisplayName("name")]
        public string Name { get; set; }
    }
}
