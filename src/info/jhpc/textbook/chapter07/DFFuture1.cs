
using System;
using info.jhpc.thread;

namespace info.jhpc.textbook.chapter07
{
	public class DFFuture1 : Future, Op1 {
	    public void op(Object opnd) {
	        setValue(opnd);
	    }
	}
}
