
using System;

namespace info.jhpc.textbook.chapter08
{
	public class Test1Chapter08 {
        public static void Main(string[] args) {
            int[] a = new int[25];
            int i;
        	Random rand=new Random();
        	
            for (i = a.Length - 1; i >= 0; i--) {
                a[i] = (int) (rand.NextDouble() * 100);
            }
            for (i = 0; i < a.Length - 1; i++) {
                Console.Write(" " + a[i]);
            }
            Console.WriteLine();
            ParQuickSort2 s = new ParQuickSort2(3);
            s.sort(a);
            for (i = 0; i < a.Length - 1; i++) {
                Console.Write(" " + a[i]);
            }
            Console.WriteLine();
        }
    }
}
