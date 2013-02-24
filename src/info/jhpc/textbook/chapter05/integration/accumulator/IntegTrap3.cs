
using System;
using info.jhpc.thread;
using System.Threading;


namespace info.jhpc.textbook.chapter05.integration.accumulator
{
	public class IntegTrap3 {
	
	    int numThreads;
	    int numRegions;
	    int granularity;
	
	    /**
	     * MAIN
	     */
	    public static void Main(string[] args) {
	
	        if (args.Length != 5) {
	            Console.WriteLine("\nUSAGE: IntegTrap3 <num threads>" +
	                    " <num regions> <range start> <range end> <granularity>");
	            Console.WriteLine("version: 3\n");
	            Environment.Exit(1);
	        }
	
	        int num_t = Convert.ToInt32(args[0]);
	        int num_r = Convert.ToInt32(args[1]);
	        double start = Convert.ToDouble(args[2]);
	        double end = Convert.ToDouble(args[3]);
	        int gran = Convert.ToInt32(args[4]);
	
	        Console.WriteLine("Starting calculations...");
	
	        // begin timing the process from thread creation time
	        long begin_time = DateTime.Now.Ticks;
	
	        IntegTrap3 integeral = new IntegTrap3(num_t, num_r, gran);
	        double area = integeral.integrate(start, end, new F_of_x(f));
	
	        // stop timing; all threads have completed
	        long end_time = DateTime.Now.Ticks;
	
	        // output results
	        printResults(area, num_t, num_r, gran, (end_time - begin_time));
	    }
	
	
	    /**
	     * Constructor - creates and initiates child threads
	     * for performing the calculation.
	     */
	    public IntegTrap3(int numThreads, int numRegions,
	                      int granularity) {
	
	        // check for invalid integration options
	        try {
	            if (numThreads < 1)
	                throw new BadThreadCountException();
	
	            if (numRegions < 1)
	                throw new BadRegionCountException();
	
	            if (granularity < 1)
	                throw new BadGranularityException();
	        } catch (Exception e) {
	            Console.WriteLine(e.ToString());
	            Environment.Exit(1);
	        }
	        this.numThreads = numThreads;
	        this.numRegions = numRegions;
	        this.granularity = granularity;
	    }
	    
	    
	    /**
         * Define the delegate to integrate that
         * implements F_of_x. This endures that the object
         * will provide the proper method for computing the
         * result of a true function.
         */
        public static double f(double x) {
            return x * x;  // a parabola F(x) = x^2
        }
	
	
	    public double integrate(double a, double b, F_of_x fn) {
	        int i;
	
	        // area under curve
	        double totalArea = 0.0d;
	
	        Accumulator acc = null;
	
	
	        if (a > b)
	            throw new BadRangeException();
	        if (a == b)
	            throw new NoRangeException();
	
	        // create a RunQueue with the defined max. number of threads
	        RunQueue regionQueue = new RunQueue(numThreads);
	
	
	        try {
	            double range = b - a;
	            double start = a;
	            double end = a + ((1.0d) / numRegions * range);
	
	            acc = new Accumulator(numRegions, ((double)0.0));
	
	            for (i = 0; i < numRegions; i++) {
	
	                // create a IntegTrap3Region with the designated
	                // Accumulator and pass it to the RunQueue
	                IntegTrap3Region it=new IntegTrap3Region(start, end, granularity, fn, acc);
	                regionQueue.put(new ThreadStart(it.run));
	
	                // set the range for the next thhread
	                start = end;
	                end = a + ((i + 2.0d) / numRegions * range);
	            }
	        } catch (Exception e) {
	            Console.WriteLine("Exception occured in creating " +
	                    "and initializing thread.\n" + e.ToString());
	        }
	
	        try {
	            totalArea = ((double) acc.getFuture().getValue());
	        } catch (Exception e) {
	            Console.WriteLine("Could not retrieve value from Accumulator's Future.");
	            Environment.Exit(1);
	        }
	        regionQueue.setMaxThreadsWaiting(0);
	
	
	        return totalArea;
	    }
	
	
	    /**
	     * Report the final results of the calculation.
	     */
	    public static void printResults(double totalArea, int threadCount,
	                                    int numRegions, int granularity, long run_time) {
	
	        Console.WriteLine("\n             RESULTS           ");
	        Console.WriteLine("===============================");
	        Console.WriteLine("Total area under curve : "
	                + totalArea);
	        Console.WriteLine("Number of threads used : "
	                + threadCount);
	        Console.WriteLine("Number of regions      : "
	                + numRegions);
	        Console.WriteLine("Granularity of calc.   : "
	                + granularity + " trapaziods per sub-region");
	        Console.WriteLine("Total run time         : "
	                + run_time + " msec.");
	        Console.WriteLine("===============================\n");
	
	    }
	}
}
