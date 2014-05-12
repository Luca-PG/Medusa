/*
 * Copyright © 2011 Luca Pieracci Galante
 * 
 * This program is licensed under the Microsoft Public License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://medusa.codeplex.com/license.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrailDLL
{
    public interface ITrail : IDrawable, IGameComponent
    {
        /// <summary>
        /// If true, the trail is not moving, shrinking or closing.
        /// </summary>
        bool         IsFrozen               { get; }
        
        /// <summary>
        /// Determines if the trail tail is shrinking
        /// </summary>
        bool         Shrinking              { get; set; }
        
        /// <summary>
        /// The lenght of the trail, expressed in segments
        /// </summary>
        int          Length                 { get; set; }

        /// <summary>
        /// The radius (or half-height) of the trail at its head.
        /// </summary>
        float        Radius                 { get; set; }

        /// <summary>
        /// Determines how many times the trail texture will be repeated on the trail
        /// </summary>
        float        TextureRepetition      { get; set; }

        /// <summary>
        /// The color of the trail head
        /// </summary>
        Color        StartColor             { get; set; }

        /// <summary>
        /// The color of the trail tail
        /// </summary>
        Color        EndColor               { get; set; }

        /// <summary>
        /// Trail blending
        /// </summary>
        BlendState   Blending               { get; set; }

        /// <summary>
        /// Texture sampler state
        /// </summary>
        SamplerState SamplerState           { get; set; }

        /// <summary>
        /// Moves the head of the trail in a new position, with a new direction. Positions and 
        /// directions are queued into a list, so calling this method can be called more than once
        /// for update.
        /// </summary>
        /// <param name="vPosition">The new head position</param>
        /// <param name="vDirection">The new head direction</param>
        void Move(Vector3 vPosition, Vector3 vDirection);
        void Move(ref Vector3 vPosition, ref Vector3 vDirection);

        /// <summary>
        /// Moves all the segments of the trail into the same position, with maximized radius.
        /// </summary>
        void Reset();
        void Reset(Vector3 vNewPos);
        void Reset(ref Vector3 vNewPos);

        /// <summary>
        /// Freezes or unfreezes the trail.
        /// </summary>
        void Freeze();

    }
}