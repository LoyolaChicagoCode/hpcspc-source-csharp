
using System;

namespace info.jhpc.textbook.chapter06
{
	public class Warshall1TestTime2 {
        public static void Main(string[] args) {
            if (args.Length < 2) {
                Console.WriteLine("Usage: Warshall1.TestTime2 N nt");
                Environment.Exit(0);
            }
            int N = Convert.ToInt32(args[0]);
            int nt = Convert.ToInt32(args[1]);
            int i, j;
            double probTrue = 0.3;
            bool[,] a = new bool[N,N];

            Warshall1 w = new Warshall1(nt,N);

            Random rand = new Random();
            for (i = 0; i < N; i++) {
                for (j = 0; j < N; j++) {
                    a[i,j] = (rand.NextDouble() <= probTrue);
                }
            }
            long start = DateTime.Now.Ticks;
            w.closure(a);
            Console.WriteLine("Warshall1\t" + N + "\t" + nt + "\t" +
                    (DateTime.Now.Ticks - start));
        }
    }
}
