using System;
using System.Collections.Generic;
namespace myprograme
{
    interface ICinemaOperations
    {
        void ViewShowList();
        void BookSeats();
        void ViewAvailableSeats();
    }

    abstract class CinemaBase
    {
        public abstract void AddHall(Hall hall);
    }

    class StarCinema : CinemaBase
    {
        private static List<Hall> hallList = new List<Hall>();

        public override void AddHall(Hall hall)
        {
            hallList.Add(hall);
        }

        public static List<Hall> GetHallList()
        {
            return hallList;
        }
    }

    class Hall : ICinemaOperations
    {
        private Dictionary<int, int[,]> seats;
        private List<(string movieName, int showId, string time)> showList;
        private int row;
        private int col;
        private int hallNo;

        public int Row { get { return row; } }
        public int Col { get { return col; } }
        public int HallNo { get { return hallNo; } }

        public Hall(int row, int col, int hallNo)
        {
            seats = new Dictionary<int, int[,]>();
            showList = new List<(string, int, string)>();
            this.row = row;
            this.col = col;
            this.hallNo = hallNo;
            new StarCinema().AddHall(this);
        }

        public void EntryShow(int showId, string movieName, string time)
        {
            showList.Add((movieName, showId, time));
            SeatMatrix(showId);
        }

        private void SeatMatrix(int showId)
        {
            seats[showId] = new int[row, col];
        }

        public void ViewShowList()
        {
            foreach (var show in showList)
            {
                Console.WriteLine($"\tMovie Name: {show.movieName}, Show Id: {show.showId}, Time: {show.time}");
            }
        }

        public void BookSeats()
        {
            try
            {
                Console.Write("Enter show id to book tickets: ");
                int showId = int.Parse(Console.ReadLine());

                bool showExists = false;
                foreach (var hall in StarCinema.GetHallList())
                {
                    if (hall.seats.ContainsKey(showId))
                    {
                        showExists = true;
                        break;
                    }
                }

                if (!showExists)
                {
                    Console.WriteLine("\tShow id does not exist");
                    return;
                }

                Console.Write("Enter the row number: ");
                int row = int.Parse(Console.ReadLine());

                Console.Write("Enter the column number: ");
                int col = int.Parse(Console.ReadLine());

                if (row < 0 || row >= this.row || col < 0 || col >= this.col)
                {
                    Console.WriteLine("\tInvalid seat position.");
                    return;
                }

                if (seats[showId][row, col] == 1)
                {
                    Console.WriteLine("\tThe seat is already booked.");
                }
                else
                {
                    seats[showId][row, col] = 1;
                    Console.WriteLine($"\tSeat at position ({row}, {col}) successfully booked.");
                    foreach (var show in showList)
                    {
                        Console.WriteLine($"\tSee online:https://www.moviesexample.com/{show.movieName}/{show.showId}");
                    }
                    
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\tInvalid input. Please enter numeric values.");
            }
        }

        public void ViewAvailableSeats()
        {
            try
            {
                Console.Write("Enter the show id to view available seats: ");
                int showId = int.Parse(Console.ReadLine());

                if (!seats.ContainsKey(showId))
                {
                    Console.WriteLine("\tShow id does not exist");
                    return;
                }

                Console.WriteLine($"Available seats for show {showId}:");
                int[,] seatMatrix = seats[showId];

                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        Console.Write(seatMatrix[i, j] == 0 ? "O " : "X ");
                    }
                    Console.WriteLine();
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\tInvalid input. Please enter numeric values.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Hall hallOne = new Hall(5, 10, 1);
            Hall hallTwo = new Hall(6, 8, 2);

            hallOne.EntryShow(101, "Kung Fu Panda 1", "24/05/2024 3:00 PM");
            hallTwo.EntryShow(102, "Kung Fu Panda 2", "24/05/2024 4:00 PM");

            while (true)
            {
                Console.WriteLine("\nOptions: ");
                Console.WriteLine("1 : View all shows today");
                Console.WriteLine("2 : View available seats");
                Console.WriteLine("3 : Book Ticket");
                Console.WriteLine("4 : Exit");

                try
                {
                    Console.Write("Enter Option: ");
                    int choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("\nCurrently running shows:");
                            foreach (var hall in StarCinema.GetHallList())
                            {
                                hall.ViewShowList();
                            }
                            break;
                        case 2:
                            Console.WriteLine("Choose a hall:");
                            foreach (var hall in StarCinema.GetHallList())
                            {
                                hall.ViewAvailableSeats();
                            }
                            break;
                        case 3:
                            Console.WriteLine("Choose a hall:");
                            foreach (var hall in StarCinema.GetHallList())
                            {
                                hall.BookSeats();
                            }
                            break;
                        case 4:
                            Console.WriteLine("\nExiting the system. Have a nice day!");
                            return;
                        default:
                            Console.WriteLine("\nInvalid Option. Please try again.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("\tInvalid input. Please enter an integer.");
                }
            }
        }
    }
}
