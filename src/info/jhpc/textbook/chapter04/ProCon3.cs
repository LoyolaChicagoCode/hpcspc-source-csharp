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
	
	public class ProCon3 {
	
		static BoundedBuffer3 buf=null;
	
	
	    public static void Main(String[] x) {
	        buf = new BoundedBuffer3(3);
	        Thread pro1 = new Thread(new ThreadStart(Producer3));
	        Thread con1 = new Thread(new ThreadStart(Consumer3));
	        Thread pro2 = new Thread(new ThreadStart(Producer3));
	        Thread con2 = new Thread(new ThreadStart(Consumer3));
	        
	        pro1.Start();
	        pro2.Start();
	        con1.Start();
	        con2.Start();
	    	
	        try {
	            pro1.Join();
	            con1.Join();
	            pro2.Join();
	            con2.Join();
	        } catch (ThreadInterruptedException e) {
	            return;
	        }
	    }
	    
	 	   
	    public static void Consumer3() {
	        int j=(int)buf.get();
	    	
            while (j != -1) {
            	
                lock (Console.Out) {
                    Console.WriteLine(j);	
                }
                //yield();
                j=(int)buf.get();
            }
	    }
	
	
	    public static void Producer3() {
	        int i;
	    	
            for (i = 0; i < 10; i++) {
                buf.put(i);
                //buf.put(new Integer(i));
                //yield();
            }
            buf.put(-1);
            //buf.put(new Integer(-1));
	    }
	}
	
	
	class BoundedBuffer3 : info.jhpc.thread.Monitor {

	    Object[] buffer;
	
	    volatile int hd = 0, tl = 0;
	
	    Condition notEmpty = new Condition();
	    Condition notFull = new Condition();
	
	    public BoundedBuffer3(int size) {
	        buffer = new Object[size];
	    }
	
	    public Object get() {
	        enter();
	        Object v;
	        if (tl == hd) notEmpty.await();
	        v = buffer[hd++ % buffer.Length];
	        notFull.signal();
	        leave();
	        return v;
	    }
	
	    public void put(Object v) {
	        enter();
	        if (tl - hd >= buffer.Length) notFull.await();
	        buffer[tl++ % buffer.Length] = v;
	        notEmpty.signal();
	        leave();
	    }
	}
}
