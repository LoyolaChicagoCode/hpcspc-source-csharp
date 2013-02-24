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


//Locks (binary semaphores)
namespace info.jhpc.thread
{
    /**
     * Lock single or multiple locks.
     *
     * @author Thomas W. Christopher (Tools of Computing LLC)
     * @version 0.2 Beta
     */

    public class LockTable
    {
        protected Hashtable table = new Hashtable();

        /**
         * Lock single lock indicated by object s. Since
         * LockTable uses a hashtable to maintain the locks,
         * it uses equals() to detect whether two locks are
         * the same. Thus two strings can be equal and indicate
         * the same lock, even though they are not the same
         * object.
         */
        public void Lock(Object s)
        {
            try
            {
                NativeMonitor.Enter(this);
                while (table[s] != null) NativeMonitor.Wait(this);
                table.Add(s, s);
            }
            finally
            {
                NativeMonitor.Exit(this);
            }
        }

        /**
         * Unlock the single lock indicated by object s.
         */
        public void unlock(Object s)
        {
            try
            {
                NativeMonitor.Enter(this);
                table.Remove(s);
                NativeMonitor.PulseAll(this);
            }
            finally
            {
                NativeMonitor.Exit(this);
            }
        }

        /**
         * Simultaneously lock all the locks indicated by the
         * objects in the array sa. Since
         * LockTable uses a hashtable to maintain the locks,
         * it uses equals() to detect whether two locks are
         * the same. Thus two strings can be equal and indicate
         * the same lock, even though they are not the same
         * object.
         */
        public void Lock(Object[] sa)
        {
            try
            {
                NativeMonitor.Enter(this);
                int i;
                bool tryToLock;

                while (true)
                {
                    tryToLock = false;

                    for (i = 0; i < sa.Length; i++)
                    {
                        if (table[sa[i]] != null)
                        {
                            NativeMonitor.Wait(this);
                            tryToLock = true;
                            break;
                        }
                    }

                    if (tryToLock)
                        continue;

                    for (i = 0; i < sa.Length; i++)
                        table.Add(sa[i], sa[i]);
                    return;
                }
            }
            finally
            {
                NativeMonitor.Exit(this);
            }
        }
        /**
         * Simultaneously unlock all the locks indicated by the
         * objects in the array sa.
         */
        public void unlock(Object[] sa)
        {
            try
            {
                NativeMonitor.Enter(this);
                int i;
                for (i = 0; i < sa.Length; i++)
                    table.Remove(sa[i]);
                NativeMonitor.PulseAll(this);
            }
            finally
            {
                NativeMonitor.Exit(this);
            }
        }
    }
}
