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
// use SharedTableOfQueues.getSkip to receive

using System;
using System.Threading;
using info.jhpc.thread;

namespace info.jhpc.textbook.chapter04
{

	class TestSTOQ3 {
	    const int numSenders = 2, numReceivers = 3, maxMsg = 1000;
	    static SharedTableOfQueues stoq = new SharedTableOfQueues();
	    int myId, next, step=numSenders;
	    bool receiver=false;
	
	    public static void Main(string[] x) {
	        int i;
	    	TestSTOQ3 test;
	    	
	        Console.WriteLine("Hi!");
	        Console.WriteLine("create a bunch of threads to put strings into");
	        Console.WriteLine(" and take them out of an array");
	        Console.WriteLine(" one queue per array element");
	        Console.WriteLine(" use getSkip to receive");
	    	
	        for (i = 0; i < numSenders; i++) {
	            test = new TestSTOQ3(i, numSenders, false);
	            (new Thread(new ThreadStart(test.run))).Start();
	        }
	        for (i = 0; i < numReceivers; i++) {
	            test = new TestSTOQ3(i, numReceivers, true);
	            (new Thread(new ThreadStart(test.run))).Start();
	        }
	    }
	
	
		TestSTOQ3(int me, int stride, bool Ireceive) {
	        myId = next = me;
	        step = stride;
	        receiver = Ireceive;
	    }
	
	
	    public void run() {
	        while (next <= maxMsg) {
	            if (receiver)
	                while (stoq.getSkip("" + next) == null) ;
	            else
	                stoq.put("" + next, "" + next);
	            next += step;
	        }
	        Console.WriteLine((receiver ? "receiver " : "sender ") + myId + " done");
	    }
	}
}
