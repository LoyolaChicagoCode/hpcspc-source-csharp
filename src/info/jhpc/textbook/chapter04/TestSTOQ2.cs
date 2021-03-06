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
//Test SharedTableOfQueues
//create a bunch of threads to put strings into and take them out of queues

using System;
using System.Threading;


namespace info.jhpc.textbook.chapter04
{
	class TestSTOQ2 {
	    const int numSenders = 2, numReceivers = 3, maxMsg = 1000;
	    static SharedTableOfQueues stoq = new SharedTableOfQueues();
	    int myId, next, step;
	    bool receiver=false;
	
	    public static void Main(string[] x) {
	        int i;
	    	TestSTOQ2 test;
	    	
	        Console.WriteLine("create a bunch of threads to put strings into");
	        Console.WriteLine(" and take them out of an array");
	        Console.WriteLine(" one queue per array element");
	        Console.WriteLine(" use get to receive");
	        
	        for (i = 0; i < numSenders; i++) {
            	test=new TestSTOQ2(i, numSenders, false);
           		(new Thread(new ThreadStart(test.run))).Start();
	        }
	        
	        for (i = 0; i < numReceivers; i++) {
	         	test=new TestSTOQ2(i, numReceivers, true);
	            (new Thread(new ThreadStart(test.run))).Start();
	        }
	    }
	
	
		TestSTOQ2(int me, int stride, bool Ireceive) {
	        myId = next = me;
	        step = stride;
	        receiver = Ireceive;
	    }
	    
	    
		public void run() {
	        try {
	            while (next <= maxMsg) {
	                if (receiver)
	                    stoq.get("" + next);
	                else
	                    stoq.put("" + next, "" + next);
	                next += step;
	            }
	            Console.WriteLine((receiver ? "receiver " : "sender ") + myId + " done");
	        } catch (ThreadInterruptedException ie) { }
	    }
	}
}
