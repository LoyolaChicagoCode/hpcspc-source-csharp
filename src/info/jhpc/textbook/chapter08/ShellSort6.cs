
using System;
using System.Threading;
using info.jhpc.thread;

namespace info.jhpc.textbook.chapter08
{
	public class ShellSort6 {
	    int numThreads = 8;
	    int minDivisible = 16;
	
	    internal class SortPass {
	        static int[] ary;
	    	static int i, k, n;
	        Accumulator finish;
	
	        public SortPass(int[] aray, int I, int K, int N,
	                 RunDelayed start, Accumulator finish) {
	            ary = aray;
	            i = I;
	            k = K;
	            n = N;
	            this.finish = finish;
	            start.runDelayed(new ThreadStart(this.run));
	        }
	
	        public void run() {
	            isort(ary, i, k, n);
	            finish.signal();
	        }
	    }
	
	    internal class IMerge {
	        int[] ary;
	    	int i, k, m, n;
	        Accumulator finish;
	
	        public IMerge(int[] ary, int i, int k, int m, int n,
	               RunDelayed start, Accumulator finish) {
	            this.ary = ary;
	            this.i = i;
	            this.k = k;
	            this.m = m;
	            this.n = n;
	            this.finish = finish;
	            start.runDelayed(new ThreadStart(this.run));
	        }
	
	        public void run() {
	            imerge(ary, i, k, m, n);
	            finish.signal();
	        }
	    }
	
	    int numInSequence(int i, int k, int n) {
	        return (n - i + k - 1) / k;
	    }
	
	    int midpoint(int i, int k, int n) {
	        return i + numInSequence(i, k, n) / 2 * k;
	    }
	
	    void setupSequence(int[] ary, int i, int k, int n,
	                       RunDelayed start, Accumulator finish,
	                       AccumulatorFactory af) {
	        if (numInSequence(i, k, n) <= minDivisible)
	            new SortPass(ary, i, k, n, start, finish);
	        else {
	            Accumulator a = af.make(2);
	            int m = midpoint(i, k, n);
	            setupSequence(ary, i, k, m, start, a, af);
	            setupSequence(ary, m, k, n, start, a, af);
	            new IMerge(ary, i, k, m, n, a, finish);
	        }
	    }
	
	    Accumulator setupPass(int[] ary, RunDelayed start, int k,
	                          AccumulatorFactory af) {
	        Accumulator finish = af.make(k);
	        for (int i = 0; i < k; i++) {
	            setupSequence(ary, i, k, ary.Length, start, finish, af);
	        }
	        return finish;
	    }
	
	    public ShellSort6(int numThreads) {
	        this.numThreads = numThreads;
	    }
	
	    public void sort(int[] a) {
	        int N = a.Length;
	        if (N < minDivisible) {
	            isort(a, 0, 1, N);
	            return;
	        }
	        RunQueue rq = new RunQueue();
	        rq.setMaxThreadsCreated(numThreads);
	        FutureFactory ff = new FutureFactory(rq);
	        AccumulatorFactory af = new AccumulatorFactory(ff);
	        Accumulator waitFor = af.make(1);
	        waitFor.signal();
	        int k, m;
	        k = N / 5;
	        waitFor = setupPass(a, waitFor, k, af);
	        k = N / 7;
	        waitFor = setupPass(a, waitFor, k, af);
	        for (k = (int) (k / 2.2); k > 0; k = (int) (k / 2.2)) {
	            if (k == 2) k = 1;
	            waitFor = setupPass(a, waitFor, k, af);
	        }
	        try {
	            waitFor.getFuture().getValue();
	        } catch (ThreadInterruptedException ie) {
	        }
	        ff.getRunQueue().setMaxThreadsWaiting(0);
	    }
	
	    static void isort(int[] a, int m, int k, int n) {
	        int i, j;
	        for (j = m + k; j < n; j += k) {
	            for (i = j; i >= m + k && a[i] < a[i - k]; i -= k) {
	                int tmp = a[i];
	                a[i] = a[i - k];
	                a[i - k] = tmp;
	            }
	        }
	    }
	
	    static void imerge(int[] a, int m, int k, int mid, int n) {
	        int i, j;
	        for (j = mid; j < n; j += k) {
	            if (a[j] >= a[j - k]) return;
	            for (i = j; i >= m + k && a[i] < a[i - k]; i -= k) {
	                int tmp = a[i];
	                a[i] = a[i - k];
	                a[i - k] = tmp;
	            }
	        }
	    }
	}
}
