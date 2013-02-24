
using System;

namespace info.jhpc.textbook.chapter06
{
	public class ShellsortBarrierTestTime1 {
        public static void Main(string[] args) {
            if (args.Length < 2) {
                Console.WriteLine("Usage: ShellsortBarrierTestTime1 N T");
                Environment.Exit(0);
            }
            Random rand=new Random();
            int N = Convert.ToInt32(args[0]);
            int T = Convert.ToInt32(args[1]);
            int[] a = new int[N];
            int i;
            long time;
            for (i = a.Length - 1; i >= 0; i--) {
                a[i] = (int) (rand.Next() * N);
            }
           
            ShellsortBarrier s = new ShellsortBarrier(T);
            time = DateTime.Now.Ticks;
            s.sort(a);
            time = DateTime.Now.Ticks - time;
            Console.WriteLine("ShellsortBarrier\t" + N + "\t" + T + "\t" + time);
        }
    }	
}

