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
	//dynamic allocation
	public class Warshall2 {
	    static int numThreads, size;
	
	    public Warshall2(int threads, int s) {
	        numThreads = threads;
	    	size=s;
	    }
	
	    internal class Close {
	        bool[,] a;
	        DynAlloc d;
	        SimpleBarrier b;
	        Accumulator done;
	
	        public Close(bool[,] a, DynAlloc d,
	              SimpleBarrier b, Accumulator done) {
	            this.a = a;
	            this.d = d;
	            this.b = b;
	            this.done = done;
	        }
	
	        public void run() {
	            try {
	                int i, j, k;
	                DynAlloc.Range r = new DynAlloc.Range();
	                for (k = 0; k < size; k++) {
	                    while (d.alloc(r)) {
	                        for (i = r.start; i < r.end; i++) {
	                            if (a[i,k])
	                                for (j = 0; j < size; j++) {
	                                    a[i,j] = a[i,j] | a[k,j];
	                                }
	                        }
	                    }
	                    b.gather();
	                }
	                done.signal();
	            } catch (ThreadInterruptedException ex) {
	            }
	        }
	    }
	
	    public void closure(bool[,] a, int size) {
	        int i;
	    	Close close=null;
	        Accumulator done = new Accumulator(numThreads);
	        SimpleBarrier b = new SimpleBarrier(numThreads);
	        DynAllocShare d = new DynAllocShare(size, numThreads, 2);
	        for (i = 0; i < numThreads; i++) {
	            close=new Close(a, d, b, done);
	        	(new Thread(new ThreadStart(close.run))).Start();
	        }
	        try {
	            done.getFuture().getValue();
	        } catch (ThreadInterruptedException ex) {
	        }
	    }
	}
}
