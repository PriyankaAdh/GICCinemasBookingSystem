using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GICCinemasBookingSystem.Interfaces
{
    public interface IBooking
    {
        string BookingId { get; }
        List<(int row, int seat)> Seats { get; }
        int Seat { get; }
    }
}
