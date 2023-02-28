using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_19
{
    public enum Decisions
    {
        BuildOreRobot,
        BuildClayRobot,
        BuildObsidianRobot,
        BuildGeodeRobot
    }
    public class MaterialCost
    {
        public MaterialCost(int oreCost, int clayCost, int obsidianCost)
        {
            OreCost = oreCost;
            ClayCost = clayCost;
            ObsidianCost = obsidianCost;
        }

        public int OreCost { get; set; }
        public int ClayCost { get; set; }
        public int ObsidianCost { get; set; }
    }
    public class Blueprint
    {
        public Blueprint(MaterialCost oreRobot, MaterialCost clayRobot, MaterialCost obsidianRobot, MaterialCost geodeRobot)
        {
            OreRobot = oreRobot;
            ClayRobot = clayRobot;
            ObsidianRobot = obsidianRobot;
            GeodeRobot = geodeRobot;
            MaxOreRobots = Math.Max(Math.Max(ClayRobot.OreCost, ObsidianRobot.OreCost), GeodeRobot.OreCost);
        }

        public MaterialCost OreRobot { get; set; }
        public MaterialCost ClayRobot { get; set; }
        public MaterialCost ObsidianRobot { get; set; }
        public MaterialCost GeodeRobot { get; set; }
        public int MaxOreRobots { get; set; }
    }

    public class State
    {
        public State(int minute, int ore, int clay, int obsidian, int geodes, int oreRobots, int clayRobots, int obsidianRobots, int geodeRobots)
        {
            Minute = minute;
            Ore = ore;
            Clay = clay;
            Obsidian = obsidian;
            Geodes = geodes;
            OreRobots = oreRobots;
            ClayRobots = clayRobots;
            ObsidianRobots = obsidianRobots;
            GeodeRobots = geodeRobots;
        }
        public State(State s)
        {
            Minute = s.Minute;
            Ore = s.Ore;
            Clay = s.Clay;
            Obsidian = s.Obsidian;
            Geodes = s.Geodes;
            OreRobots = s.OreRobots;
            ClayRobots = s.ClayRobots;
            ObsidianRobots = s.ObsidianRobots;
            GeodeRobots = s.GeodeRobots;
        }

        public State()
        {
        }

        public int Minute { get; set; }
        public int Ore { get; set; }
        public int Clay { get; set; }
        public int Obsidian { get; set; }
        public int Geodes { get; set; }
        public int OreRobots { get; set; }
        public int ClayRobots { get; set; }
        public int ObsidianRobots { get; set; }
        public int GeodeRobots { get; set; }
    }

    internal class Program
    {
        public static List<Blueprint> ParseBlueprints(string[] input)
        {
            var blueprints = new List<Blueprint>();

            foreach (var line in input)
            {
                var splitLine = line.Split(' ', ':');
                var oreRobot = new MaterialCost(Convert.ToInt32(splitLine[7]), 0, 0);
                var clayRobot = new MaterialCost(Convert.ToInt32(splitLine[13]), 0, 0);
                var obsidianRobot = new MaterialCost(Convert.ToInt32(splitLine[19]), Convert.ToInt32(splitLine[22]), 0);
                var geodeRobot = new MaterialCost(Convert.ToInt32(splitLine[28]), 0, Convert.ToInt32(splitLine[31]));

                blueprints.Add(new Blueprint(oreRobot, clayRobot, obsidianRobot, geodeRobot));
            }

            return blueprints;
        }

        public static List<Decisions> GetDecisions(State currentState, Blueprint blueprint)
        {
            var decisions = new List<Decisions>();
            if (currentState.ObsidianRobots > 0)
                decisions.Add(Decisions.BuildGeodeRobot);
            if (currentState.ClayRobots > 0 /*&& blueprint.ObsidianRobot.OreCost >= currentState.Ore && blueprint.ObsidianRobot.ClayCost >= currentState.Clay*/)
                decisions.Add(Decisions.BuildObsidianRobot);
            if(blueprint.ClayRobot.OreCost >= currentState.Ore - 1)
                decisions.Add(Decisions.BuildClayRobot);
            if (blueprint.MaxOreRobots >= currentState.OreRobots && blueprint.OreRobot.OreCost >= currentState.Ore - 1)
                decisions.Add(Decisions.BuildOreRobot);
            
            

            return decisions;
        }

        public static void UpdateStateMats(State currentState, int minutes, MaterialCost robotCost)
        {
            currentState.Ore += currentState.OreRobots * minutes;
            currentState.Clay += currentState.ClayRobots * minutes;
            currentState.Obsidian += currentState.ObsidianRobots * minutes;
            currentState.Geodes += currentState.GeodeRobots * minutes;

            currentState.Ore -= robotCost.OreCost;
            currentState.Clay -= robotCost.ClayCost;
            currentState.Obsidian -= robotCost.ObsidianCost;



            currentState.Minute += minutes;
        }
        public static void UpdateStateMats(State currentState, int minutes)
        {
            currentState.Ore += currentState.OreRobots * minutes;
            currentState.Clay += currentState.ClayRobots * minutes;
            currentState.Obsidian += currentState.ObsidianRobots * minutes;
            currentState.Geodes += currentState.GeodeRobots * minutes;

            currentState.Minute += minutes;
        }

        public static State UpdateState(State currentState, Decisions decision, Blueprint blueprint)
        {
            int minutes;
            State updateState;
            switch(decision)
            {
                case Decisions.BuildOreRobot:
                    minutes = Math.Max(0, (blueprint.OreRobot.OreCost - currentState.Ore) / currentState.OreRobots);
                                                                                                                     //required materials / robots for that material + 1 (building the robot) + previous state minutes
                    if (minutes * currentState.OreRobots + currentState.Ore < blueprint.OreRobot.OreCost)            
                        minutes++;
                    minutes++;  //Building the robot
                    updateState = new State(currentState);
                    UpdateStateMats(updateState, minutes, blueprint.OreRobot);
                    updateState.OreRobots++; return updateState;

                case Decisions.BuildClayRobot:
                    minutes = Math.Max(0, (blueprint.ClayRobot.OreCost - currentState.Ore) / currentState.OreRobots);

                    if (minutes * currentState.OreRobots + currentState.Ore < blueprint.ClayRobot.OreCost)
                        minutes++;
                    minutes++;  //Building the robot
                    updateState = new State(currentState);
                    UpdateStateMats(updateState, minutes, blueprint.ClayRobot);
                    updateState.ClayRobots++; return updateState;

                case Decisions.BuildObsidianRobot:
                    minutes = Math.Max(0, Math.Max((blueprint.ObsidianRobot.OreCost - currentState.Ore) / currentState.OreRobots, (blueprint.ObsidianRobot.ClayCost - currentState.Clay) / currentState.ClayRobots));

                    if (minutes * currentState.OreRobots + currentState.Ore < blueprint.ObsidianRobot.OreCost) minutes++;
                    if (minutes * currentState.ClayRobots + currentState.Clay < blueprint.ObsidianRobot.ClayCost) minutes++;
                    minutes++;  //Building the robot
                    updateState = new State(currentState);
                    UpdateStateMats(updateState, minutes, blueprint.ObsidianRobot);
                    updateState.ObsidianRobots++; return updateState;

                case Decisions.BuildGeodeRobot:
                    minutes = Math.Max(0, Math.Max((blueprint.GeodeRobot.OreCost - currentState.Ore) / currentState.OreRobots, (blueprint.GeodeRobot.ObsidianCost - currentState.Obsidian) / currentState.ObsidianRobots));

                    if (minutes * currentState.OreRobots + currentState.Ore < blueprint.GeodeRobot.OreCost) minutes++;
                    if (minutes * currentState.ObsidianRobots + currentState.Obsidian < blueprint.GeodeRobot.ObsidianCost) minutes++;
                    minutes++;  //Building the robot
                    updateState = new State(currentState);
                    UpdateStateMats(updateState, minutes, blueprint.GeodeRobot);
                    updateState.GeodeRobots++; return updateState;
                default: return null;
            }
        }
        public static void Dfs(State currentState, Blueprint blueprint, ref int maxGeodes, int maxMinutes)
        {
            if (currentState.Minute <= maxMinutes - 1)
            {
                var decisions = GetDecisions(currentState, blueprint);
                foreach (var decision in decisions)
                {
                    State updateState = UpdateState(currentState, decision, blueprint);
                    Dfs(updateState, blueprint, ref maxGeodes, maxMinutes);
                }
            }

            UpdateStateMats(currentState, maxMinutes - currentState.Minute);
            if(currentState.Geodes > maxGeodes)
                maxGeodes = currentState.Geodes;
        }

        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            string[] input = System.IO.File.ReadAllLines("input.txt");
            var blueprints = ParseBlueprints(input);
            int qualityLevels = 0;


            for (int i = 0; i < blueprints.Count; i++)
            {
                int maxGeodes = 0;
                var currentState = new State(0, 0, 0, 0, 0, 1, 0, 0, 0);
                Dfs(currentState, blueprints[i], ref maxGeodes, 24);
                qualityLevels += maxGeodes * (i + 1);
            }

            Console.WriteLine(qualityLevels);
            sw.Stop();
            Console.WriteLine();
            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();
        }
    }
}
