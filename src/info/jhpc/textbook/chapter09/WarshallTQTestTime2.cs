
using System;

namespace info.jhpc.textbook.chapter09
{
	public class WarshallTQTestTime2 {
        public static void Main(string[] args) {
            if (args.Length < 2) {
                Console.WriteLine("usage: WarshallTQTestTime2 N blksize");
                Environment.Exit(0);
            }
            int N = Convert.ToInt32(args[0]);
            int bsize = Convert.ToInt32(args[1]);
            int i, j;
            double probTrue = 0.3;
            bool[][] a = new bool[N][];

            WarshallTQ w = new WarshallTQ(bsize);

            Random rand = new Random();
            for (i = 0; i < N; i++) {
            	a[i]=new bool[N];
            	
                for (j = 0; j < N; j++) {
                    a[i][j] = (rand.NextDouble() <= probTrue);
                }
            }
            long start = DateTime.Now.Ticks;
            w.closure(a);
            Console.WriteLine("WarshallTQ\t" + N + "\t" + bsize + "\t" +
                    (DateTime.Now.Ticks - start));
        }
    }
}

