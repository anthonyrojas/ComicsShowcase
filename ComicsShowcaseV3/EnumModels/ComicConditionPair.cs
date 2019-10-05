using System.ComponentModel;
using ComicsShowcase.Models;

namespace ComicsShowcaseV3.EnumModels
{
    public class ComicConditionPair
    {
        [DisplayName("enumValue")]
        public ComicCondition EnumValue { get; set; }

        [DisplayName("name")]
        public string Name { get; set; }
    }
}
