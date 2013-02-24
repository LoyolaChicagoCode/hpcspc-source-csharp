
using System;

namespace info.jhpc.textbook.chapter07
{
	public class Binop2 : StoreOp {
	    Object lopnd, ropnd;
	    Op2 continuation;
	    int needed = 2;
	
	    public Binop2(Op2 continuation) {
	        this.continuation = continuation;
	    }
	
	    public void store(int i, Object val) {
	        if (i == 0)
	            lopnd = val;
	        else
	            ropnd = val;
	        if (--needed == 0) continuation.op(lopnd, ropnd);
	    }
	}
}
