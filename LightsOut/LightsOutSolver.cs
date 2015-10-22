using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightsOut
{
    /// <summary>
    /// The algorithm provides a solution for a Matrix of NxN table, for the Lights out problem
    /// Its been provided a matrix with the a proposed possible solution in solve(). If one is found
    /// then this previously provided is given for test to UI.
    /// 
    /// </summary>
    public static class LightsOutSolver
    {
        private static int[][] mat { get; set; }
        private static int [][] cells { get; set; }
        private static int[] cols { get; set; }
        public static int imgcount { get; private set; }

        private static int m;      
        private static int n;      
        private static int np;     
        private static int r;      
        private static int maxr;   
        private static int N;

        public static void InitLightsOutSolver(int n)
        {
            N = n;
        }

        static int a(int i, int j) { return mat[i][cols[j]]; }
        static void setmat(int i, int j, int val) { mat[i][cols[j]] = modulate(val); }

        public static bool solve(int[][] allcells)
        {
            int col;
            int row;
            cells = allcells;
            for (int goal = 0; goal < N; goal++)
            {
                if (solveProblem(goal))
                { // found an integer solution
                    int[] anscols = new int[N*N];
                    int j;
                    for (j = 0; j < n; j++) anscols[cols[j]] = j;
                    for (col = 0; col < N; col++)
                        for (row = 0; row < N; row++)
                        {
                            int value;
                            j = anscols[row * N + col];
                            if (j < r) value = a(j, n); else value = 0;
                            
                        }
                    return true;
                }
            }

            return false;
        }
        

        static void initMatrix()
        {
            maxr = Math.Min(m, n);
            mat = new int[N*N][];
            for (int i = 0; i < N*N; i++)
                mat[i] = new int[N*N];

            for (int col = 0; col < N; col++)
                for (int row = 0; row < N; row++)
                {
                    var i = row * N + col;
                    int[] line = new int[N*N];
                    mat[i] = line;
                    for (int j = 0; j < n; j++) line[j] = 0;
                    line[i] = 1;
                    if (col > 0) line[i - 1] = 1;
                    if (row > 0) line[i - N] = 1;
                    if (col < N - 1) line[i + 1] = 1;
                    if (row < N - 1) line[i + N] = 1;
                }
            cols = new int[N * N];
            for (int j = 0; j < np; j++) cols[j] = j;
        }
        static bool solveProblem(int goal)
        {
            int size = N;
            m = size;
            n = size;
            np = n + 1;
            initMatrix();
            for (int col = 0; col < N; col++)
                for (int row = 0; row < N; row++)
                    mat[row * N + col][n] = modulate(goal - cells[col][row]);
            return sweep();
        }
        static bool sweep()
        {
            for (r = 0; r < maxr; r++)
            {
                if (!sweepStep()) return false; // failed in founding a solution
                if (r == maxr) break;
            }
            return true; // successfully found a solution
        }
        static bool sweepStep()
        {
            int i;
            int j;
            bool finished = true;
            for (j = r; j < n; j++)
            {
                for (i = r; i < m; i++)
                {
                    int aij = a(i, j);
                    if (aij != 0) finished = false;
                    int inv = invert(aij);
                    if (inv != 0)
                    {
                        for (int jj = r; jj < np; jj++)
                            setmat(i, jj, a(i, jj) * inv);
                        doBasicSweep(i, j);
                        return true;
                    }
                }
            }
            if (finished)
            { // we have: 0x = b (every matrix element is 0)
                maxr = r;   // rank(A) == maxr
                for (j = n; j < np; j++)
                    for (i = r; i < m; i++)
                        if (a(i, j) != 0) return false; // no solution since b != 0
                return true;    // 0x = 0 has solutions including x = 0
            }
            return false;   // failed in finding a solution
        }

        static void swap(int[] array, int x, int y)
        {
            int tmp = array[x];
            array[x] = array[y];
            array[y] = tmp;
        }

        static void swap(int[][] array, int x, int y)
        {
            int[] tmp = array[x];
            array[x] = array[y];
            array[y] = tmp;
        }

        static void doBasicSweep(int pivoti, int pivotj)
        {
            if (r != pivoti) swap(mat, r, pivoti);
            if (r != pivotj) swap(cols, r, pivotj);
            for (int i = 0; i < m; i++)
            {
                if (i != r)
                {
                    int air = a(i, r);
                    if (air != 0)
                        for (int j = r; j < np; j++)
                            setmat(i, j, a(i, j) - a(r, j) * air);
                }
            }
        }

        static int modulate(int x)
        {
            // returns z such that 0 <= z < N and x == z (mod N)
            if (x >= 0) return x % N;
            x = (-x) % N;
            if (x == 0) return 0;
            return N - x;
        }

        static int invert(int value)
        { // call when: 0 <= value < imgcount
          // returns z such that value * z == 1 (mod imgcount), or 0 if no such z
            if (value <= 1) return value;
            var seed = gcd(value, imgcount);
            if (seed != 1) return 0;
            int a = 1, b = 0, x = value;    // invar: a * value + b * imgcount == x
            int c = 0, d = 1, y = imgcount; // invar: c * value + d * imgcount == y
            while (x > 1)
            {
                int tmp = y / x;
                y -= x * tmp;
                c -= a * tmp;
                d -= b * tmp;
                tmp = a; a = c; c = tmp;
                tmp = b; b = d; d = tmp;
                tmp = x; x = y; y = tmp;
            }
            return a;
        }

        static int gcd(int x, int y)
        { // call when: x >= 0 and y >= 0
            if (y == 0) return x;
            if (x == y) return x;
            if (x > y) x = x % y; // x < y
            while (x > 0)
            {
                y = y % x; // y < x
                if (y == 0) return x;
                x = x % y; // x < y
            }
            return y;
        }



    }
}
