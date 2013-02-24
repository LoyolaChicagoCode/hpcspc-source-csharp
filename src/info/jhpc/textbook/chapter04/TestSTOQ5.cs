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
//one thread will write strings into a single queue
//another will remove them and check their order
//  they will be removed using a getSkip loop

using System;
using System.Threading;
using info.jhpc.thread;

namespace info.jhpc.textbook.chapter04
{
	class TestSTOQ5 {
	    const int maxMsg = 1000;
	    static SharedTableOfQueues stoq = new SharedTableOfQueues();
		int myId, next;
	    bool receiver=true;
	
	
	    public static void Main(string[] x) {
	    	TestSTOQ5 test;
	    	
	        Console.WriteLine("one thread will write strings into an array");
	        Console.WriteLine(" (one queue per element)");
	        Console.WriteLine("another will remove them and check their order");
	        Console.WriteLine("  they will be removed using a getSkip loop");
	
			test=new TestSTOQ5(true);
	        (new Thread(new ThreadStart(test.run))).Start();
	        test=new TestSTOQ5(false);
	        (new Thread(new ThreadStart(test.run))).Start();
	    }
	
		
		TestSTOQ5(bool Ireceive) {
	        receiver = Ireceive;
	    }
	
	
	    public void run() {
	        try {
	            string s, nextstr;
	            next = 0;
	            while (next <= maxMsg) {
	                nextstr = "" + next;
	                if (receiver) {
	                    while ((s = (string) stoq.get("queue")) == null) ;
	                    if (s != nextstr)
	                        Console.WriteLine("received " + s + " not " + nextstr);
	                } else
	                    stoq.put("queue", nextstr);
	                next++;
	            }
	            Console.WriteLine((receiver ? "receiver " : "sender ") + " done");
	        } catch (ThreadInterruptedException ie) { }
	    }
	}
}
