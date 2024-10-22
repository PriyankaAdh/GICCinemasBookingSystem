using System;

namespace GICCinemasBookingSystem.Tests
{
    public class CinemaTests
    {
        [Fact]
        public void BookSeats_ShouldBookSeats_WhenSeatsAreAvailable()
        {
            // Arrange
            var cinema = new Cinema("Test Cinema", 5, 10);
            int numberOfSeats = 3;

            // Act
            var result = cinema.BookSeats(numberOfSeats);

            // Assert
            Assert.NotNull(result.BookingId);
            Assert.Equal(numberOfSeats, result.BookedSeats.Count);
            foreach (var seat in result.BookedSeats)
            {
                Assert.True(cinema.Seats[seat.Row - 1, seat.Seat - 1]);
            }
        }

        [Fact]
        public void BookSeats_ShouldThrowException_WhenNotEnoughSeatsAvailable()
        {
            // Arrange
            var cinema = new Cinema("Test Cinema", 1, 2);
            int numberOfSeats = 3;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => cinema.BookSeats(numberOfSeats));
        }

        [Fact]
        public void GetBookingById_ShouldReturnBooking_WhenBookingExists()
        {
            // Arrange
            var cinema = new Cinema("Test Cinema", 5, 10);
            var booking = cinema.BookSeats(2);

            // Act
            var result = cinema.GetBookingById(booking.BookingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(booking.BookingId, result.BookingId);
        }

        [Fact]
        public void GetBookingById_ShouldThrowException_WhenBookingDoesNotExist()
        {
            // Arrange
            var cinema = new Cinema("Test Cinema", 5, 10);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => cinema.GetBookingById("NonExistentId"));
        }
       
    }
}