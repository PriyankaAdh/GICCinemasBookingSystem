using GICCinemasBookingSystem.Interfaces;

namespace GICCinemasBookingSystem
{
    public class Booking: IBooking
    {
        public string BookingId { get; }
        public List<(int row, int seat)> Seats { get; }
        public int Seat { get; }

        public Booking(string bookingId, List<(int row, int seat)> seats)
        {
            BookingId = bookingId;
            Seats = seats;          
        }
    }
}
