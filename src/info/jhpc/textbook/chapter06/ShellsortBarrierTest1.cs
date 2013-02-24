
using System;

namespace info.jhpc.textbook.chapter06
{
	public class ShellsortBarrierTest1 {
        public static void Main(string[] args) {
            int[] a = new int[20];
            int i;
        	Random rand=new Random();
            for (i = a.Length - 1; i >= 0; i--) {
                a[i] = (int) (rand.NextDouble() * 100);
            }
            for (i = a.Length - 1; i >= 0; i--) {
                Console.Write(" " + a[i]);
            }
            Console.WriteLine();
            ShellsortBarrier s = new ShellsortBarrier(3);
            s.sort(a);
            for (i = a.Length - 1; i >= 0; i--) {
                Console.Write(" " + a[i]);
            }
            Console.WriteLine();
        }
    }	
}
