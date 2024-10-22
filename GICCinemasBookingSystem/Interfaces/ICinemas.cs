namespace GICCinemasBookingSystem.Interfaces
{
    public interface ICinema
    {
        string Title { get; set; }
        bool[,] Seats { get; set; }
        int Rows { get; set; }
        int SeatsPerRow { get; set; }
        List<Booking> Bookings { get; set; }
        (string BookingId, List<(int Row, int Seat)> BookedSeats) BookSeats(int numberOfSeats);
        void DisplaySeatGraph(List<(int Row, int Seat)> bookedSeats);
        Booking GetBookingById(string bookingId);
        int AvailableSeats { get; set; }
    }
}
