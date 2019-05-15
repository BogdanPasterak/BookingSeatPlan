using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookingSeatPlan
{
    class Validator
    {
        internal static bool NameCourse(TextBox txtCourseName)
        {
            int length = txtCourseName.Text.Trim().Length;
            if ( length > 2 && length < 45)
            {
                txtCourseName.Text.Trim();
                return true;
            }
            else
            {
                txtCourseName.Focus();
                return false;
            }
        }

        internal static bool DateCourse(DateTimePicker dtpCourseDate)
        {
            if (dtpCourseDate.Value >= DateTime.Today &&
                dtpCourseDate.Value < DateTime.Today.AddYears(3))
            {
                return true;
            }
            else
            {
                dtpCourseDate.Focus();
                return false;
            }
        }

        internal static bool Date(string date)
        {
            DateTime dt;

            if (DateTime.TryParse(date, out dt) && dt >= DateTime.Today &&
                dt < DateTime.Today.AddYears(3))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static bool CostCourse(TextBox txtCourseCost)
        {
            string costText = txtCourseCost.Text.Trim();
            string valute = "£";
            decimal cost;

            if (costText.StartsWith("£") || costText.StartsWith("$") || costText.StartsWith("€"))
            {
                valute = costText.Substring(0, 1);
                costText = costText.Substring(1).Trim();
            }

            if (decimal.TryParse(costText, out cost) &&
                cost == Math.Round(cost, 2) &&
                cost >= 20 && cost <= 2000)
            {
                txtCourseCost.Text = valute + cost.ToString("F2");
                return true;
            }
            else
            {
                txtCourseCost.Focus();
                return false;
            }
        }

        internal static bool Cost(string costText)
        {
            costText = costText.Trim();
            decimal cost;

            if (costText.StartsWith("£") || costText.StartsWith("$") || costText.StartsWith("€"))
            {
                costText = costText.Substring(1).Trim();
            }

            if (decimal.TryParse(costText, out cost) &&
                cost == Math.Round(cost, 2) &&
                cost >= 20 && cost <= 2000)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        internal static bool Seat(string seat)
        {
            if (seat.Length != 12)
                return false;
            foreach (char c in seat.ToCharArray())
            {
                if (c != 'F' && c != 'B')
                    return false;
            }
            return true;
        }

        internal static bool EndTyping(TextBox txtCourseName)
        {
            if (txtCourseName.Text.Trim().ToLower().Equals("finish"))
            {
                return true;
            }
            return false;
        }
    }
}
