
using System;

namespace info.jhpc.textbook.chapter07
{
	public class Store : Op1 {
	    StoreOp dst;
	    int pos;
	
	    public Store(StoreOp dst, int pos) {
	        this.dst = dst;
	        this.pos = pos;
	    }
	
	    public void op(Object val) {
	        dst.store(pos, val);
	    }
	}
}
