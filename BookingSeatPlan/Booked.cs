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

        public override bool Equals(object obj)
        {
            var booked = obj as Booked;
            return booked != null &&
                   IndexCourse == booked.IndexCourse &&
                   SeatNumber == booked.SeatNumber &&
                   Occupied == booked.Occupied;
        }

        public override string ToString()
        {
            return "Booked:[index:" + IndexCourse.ToString() +
                ", seat:" + SeatNumber.ToString() + ", Occuped:" + Occupied.ToString() + "]";
        }

    }
}
