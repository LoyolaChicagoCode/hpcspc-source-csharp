
using System;

namespace info.jhpc.textbook.chapter06
{
	public class Warshall2Test1 {
        public static void Main(string[] args) {
            int N = 10;
            int nt = 3;
            int i;
            bool[,] a = new bool[N,N];
            Warshall2 w = new Warshall2(nt,N);
            for (i = 0; i < N; i++) a[i,(i + 1) % N] = true;
            show(a,N);
            Console.WriteLine();
            w.closure(a,N);
            show(a,N);
        }

        static void show(bool[,] a, int size) {
            int i, j;
            for (i = 0; i < size; i++) {
                for (j = 0; j < size; j++) {
                    Console.Write(a[i,j] ? '1' : '0');
                }
                Console.WriteLine();
            }
        }
    }
}
