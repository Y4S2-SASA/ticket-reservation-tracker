using System.Text;
using TRT.Application.Common.Constants;

namespace TRT.Application.Common.Helpers
{
    public class ReservationNumberGenerator
    {
        public static string GenerateTicketReferenceCode()
        {
            var length = NumberConstant.FIVE;
            var random = new Random();

            const string chars = ApplicationLevelConstant.REFERENCE_NUMBER_HELPER_CHARS;

            var code = new StringBuilder(length);

            for (int index = NumberConstant.ZERO; index < length; index++)
            {
                code.Append(chars[random.Next(chars.Length)]);
            }

            return code.ToString();
        }
    }
}
