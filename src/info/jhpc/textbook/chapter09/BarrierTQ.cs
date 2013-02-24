
using System;
using System.Threading;
using info.jhpc.thread;

namespace info.jhpc.textbook.chapter09
{
	public class BarrierTQ {
	    private static IndexedKey initialKey = IndexedKey.unique(0);
	    private static SharedTableOfQueues stoq = new SharedTableOfQueues();
	    private int stillToRegister;
	
	    internal class X {
	        public int remaining, count;
	
	        public X(int c) {
	            remaining = count = c;
	        }
	    }
	
	    public BarrierTQ(int num) {
	        stillToRegister = num;
	        stoq.put(initialKey, new X(num));
	    }
	
	    public class Handle {
	        private IndexedKey current = initialKey;
	
			public Handle() { ; }
	
	        public void gather() {
	            try {
	                X x = (X) stoq.get(current);
	                x.remaining--;
	                if (x.remaining == 0) {
	                    x.remaining = x.count;
	                    current = current.add(1);
	                    stoq.put(current, x);
	                } else {
	                    stoq.put(current, x);
	                    current = current.add(1);
	                    stoq.look(current);
	                }
	            } catch (ThreadInterruptedException e) {
	            }
	        }
	    }
	
	    public Handle register() {
	        if (stillToRegister-- > 0)
	            return new Handle();
	        else
	            throw new ApplicationException();
	    }
	
	    public String toString() {
	        return "BarrierTQ(" + initialKey.getId() + ")";
	    }
	}

    public class Chapter09 {
        static BarrierTQ bar;
        int me;
        static int iters = 10;

        public Chapter09(int me) {
            this.me = me;
        }

        public static void Main(string[] args) {
            try {
            	if(args.Length != 1) {
            		Console.WriteLine("usage: BarrierTQ n");
            		Environment.Exit(0);
            	}
            	
                int i;
                int num = Convert.ToInt32(args[0]);
                bar = new BarrierTQ(num + 1);
                BarrierTQ.Handle b = bar.register();
            	Chapter09 test=null;
            	
            	for (i = num; i > 0; i--) {
                	test=new Chapter09(i);
            		(new Thread(new ThreadStart(test.run))).Start();
            	}
            	
                for (i = 1; i <= iters; i++) {
                    b.gather();
                    Console.WriteLine();
                    b.gather();
                }
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        public void run() {
            BarrierTQ.Handle b = bar.register();
            for (int i = 1; i <= iters; i++) {
                Console.Write(me);
                b.gather();
                b.gather();
            }
        }
    }
}
