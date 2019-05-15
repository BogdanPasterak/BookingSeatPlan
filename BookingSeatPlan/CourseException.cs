using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSeatPlan
{
    class CourseException : Exception
    {
        public CourseException (string message)
            : base( GetMessage(message) )
        { }

        // convert number as string to Error message
        private static string GetMessage(string messageNumber)
        {
            int number;
            string message = "Error : ";
            int.TryParse(messageNumber, out number);

            number = (number >= 0 && number <= 999) ? number : 0;
            message += ((number > 10) ? "00" : "0") + number.ToString() + Environment.NewLine;

            switch (number)
            {
                case 1:
                    message += "File incorrect format or\nmissing or dialog cancelled";
                    break;
                case 2:
                    message += "File open error or dialog cancelled";
                    break;
                case 3:
                    message += "File save error";
                    break;
                case 4:
                    message += "Invalid Date";
                    break;
                case 5:
                    message += "Invalid Name of Course";
                    break;
                case 6:
                    message += "Invalid Cost of Course";
                    break;
                default:
                    message += "Unknown error";
                    break;
            }

            return message;
        }
    }
}
