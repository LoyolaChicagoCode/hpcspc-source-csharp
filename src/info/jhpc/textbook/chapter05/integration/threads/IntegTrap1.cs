/*
To accompany High-Performance Java Platform(tm) Computing:
Threads and Networking, published by Prentice Hall PTR and
Sun Microsystems Press.

Threads and Networking Library
Copyright (C) 1999-2000
Thomas W. Christopher and George K. Thiruvathukal

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Library General Public
License as published by the Free Software Foundation; either
version 2 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Library General Public License for more details.

You should have received a copy of the GNU Library General Public
License along with this library; if not, write to the
Free Software Foundation, Inc., 59 Temple Place - Suite 330,
Boston, MA  02111-1307, USA.
*/

using System;
using System.Threading;

namespace info.jhpc.textbook.chapter05.integration.threads
{
	/**
	 * This class manages a choosen number of threads to complete the
	 * integeral computation of a defined, single variable function
	 * by means of trapazoidal areas.
	 *
	 * @author John Shafaee & Thomas Christopher
	 *         Date: July 5, 1999
	 */
	
	
	public class IntegTrap1 {
	
	    /**
	     * number of threads to compute concurrently.
	     */
	
	    int numThreads;
	
	    /**
	     * number of trapazoids to compute per thread.
	     */
	
	    int granularity;
	
	    
        /**
         * MAIN
         */
        public static void Main(string[] args) {

            if (args.Length != 4) {
                Console.WriteLine("\nUSAGE: IntegTrap1$Test1 <num threads>" +
                        " <range start> <range end> <granularity>");
                Console.WriteLine("version: 1\n");
                Environment.Exit(1);
            }

            int num_t = Convert.ToInt32(args[0]);
            double start = Convert.ToDouble(args[1]);
            double end = Convert.ToDouble(args[2]);
            int gran = Convert.ToInt32(args[3]);

            Console.WriteLine("Starting calculations...");

            // initiate the start and finish time variable
            long begin_time = 0;
            long end_time = 0;

            // begin timing the process from thread creation time
            begin_time = DateTime.Now.Ticks;

            IntegTrap1 integeral = new IntegTrap1(num_t, gran);
            double area = integeral.integrate(start, end, new F_of_x(f));

            // stop timing; all threads have completed
            end_time = DateTime.Now.Ticks;

            // output results
            printResults(area, num_t, gran, (end_time - begin_time));

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
	    
	
	    /**
	     * Constructor - creates and initiates child threads
	     * for performing the calculation.
	     */
	    public IntegTrap1(int numThreads,
	                      int granularity) {
	
	        // check for invalid integration options
	        try {
	            if (numThreads < 1)
	                throw new BadThreadCountException();
	
	            if (granularity < 1)
	                throw new BadGranularityException();
	        } catch (Exception e) {
	            Console.WriteLine(e.ToString());
	            Environment.Exit(1);
	        }
	        this.numThreads = numThreads;
	        this.granularity = granularity;
	
	    }
	
	    public double integrate(double a, double b, F_of_x fn) {
	        int i;
	        // initiate the array for managing child threads
	        Thread[] childThreads= new Thread[numThreads];
	    	IntegTrap1Region[] its=new IntegTrap1Region[numThreads];
	        // area under curve
	        double totalArea = 0.0d;
	
	        if (a > b)
	            throw new BadRangeException();
	        if (a == b)
	            throw new NoRangeException();
	        try {
	            double range = b - a;
	            double start = a;
	            double end = a + ((1.0d) / numThreads * range);
	
	            for (i = 0; i < numThreads; i++) {
	                // create and start new child threads
	            	its[i]=new IntegTrap1Region(start, end, granularity, fn);
	            	childThreads[i] = new Thread(new ThreadStart(its[i].run));
	                childThreads[i].Start();
	
	                // set the range for the next thhread
	                start = end;
	                end = a + ((i + 2.0d) / numThreads * range);
	            }
	        } catch (Exception e) {
	            Console.WriteLine("Exception occured in creating and" +
	                    " initializing thread.\n" + e.ToString());
	        }
	
	        for (i = 0; i < numThreads; i++) {
	            try {
	                childThreads[i].Join();
	                totalArea += its[i].getArea();
	            } catch (Exception e) {
	                Console.WriteLine("Could not join with child threads!");
	                Environment.Exit(1);
	            }
	        }
	        return totalArea;
	    }
	
	    /**
	     * Report the final results of the calculation.
	     */
	    public static void printResults(double totalArea, int threadCount,
	                                    int granularity, long run_time) {
	
	        Console.WriteLine("\n             RESULTS           ");
	        Console.WriteLine("===============================");
	        Console.WriteLine("Total area under curve : "
	                + totalArea);
	        Console.WriteLine("Number of threads used : "
	                + threadCount);
	        Console.WriteLine("Granularity of calc.   : "
	                + granularity + " trapaziods per sub-region");
	        Console.WriteLine("Total run time         : "
	                + run_time + " msec.");
	        Console.WriteLine("===============================\n");
	
	    }
	
	}
}
