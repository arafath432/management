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

        public static Hall GetHallByNumber(int hallNumber)
        {
            return hallList.Find(hall => hall.HallNo == hallNumber);
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
            seats[showId] = new int[row, col];
        }

        public void ViewShowList()
        {
            Console.WriteLine($"\nHall {hallNo} - Show List:");
            foreach (var show in showList)
            {
                Console.WriteLine($"\tMovie: {show.movieName}, Show ID: {show.showId}, Time: {show.time}");
            }
        }

        public void BookSeats()
        {
            Console.Write("Enter show ID to book tickets: ");
            if (!int.TryParse(Console.ReadLine(), out int showId) || !seats.ContainsKey(showId))
            {
                Console.WriteLine("\tInvalid show ID.");
                return;
            }

            Console.Write("Enter the number of tickets to book: ");
            if (!int.TryParse(Console.ReadLine(), out int ticketCount) || ticketCount <= 0)
            {
                Console.WriteLine("\tInvalid ticket count.");
                return;
            }

            for (int t = 0; t < ticketCount; t++)
            {
                Console.Write($"Enter row number for ticket {t + 1}: ");
                if (!int.TryParse(Console.ReadLine(), out int row) || row < 0 || row >= this.row)
                {
                    Console.WriteLine("\tInvalid row number.");
                    continue;
                }

                Console.Write($"Enter column number for ticket {t + 1}: ");
                if (!int.TryParse(Console.ReadLine(), out int col) || col < 0 || col >= this.col)
                {
                    Console.WriteLine("\tInvalid column number.");
                    continue;
                }

                if (seats[showId][row, col] == 1)
                {
                    Console.WriteLine($"\tSeat ({row}, {col}) is already booked.");
                }
                else
                {
                    seats[showId][row, col] = 1;
                    Console.WriteLine($"\tSeat ({row}, {col}) successfully booked.");
                }
            }
        }

        public void ViewAvailableSeats()
        {
            Console.Write("Enter the show ID to view available seats: ");
            if (!int.TryParse(Console.ReadLine(), out int showId) || !seats.ContainsKey(showId))
            {
                Console.WriteLine("\tInvalid show ID.");
                return;
            }

            Console.WriteLine($"Available seats for Show {showId}:");
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
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Adding Halls and Shows
            Hall hallOne = new Hall(5, 10, 1);
            Hall hallTwo = new Hall(6, 8, 2);

            hallOne.EntryShow(101, "Kung Fu Panda 1", "24/05/2024 3:00 PM");
            hallTwo.EntryShow(102, "Kung Fu Panda 2", "24/05/2024 4:00 PM");

            while (true)
            {
                Console.WriteLine("\n\tMOVIE THEATER TICKET MANAGEMENT SYSTEM");
                Console.WriteLine("Options:");
                Console.WriteLine("1: View all shows");
                Console.WriteLine("2: View available seats");
                Console.WriteLine("3: Book tickets");
                Console.WriteLine("4: Exit");

                Console.Write("Enter your choice: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("\tInvalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        foreach (var hall in StarCinema.GetHallList())
                        {
                            hall.ViewShowList();
                        }
                        break;

                    case 2:
                        Console.Write("Enter hall number to view seats: ");
                        if (!int.TryParse(Console.ReadLine(), out int hallNumber) || StarCinema.GetHallByNumber(hallNumber) == null)
                        {
                            Console.WriteLine("\tInvalid hall number.");
                            break;
                        }
                        StarCinema.GetHallByNumber(hallNumber).ViewAvailableSeats();
                        break;

                    case 3:
                        Console.Write("Enter hall number to book tickets: ");
                        if (!int.TryParse(Console.ReadLine(), out hallNumber) || StarCinema.GetHallByNumber(hallNumber) == null)
                        {
                            Console.WriteLine("\tInvalid hall number.");
                            break;
                        }
                        StarCinema.GetHallByNumber(hallNumber).BookSeats();
                        break;

                    case 4:
                        Console.WriteLine("\tExiting. Have a great day!");
                        return;

                    default:
                        Console.WriteLine("\tInvalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}
