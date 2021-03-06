﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using System;

namespace SiliconStudio.Assets
{
    /// <summary>
    /// Associates a type name used in YAML content.
    /// </summary>
    public class AssetAliasAttribute : Attribute
    {
        private readonly string alias;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetAliasAttribute"/> class.
        /// </summary>
        /// <param name="alias">The type name.</param>
        public AssetAliasAttribute(string @alias)
        {
            this.alias = alias;
        }

        /// <summary>
        /// Gets the type name.
        /// </summary>
        /// <value>The type name.</value>
        public string Alias
        {
            get
            {
                return alias;
            }
        }
    }
}