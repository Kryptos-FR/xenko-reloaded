﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using System.Collections.Generic;

namespace SiliconStudio.Core.Serialization.Contents
{
    partial class ContentManager
    {
        /// <summary>
        /// Internal object that represents a loaded asset, with its url and reference counts.
        /// </summary>
        internal class Reference
        {
            /// <summary>
            /// The next item in the linked list.
            /// </summary>
            public Reference Next, Prev;

            public bool Deserialized;

            /// <summary>
            /// The object being referenced.
            /// </summary>
            public object Object;

            /// <summary>
            /// The URL.
            /// </summary>
            public readonly string Url;

            /// <summary>
            /// The public reference count (corresponding to ContentManager.Load/Unload).
            /// </summary>
            public int PublicReferenceCount;

            /// <summary>
            /// The private reference count (corresponding to an object being referenced indirectly by other loaded objects).
            /// </summary>
            public int PrivateReferenceCount;

            // Used internally for GC (maybe we could just use higher byte of PrivateReferenceCount or something like that?)
            public uint CollectIndex;

            // TODO: Lazily create this list?
            public HashSet<Reference> References = new HashSet<Reference>();

            public Reference(string url, bool publicReference)
            {
                Url = url;
                PublicReferenceCount = publicReference ? 1 : 0;
                PrivateReferenceCount = publicReference ? 0 : 1;
                CollectIndex = uint.MaxValue;
            }

            public override string ToString()
            {
                return $"{Object}, references: {PublicReferenceCount} public(s), {PrivateReferenceCount} private(s)";
            }
        }
    }
}
