using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode_15.Program;


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
            private int start;
            private int end;
            public int Start { get => start; set => start = value; }
            public int End { get => end; set => end = value; }
            public int getSize() => End - Start + 1;
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
                BeaconX = 0;
                BeaconY = 0;
                SensorX = 0;
                SensorY = 0;
            }
            public Sensor(int sensorX, int sensorY, int beaconX, int beaconY)
            {
                SensorX = sensorX;
                SensorY = sensorY;
                BeaconX = beaconX;
                BeaconY = beaconY;
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
        public static void RemoveUnusedSensors(List<Sensor> sensors, int currentY)
        {
            List<int> toRemove = new List<int>();
            for (int i = 0; i < sensors.Count; i++)
            {
                Sensor currentSensor = sensors[i];
                if (currentSensor.SensorY > currentY)
                    if (currentSensor.SensorY - currentSensor.ManhattanDistance() > currentY)
                        toRemove.Add(i);
                if (currentSensor.SensorY < currentY)
                    if (currentSensor.SensorY + currentSensor.ManhattanDistance() < currentY)
                        toRemove.Add(i);
            }
            for (int i = toRemove.Count - 1; i > 0; i--)
                sensors.Remove(sensors[toRemove[i]]);
        }
        public static void PrintSensors(List<Sensor> sensors)
        {
            foreach (Sensor sensor in sensors)
                Console.WriteLine(sensor.ToString() + "\t" + sensor.ManhattanDistance());
        }
        public static int CompareByEnd(Range r1, Range r2) => r1.End.CompareTo(r2.End);
        public static int CompareByStart(Range r1, Range r2) => r1.Start.CompareTo(r2.Start);
        public static List<Range> CreateRanges(List<Sensor> newSensors, int currentY)
        {
            List<Range> ranges = new List<Range>();
            int temp = 0;

            for (int i = 0; i < newSensors.Count; i++)
            {
                Sensor currentSensor = newSensors[i];
                if (currentSensor.SensorY <= currentY)
                    temp = Math.Abs(currentSensor.SensorY + currentSensor.ManhattanDistance() - currentY);
                if (currentSensor.SensorY > currentY)
                    temp = Math.Abs(currentSensor.SensorY - currentSensor.ManhattanDistance() - currentY);
                Range range = new Range();
                range.Start = currentSensor.SensorX - temp;
                range.End = currentSensor.SensorX + temp;
                if(range.Start < 0)
                    range.Start = 0;
                if(range.End < 0)
                    range.End = 0;
                if(range.Start > 4000000)
                    range.Start = 4000000;
                if (range.End > 4000000)
                    range.End = 4000000;
                ranges.Add(range);
            }
            return ranges;
        }
        public static List<Sensor> CreateNewSensors(List<Sensor> sensors)
        {
            List<Sensor> ret = new List<Sensor>();
            foreach(Sensor sensor in sensors)
                ret.Add(sensor);
            return ret;
        }
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            string[] input = System.IO.File.ReadAllLines("input.txt");
            List<Sensor> sensors = ParseSensors(input);
            List<Range> ranges = new List<Range>();
            int currentLargestStart = 0;
            int currentLargestEnd = 0;
            int resultY = 0;
            for(int currentY = 0; currentY <= 4000000; currentY++)
            {
                List<Sensor> newSensors = CreateNewSensors(sensors);
                RemoveUnusedSensors(newSensors, currentY);
                ranges = CreateRanges(newSensors, currentY);
                ranges.Sort(new Comparison<Range>(CompareByStart));
                int notBeaconsInLine = 0;
                for (int i = 0; i < ranges.Count; i++)
                {
                    Range currentRange = ranges[i];
                    if (i == 0)
                    {
                        notBeaconsInLine += currentRange.getSize();
                        currentLargestStart = currentRange.Start;
                        currentLargestEnd = currentRange.End;
                        continue;
                    }
                    if (currentRange.End == currentLargestEnd)
                        continue;
                    if (currentRange.Start > currentLargestEnd && currentRange.End > currentLargestEnd)
                    {
                        notBeaconsInLine += currentRange.getSize();
                        currentLargestEnd = currentRange.End;
                        continue;
                    }
                    if (currentRange.End > currentLargestEnd)
                    {
                        if (currentRange.Start <= currentLargestEnd)
                            notBeaconsInLine += currentRange.End - currentLargestEnd;
                        currentLargestEnd = currentRange.End;
                        continue;
                    }
                }
                if (notBeaconsInLine < 4000001)
                {
                    resultY = currentY;
                    break;
                }
            }
            List<int> allNotBeacons = new List<int>();
            for (int i = 0; i < ranges.Count; i++)
            {
                Range currentRange = ranges[i];
                for (int j = currentRange.Start; j <= currentRange.End; j++)
                    allNotBeacons.Add(j);
            }
            List<int> uniqueNotBeacons = allNotBeacons.Distinct().ToList();
            int resultX;
            for (resultX = 0; resultX == uniqueNotBeacons[resultX]; resultX++){}
            Console.WriteLine(resultX + ";" + resultY);
            Console.WriteLine(Convert.ToInt64(resultX) * 4000000 + Convert.ToInt64(resultY));
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();
        }
    }
}
