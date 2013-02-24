
using System;

namespace info.jhpc.textbook.chapter09
{
	public class WarshallTQTest2 {
        public static void Main(string[] args) {
            int N = 10;
            int bsize = 3;
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
            show(a);
            Console.WriteLine();
            w.closure(a);
            show(a);
        }

        static void show(bool[][] a) {
            int i, j;
            for (i = 0; i < a.Length; i++) {
                for (j = 0; j < a.Length; j++) {
                    Console.Write(a[i][j] ? '1' : '0');
                }
                Console.WriteLine();
            }
        }
    }
}
