using GICCinemasBookingSystem;

namespace GICCinemaBookingSystemTest
{
    public class CinemaManagerTest
    {
        [Fact]
        public void DisplayMainMenu_ShouldReturnUserInput()
        {
            // Arrange
            var cinemaManager = new CinemaManager();
            var input = "[Test Movie][5][10]";
            var inputReader = new System.IO.StringReader(input);
            Console.SetIn(inputReader);

            // Act
            var result = cinemaManager.DisplayMainMenu();

            // Assert
            Assert.Equal(input, result);
        }

        [Fact]
        public void ValidateUserInput_ShouldReturnTrue_WhenInputIsValid()
        {
            // Arrange
            var cinemaManager = new CinemaManager();
            var input = "[Test Movie][5][10]";

            // Act
            var isValid = cinemaManager.ValidateUserInput(input, out string title, out int rows, out int seatsPerRow);

            // Assert
            Assert.True(isValid);
            Assert.Equal("Test Movie", title);
            Assert.Equal(5, rows);
            Assert.Equal(10, seatsPerRow);
        }

        [Fact]
        public void ValidateUserInput_ShouldReturnFalse_WhenInputIsInvalid()
        {
            // Arrange
            var cinemaManager = new CinemaManager();
            var input = "[Test Movie][5][invalid]";

            // Act
            var isValid = cinemaManager.ValidateUserInput(input, out string title, out int rows, out int seatsPerRow);

            // Assert
            Assert.False(isValid);
        }
    }
   
}
