using System;
using System.Collections.Generic;

namespace Marshrutka
{
    class Passenger
    {
        public int Number { get; private set; }
        public Passenger(int number)
        {
            Number = number;
        }
    }

    class Driver
    {
        public string Name { get; private set; }

        public Driver(string name)
        {
            Name = name;
        }

        public void OnRouteFull(object sender, EventArgs e)
        {
            Console.WriteLine($"{Name}: Еду по маршруту.");
        }

        public void OnRouteEmpty(object sender, EventArgs e)
        {
            Console.WriteLine($"{Name}: Еду в парк.");
        }
    }

    class Route
    {
        public int Capacity { get; private set; }
        public List<Passenger> Passengers { get; private set; }
        public bool HasFreeSeats => Passengers.Count < Capacity;

        public event EventHandler RouteFull;
        public event EventHandler RouteEmpty;

        public Route(int capacity)
        {
            Capacity = capacity;
            Passengers = new List<Passenger>();
        }

        public void AddPassenger(Passenger passenger)
        {
            if (HasFreeSeats)
            {
                Passengers.Add(passenger);
                Console.WriteLine($"Пассажир {passenger.Number} сел в маршрутку.");
                if (Passengers.Count == Capacity)
                {
                    RouteFull?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                Console.WriteLine("Мест нет!");
            }
        }

        public void RemovePassenger(int index)
        {
            if (Passengers.Count > 0 && index < Passengers.Count)
            {
                Console.WriteLine($"Пассажир {Passengers[index].Number} вышел из маршрутки.");
                Passengers.RemoveAt(index);
                if (Passengers.Count == 0)
                {
                    RouteEmpty?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                Console.WriteLine("Нет пассажиров для высадки!");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите количество мест в маршрутке: ");
            int capacity = int.Parse(Console.ReadLine());

            Route route = new Route(capacity);
            Driver driver = new Driver("Иван");

            // Подписка на события
            route.RouteFull += driver.OnRouteFull;
            route.RouteEmpty += driver.OnRouteEmpty;

            // Моделируем посадку пассажиров
            int passengerNumber = 1;
            while (route.HasFreeSeats)
            {
                Passenger passenger = new Passenger(passengerNumber++);
                route.AddPassenger(passenger);
            }

            // Моделируем высадку пассажиров
            while (route.Passengers.Count > 0)
            {
                route.RemovePassenger(0); // Всегда высаживаем первого в списке
            }
        }
    }
}
