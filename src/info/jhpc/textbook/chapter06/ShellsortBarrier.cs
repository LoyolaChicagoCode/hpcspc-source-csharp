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
using System.Threading;
using info.jhpc.thread;


namespace info.jhpc.textbook.chapter06
{
	public class ShellsortBarrier {
	    static int minDivisible = 3;
	    static int numThreads;
	
	    public ShellsortBarrier(int threads) {
	        numThreads = threads;
	    }
	
	    internal class Sort {
	        int[] a;
	        int i, h;
	        SimpleBarrier b;
	
	        public Sort(int[] a, int i, int h, SimpleBarrier b) {
	            this.a = a;
	            this.i = i;
	            this.h = h;
	            this.b = b;
	        }
	
	        public void run() {
	            try {
	                while (h > 0) {
	                    if (h == 2) h = 1;
	                    for (int m = i; m < h; m += numThreads) {
	                        isort(a, i, h);
	                    }
	                    h = (int) (h / 2.2);
	                    b.gather();
	                }
	            } catch (Exception ex) {
	            }
	        }
	    }
	
	    static void isort(int[] a, int m, int h) {
	        int i, j;
	        for (j = m + h; j < a.Length; j += h) {
	            for (i = j; i > m && a[i] > a[i - h]; i -= h) {
	                int tmp = a[i];
	                a[i] = a[i - h];
	                a[i - h] = tmp;
	            }
	        }
	    }
	
	    public void sort(int[] a) {
	        if (a.Length < minDivisible) {
	            isort(a, 0, 1);
	            return;
	        }
	        Sort sort=null;
	        SimpleBarrier b = new SimpleBarrier(numThreads);
	    	
	    	for (int i = numThreads - 1; i > 0; i--) {
	    		sort=new Sort(a, i, a.Length / minDivisible, b);
	            (new Thread(new ThreadStart(sort.run))).Start();
	    	}
	    	
	        (new Sort(a, 0, a.Length / minDivisible, b)).run();
	    }
	}
}
