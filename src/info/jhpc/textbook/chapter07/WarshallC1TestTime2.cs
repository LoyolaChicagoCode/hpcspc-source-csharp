
using System;

namespace info.jhpc.textbook.chapter07
{
	public class WarshallC1TestTime2 {
        public static void Main(string[] args) {
            if (args.Length < 1) {
                Console.WriteLine("usage: WarshallC1TestTime2 N");
                Environment.Exit(0);
            }
            int N = Convert.ToInt32(args[0]);
            int i, j;
            double probTrue = 0.3;
            bool[][] a = new bool[N][];

            WarshallC1 w = new WarshallC1();

            Random rand = new Random();
            for (i = 0; i < N; i++) {
            	a[i]=new bool[N];
            	
                for (j = 0; j < N; j++) {
                    a[i][j] = (rand.NextDouble() <= probTrue);
                }
            }
            long start = DateTime.Now.Ticks;
            a = w.closure(a);
            Console.WriteLine("WarshallC1\t" + N + "\t" +
                    (DateTime.Now.Ticks - start));
        }
    }
}
