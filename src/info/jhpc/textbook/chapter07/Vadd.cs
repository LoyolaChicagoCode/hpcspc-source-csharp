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

namespace info.jhpc.textbook.chapter07
{
	public class Vadd : Op2 {
	    Op1 continuation;
	
	    public Vadd(Op1 contin) {
	        continuation = contin;
	    }
	
	    public void op(Object lopnd, Object ropnd) {
	        double[] x, y, z;
	        if (lopnd is Exception)
	            continuation.op(lopnd);
	        if (ropnd is Exception)
	            continuation.op(ropnd);
	        try {
	            x = (double[]) lopnd;
	            y = (double[]) ropnd;
	            z = new double[x.Length];
	            for (int i = 0; i < z.Length; ++i)
	                z[i] = x[i] + y[i];
	            continuation.op(z);
	        } catch (Exception e) {
	            continuation.op(e);
	        }
	    }
	}
}
