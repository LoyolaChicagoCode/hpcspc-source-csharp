/*
Copyright (c) 2000, Thomas W. Christopher and George K. Thiruvathukal

Java and High Performance Computing (JHPC) Organzization
Tools of Computing LLC

All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are
met:

Redistributions of source code must retain the above copyright notice,
this list of conditions and the following disclaimer.

Redistributions in binary form must reproduce the above copyright
notice, this list of conditions and the following disclaimer in the
documentation and/or other materials provided with the distribution.

The names Java and High-Performance Computing (JHPC) Organization,
Tools of Computing LLC, and/or the names of its contributors may not
be used to endorse or promote products derived from this software
without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

This license is based on version 2 of the BSD license. For more
information on Open Source licenses, please visit
http://opensource.org.
*/

using System;
using System.Collections;


namespace info.jhpc.thread
{
	/**
	 * A class used to provide FIFO queues for the other classes
	 * in the thread package. THIS IS NOT THREAD-SAFE. DO NOT
	 * USE THIS ALONE FOR INTER-THREAD COMMUNICATION. This
	 * must be contained within a locked object.
	 *
	 * @author Thomas W. Christopher (Tools of Computing LLC)
	 * @version 0.2 Beta
	 */
	
	public class QueueComponent {
	    Stack hd = new Stack(), tl = new Stack();
	
	    /**
	     * Removes and returns the first element in the queue.
	     *
	     * @throws EmptyQueueException if the queue is empty.
	     */
	    public Object get() {
	        if (hd.Count > 0) return hd.Pop();
	        while (tl.Count > 0) hd.Push(tl.Pop());
	        if (hd.Count == 0) throw new EmptyQueueException();
	        return hd.Pop();
	    }
	
	    /**
	     * Returns a reference to the first element in the queue
	     * without removing it.
	     *
	     * @throws EmptyQueueException if the queue is empty.
	     */
	    public Object firstElement() {
	        if (hd.Count > 0) return hd.Peek();
	        while (tl.Count > 0) hd.Push(tl.Pop());
	        if (hd.Count == 0) throw new EmptyQueueException();
	        return hd.Peek();
	    }
	
	    /**
	     * Enqueues its parameter.
	     *
	     * @param elem the value to be enqueued.
	     */
	    public void put(Object elem) {
	        tl.Push(elem);
	    }
	
	    /**
	     * Returns true if the queue is empty; false, if it is not empty.
	     */
	    public bool isEmpty() {
	        return (hd.Count == 0) && (tl.Count == 0);
	    }
	
	    /**
	     * Removes all elements from the queue, leaving it empty.
	     */
	    public void clear() {
	        hd.Clear();
	        tl.Clear();
	    }
	
	    /**
	     * Tests the queue.
	     */
	    public static void Main(String[] args) {
	        int total, init;
	        int i;
	        QueueComponent q = new QueueComponent();
	        String s = "abc";
	        for (i = 0; i < s.Length; i++) {
	            q.put(s.Substring(i, i + 1));
	        }
	        String t = "";
	        while (!q.isEmpty()) {
	            t += ((String) q.get());
	        }
	        if (!(s == t)) {
	            Console.WriteLine("Bug. Put in \"" + s + "\", got \"" + t + "\"");
	        } else {
	            Console.WriteLine("Tests okay.");
	        }
	        if (args.Length < 1) {
	            Console.WriteLine("usage: java QueueComponent total [init]");
	            Environment.Exit(0);
	        }
	        total = Convert.ToInt32(args[0]);
	        init = args.Length < 2 ? 0 : Convert.ToInt32(args[1]);
	        if (total < init) {
	            Console.WriteLine("total elements to enqueue must be greater than initial");
	            Environment.Exit(0);
	        }
	
	        q = new QueueComponent();
	
	        long startTime = DateTime.Now.Ticks;
	        for (i = 0; i < init; i++) q.put("X");
	        for (; i < total; i++) {
	            q.put("X");
	            q.get();
	        }
	        while (!q.isEmpty()) q.get();
	        Console.WriteLine(DateTime.Now.Ticks - startTime);
	    }
	}
}
