using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRT.Domain.Enums
{
    public enum AvailableDays
    {
        [Description("Monday")]
        Monday = 1,

        [Description("Tuesday")]
        Tuesday = 2,

        [Description("Wednesday")]
        Wednesday = 3,

        [Description("Thursday")]
        Thursday = 4,

        [Description("Friday")]
        Friday = 5,

        [Description("Saturday")]
        Saturday = 6,

        [Description("Sunday")]
        Sunday = 7,

        [Description("Monday to Sunday")]
        MondayToSunday = 8,

        [Description("Weekdays")]
        Weekdays = 9,

        [Description("Weekend")]
        Weekend = 10
    }
}
