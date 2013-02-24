

using System;
using info.jhpc.thread;

namespace info.jhpc.textbook.chapter07
{
	public class DFFuture2 : Future, Op1, StoreOp {
	    public void op(Object opnd) {
	        setValue(opnd);
	    }
	
	    public void store(int i, Object val) {
	        setValue(val);
	    }
	}
}
