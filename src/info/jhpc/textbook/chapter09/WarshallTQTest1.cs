
using System;

namespace info.jhpc.textbook.chapter09
{
	public class WarshallTQTest1 {
        public static void Main(string[] args) {
            int N = 10;
            int bsize = 3;
            int i;
            bool[][] a = new bool[N][];
        	
        	for(i=0; i < N; i++)
        		a[i]=new bool[N];
        	
            WarshallTQ w = new WarshallTQ(bsize);
            for (i = 0; i < N; i++) a[i][(i + 1) % N] = true;
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
