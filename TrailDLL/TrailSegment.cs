/*
 * Copyright © 2011 Luca Pieracci Galante
 * 
 * This program is licensed under the Microsoft Public License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://medusa.codeplex.com/license.
 */

using Microsoft.Xna.Framework;

namespace TrailDLL
{
	internal class TrailSegment
	{
		public float	Radius;
		public Vector3	Position;
		public Vector3	RadiusDirection;

		public Vector3  TopPoint
		{
			get
			{
				return Position + RadiusDirection * Radius;
			}
		}
		public Vector3  BottomPoint
		{
			get
			{
				return Position - RadiusDirection * Radius;			 
			}
		}

		public TrailSegment(float fRadius)
		{
			Radius              = fRadius;	
			Position            = new Vector3(0);
			RadiusDirection     = new Vector3(0,1f,0);
		}
	}
}