using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GICCinemasBookingSystem
{
    public class CinemaManager
    {     
            private const int MaxRows = 26; // Maximum rows
            private const int MaxSeatsPerRow = 50; // Maximum seats per row

            public string DisplayMainMenu()
            {
                // Display a menu of choices
                Console.Write("Please define movie title and seating map in [Title] [Row] [SeatPerRow] format:");
                return Console.ReadLine() ?? string.Empty;
            }

            public void ProcessUserInput(string userInput)
            {
                try
                {
                    if (ValidateUserInput(userInput, out string title, out int rows, out int seatsPerRow))
                    {
                        // create cinema object
                        Cinema cinema = new Cinema(title, rows, seatsPerRow);
                        ManageBookings(cinema);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input format or Seat choices. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            public bool ValidateUserInput(string input, out string title, out int rows, out int seatsPerRow)
            {
                title = string.Empty;
                rows = 0;
                seatsPerRow = 0;

                // RegEx to validate the expected format
                string pattern = @"^\[(.+?)\]\[(\d+)\]\[(\d+)\]$";
                var match = Regex.Match(input, pattern);

                if (match.Success)
                {
                    title = match.Groups[1].Value; // Extract title
                    if (int.TryParse(match.Groups[2].Value, out rows) && rows > 0 && rows <= MaxRows &&
                        int.TryParse(match.Groups[3].Value, out seatsPerRow) && seatsPerRow > 0)
                    {
                        return true; // Input is valid
                    }
                }
                return false; // Input is invalid
            }

            public void ManageBookings(Cinema cinema)
            {
                // Loop until the user chooses to exit
                while (true)
                {
                    DisplayMenu(cinema.Title, cinema.AvailableSeats);
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            BookSeats(cinema);
                            break;
                        case "2":
                            CheckBooking(cinema);
                            break;
                        case "3":
                            Console.WriteLine("Thank you for using GIC Cinema system. bye!");
                            return; // Exit the program
                        default:
                            Console.WriteLine("Invalid choice. Please enter a number between 1 to 3.");
                            break;
                    }
                    Console.WriteLine();
                }
            }

            public void DisplayMenu(string title, int availableSeats)
            {
                // Display a menu of choices
                Console.WriteLine("Welcome to GIC Cinemas");
                Console.WriteLine($"[1] Book tickets for {title} ({availableSeats} seats available)");
                Console.WriteLine("[2] Check Bookings");
                Console.WriteLine("[3] Exit");
                Console.WriteLine("Please enter your selection:");
            }

            public void BookSeats(Cinema cinema)
            {
                Console.Write("Enter number of tickets to book, or enter blank to go back to main menu:");
                var ticketsToBook = Console.ReadLine();

                if (int.TryParse(ticketsToBook, out int bookSeat) && bookSeat >= 1 && bookSeat <= MaxRows)
                {
                    try
                    {
                        var (bookingId, bookedSeats) = cinema.BookSeats(bookSeat);
                        Console.WriteLine($"Successfully reserved {bookSeat} {cinema.Title} tickets");
                        Console.WriteLine($"Your Booking ID is {bookingId}.");
                        Console.WriteLine("Selected Seats:");
                        cinema.DisplaySeatGraph(bookedSeats);
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid seat number.");
                }
            }

            public void CheckBooking(Cinema cinema)
            {
                Console.Write("Enter Booking ID to check status: ");
                string bookingId = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(bookingId))
                {
                    try
                    {
                        var booking = cinema.GetBookingById(bookingId);
                        Console.WriteLine($"Booking ID: {booking.BookingId}");
                        Console.WriteLine("Booked Seats:");
                        cinema.DisplaySeatGraph(booking.Seats);
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Booking ID format. Please enter a valid number.");
                }
            }
        }
}
