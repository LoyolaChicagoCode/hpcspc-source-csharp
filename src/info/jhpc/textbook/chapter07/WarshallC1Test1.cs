
using System;

namespace info.jhpc.textbook.chapter07
{
	public class WarshallC1Test1 {
        public static void Main(string[] args) {
            int N = 10;
            int i;
            bool[][] a = new bool[N][];
            WarshallC1 w = new WarshallC1();
        	
        	for(i=0; i < N; i++)
        		a[i]=new bool[N];
        	
            for (i = 0; i < N; i++) 	
   				a[i][(i + 1) % N] = true;
            
            show(a);
            Console.WriteLine();
            a = w.closure(a);
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
