
using System;	
using System.Threading;
using info.jhpc.thread;

namespace info.jhpc.textbook.chapter08
{
	public class ParQuickSort2 {
	    int numThreads;
	    static int minDivisible = 8;
	
	    internal class QuickSortThread2 {
	        int[] ary;
	    	int m, n;
	        TerminationGroup tg;
	        RunQueue rq;
	
	        public QuickSortThread2(int[] ary, int mm, int nn,
	                                TerminationGroup t, RunQueue rq) {
	            this.ary = ary;
	            m = mm;
	            n = nn;
	            tg = t;
	            this.rq = rq;
	        }
	
	        public void run() {
	            quicksort(m, n);
	            tg.terminate();
	        }
	
	        private void quicksort(int m, int n) {
	            //Console.WriteLine("quicksort("+m+","+n+")");
	            int i, j, pivot, tmp;
	            if (n - m < minDivisible) {
	                for (j = m + 1; j < n; j++) {
	                    for (i = j; i > m && ary[i] < ary[i - 1]; i--) {
	                        tmp = ary[i];
	                        ary[i] = ary[i - 1];
	                        ary[i - 1] = tmp;
	                    }
	                }
	                return;
	            }
	            i = m;
	            j = n;
	            pivot = ary[i];
	            while (i < j) {
	                j--;
	                while (pivot < ary[j]) j--;
	                if (j <= i) break;
	                tmp = ary[i];
	                ary[i] = ary[j];
	                ary[j] = tmp;
	                i++;
	                while (pivot > ary[i]) i++;
	                tmp = ary[i];
	                ary[i] = ary[j];
	                ary[j] = tmp;
	            }
	           	QuickSortThread2 subsort;
	            if (i - m > n - i) {
	                subsort = new QuickSortThread2(ary, m, i, tg.fork(), rq);
	                rq.run(new ThreadStart(subsort.run));
	                quicksort(i + 1, n);
	            } else {
	                subsort = new QuickSortThread2(ary, i + 1, n, tg.fork(), rq);
	                rq.run(new ThreadStart(subsort.run));
	                quicksort(m, i);
	            }
	        }
	    }
	
	    public ParQuickSort2(int numThreads) {
	        this.numThreads = numThreads;
	    }
	
	    public void sort(int[] ary) {
	        int N = ary.Length;
	        //Console.WriteLine("sort()");
	        TerminationGroup terminationGroup;
	        RunQueue rq = new RunQueue();
	        FutureFactory ff = new FutureFactory(rq);
	        TerminationGroupFactory tgf = new SharedTerminationGroupFactory(ff);
	        QuickSortThread2 subsort;
	        rq.setMaxThreadsCreated(numThreads);
	        terminationGroup = tgf.make();
	        subsort = new QuickSortThread2(ary, 0, N, terminationGroup, rq);
	        rq.run(new ThreadStart(subsort.run));
	        try {
	            terminationGroup.awaitTermination();
	        } catch (ThreadInterruptedException e) {
	        }
	        rq.setMaxThreadsWaiting(0);
	    }
	}
}
