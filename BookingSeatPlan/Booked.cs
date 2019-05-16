using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSeatPlan
{
    public class Booked
    {
        public int IndexCourse { get; set; }
        public int SeatNumber { get; set; }
        public bool Occupied { get; set; }

        public override string ToString()
        {
            return "Booked:[index:" + IndexCourse.ToString() +
                ", seat:" + SeatNumber.ToString() + ", Occuped:" + Occupied.ToString() + "]";
        }
    }
}
