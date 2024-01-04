using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultiQueueModels.Enums;
using System.Runtime.ConstrainedExecution;

namespace MultiQueueModels
{

    public class SimulationSystem
    {
        public SimulationSystem()
        {
            this.Servers = new List<Server>();
            this.InterarrivalDistribution = new List<TimeDistribution>();
            this.PerformanceMeasures = new PerformanceMeasures();
            this.SimulationTable = new List<SimulationCase>();


        }

        ///////////// INPUTS ///////////// 
        public int NumberOfServers { get; set; }
        public int StoppingNumber { get; set; }
        public List<Server> Servers { get; set; }
        public List<TimeDistribution> InterarrivalDistribution { get; set; }
        public Enums.StoppingCriteria StoppingCriteria { get; set; }
        public Enums.SelectionMethod SelectionMethod { get; set; }

        ///////////// OUTPUTS /////////////
        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }
        public decimal totalSimulationTime = 0;
        public decimal waitingSum = 0; // total time customer waited
        public decimal ctr = 0; //number of customer who waited
        public decimal ttc = 0; //total customer 
        public decimal t = 0; //once waiting time temp
        public int servingTime = 0; // work time for each server

        public void Read_file(string FP)
        {
            int ia = -1;
            decimal val33 = 0;
            int down_int = 0, upper_int = 0;

            List<int> arr = new List<int>();
            List<TimeDistribution> TDS = new List<TimeDistribution>();
            try
            {
                using (StreamReader reader = new StreamReader(FP))
                {
                    string line;
                    string currentKey = null;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        if (line == "NumberOfServers")
                        {
                            currentKey = "NumberOfServers";
                        }
                        else if (line == "StoppingNumber")
                        {
                            currentKey = "StoppingNumber";
                        }
                        else if (line == "StoppingCriteria")
                        {
                            currentKey = "StoppingCriteria";
                        }
                        else if (line == "SelectionMethod")
                        {
                            currentKey = "SelectionMethod";
                        }
                        else if (line == "InterarrivalDistribution")
                        {
                            currentKey = "InterarrivalDistribution";
                        }
                        else if (line.StartsWith("ServiceDistribution"))
                        {
                            currentKey = "ServiceDistribution";

                            Servers.Add(new Server());

                            ia++;
                            val33 = 0;
                            down_int = 0;
                            upper_int = 0;
                        }
                        else
                        {
                            int value;
                            if (currentKey == "NumberOfServers" && int.TryParse(line, out value))
                            {
                                NumberOfServers = value;
                            }
                            else if (currentKey == "StoppingNumber" && int.TryParse(line, out value))
                            {
                                StoppingNumber = value;
                            }
                            else if (currentKey == "StoppingCriteria" && int.TryParse(line, out value))
                            {

                                if (value == 1)
                                {
                                    StoppingCriteria = Enums.StoppingCriteria.NumberOfCustomers;

                                }
                                else
                                {
                                    StoppingCriteria = Enums.StoppingCriteria.SimulationEndTime;
                                }

                            }
                            else if (currentKey == "SelectionMethod" && int.TryParse(line, out value))
                            {

                                if (value == 1)
                                {
                                    SelectionMethod = Enums.SelectionMethod.HighestPriority;

                                }
                                else if (value == 2)
                                {
                                    SelectionMethod = Enums.SelectionMethod.Random;
                                }
                                else if (value == 3)
                                {
                                    SelectionMethod = Enums.SelectionMethod.LeastUtilization;
                                }
                            }
                            else if (currentKey == "InterarrivalDistribution")
                            {


                                string[] parts = line.Split(',');

                                if (parts.Length == 2 && int.TryParse(parts[0], out int va111) && decimal.TryParse(parts[1], out decimal va122))
                                {

                                    val33 += va122;
                                    down_int = (upper_int + 1);
                                    upper_int = (int)(val33 * 100);


                                    InterarrivalDistribution.Add(new TimeDistribution { Time = va111, Probability = va122, CummProbability = val33, MinRange = down_int, MaxRange = upper_int });
                                }


                            }
                            else if (currentKey == "ServiceDistribution")
                            {


                                string[] parts = line.Split(',');

                                if (parts.Length == 2 && int.TryParse(parts[0], out int va111) && decimal.TryParse(parts[1], out decimal va122))
                                {

                                    val33 += va122;
                                    down_int = (upper_int + 1);
                                    upper_int = (int)(val33 * 100);
                                    Servers[ia].TimeDistribution.Add(new TimeDistribution { Time = va111, Probability = va122, CummProbability = val33, MinRange = down_int, MaxRange = upper_int });
                                    Servers[ia].FinishTime = 0;
                                    Servers[ia].ID = (ia + 1);
                                    //Servers.Add( TimeDistribution.Add(new TimeDistribution { Time = va111, Probability = va122, CummProbability = val33, MinRange = down_int, MaxRange = upper_int }));
                                }


                            }
                        }
                    }
                }

            }
            catch
            {

            }
        }

        public List<SimulationCase> return_data_of_server(int server_number)
        {
            List<SimulationCase> SimulationTables = new List<SimulationCase>();
            foreach (var SimulationCase in SimulationTable)
            {
                if (SimulationCase.AssignedServer.ID == server_number)
                {
                    SimulationTables.Add(SimulationCase);
                }
            }
            return SimulationTables;
        }

        //public void idelprobab()
        //{
        //    int maxi = SimulationTable.Max((SimulationCase e) => e.EndTime);

        //    Console.WriteLine("Maxiiiiiiiiiii");
        //    Console.WriteLine(maxi);
        //    for (int i = 0; i < NumberOfServers; i++)
        //    {
        //        List<SimulationCase> SimulationTableas = return_data_of_server(i);
        //        decimal sums = 0;
        //        for (int j = 1; j < SimulationTableas.Count(); j++)
        //        {
        //            sums += SimulationTableas[j].ServiceTime;
        //        }
        //        Servers[i].IdleProbability = ((decimal)(maxi - sums) / (decimal)maxi);
        //        Console.WriteLine(Servers[i].IdleProbability);

        //    }

        //}
        //public TimeDistribution Get_Result_of_Random_Numbers(List<TimeDistribution> Distribution, int RandomValue) //random digit 
        //{

        //    TimeDistribution matchingDistribution = null;
        //    foreach (var timeDistribution in Distribution)
        //    {
        //        if (RandomValue >= timeDistribution.MinRange && RandomValue <= timeDistribution.MaxRange)
        //        {
        //            matchingDistribution = timeDistribution;
        //            break; // Exit the loop when a match is found
        //        }
        //    }
        //    return matchingDistribution;

        //}
        public int map_randomnumbers(List<TimeDistribution> dt, int randomnum)
        {
            int i = 0;
            int Range = -1;

            for (; i < dt.Count; i++)
            {
                if (randomnum >= dt[i].MinRange && randomnum <= dt[i].MaxRange)
                {
                    Range = dt[i].Time;
                }
            }


            return Range;


        }


        public void Fun_P()
        {
            Random randoms = new Random();

            // Generate a random number within the specified range


            if (StoppingCriteria.ToString() == "NumberOfCustomers")
            {
                Console.WriteLine(StoppingCriteria);
                for (int i = 1; i <= StoppingNumber; i++)
                {
                    int RandomServices = randoms.Next(1, 100);

                    int Randomfor_Intrearival = randoms.Next(1, 100);
                    Console.WriteLine(Randomfor_Intrearival);


                    if (i == 1)
                    {
                        int matchingDistribution = map_randomnumbers(Servers[0].TimeDistribution, RandomServices);

                        SimulationTable.Add(new SimulationCase
                        {
                            AssignedServer = Servers[0],
                            CustomerNumber = i,
                            RandomInterArrival = Randomfor_Intrearival,
                            ArrivalTime = 0,
                            InterArrival = 0,
                            RandomService = RandomServices,
                            StartTime = 0,
                            ServiceTime = matchingDistribution,
                            EndTime = matchingDistribution,
                            TimeInQueue = 0
                        });
                        //if (i == StoppingNumber)
                        //{
                            //totalSimulationTime = matchingDistribution;
                       // }
                        Servers[0].FinishTime = matchingDistribution;
                        ttc++;
                        servingTime = matchingDistribution;
                        Servers[0].twt += servingTime;
                        Servers[0].customersPerServer++;
                    }
                    else

                    {
                        int matchingDistribution_for_arrival = map_randomnumbers(InterarrivalDistribution, Randomfor_Intrearival);

                        bool temp = false;
                        int ser = 0, ArrivalTimes = (matchingDistribution_for_arrival + SimulationTable[i - 2].ArrivalTime), min_wait = 10000, server_if_not = -1;

                        foreach (var se in Servers)
                        {
                            if (ArrivalTimes < se.FinishTime)
                            {
                                if (se.FinishTime - ArrivalTimes < min_wait)
                                {

                                    min_wait = se.FinishTime - ArrivalTimes;
                                    server_if_not = ser;
                                }
                                temp = false;
                                ser++;

                            }
                            else
                            {
                                temp = true;
                                break; // Exit the loop when a match is found        
                            }
                        }


                        if (temp == true)
                        {
                            int matchingDistribution = map_randomnumbers(Servers[ser].TimeDistribution, RandomServices);

                            SimulationTable.Add(new SimulationCase
                            {
                                AssignedServer = Servers[ser],
                                CustomerNumber = i,
                                RandomInterArrival = Randomfor_Intrearival,
                                ArrivalTime = ArrivalTimes,
                                InterArrival = matchingDistribution_for_arrival,
                                RandomService = RandomServices,
                                StartTime = ArrivalTimes,
                                ServiceTime = matchingDistribution,
                                EndTime = ArrivalTimes + matchingDistribution,
                                TimeInQueue = 0
                            });

                            if (i == StoppingNumber)
                            {
                                totalSimulationTime = ArrivalTimes + matchingDistribution;
                            }
                            Servers[ser].FinishTime = ArrivalTimes + matchingDistribution;
                            ttc++;
                            servingTime = matchingDistribution;
                            Servers[ser].twt += servingTime;
                            Servers[ser].customersPerServer++;
                        }
                        else
                        {
                            int matchingDistribution = map_randomnumbers(Servers[server_if_not].TimeDistribution, RandomServices);

                            SimulationTable.Add(new SimulationCase
                            {
                                AssignedServer = Servers[server_if_not],
                                CustomerNumber = i,
                                RandomInterArrival = Randomfor_Intrearival,
                                ArrivalTime = ArrivalTimes,
                                InterArrival = matchingDistribution_for_arrival,
                                RandomService = RandomServices,
                                StartTime = Servers[server_if_not].FinishTime,
                                ServiceTime = matchingDistribution,
                                EndTime = Servers[server_if_not].FinishTime + matchingDistribution,
                                TimeInQueue = Servers[server_if_not].FinishTime - ArrivalTimes

                            });
                            if (i == StoppingNumber)
                            {
                                totalSimulationTime = Servers[server_if_not].FinishTime + matchingDistribution;
                            }
                            t = Servers[server_if_not].FinishTime - (ArrivalTimes);
                            Servers[server_if_not].FinishTime += matchingDistribution;
                            servingTime = matchingDistribution;
                            Servers[server_if_not].twt += servingTime;
                            Servers[server_if_not].customersPerServer++;
                            // Console.WriteLine("waiting time of customer "+ i+"is "+ t);
                            waitingSum += t;
                            t = 0;
                            ctr++;
                            ttc++;

                        }



                    }
                }

            }



        }




        public void Least_utlization()
        {
            Random randoms = new Random();

            // Generate a random number within the specified range


            if (StoppingCriteria.ToString() == "NumberOfCustomers")
            {
                Console.WriteLine(StoppingCriteria);
                for (int i = 1; i <= StoppingNumber; i++)
                {
                    int RandomServices = randoms.Next(1, 101);

                    int Randomfor_Intrearival = randoms.Next(1, 101);
                    Console.WriteLine(Randomfor_Intrearival);


                    if (i == 1)
                    {
                        int matchingDistribution = map_randomnumbers(Servers[0].TimeDistribution, RandomServices);
                        //foreach (var timeDistribution in Servers[0].TimeDistribution)
                        //{
                        //    if (RandomServices >= timeDistribution.MinRange && RandomServices <= timeDistribution.MaxRange)
                        //    {
                        //        matchingDistribution = timeDistribution;
                        //        break; // Exit the loop when a match is found
                        //    }
                        //}
                        SimulationTable.Add(new SimulationCase
                        {
                            AssignedServer = Servers[0],
                            CustomerNumber = i,
                            RandomInterArrival = Randomfor_Intrearival,
                            ArrivalTime = 0,
                            InterArrival = 0,
                            RandomService = RandomServices,
                            StartTime = 0,
                            ServiceTime = matchingDistribution,
                            EndTime = matchingDistribution,
                            TimeInQueue = 0
                        });
                        if (i == StoppingNumber)
                        {
                            totalSimulationTime = matchingDistribution;
                        }
                        Servers[0].FinishTime = matchingDistribution;
                        Servers[0].TotalWorkingTime += matchingDistribution;
                        servingTime = matchingDistribution;
                        Servers[0].twt += servingTime;
                        Servers[0].customersPerServer++;

                        ttc++;
                    }
                    else
                    {
                        int matchingDistribution_for_arrival = map_randomnumbers(InterarrivalDistribution, Randomfor_Intrearival);

                        bool temp = false;
                        int ser = 0, ArrivalTimes = (matchingDistribution_for_arrival + SimulationTable[i - 2].ArrivalTime), min_wait = int.MaxValue, server_if_not = -1;
                        int minutili = int.MaxValue, serv_if_least = -1;
                        foreach (var se in Servers)
                        {

                            if (ArrivalTimes < se.FinishTime)
                            {
                                if (se.FinishTime - ArrivalTimes < min_wait)
                                {

                                    min_wait = se.FinishTime - ArrivalTimes;
                                    server_if_not = ser;
                                }
                                // temp = false;


                            }
                            else
                            {
                                if (minutili > se.TotalWorkingTime)
                                {
                                    minutili = se.TotalWorkingTime;
                                    serv_if_least = ser;

                                }
                                temp = true;

                            }
                            ser++;
                        }


                        if (temp == true)
                        {
                            int matchingDistribution = map_randomnumbers(Servers[serv_if_least].TimeDistribution, RandomServices);


                            SimulationTable.Add(new SimulationCase
                            {
                                AssignedServer = Servers[serv_if_least],
                                CustomerNumber = i,
                                RandomInterArrival = Randomfor_Intrearival,
                                ArrivalTime = ArrivalTimes,
                                InterArrival = matchingDistribution_for_arrival,
                                RandomService = RandomServices,
                                StartTime = ArrivalTimes,
                                ServiceTime = matchingDistribution,
                                EndTime = ArrivalTimes + matchingDistribution,
                                TimeInQueue = 0
                            });
                            if (i == StoppingNumber)
                            {
                                totalSimulationTime = matchingDistribution;
                            }
                            Servers[serv_if_least].FinishTime = ArrivalTimes + matchingDistribution;
                            Servers[serv_if_least].TotalWorkingTime += matchingDistribution;
                            servingTime = matchingDistribution;
                            Servers[server_if_not].twt += servingTime;
                            Servers[server_if_not].customersPerServer++;
                            ttc++;

                        }
                        else
                        {
                            int matchingDistribution = map_randomnumbers(Servers[server_if_not].TimeDistribution, RandomServices);
                          
                            SimulationTable.Add(new SimulationCase
                            {
                                AssignedServer = Servers[server_if_not],
                                CustomerNumber = i,
                                RandomInterArrival = Randomfor_Intrearival,
                                ArrivalTime = ArrivalTimes,
                                InterArrival = matchingDistribution_for_arrival,
                                RandomService = RandomServices,
                                StartTime = Servers[server_if_not].FinishTime,
                                ServiceTime = matchingDistribution,
                                EndTime = Servers[server_if_not].FinishTime + matchingDistribution,
                                TimeInQueue = Servers[server_if_not].FinishTime - ArrivalTimes
                            });
                            if (i == StoppingNumber)
                            {
                                totalSimulationTime = matchingDistribution;
                            }
                            Servers[server_if_not].FinishTime += matchingDistribution;
                            Servers[server_if_not].TotalWorkingTime += matchingDistribution;
                            servingTime = matchingDistribution;
                            Servers[server_if_not].twt += servingTime;
                            Servers[server_if_not].customersPerServer++;
                            t = Servers[server_if_not].FinishTime - (ArrivalTimes); //time in queue

                            // Console.WriteLine("waiting time of customer "+ i+"is "+ t);
                            waitingSum += t;
                            t = 0;
                            ctr++;
                            ttc++;
                        }



                    }
                }

            }
        }

        public void RandomMethod()
        {
            Random randoms = new Random();

            if (StoppingCriteria.ToString() == "NumberOfCustomers")
            {

                List<int> server_id = new List<int>();
                Random r = new Random();
                int rand = r.Next(1, 100);
                int serv = randoms.Next(0, NumberOfServers);

                SimulationTable[0].CustomerNumber = 1;
                SimulationTable[0].RandomInterArrival = 1;
                SimulationTable[0].InterArrival = 0;
                SimulationTable[0].ArrivalTime = 0;
                SimulationTable[0].RandomService = rand;

                SimulationTable[0].AssignedServer.ID = serv;
                SimulationTable[0].StartTime = 0;
                Servers[serv].StartTime = 0;
                SimulationTable[0].ServiceTime = map_randomnumbers(Servers[serv].TimeDistribution, rand);
                SimulationTable[0].EndTime = SimulationTable[0].ServiceTime;
                Servers[serv].FinishTime = SimulationTable[0].EndTime;
                SimulationTable[0].TimeInQueue = Math.Abs(SimulationTable[0].ArrivalTime - SimulationTable[0].StartTime);
                // SimulationTable[0].AssignedServer.Number_of_customers = 1;

                if (StoppingNumber == 1)
                {
                    totalSimulationTime = SimulationTable[0].ServiceTime;
                }
                servingTime = SimulationTable[0].ServiceTime;
                Servers[serv].twt += servingTime;
                Servers[serv].customersPerServer++;
                ttc++;


                for (int k = 1; k < StoppingNumber; k++)
                {
                    int rand2 = r.Next(1, 100);
                    SimulationTable[k].CustomerNumber = k + 1;
                    SimulationTable[k].RandomInterArrival = rand2;
                    SimulationTable[k].InterArrival = map_randomnumbers(Servers[k].TimeDistribution, rand2);
                    SimulationTable[k].ArrivalTime = SimulationTable[k].InterArrival + SimulationTable[k - 1].ArrivalTime;
                    SimulationTable[k].RandomService = r.Next(1, 100);


                    for (int j = 0; j < NumberOfServers; j++)
                    {
                        if (SimulationTable[k].ArrivalTime >= Servers[j].FinishTime)
                        {

                            server_id.Add(j);

                        }

                    }
                    if (server_id.Count != 0)
                    {
                        int num = r.Next(server_id.Count);

                        SimulationTable[k].AssignedServer = Servers[num];
                        SimulationTable[k].RandomService = rand2;
                        SimulationTable[k].StartTime = SimulationTable[k].ArrivalTime;
                        Servers[num].StartTime = SimulationTable[k].StartTime;
                        Servers[num].ID = num + 1;
                        SimulationTable[k].ServiceTime = map_randomnumbers(Servers[num].TimeDistribution, SimulationTable[k].RandomService);

                        Servers[num].FinishTime = SimulationTable[k].StartTime + SimulationTable[k].ServiceTime;
                        SimulationTable[k].EndTime = Servers[num].FinishTime;
                        SimulationTable[k].TimeInQueue = Math.Abs((SimulationTable[k].ArrivalTime) - (SimulationTable[k].StartTime));
                        // Servers[num].Number_of_customers++;
                        // flag = true;
                        if (k == StoppingNumber)
                        {
                            totalSimulationTime = SimulationTable[k].EndTime;
                        }
                        Servers[num].FinishTime = SimulationTable[k].EndTime;
                        ttc++;
                        servingTime = SimulationTable[k].ServiceTime;
                        Servers[num].twt += servingTime;
                        Servers[num].customersPerServer++;

                        server_id.Remove(num);

                    }
                    else
                    {
                        int min = 1000;
                        int x = 0;
                        for (int j = 0; j < NumberOfServers; j++)
                        {
                            if (min > (Servers[j].FinishTime))
                            {
                                min = Servers[j].FinishTime;
                                x = j;

                            }
                        }

                        SimulationTable[k].AssignedServer = Servers[x];
                        Servers[x].ID = x + 1;
                        SimulationTable[k].RandomService = rand2;
                        SimulationTable[k].ServiceTime = map_randomnumbers(Servers[x].TimeDistribution, SimulationTable[k].RandomService);
                        SimulationTable[k].StartTime = Servers[x].FinishTime;
                        //s.Servers[x].StartTime = s.SimulationTable[k].StartTime;
                        Servers[x].StartTime = SimulationTable[k].StartTime;
                        // SimulationTable[x].EndTime = SimulationTable[k].StartTime + SimulationTable[k].ServiceTime;
                        Servers[x].FinishTime = SimulationTable[k].StartTime + SimulationTable[k].ServiceTime;
                        SimulationTable[k].EndTime = Servers[x].FinishTime;
                        //s.SimulationTable[k].EndTime = s.SimulationTable[x].EndTime;
                        SimulationTable[k].TimeInQueue = Math.Abs((SimulationTable[k].ArrivalTime) - (SimulationTable[k].StartTime));
                        //Servers[x].Number_of_customers++;

                        if (k == StoppingNumber)
                        {
                            totalSimulationTime = Servers[x].FinishTime + SimulationTable[k].ServiceTime;
                        }
                        t = Servers[x].FinishTime - (SimulationTable[k].ArrivalTime);

                        servingTime = SimulationTable[k].ServiceTime;
                        Servers[x].twt += servingTime;
                        Servers[x].customersPerServer++;
                        Console.WriteLine("waiting time of customer " + k + " is " + t);
                        waitingSum += t;
                        t = 0;
                        ctr++;
                        ttc++;
                    }

                }
            }

        }

        public int max_q()
        {
            //int Max_Queue_Length = 0;

            //for (int i = 1; i < StoppingNumber; i++)
            //{
            //    int max = 1;
            //    if (SimulationTable[i].TimeInQueue > 0)
            //    {

            //        for (int j = i + 1; j < StoppingNumber; j++)
            //        {
            //            int result = SimulationTable[j].ArrivalTime - SimulationTable[i].ArrivalTime;
            //            if (result < SimulationTable[i].TimeInQueue)
            //            {
            //                max++;
            //            }
            //            else
            //            {
            //                if (max > Max_Queue_Length)
            //                {
            //                    Max_Queue_Length = max;

            //                }
            //                break;
            //            }

            //        }
            //    }
            //}
            //PerformanceMeasures.MaxQueueLength = Max_Queue_Length;






            Dictionary<int, int> em = new Dictionary<int, int>(); //vector


            for (int i = 0; i < SimulationTable.Count(); i++)
            {
                for (int j = SimulationTable[i].ArrivalTime; j < SimulationTable[i].StartTime; j++)
                {
                    if (em.ContainsKey(j))
                    {
                        em[j] += 1;
                    }
                    else
                    {
                        em[j] = 1;
                    }

                }


            }
            Console.WriteLine("Dict");
            if (em.Count > 0)
            {
                PerformanceMeasures.MaxQueueLength = em.Values.Max();
            }
            else
            {
                PerformanceMeasures.MaxQueueLength = 0;
            }


            return 0;
            

        }
    }
}






