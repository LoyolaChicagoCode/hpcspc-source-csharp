
using System;
using info.jhpc.thread;


namespace info.jhpc.textbook.chapter05.integration.accumulator
{
	class IntegTrap3Region {
	
	    // Provate variables used in calculating a specified region
	    private string name;
	    private double x_start, x_end;
	    private int granularity;
	    private F_of_x f;
	    private Accumulator result;
	
	
	    /**
	     * Constructor
	     */
	    public IntegTrap3Region(double x_start, double x_end,
	                            int granularity, F_of_x f, Accumulator result) {
	
	        this.name = x_start + "-" + x_end;
	        this.x_start = x_start;
	        this.x_end = x_end;
	        this.granularity = granularity;
	        this.f = f;
	        this.result = result;
	    }
	
	
	    /**
	     * This is the method that is implemented as directed by the
	     * Runnable interface.  The code within this method is
	     * called when the thread is started. All of the calculations
	     * that the thread will perform are defined within this method.
	     * The equation used to calculate the area of the trapazoid
	     * is defined as a seperate provate method.
	     */
	    public void run() {
	
	        Console.WriteLine("Thread: " + this.name + " started!");
	
	        double area = 0.0d;
	        double range = x_end - x_start;
	        double g = granularity;
	
	        for (int i = granularity - 1; i > 0; i--) {
	            area += f((i / g) * range + x_start);
	        }
	        area += (f(x_start) + f(x_end)) / 2.0;
	        area = area * (range / g);
	
	        lock(result) {
	            result.setData(area + ((double)result.getData()));
	        }
	        result.signal();
	
	        Console.WriteLine("Thread: " + this.name + " completed! ");
	    }	
	}
}
