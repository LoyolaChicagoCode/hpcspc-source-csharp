
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


using System;
using System.Threading;
using info.jhpc.thread;


namespace info.jhpc.textbook.chapter04
{
	public class SimpleFuture : info.jhpc.thread.Monitor {
	
	    private volatile Object val;
	    private Condition is_set = new Condition();
	
	    public SimpleFuture() {
	        val = this;
	    }
	
	    public SimpleFuture(Object v) {
	        val = v;
	    }
	
	    public Object getValue() {
	        try {
	            enter();
	            if (val == this) is_set.await();
	            is_set.leaveWithSignal();
	            return val;
	        } catch (ThreadInterruptedException ie) {
	            leave();
	            throw ie;
	        }
	    }
	
	    public bool isSet() {
	        enter();
	        try {
	            return (val != this);
	        } finally {
	            leave();
	        }
	    }
	
	    public void setValue(Object v) {
	        enter();
	        if (val != this) {
	            leave();
	            return;
	        }
	        val = v;
	        is_set.leaveWithSignal();
	    }	
	}
}
