
using System;
using info.jhpc.thread;
using System.Threading;
using System.Collections;


namespace info.jhpc.textbook.chapter05
{
	public class Knapsack1 {
	
	    public struct Item {
	        public int profit, weight, pos;
	        public float profitPerWeight;
	    }
	
	    BitArray selected;
	    int capacity;
	    static float bestProfit = 0;
	    static Item[] item;
	    Future done;
	    SharedTerminationGroup tg;
	
	    public BitArray getSelected() {
	        done.getValue();
	        BitArray s = new BitArray(item.Length);
	        for (int i = 0; i < item.Length; i++) {
	            if (selected.Get(i))
	                s.Set(item[i].pos, true);
	        }
	        return s;
	    }
	
	    public int getProfit() {
	        done.getValue();
	        return (int) bestProfit;
	    }
	
	    public Knapsack1(int[] weights, int[] profits, int capacity) {
	        if (weights.Length != profits.Length)
	            throw new ArgumentException("0/1 Knapsack: differing numbers of weights and profits");
	        if (capacity <= 0)
	            throw new ArgumentException("0/1 Knapsack: capacity<=0");
	        
	        item = new Item[weights.Length];
	    	done = new Future();
	    	tg = new SharedTerminationGroup(done);
	        int i;
	    	Search search=null;
	        
	        for (i = 0; i < weights.Length; i++) {
	            item[i] = new Item();
	            item[i].profit = profits[i];
	            item[i].weight = weights[i];
	            item[i].pos = i;
	            item[i].profitPerWeight = ((float) profits[i]) / weights[i];
	        }
	        int j;
	        for (j = 1; j < item.Length; j++) {
	            for (i = j; i > 0
	                    && item[i].profitPerWeight > item[i - 1].profitPerWeight; i--) {
	                Item tmp = item[i];
	                item[i] = item[i - 1];
	                item[i - 1] = tmp;
	            }
	        	
	        	search=new Search(0, capacity, 0, new BitArray(item.Length), tg);
	        	(new Thread(new ThreadStart(search.run))).Start();
	    	}
	    }
	    
	    
	    public static void Main(string[] args) {
            try {
                int num = 20;
                int max = 100;
                int[] p = new int[num];
                int[] w = new int[num];
                int capacity = (int) (num * (max / 2.0) * 0.7);
                int i;
            	Random rand=new Random();
            	
                for (i = p.Length - 1; i >= 0; i--) {
                    p[i] = 1 + (int) (rand.Next() * (max - 1));
                    w[i] = 1 + (int) (rand.Next() * (max - 1));
                }
                
                Console.WriteLine("p:");
                
                for (i = p.Length - 1; i >= 0; i--) {
                    Console.WriteLine(" " + p[i]);
                }
                Console.WriteLine();
                Console.WriteLine("w:");
                for (i = p.Length - 1; i >= 0; i--) {
                    Console.WriteLine(" " + w[i]);
                }
                Console.WriteLine();
                Knapsack1 ks = new Knapsack1(w, p, capacity);
                BitArray s = ks.getSelected();
                Console.WriteLine("s:");
                for (i = p.Length - 1; i >= 0; i--) {
                    Console.WriteLine(" " + (s.Get(i) ? "1" : "0") + " ");
                }
                Console.WriteLine();
                Console.WriteLine("Profit: " + ks.getProfit());
            } catch (Exception exc) {
                Console.WriteLine(exc);
            }
        }
        
        
        public class Search {
	        BitArray selected;
	        int from;
	        int startWeight = 0;
	        int startProfit = 0;
	        SharedTerminationGroup tg;
	
	        public Search(int from, int remainingWeight, int profit, BitArray selected,
	               SharedTerminationGroup tg) {
	            this.from = from;
	            startWeight = remainingWeight;
	            startProfit = profit;
	            this.selected = selected;
	            this.tg = tg;
	        }
	
	        void dfs(int i, int rw, int p) {
	            if (i >= item.Length) {
	                if (p > bestProfit) {
	                    bestProfit = p;
	                    selected = (BitArray) selected.Clone();
	                    Console.WriteLine("new best: " + p);
	                }
	                return;
	            }
	            if (p + rw * item[i].profitPerWeight < bestProfit)
	                return;
	            if (rw - item[i].weight >= 0) {
	                selected.Set(i, true);
	                dfs(i + 1, rw - item[i].weight, p + item[i].profit);
	            }
	            selected.Set(i, false);
	            dfs(i + 1, rw, p);
	            return;
	        }
	
	        public void run() {
	            dfs(from, startWeight, startProfit);
	            tg.terminate();
	        }
		}
	}
}
