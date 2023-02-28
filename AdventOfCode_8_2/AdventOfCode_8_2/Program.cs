using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace AdventOfCode_8
{
    class Program
    {
        class tree
        {
            bool leftVisible;
            bool rightVisible;
            bool upVisible;
            bool downVisible;
            char height;
            int row;
            int column;

            public tree()
            {
                this.leftVisible = true;
                this.rightVisible = true;
                this.upVisible = true;
                this.downVisible = true;
                this.height = '0';
                this.row = 0;
                this.column = 0;
            }
            public tree(char height, int row, int column)
            {
                this.leftVisible = true;
                this.rightVisible = true;
                this.upVisible = true;
                this.downVisible = true;
                this.height = height;
                this.row = row;
                this.column = column;
            }

            public void set_leftVisible(bool visible)
            {
                this.leftVisible = visible;
            }
            public void set_rightVisible(bool visible)
            {
                this.rightVisible = visible;
            }
            public void set_upVisible(bool visible)
            {
                this.upVisible = visible;
            }
            public void set_downVisible(bool visible)
            {
                this.downVisible = visible;
            }

            public int get_column()
            {
                return this.column;
            }
            public int get_row()
            {
                return this.row;
            }
            public char get_height()
            {
                return this.height;
            }

            public bool get_visible()
            {
                if (!rightVisible && !leftVisible && !upVisible && !downVisible)
                    return false;
                else
                    return true;
            }
        }
        static void Main(string[] args)
        {
            List<string> rows = File.ReadAllLines("input.txt").ToList();
            List<string> columns = new List<string>();
            List<tree> allTrees = new List<tree>();
            tree currentTree = new tree();
            int sumOfVisibleTrees = 0;
            int highestScenicScore = 0;
            int visibleLeft = 0;
            int visibleRight = 0;
            int visibleDown = 0;
            int visibleUp = 0;

            for (int i = 0; i < rows[0].Length; i++)
            {
                columns.Add("");
                int j = 0;
                foreach (string row in rows)
                {
                    allTrees.Add(new tree(row[i], j, i));
                    columns[i] += row[i];
                    j++;
                }
            }

            for (int i = 0; i < allTrees.Count; i++)
            {
                currentTree = allTrees[i];
                char currentHeight = currentTree.get_height();
                int currentRow = currentTree.get_row();
                int currentColumn = currentTree.get_column();
                


                visibleLeft = 0;
                visibleRight = 0;
                visibleDown = 0;
                visibleUp = 0;
                for (int rowIndex = currentColumn - 1; rowIndex >= 0; rowIndex--)
                {
                    if (currentHeight <= rows[currentRow][rowIndex])
                    {
                        visibleLeft++;
                        break;
                    }
                    visibleLeft++;

                }
                for (int rowIndex = currentColumn + 1; rowIndex < rows.Count; rowIndex++)
                {
                    if (currentHeight <= rows[currentRow][rowIndex])
                    {
                        visibleRight++;
                        break;
                    }
                    visibleRight++;
                }
                for (int columnIndex = currentRow - 1; columnIndex >= 0; columnIndex--)
                {
                    if (currentHeight <= columns[currentColumn][columnIndex])
                    {
                        visibleUp++;
                        break;
                    }
                    visibleUp++;
                }
                for (int columnIndex = currentRow + 1; columnIndex < columns.Count; columnIndex++)
                {
                    if (currentHeight <= columns[currentColumn][columnIndex])
                    {
                        visibleDown++;
                        break;
                    }
                    visibleDown++;
                }
                if (highestScenicScore < visibleDown * visibleLeft * visibleRight * visibleUp)
                    highestScenicScore = visibleDown * visibleLeft * visibleRight * visibleUp;
            }

            Console.WriteLine(highestScenicScore);
            Console.ReadKey();
        }
    }
}
