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
        private Light[,] LightsMatrix { get; set; }
        private int N { get; set; }
        public LightsOut(int N)
        {
            this.N = N;
            LightsMatrix = new Light[N, N];
            Init();
            ConnectNodes();
            cnt = 0;
        }

        private void ConnectNodes()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    LightsMatrix[i,j].Top = getTop(i, j);
                    LightsMatrix[i,j].Bottom = getBottom(i, j);
                    LightsMatrix[i,j].Left = getLeft(i, j);
                    LightsMatrix[i,j].Right = getRight(i, j);
                }
            }

        }

        protected Light getRight(int i, int j)
        {
            if (j == N - 1)
                return null;    // last column does not have Right adjacent light

            if (j >= N)
                throw new InvalidOperationException("Not permited node on Right");    // safety check

            return LightsMatrix[i, j + 1];
        }

        protected Light getLeft(int i, int j)
        {
            if (j == 0)
                return null;    // 1st column does not have Left adjacent light
            if (j >= N)
                throw new InvalidOperationException("Not permited node on Left");    // safety check

            return LightsMatrix[i, j - 1];
        }

        protected Light getBottom(int i, int j)
        {
            if (i == N - 1)
                return null;    // last row does not have botton adjacent light

            if (i >= N)
                throw new InvalidOperationException("Not permited node on Bottom");    // safety check

            return LightsMatrix[i + 1, j];
        }

        protected Light getTop(int i, int j)
        {
            if (i == 0)
                return null;    // first row does not have top adjacent buttons
            if (i >= N)
                throw new InvalidOperationException("Not permited node on Top");    // safety check

            return LightsMatrix[i - 1, j];
        }

        private void Init()
        {

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    LightsMatrix[i, j] = new Light()
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

            return LightsMatrix[row, column];
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

        public Light[,] GetMatrix()
        {
            return this.LightsMatrix;
        }

        private int cnt { get; set; }

        /// <summary>
        /// For given NxN Matrix after initialization, random matrix is generated
        /// Polyonimial algorithm for its solution is invoked, and if path for soltion is found then matrix is given as solution.
        /// </summary>
        /// <param name="listRandomFirstCell"></param>
        /// <returns></returns>
        public bool Startup(Dictionary<int, Light[,]> notSolvedMatrixes)
        {
            // risky but always there is a solution..
            //if (notSolvedMatrixes == null)
            //    return false;
            
            Light[,] randomGenerated = GenerateRandomMatrix();
                
            //// if a solution is not provided previously with this matrix, then try another
            //if(notSolvedMatrixes.ContainsKey(randomGenerated.GetHashCode()))
            //{
            //    notSolvedMatrixes.Add(randomGenerated.GetHashCode(), randomGenerated);
            //    Startup(notSolvedMatrixes);
            //    return false;
            //}
                
            if (SolveLightsOut(randomGenerated, N))
            {
                return true;
            }

            //notSolvedMatrixes.Add(randomGenerated.GetHashCode(), randomGenerated);
            Startup(notSolvedMatrixes);
            return false;
        }

        //compare two Martix of lights
        private bool CompareMatrixes(Light[,] firtsdMatrix, Light[,] secondMatrix)
        {
            for(int i = 0; i< N; i++)
                for (int j = 0; j < N; j++)
                    if(firtsdMatrix[i,j] != secondMatrix[i,j])
                        return false;
            return true;
        }
        
        private static bool SolveLightsOut(Light[,] LightsArray, int N)
        {
            LightsOutSolver.InitLightsOutSolver(N);

            return LightsOutSolver.solve(ConvertLightsToInt(LightsArray, N));
        }

        private static int[][] ConvertLightsToInt(Light[,] lightsArray, int N)
        {
            int[][] arr = new int[N][];
            for (int i = 0; i < N; i++)
                arr[i] = new int[N];

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    arr[i][j] = (lightsArray[i, j].Switch == Switch.Off ? 0 : 1);

            return arr;
        }

        private Light[,] GenerateRandomMatrix()
        {
            Random rnd = new Random();
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    // NextDouble is fast enough
                    this.LightsMatrix[i, j].Switch = (rnd.NextDouble() > 0.5 ? Switch.Off : Switch.On); 
                }

            return LightsMatrix;
        }

        internal bool IsSolved()
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    if (LightsMatrix[i, j].Switch == Switch.On)
                        return false;

            return true;
        }

        internal void ClearGame()
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    LightsMatrix[i, j].Switch = Switch.Off;
        }
    }
}
