

using System;
using info.jhpc.thread;
using System.Collections;


namespace info.jhpc.textbook.chapter04
{
	public class SharedTableOfQueues : info.jhpc.thread.Monitor {
	
	    Hashtable tbl = new Hashtable();
	
	    public void put(Object key, Object val) {
	        enter();
	        Folder f = (Folder) tbl[key];
	        if (f == null) tbl.Add(key, f = new Folder());
	        f.q.put(val);
	        f.notEmpty.leaveWithSignal();
	    }
	
	    public Object get(Object key) {
	        Folder f = null;
	        enter();
	        try {
	            f = (Folder) tbl[key];
	            if (f == null) tbl.Add(key, f = new Folder());
	            f.numWaiting++;
	            if (f.q.isEmpty()) f.notEmpty.await();
	            f.numWaiting--;
	            return f.q.get();
	        } finally {
	            if (f != null && f.q.isEmpty() && f.numWaiting == 0)
	                tbl.Remove(key);
	            leave();
	        }
	    }
	
	    public Object getSkip(Object key) {
	        Folder f = null;
	        enter();
	        try {
	            f = (Folder) tbl[key];
	            if (f == null || f.q.isEmpty()) {
	                return null;
	            }
	            return f.q.get();
	        } finally {
	            if (f != null && f.q.isEmpty() && f.numWaiting == 0)
	                tbl.Remove(key);
	            leave();
	        }
	    }
	    
	    
		internal class Folder {
        	public volatile QueueComponent q = new QueueComponent();	
        	public volatile Condition notEmpty = new Condition();
        	public volatile int numWaiting = 0;
    	}
	}
}
