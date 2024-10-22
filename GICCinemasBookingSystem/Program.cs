using System;
using System.Text.RegularExpressions;

namespace GICCinemasBookingSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            var cinemaManager = new CinemaManager();
            string userInput = cinemaManager.DisplayMainMenu();
            cinemaManager.ProcessUserInput(userInput);
        }
    }   

}