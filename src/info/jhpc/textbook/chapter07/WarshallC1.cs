
using System;
using info.jhpc.thread;
using System.Threading;


namespace info.jhpc.textbook.chapter07
{
	public class WarshallC1 {
	
		public WarshallC1() { ; }
	
		internal class Row {
	        bool[] row;
	        int myRowNumber;
	        Future[] row_k_step_k;
	        Accumulator done;
	
	        public Row(bool[] row,
	            int myRowNumber,
	            Future[] row_k_step_k,
	            Accumulator done) {
	            this.row = row;
	            this.myRowNumber = myRowNumber;
	            this.row_k_step_k = row_k_step_k;
	            this.done = done;
	        }
	
	        public void run() {
	            try {
	                int j, k;
	                bool[] row_k;
	                for (k = 0; k < row_k_step_k.Length; k++) {
	                    if (k == myRowNumber)
	                        row_k_step_k[k].setValue(row.Clone());
	                    else if (row[k]) {
	                        row_k = (bool[]) row_k_step_k[k].getValue();
	                        for (j = 0; j < row.Length; j++) {
	                            row[j] |= row_k[j];
	                        }
	                    }
	                }
	                bool[][] result = (bool[][]) done.getData();
	                result[myRowNumber] = row;
	                done.signal();
	            } catch (ThreadInterruptedException ex) {
	            }
	        }
    	}

	    public bool[][] closure(bool[][] a) {
	        int i;
	    	Row row=null;
	        Future[] kthRows = new Future[a.Length];
	        for (i = 0; i < kthRows.Length; ++i)
	            kthRows[i] = new Future();
	        Accumulator done = new Accumulator(a.Length,
	                new bool[a.Length][]);
	        for (i = 0; i < a.Length; i++) {
	            row=new Row((bool[]) a[i].Clone(), i, kthRows, done);
	        	(new Thread(new ThreadStart(row.run))).Start();
	        }
	        bool[][] result = null;
	        try {
	            result = (bool[][]) done.getFuture().getValue();
	        } catch (ThreadInterruptedException ex) {
	        }
	        return result;
	    }
	}
}
