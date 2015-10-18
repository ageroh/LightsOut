using System;
using System.Collections.Generic;
using System.Linq;

namespace LightsOut
{

    public enum Switch
    {
        Off = 0, On
    }

    /// <summary>
    /// We create the class for main logic here. An array of NxN items is firstly initialized with elements
    /// of Cell. It's Cell represents a light in the NxN array of lights. This class may handle a NxN implementation 
    /// but for now we stand for a 5x5 matrix. After the matrix is initialized with Cells, a next iteration occurs 
    /// for every light in order to match its adjacent lights (top, left, right, bottom). At last a graph with nodes of Cell
    /// is the result of the initialization and mapping, ready to provide the set for the algorithm of random generation of 
    /// solution for the game. RandomMatrixGeneration() provides the first random solution for the game. This means that at least one 
    /// solution could be found, but not least.
    /// 
    /// </summary>
    public class LightsOut
    {
        private Light[,] LightsArray { get; set; }
        private int N { get; set; }
        public LightsOut(int N)
        {
            this.N = N;
            LightsArray = new Light[N, N];
            Init();
            ConnectNodes();
        }

        private void ConnectNodes()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    LightsArray[i,j].Top = getTop(i, j);
                    LightsArray[i,j].Bottom = getBottom(i, j);
                    LightsArray[i,j].Left = getLeft(i, j);
                    LightsArray[i,j].Right = getRight(i, j);
                }
            }

        }

        protected Light getRight(int i, int j)
        {
            if (j == N - 1)
                return null;    // last column does not have Right adjacent light

            if (j >= N)
                throw new InvalidOperationException("Not permited node on Right");    // safety check

            return LightsArray[i, j + 1];
        }

        protected Light getLeft(int i, int j)
        {
            if (j == 0)
                return null;    // 1st column does not have Left adjacent light
            if (j >= N)
                throw new InvalidOperationException("Not permited node on Left");    // safety check

            return LightsArray[i, j - 1];
        }

        protected Light getBottom(int i, int j)
        {
            if (i == N - 1)
                return null;    // last row does not have botton adjacent light

            if (i >= N)
                throw new InvalidOperationException("Not permited node on Bottom");    // safety check

            return LightsArray[i + 1, j];
        }

        protected Light getTop(int i, int j)
        {
            if (i == 0)
                return null;    // first row does not have top adjacent buttons
            if (i >= N)
                throw new InvalidOperationException("Not permited node on Top");    // safety check

            return LightsArray[i - 1, j];
        }

        private void Init()
        {

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    LightsArray[i, j] = new Light()
                    {
                        Switch = Switch.Off,
                        Row = i,
                        Column = j,
                    };
                }
            }
        }
        
        private Light GetLight(int row, int column)
        {
            if (row < 0 || row >= N || column < 0 || column >= N) // safety check for boundaries of array.
                return null;

            return LightsArray[row, column];
        }


        public Light ToggleLight(int row, int column)
        {
            Light light = GetLight(row, column);

            // toggle light self      
            ToggleLight(light);

            // toggle light adjacents
            ToggleLightAdjecent(light.Top);
            ToggleLightAdjecent(light.Bottom);
            ToggleLightAdjecent(light.Left);
            ToggleLightAdjecent(light.Right);
            
            return light;
        }

        private void ToggleLight(Light light)
        {
            light.Switch = (light.Switch == Switch.Off ? Switch.On : Switch.Off);
        }

        private void ToggleLightAdjecent(Light light)
        {
            if (light != null)
            {
                ToggleLight(light);
            }
        }


        public Light RandomMatrixGeneration(List<Light> listRandomFirstCell)
        {
            // start light from random cell
            Random rnd = new Random();
            int x = rnd.Next(N);
            int y = rnd.Next(N);

            // find one random light 
            Light light = GetLight(x, y);
            ToggleLight(light);

            listRandomFirstCell.Add(GetLight(x, y));

            // do the tries.
            // ...
            //...

            return listRandomFirstCell.LastOrDefault();

        }

        internal bool IsSolved()
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    if (LightsArray[i, j].Switch == Switch.On)
                        return false;

            return true;
        }

        internal void ClearGame()
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    LightsArray[i, j].Switch = Switch.Off;
        }
    }
}
