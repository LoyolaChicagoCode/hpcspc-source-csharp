
using System;
using System.Threading;
using info.jhpc.thread;

namespace info.jhpc.textbook.chapter07
{
	public class Fetch2nd : Op1 {
	    Object lopnd;
	    Future src;
	    Op2 continuation;
	
	    public Fetch2nd(Future src, Op2 continuation) {
	        this.src = src;
	        this.continuation = continuation;
	    }
	
	    public void op(Object val) {
	        lopnd = val;
	        run();
	    }
	
	    public void run() {
	        try {
	            if (!src.isSet()) src.runDelayed(new ThreadStart(this.run));
	            continuation.op(lopnd, src.getValue());
	        } catch (ThreadInterruptedException e) {
	            continuation.op(lopnd, e);
	        }
	    }
	}
}
