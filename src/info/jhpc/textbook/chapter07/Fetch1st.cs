
using System;
using System.Threading;
using info.jhpc.thread;

namespace info.jhpc.textbook.chapter07
{
	public class Fetch1st {
	    Future src;
	    Op1 continuation;
	
	    public Fetch1st(Future src, Op1 continuation) {
	        this.src = src;
	        this.continuation = continuation;
	    }
	
	    public void run() {
	        try {
	            if (!src.isSet()) src.runDelayed(new ThreadStart(this.run));
	            continuation.op(src.getValue());
	        } catch (ThreadInterruptedException e) {
	            continuation.op(e);
	        }
	    }
	}
}
