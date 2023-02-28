using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace AdventOfCode_8
{
    class Program
    {
        class Tree
        {
            bool leftVisible;
            bool rightVisible;
            bool upVisible;
            bool downVisible;
            char height;
            int row;
            int column;

            public Tree()
            {
                this.leftVisible = true;
                this.rightVisible = true;
                this.upVisible = true;
                this.downVisible = true;
                this.height = '0';
                this.row = 0;
                this.column = 0;   
            }
            public Tree(char height, int row, int column)
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
            List<Tree> allTrees = new List<Tree>();
            Tree currTree = new Tree();
            int sumOfVisibleTrees = 0;

            for (int i = 0; i < rows[0].Length; i++)
            {
                columns.Add("");
                int j = 0;
                foreach (string row in rows)
                {
                    allTrees.Add(new Tree(row[i], j, i));
                    columns[i] += row[i];
                    j++;
                }
            }
            int borderTrees = 0;

            for (int i = 0; i < allTrees.Count; i++)
            {
                currTree = allTrees[i];
                char currHeight = currTree.get_height();
                int currRow = currTree.get_row();
                int currColumn = currTree.get_column();

                if(currRow == 0 || currColumn == 0 || currRow == rows.Count - 1 || currColumn == columns.Count - 1)
                {
                    borderTrees++;
                    continue;
                }

                for (int rowIndex = currColumn - 1; rowIndex >= 0; rowIndex--)
                {
                    if (currHeight <= rows[currRow][rowIndex])
                    {
                        currTree.set_leftVisible(false);
                        break;
                    }
                }
                for (int rowIndex = currColumn + 1; rowIndex < rows.Count; rowIndex++)
                {
                    if (currHeight <= rows[currRow][rowIndex])
                    {
                        currTree.set_rightVisible(false);
                        break;
                    }
                }
                for (int columnIndex = currRow - 1; columnIndex >= 0; columnIndex--)
                {
                    if (currHeight <= columns[currColumn][columnIndex])
                    {
                        currTree.set_upVisible(false);
                        break;
                    }
                }
                for (int columnIndex = currRow + 1; columnIndex < columns.Count; columnIndex++)
                {
                    if (currHeight <= columns[currColumn][columnIndex])
                    {
                        currTree.set_downVisible(false);
                        break;
                    }
                }
            }
            foreach(Tree t in allTrees)
            {
                if (!t.get_visible())
                    sumOfVisibleTrees++;
            }
            Console.WriteLine(rows.Count * columns.Count - sumOfVisibleTrees);
            Console.ReadKey();
        }
    }
}
