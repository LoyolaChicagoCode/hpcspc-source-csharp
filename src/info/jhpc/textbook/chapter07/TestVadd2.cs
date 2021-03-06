

using System;
using System.Threading;
using info.jhpc.thread;

namespace info.jhpc.textbook.chapter07
{
	public class TestVadd2 {
	    public static void Main(string[] args) {
	        double[] x = {1.0, 2.0, 3.0};
	        double[] y = {4.0, 5.0, 6.0};
	        DFFuture1 f1 = new DFFuture1();
	        DFFuture1 f2 = new DFFuture1();
	        DFFuture1 f3 = new DFFuture1();
	        Binop2 bop = new Binop2(new Vadd(f3));
	    	Fetch fetch=new Fetch(f1, new Store(bop, 0));
	        f1.runDelayed(new ThreadStart(fetch.run));
	    	fetch=new Fetch(f2, new Store(bop, 1));
	        f2.runDelayed(new ThreadStart(fetch.run));
	        f1.setValue(x);
	        f2.setValue(y);
	        double[] z = (double[]) f3.getValue();
	        for (int i = 0; i < z.Length; ++i)
	            Console.Write(z[i] + " ");
	        Console.WriteLine();
	        Future.getClassRunQueue().setMaxThreadsWaiting(0);
	    }
	}
}
