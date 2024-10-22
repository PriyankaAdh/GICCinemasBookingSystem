using GICCinemasBookingSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GICCinemasBookingSystem
{
    public class Cinema : ICinema
    {
        private int nextBookingId = 1;
        public string Title { get; set; }
        public bool[,] Seats { get; set; }
        public int Rows { get; set; }
        public int SeatsPerRow { get; set; }
        public int AvailableSeats { get; set; }        
        public List<Booking> Bookings { get; set; }

        public Cinema(string title, int rows, int seatsPerRow)
        {
            Title = title;
            Rows = rows;
            SeatsPerRow = seatsPerRow;
            Seats = new bool[rows, seatsPerRow];
            Bookings = new List<Booking>();
            AvailableSeats = rows * seatsPerRow;
        }

        public (string BookingId, List<(int Row, int Seat)> BookedSeats) BookSeats(int numberOfSeats)
        {
            List<(int Row, int Seat)> bookedSeats = new List<(int Row, int Seat)>();

            if (numberOfSeats > AvailableSeats)
            {
                throw new InvalidOperationException("Not enough seats available.");
            }

            int startCol = CalculateStartColumn(); // Calculate the starting position for seat allocation

            for (int i = 0; i < Rows && numberOfSeats > 0; i++)
            {
                List<(int Row, int Seat)> currentRowSeats = FindAvailableSeatsInRow(i, startCol, numberOfSeats);

                // Book the found seats
                BookSeatsInCurrentRow(currentRowSeats, bookedSeats);

                // Update the remaining number of seats to book
                numberOfSeats -= currentRowSeats.Count;

                // If we have booked enough seats, create the booking
                if (numberOfSeats <= 0)
                {
                    return CreateBooking(bookedSeats);
                }
            }

            // If we didn't find enough seats in the initial rows, try to book in the next available row
            if (numberOfSeats > 0 && Rows > 0)
            {
                int nextRowIndex = 1; // Start from the next row after the first available row
                List<(int Row, int Seat)> remainingSeats = new List<(int Row, int Seat)>();

                while (nextRowIndex < Rows && numberOfSeats > 0)
                {
                    // Attempt to book additional seats in the next available row
                    List<(int Row, int Seat)> nextRowSeats = FindAvailableSeatsInRow(nextRowIndex, startCol, numberOfSeats);
                    remainingSeats.AddRange(nextRowSeats);
                    BookSeatsInCurrentRow(nextRowSeats, bookedSeats);

                    numberOfSeats -= nextRowSeats.Count;
                    nextRowIndex++;
                }
            }

            // If we reach here, we didn't find enough seats
            throw new InvalidOperationException("Not enough seats available in the requested row or adjacent rows.");
        }

        private int CalculateStartColumn()
        {
            return SeatsPerRow % 2 == 0 ? (SeatsPerRow / 2) - 1 : SeatsPerRow / 2; // Middle column
        }

        private List<(int Row, int Seat)> FindAvailableSeatsInRow(int rowIndex, int startCol, int numberOfSeats)
        {
            List<(int Row, int Seat)> currentRowSeats = new List<(int Row, int Seat)>();

            for (int j = 0; j < SeatsPerRow; j++)
            {
                // Check left and right from the starting column
                int leftIndex = startCol - j;
                int rightIndex = startCol + j;

                // Check left side
                if (leftIndex >= 0 && !Seats[rowIndex, leftIndex])
                {
                    currentRowSeats.Add((Row: rowIndex + 1, Seat: leftIndex + 1)); // Store 1-based seat info
                }

                // Check right side
                if (rightIndex < SeatsPerRow && !Seats[rowIndex, rightIndex])
                {
                    currentRowSeats.Add((Row: rowIndex + 1, Seat: rightIndex + 1)); // Store 1-based seat info
                }

                // If we have enough seats in this row, break
                if (currentRowSeats.Count >= numberOfSeats)
                {
                    break;
                }
            }

            return currentRowSeats;
        }

        private void BookSeatsInCurrentRow(List<(int Row, int Seat)> currentRowSeats, List<(int Row, int Seat)> bookedSeats)
        {
            foreach (var seat in currentRowSeats)
            {
                if (!Seats[seat.Row - 1, seat.Seat - 1]) // Check if the seat is still available
                {
                    Seats[seat.Row - 1, seat.Seat - 1] = true; // Mark seat as booked
                    bookedSeats.Add(seat); // Add seat to booked seats list
                }
            }
        }

        private (string BookingId, List<(int Row, int Seat)> BookedSeats) CreateBooking(List<(int Row, int Seat)> bookedSeats)
        {
            var booking = new Booking($"GIC000{nextBookingId++}", bookedSeats); // Create booking
            Bookings.Add(booking); // Add booking to the list
            return (booking.BookingId, bookedSeats); // Return booking ID and booked seats
        }
        public void DisplaySeatGraph(List<(int Row, int Seat)> bookedSeats = null)
        {
            // Display the screen header
            Console.WriteLine(ScreenHeader(Rows)); // Print with padding
            Console.WriteLine(new string('-', SeatsPerRow * 2 + 3)); // Separator line

            for (int i = Rows - 1; i >= 0; i--) // Start from the last row (Row A at the top)
            {
                Console.Write($"{(char)('A' + i)} | "); // Print row letters

                for (int j = 0; j < SeatsPerRow; j++)
                {
                    Console.Write(Seats[i, j] ? "X " : "O "); // Mark booked seats with 'X' and available seats with 'O'
                }
                Console.WriteLine();
            }

            // Print column numbers at the end
            Console.Write("   "); // Space for row header
            for (int j = 1; j <= SeatsPerRow; j++)
            {
                Console.Write($"{j} "); // Print column numbers
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', SeatsPerRow * 2 + 2)); // Final separator line
        }
        public string ScreenHeader(int Rows)
        {
            // Center align the "**** SCREEN ****" header
            string screenHeader = "S C R E E N ";          
            int consoleWidth = SeatsPerRow * 2 + 3; 
            int headerLength = screenHeader.Length;            
           var padding = (consoleWidth - headerLength) / 2; // Calculate left padding
            return $"{new string(' ', padding)}{screenHeader}";
        }       
        public Booking GetBookingById(string bookingId)
        {
            var booking = Bookings.Find((b => string.Equals(b.BookingId, bookingId)));
            if (booking == null)
            {
                throw new InvalidOperationException("Booking ID does not exist.");
            }
            return booking;
        }
    }  

}
