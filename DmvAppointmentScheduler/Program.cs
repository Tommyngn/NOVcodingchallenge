using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DmvAppointmentScheduler
{
    class Program
    {
        public static Random random = new Random();
        public static List<Appointment> appointmentList = new List<Appointment>();
        static void Main(string[] args)
        {
            CustomerList customers = ReadCustomerData();
            TellerList tellers = ReadTellerData();
            Calculation(customers, tellers);
            OutputTotalLengthToConsole();

        }
        private static CustomerList ReadCustomerData()
        {
            string fileName = "CustomerData.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"InputData/", fileName);
            string jsonString = File.ReadAllText(path);
            CustomerList customerData = JsonConvert.DeserializeObject<CustomerList>(jsonString);
            return customerData;

        }
        private static TellerList ReadTellerData()
        {
            string fileName = "TellerData.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"InputData/", fileName);
            string jsonString = File.ReadAllText(path);
            TellerList tellerData = JsonConvert.DeserializeObject<TellerList>(jsonString);
            return tellerData;

        }
        static void Calculation(CustomerList customers, TellerList tellers)
        {
            // Your code goes here .....
            // Re-write this method to be more efficient instead of a assigning all customers to the same teller
            int size = tellers.Teller.Count-1;
            Console.WriteLine(size);
            int count = 0;
            int count2 = 0;
            int exp = 2;
            foreach (Customer customer in customers.Customer)
            {
                
                if (count <= size)
                {
                    var appointment = new Appointment(customer, tellers.Teller[count]);
                    appointmentList.Add(appointment);
                    count = count + 1;
                    appointmentList= appointmentList.OrderBy(i => i.duration).ToList();
                }
                else
                {
                    
                    Console.WriteLine(count2);
                    var appointment = new Appointment(customer, appointmentList[count2].teller);
                    appointmentList.Add(appointment);
                    appointmentList = appointmentList.OrderBy(i => i.duration).ToList();
                    //double result = Math.Pow(size, exp);
                    //Console.WriteLine(result);
                    if (count2 == (size * exp))
                    {
                        Console.WriteLine("hitting");
                        Console.WriteLine(exp);
                        count2 = count2 - size;
                        exp = exp + 1;
                    }
                    count2 = count2 + 1;
                    
                }
                
            }
            

            for (int i = 0; i<appointmentList.Count; i++)
            {
                Console.WriteLine(appointmentList[i].teller.id +" "+ appointmentList[i].duration);
            }
        }
        static void OutputTotalLengthToConsole()
        {
            var tellerAppointments =
                from appointment in appointmentList
                group appointment by appointment.teller into tellerGroup
                select new
                {
                    teller = tellerGroup.Key,
                    totalDuration = tellerGroup.Sum(x => x.duration),
                };
            var max = tellerAppointments.OrderBy(i => i.totalDuration).LastOrDefault();
            Console.WriteLine("Teller " + max.teller.id + " will work for " + max.totalDuration + " minutes!");
        }

    }
}
