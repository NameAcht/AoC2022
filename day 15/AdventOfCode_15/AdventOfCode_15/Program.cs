using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdventOfCode_15
{
    internal class Program
    {
        public struct Coordinate
        {
            public int x;
            public int y;
        }
        public class Range
        {
            public int Start { get; set; }
            public int End { get; set; }
            public int GetSize() => End - Start + 1;
            public override string ToString() => Start + "->" + End;
            public Range()
            {
                Start = 0;
                End = 0;
            }
        }
        public class Sensor
        {
            public int SensorX { get; set; }
            public int SensorY { get; set; }
            public int BeaconX { get; set; }
            public int BeaconY { get; set; }
            public Sensor()
            {
                this.BeaconX = 0;
                this.BeaconY = 0;
                this.SensorX = 0;
                this.SensorY = 0;
            }
            public Sensor(int sensorX, int sensorY, int beaconX, int beaconY)
            {
                this.SensorX = sensorX;
                this.SensorY = sensorY;
                this.BeaconX = beaconX;
                this.BeaconY = beaconY;
            }
            public override string ToString() => "Sensor: " + SensorX + ";" + SensorY + "\t\tClosest Beacon: " + BeaconX + ";" + BeaconY;
            public int ManhattanDistance() => Math.Max(SensorX, BeaconX) - Math.Min(SensorX, BeaconX) + Math.Max(SensorY, BeaconY) - Math.Min(SensorY, BeaconY);
        }
        public static List<Sensor> ParseSensors(string[] input)
        {
            List<Sensor> result = new List<Sensor>();
            foreach (string line in input)
            {
                char[] splitters = new char[3] { '=', ',', ':' };
                string[] coordinates = line.Split(splitters);
                result.Add(new Sensor(Convert.ToInt32(coordinates[1]), Convert.ToInt32(coordinates[3]), Convert.ToInt32(coordinates[5]), Convert.ToInt32(coordinates[7])));
            }
            return result;
        }

        public static void RemoveUselessSensors(List<Sensor> sensors)
        {
            List<int> toRemove = new List<int>();

            for (int i = 0; i < sensors.Count; i++)
            {
                Sensor currentSensor = sensors[i];
                if (currentSensor.SensorY > 2000000)
                    if (currentSensor.SensorY - currentSensor.ManhattanDistance() > 2000000)
                        toRemove.Add(i);
                if (currentSensor.SensorY < 2000000)
                    if (currentSensor.SensorY + currentSensor.ManhattanDistance() < 2000000)
                        toRemove.Add(i);
                
            }

            for (int i = toRemove.Count - 1; i >= 0; i--)
                sensors.Remove(sensors[toRemove[i]]);
        }
        public static void PrintSensors(List<Sensor> sensors)
        {
            foreach (Sensor sensor in sensors)
                Console.WriteLine(sensor.ToString() + "\t" + sensor.ManhattanDistance());
        }
        public static int CompareByEnd(Range r1, Range r2) => r1.End.CompareTo(r2.End);
        public static int CompareByStart(Range r1, Range r2) => r1.Start.CompareTo(r2.Start);
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            string[] input = System.IO.File.ReadAllLines("input.txt");
            List<Sensor> sensors = ParseSensors(input);
            List<Range> ranges = new List<Range>();
            int temp = 0;
            Comparison<Range> rangeEndComparer = new Comparison<Range>(CompareByEnd);
            Comparison<Range> rangeStartComparer = new Comparison<Range>(CompareByStart);
            RemoveUselessSensors(sensors);
            
            for (int i = 0; i < sensors.Count; i++)
            {
                Sensor currentSensor = sensors[i];
                if(currentSensor.SensorY < 2000000)
                    temp = currentSensor.SensorY + currentSensor.ManhattanDistance() - 2000000;
                if(currentSensor.SensorY > 2000000)
                    temp = Math.Abs(currentSensor.SensorY - currentSensor.ManhattanDistance() - 2000000);
                Range r = new Range();
                r.Start = currentSensor.SensorX - temp;
                r.End = currentSensor.SensorX + temp;
                ranges.Add(r);
            }
            List<int> allNotBeacons = new List<int>();
            for (int i = 0; i < ranges.Count; i++)
            {
                Range currentRange = ranges[i];
                for (int j = currentRange.Start; j < currentRange.End; j++)
                    allNotBeacons.Add(j);
            }
            sw.Stop();
            Console.WriteLine(allNotBeacons.Distinct().ToList().Count);
            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();
        }
    }
}
