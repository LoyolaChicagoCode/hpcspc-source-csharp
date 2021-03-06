/*
Copyright (c) 2000, Thomas W. Christopher and George K. Thiruvathukal

Java and High Performance Computing (JHPC) Organzization
Tools of Computing LLC

All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are
met:

Redistributions of source code must retain the above copyright notice,
this list of conditions and the following disclaimer.

Redistributions in binary form must reproduce the above copyright
notice, this list of conditions and the following disclaimer in the
documentation and/or other materials provided with the distribution.

The names Java and High-Performance Computing (JHPC) Organization,
Tools of Computing LLC, and/or the names of its contributors may not
be used to endorse or promote products derived from this software
without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

This license is based on version 2 of the BSD license. For more
information on Open Source licenses, please visit
http://opensource.org.
*/

using System;
using System.Threading;
using System.Collections;
using NativeMonitor = System.Threading.Monitor;

namespace info.jhpc.thread
{
    /**
     * Allows multiple threads and runnables to gather at a
     * point before proceeding.
     *
     * @author Thomas W. Christopher (Tools of Computing LLC)
     * @version 0.2 Beta
     */
    public class Barrier : SimpleBarrier, RunDelayed
    {

        /**
         * The Stack containing waiting Runnables. (Stack so its quick and easy to
         * have them wait and to remove them.)
         */

        protected Stack runnablesWaiting = null;

        /**
         * The default queue into which to put (runDelayed) runnables that are
         * waiting on any of these Barriers. The limit on the number
         * of threads that can be running the (runDelayed) objects is the
         * amount of memory available. The (runDelayed) objects are placed
         * in this queue by default when a Future is given a value.
         */

        protected static RunQueue classRunQueue =
                new RunQueue();

        /**
         * The queue into which to put (runDelayed) runnables that are
         * waiting on this Barrier. The limit on the number
         * of threads that can be running the (runDelayed) objects is the
         * amount of memory available. They are placed in the queue when
         * the Future is given a value.
         */

        protected RunQueue runQueue = classRunQueue;

        /**
         * Creates a Barrier at which n Threads or Runnables may repeatedly
         * gather.
         *
         * @param n total number of threads that must gather.
         */

        public Barrier(int n) : base(n) { }

        /**
         * Is called by a thread to wait for the rest of the n Threads or
         * Runnables to gather before the set of threads or runnables may
         * continue executing.
         *
         * @throws ThreadInterruptedException If interrupted while waiting.
         */
        public override void gather()
        {
            try
            {
                NativeMonitor.Enter(this);
                if (--count > 0)
                    NativeMonitor.Wait(this);
                else
                {
                    releaseRunnables();
                    count = initCount;
                    NativeMonitor.PulseAll(this);
                }
            }
            finally
            {
                NativeMonitor.Exit(this);
            }
        }

        /**
         * Is a non-delaying version of gather().
         */
        public void signal()
        {
            try
            {
                NativeMonitor.Enter(this);
                if (--count == 0)
                {
                    releaseRunnables();
                    count = initCount;
                    NativeMonitor.PulseAll(this);
                }
            }
            finally
            {
                NativeMonitor.Exit(this);
            }
        }

        protected void releaseRunnables()
        {
            RunQueue q = getRunQueue();
            if (runnablesWaiting != null)
            {
                while (runnablesWaiting.Count > 0)
                {
                    q.run((ThreadStart)runnablesWaiting.Pop());
                }
                //runnablesWaiting=null;
            }
        }

        /**
         * Get the RunQueue for a Barrier object. The run queue should be
         * changed with setRunQueue for more precise control.
         *
         * @return The RunQueue that objects runDelayed on a Barrier object
         *         will be  placed in.
         */

        public RunQueue getRunQueue()
        {
            return runQueue;
        }

        /**
         * Set the RunQueue for a Barrier object.
         */

        public void setRunQueue(RunQueue rq)
        {
            runQueue = rq;
        }

        /**
         * Get the RunQueue for the Barrier class.
         *
         * @return The RunQueue that  objects runDelayed on a pure Future
         *         will be placed in.
         */

        public static RunQueue getClassRunQueue()
        {
            return classRunQueue;
        }

        /**
         * Schedule a runnable object to execute when the Barrier has
         * gathered the correct number of threads or runnables.
         */

        public void runDelayed(ThreadStart r)
        {
            try
            {
                NativeMonitor.Enter(this);
                if (--count > 0)
                {
                    if (runnablesWaiting == null)
                    {
                        runnablesWaiting = new Stack();
                    }
                    runnablesWaiting.Push(r);
                }
                else
                {
                    releaseRunnables();
                    count = initCount;
                    NativeMonitor.PulseAll(this);
                    getRunQueue().run(r);
                }
            }
            finally
            {
                NativeMonitor.Exit(this);
            }
        }
    }
}
