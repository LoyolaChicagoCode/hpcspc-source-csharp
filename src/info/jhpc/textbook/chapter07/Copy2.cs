
using System;
using System.Threading;
using info.jhpc.thread;

namespace info.jhpc.textbook.chapter07
{
	public class Copy2 {
	    Future src;
	    StoreOp dst;
	    int dstx;
	
	    public Copy2(Future src,
	                 StoreOp dst,
	                 int dstx) {
	        this.src = src;
	        this.dst = dst;
	        this.dstx = dstx;
	    }
	
	    public void run() {
	        try {
	            if (!src.isSet()) src.runDelayed(new ThreadStart(this.run));
	            dst.store(dstx, src.getValue());
	        } catch (ThreadInterruptedException e) {
	            dst.store(dstx, e);
	        }
	    }
	}
}
