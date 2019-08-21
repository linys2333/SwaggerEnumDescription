using System.ComponentModel.DataAnnotations;

namespace Sample
{
    public enum TestEnum
    {
        [Display(Name = "一")]
        One = 1,

        [Display(Name = "二")]
        Two = 2,

        [Display(Name = "三")]
        Three = 3,

        Four = 4,

        Five = 5
    }
}