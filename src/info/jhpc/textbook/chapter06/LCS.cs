/*
To accompany High-Performance Java Platform(tm) Computing:
Threads and Networking, published by Prentice Hall PTR and
Sun Microsystems Press.

Threads and Networking Library
Copyright (C) 1999-2000
Thomas W. Christopher and George K. Thiruvathukal

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Library General Public
License as published by the Free Software Foundation; either
version 2 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Library General Public License for more details.

You should have received a copy of the GNU Library General Public
License along with this library; if not, write to the
Free Software Foundation, Inc., 59 Temple Place - Suite 330,
Boston, MA  02111-1307, USA.
*/

using System;
using info.jhpc.thread;
using System.Threading;

using Semaphore = info.jhpc.thread.Semaphore;


namespace info.jhpc.textbook.chapter06
{
	public class LCS {
	    int numThreads;
		static int size;
	    static char[] c0;
	    static char[] c1;
	    static int[,] a;
	    static Accumulator done;
	
	    public LCS(char[] C0, char[] C1, int threads) {
	        numThreads = threads;
	        c0 = C0;
	        c1 = C1;
	        int i;
	        done = new Accumulator(numThreads);
	
	        a = new int[c0.Length + 1, c1.Length + 1];
			size=c0.Length+1;
	
	        Semaphore left = new Semaphore(c0.Length), right;
	        for (i = 0; i < numThreads; i++) {
	            right = new Semaphore();
	            Band band=new Band(startOfBand(i, numThreads, c1.Length),
	                    startOfBand(i + 1, numThreads, c1.Length) - 1,
	                    left, right);
	        	(new Thread(new ThreadStart(band.run))).Start();
	            left = right;
	        }
	    }
	
	   
	    internal class Band {
	        int low;
	        int high;
	        Semaphore left, right;
	
	        public Band(int low, int high,
	            Semaphore left, Semaphore right) {
	            this.low = low;
	            this.high = high;
	            this.left = left;
	            this.right = right;
	        }
	
	        public void run() {
	        	
	            try {
	                int i, j;
	           
	                for (i = 1; i < size; i++) {
	                    left.down();
	                    for (j = low; j <= high; j++) {
	                        if (c0[i - 1] == c1[j - 1])
	                            a[i, j] = a[i - 1, j - 1] + 1;
	                        else
	                            a[i, j] = Math.Max(a[i - 1, j], a[i, j - 1]);
	                    }
	                    right.up();
	                }
	                done.signal();
	            } catch (ThreadInterruptedException ex) {
	            }
	        }
	    }
	
	    int startOfBand(int i, int nb, int N) {
	        return 1 + i * (N / nb) + Math.Min(i, N % nb);
	    }
	
	    public int getLength() {
	        try {
	            done.getFuture().getValue();
	        } catch (ThreadInterruptedException ex) {
	        }
	        return a[c0.Length, c1.Length];
	    }
	
	    public int[,] getArray() {
	        try {
	            done.getFuture().getValue();
	        } catch (ThreadInterruptedException ex) {
	        }
	        return a;
	    }
	}
	
	
	public class Test1 {
        public static void Main(string[] args) {
            if (args.Length < 2) {
                Console.WriteLine("Usage: Test1 string0 string1");
                Environment.Exit(0);
            }
            
            int nt = 3;
            int i;

            string s0 = args[0];
            string s1 = args[1];
            Console.WriteLine(s0);
            Console.WriteLine(s1);
            LCS w = new LCS(s0.ToCharArray(), s1.ToCharArray(), nt);
            Console.WriteLine(w.getLength());
        }
    }
}
