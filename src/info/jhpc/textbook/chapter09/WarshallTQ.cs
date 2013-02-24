
using System;
using System.Threading;
using info.jhpc.thread;

namespace info.jhpc.textbook.chapter09
{
	public class WarshallTQ {
	    static int blkSize = 8;	// 8x8 blocks
	
	    public WarshallTQ(int bsize) {
	        blkSize = bsize;
	    }
	
	    internal class Block {
	        bool[][] a;
	        bool[][] block;
	        int r, c; //upperleft
	        int nr, nc;
	        int N;
	        SharedTableOfQueues tbl;
	        IndexedKey rows, cols;
	        Accumulator done;
	
	        public Block(bool[][] a,
	              int r, int c,
	              SharedTableOfQueues t,
	              IndexedKey rows,
	              IndexedKey cols,
	              Accumulator done) {
	            this.a = a;
	            this.r = r;
	            this.c = c;
	            N = a.Length;
	            tbl = t;
	            this.rows = rows;
	            this.cols = cols;
	            this.done = done;
	        }
	
	        public void run() {
	            int i, j;
	            int k;
	            bool IHaveRow, IHaveColumn;
	            bool[] row = null, col = null;
	            nr = Math.Min(blkSize, a.Length - r);
	            nc = Math.Min(blkSize, a[0].Length - c);
	            this.block = new bool[nr][];
	        
	        	for (i = 0; i < nr; i++) {
	            	block[i]=new bool[nc];
	            	
	                for (j = 0; j < nc; j++)
	                    block[i][j] = a[r + i][c + j];
	        	}
	        	
	            try {
	                for (k = 0; k < N; k++) {
	                    IHaveRow = k - r >= 0 && k - r < nr;
	                    IHaveColumn = k - c >= 0 && k - c < nc;
	                    if (IHaveRow) {
	                        tbl.put(rows.at(k + c * N), block[k - r].Clone());
	                        row = block[k - r];
	                    }
	                    if (IHaveColumn) {
	                        col = new bool[nr];
	                        for (j = 0; j < nr; j++) col[j] = block[j][k - c];
	                        tbl.put(cols.at(k + r * N), col);
	                    }
	                    if (!IHaveRow) {
	                        row = (bool[]) tbl.look(rows.at(k + c * N));
	                    }
	                    if (!IHaveColumn) {
	                        col = (bool[]) tbl.look(cols.at(k + r * N));
	                    }
	                    for (i = 0; i < nr; i++)
	                        if (col[i])
	                            for (j = 0; j < nc; j++)
	                                block[i][j] |= row[j];
	                }//end for k
	
	                for (i = 0; i < nr; i++)
	                    for (j = 0; j < nc; j++)
	                        a[r + i][c + j] = block[i][j];
	                done.signal();
	            } catch (ThreadInterruptedException iex) {
	            }
	        }
	    }
	
	    public void closure(bool[][] a) {
	        int i, j, NR, NC;
	        SharedTableOfQueues tbl = new SharedTableOfQueues();
	        IndexedKey kthRows = IndexedKey.unique(0);
	        IndexedKey kthCols = IndexedKey.unique(0);
	        NR = a.Length;
	        NC = a[0].Length;
	    	Block block=null;
	        int nt = ((NR + blkSize - 1) / blkSize) * ((NC + blkSize - 1) / blkSize);
	        Accumulator done = new Accumulator(nt);
	        for (i = 0; i < NR; i += blkSize)
	            for (j = 0; j < NC; j += blkSize) {
	            	block=new Block(a, i, j, tbl, kthRows, kthCols, done);
	            	(new Thread(new ThreadStart(block.run))).Start();
	            }
	        try {
	            done.getFuture().getValue();
	        } catch (ThreadInterruptedException ex) {
	        }
	    }
	}
}
