
using System;
using System.Threading;


namespace info.jhpc.textbook.chapter04
{
	class TestSimpleFuture {
		
	    public static void Main(string[] args) {
	        int i;
	    	Add adder=null;
	    	SimpleFuture f0, f1, f2;
	        SimpleFuture i0 = f0 = new SimpleFuture();
	        SimpleFuture i1 = f1 = new SimpleFuture();
	    	
	        for (i = 2; i <= 20; i++) {
	            f2 = new SimpleFuture();
	        	adder=new Add(f0, f1, f2);	
	            (new Thread(new ThreadStart(adder.run))).Start();
	            f0 = f1;
	            f1 = f2;
	        }
	        
	        i0.setValue(0);
	        i1.setValue(1);
	    	
	        try {
	            Console.WriteLine(f1.getValue());
	        } catch (Exception e) {
	        }
	    }
	}
	
	
	public class Add {

        SimpleFuture result, left, right;

        public Add(SimpleFuture LL, SimpleFuture RR, SimpleFuture DST) {
            left = LL;
            right = RR;
            result = DST;
        }

        protected void op() {
            try {
                result.setValue(((int)left.getValue()) + ((int)right.getValue()));
            } catch (ThreadInterruptedException ie) {
            }
        }

        public void run() {
            op();
        }
    }
}
