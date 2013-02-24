/*
 * To accompany High-Performance Java Platform(tm) Computing: Threads and
 * Networking, published by Prentice Hall PTR and Sun Microsystems Press.
 * 
 * Threads and Networking Library Copyright (C) 1999-2000 Thomas W. Christopher
 * and George K. Thiruvathukal
 * 
 * This library is free software; you can redistribute it and/or modify it under
 * the terms of the GNU Library General Public License as published by the Free
 * Software Foundation; either version 2 of the License, or (at your option) any
 * later version.
 * 
 * This library is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE. See the GNU Library General Public License for more
 * details.
 * 
 * You should have received a copy of the GNU Library General Public License
 * along with this library; if not, write to the Free Software Foundation, Inc.,
 * 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
 */
// show producer & consumer synchronized by wait() and notifyAll()

using System;
using System.Threading;
using info.jhpc.thread;

namespace info.jhpc.textbook.chapter04
{
	public class ProCon4
	{
		static BoundedBuffer4 buf;
		
		
	    public static void Main(string[] x) {
	        buf = new BoundedBuffer4(3);
	        Thread pro1 = new Thread(new ThreadStart(Producer4));
	        Thread con1 = new Thread(new ThreadStart(Consumer4));
	        Thread pro2 = new Thread(new ThreadStart(Producer4));
	        Thread con2 = new Thread(new ThreadStart(Consumer4));
	        
	        con1.Start();
	        con2.Start();
	        pro1.Start();
	        pro2.Start();
	        
	        try {
	            pro1.Join();
	            con1.Join();
	            pro2.Join();
	            con2.Join();
	        } catch (ThreadInterruptedException e) {
	            return;
	        }
	    }
	    
	    
	    public static void Producer4() {
	        int i;
	        try {
	            for (i = 0; i < 10; i++)
	                buf.put(i);
	            buf.put(-1);
	        } catch (ThreadInterruptedException e) {
	            return;
	        }
	    }
	    
	    
	    public static void Consumer4() {
	        int j;
	        try {
	            j =(int)buf.get();
	            while (j != -1) {
	                lock(Console.Out) {
	                    Console.WriteLine(j);
	                }
	                //yield();
	                j=(int)buf.get();
	            }
	        } catch (ThreadInterruptedException e) {
	            return;
	        }
	    }
	}
    
    
    class BoundedBuffer4 {
	
	    info.jhpc.thread.Monitor mon = new info.jhpc.thread.Monitor();
	    info.jhpc.thread.Monitor.Condition notEmpty = new info.jhpc.thread.Monitor.Condition();
	    info.jhpc.thread.Monitor.Condition notFull = new info.jhpc.thread.Monitor.Condition();
	
	    volatile int hd = 0, tl = 0;
	
	    Object[] buffer;
	
	    public BoundedBuffer4(int size) {
	        buffer = new Object[size];
	    }
	
	    public void put(Object v) {
	        mon.enter();
	        if (tl - hd >= buffer.Length) notFull.await();
	        buffer[tl++ % buffer.Length] = v;
	        notEmpty.signal();
	        mon.leave();
	    }
	
	    public Object get() {
	        mon.enter();
	        Object v;
	        if (tl == hd) notEmpty.await();
	        v = buffer[hd++ % buffer.Length];
	        notFull.leaveWithSignal();
	        return v;
	    }
	}
}
