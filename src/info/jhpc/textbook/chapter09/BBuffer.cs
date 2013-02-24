
using System;
using System.Threading;
using info.jhpc.thread;

namespace info.jhpc.textbook.chapter09
{
	public class BBuffer {
	    private static IndexedKey fulls = IndexedKey.unique(0);
	    private IndexedKey empties = fulls.at(1);
	
	    private SharedTableOfQueues stoq = new SharedTableOfQueues();
	
	    public BBuffer(int num) {
	        for (int i = num; i > 0; i--) stoq.put(empties, "X");
	    }
	
	    public void put(Object x) {
	        try {
	            stoq.get(empties);
	            stoq.put(fulls, x);
	        } catch (ThreadInterruptedException e) {
	        }
	    }
	
	    public Object get() {
	        Object x = null;
	        try {
	            x = stoq.get(fulls);
	            stoq.put(empties, "X");
	        } catch (ThreadInterruptedException e) {
	        }
	        return x;
	    }
	
	    public override String ToString() {
	        return "BBuffer(" + fulls + ")";
	    }
	}
	
    public class BBufferTest {
        static BBuffer b = new BBuffer(5);

        public static void Main(string[] args) {
        	BBufferTest test=new BBufferTest();
            (new Thread(new ThreadStart(test.run))).Start();
            Object o;
            while ((o = b.get()) != null) Console.WriteLine(o);
        }

        public void run() {
            for (int i = 1; i <= 10; i++)
                b.put(i);
            b.put(null);
        }
    }
}
