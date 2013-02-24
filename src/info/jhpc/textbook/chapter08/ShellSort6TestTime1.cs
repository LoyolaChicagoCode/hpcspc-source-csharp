
using System;

namespace info.jhpc.textbook.chapter08
{
	public class Chapter08Time {
        public static void Main(string[] args) {
            if (args.Length < 2) {
                Console.WriteLine("Usage: ShellSort6TestTime1 n nt");
                Environment.Exit(0);
            }
            int N = Convert.ToInt32(args[0]);
            int T = Convert.ToInt32(args[1]);
            int[] a = new int[N];
            int i;
            long time;
        	Random rand=new Random();
        	
            for (i = a.Length - 1; i >= 0; i--) {
                a[i] = (int) (rand.NextDouble() * N);
            }
            //for (i=a.Length-1;i>=0;i--) {
            //	Console.Write(" "+a[i]);
            //}
            //Console.WriteLine();
            ShellSort6 s = new ShellSort6(T);
            time = DateTime.Now.Ticks;
            s.sort(a);
            time = DateTime.Now.Ticks - time;
            //for (i=a.Length-1;i>=0;i--) {
            //	Console.Write(" "+a[i]);
            //}
            //Console.WriteLine();
            Console.WriteLine("ShellSort6\t" + N + "\t" + T + "\t" + time);
        }
    }
}
