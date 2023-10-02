using System.ComponentModel;

namespace TRT.Domain.Enums
{
    public enum PassengerClass
    {
        [Description("First Class")]
        FirstClass = 1,

        [Description("Second Class")]
        SecondClass = 2,

        [Description("Third Class")]
        ThirdClass = 3
    }
}
