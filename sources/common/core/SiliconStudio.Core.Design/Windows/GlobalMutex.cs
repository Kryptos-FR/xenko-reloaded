﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using System;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace SiliconStudio.Core.Windows
{
    /// <summary>
    /// A class representing an thread-safe, process-safe mutex.
    /// </summary>
    public class GlobalMutex : IDisposable
    {
        private readonly Mutex mutex;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalMutex"/> class.
        /// </summary>
        /// <param name="mutex">A mutex for which the current thread has ownership.</param>
        private GlobalMutex(Mutex mutex)
        {
            this.mutex = mutex;
        }

        /// <summary>
        /// Releases the mutex.
        /// </summary>
        public void Dispose()
        {
            mutex.ReleaseMutex();
        }
        
        /// <summary>
        /// Tries to take ownership of the mutex without waiting.
        /// </summary>
        /// Tries to take ownership of the mutex within a given delay.
        /// <returns>A new instance of <see cref="GlobalMutex"/> if the ownership could be taken, <c>null</c> otherwise.</returns>
        /// <remarks>The returned <see cref="GlobalMutex"/> must be disposed to release the mutex.</remarks>
        public static GlobalMutex TryLock(string name)
        {
            return Wait(name, 0);
        }

        /// <summary>
        /// Waits indefinitely to take ownership of the mutex.
        /// </summary>
        /// Tries to take ownership of the mutex within a given delay.
        /// <returns>A new instance of <see cref="GlobalMutex"/> if the ownership could be taken, <c>null</c> otherwise.</returns>
        /// <remarks>The returned <see cref="GlobalMutex"/> must be disposed to release the mutex.</remarks>
        public static GlobalMutex Wait(string name)
        {
            return Wait(name, -1);
        }

        /// <summary>
        /// Tries to take ownership of the mutex within a given delay.
        /// </summary>
        /// <param name="name">A unique name identifying the global mutex.</param>
        /// <param name="millisecondsTimeout">The maximum delay to wait before returning, in milliseconds.</param>
        /// <returns>A new instance of <see cref="GlobalMutex"/> if the ownership could be taken, <c>null</c> otherwise.</returns>
        /// <remarks>
        /// The returned <see cref="GlobalMutex"/> must be disposed to release the mutex.
        /// Calling this method with 0 for <see paramref="millisecondsTimeout"/> is equivalent to call <see cref="TryLock"/>.
        /// Calling this method with a negative value for <see paramref="millisecondsTimeout"/> is equivalent to call <see cref="Wait(string)"/>.
        /// </remarks>
        public static GlobalMutex Wait(string name, int millisecondsTimeout)
        {
            var mutex = BuildMutex(name);
            try
            {
                bool hasHandle = mutex.WaitOne(millisecondsTimeout, false);
                return hasHandle == false ? null : new GlobalMutex(mutex);
            }
            catch (AbandonedMutexException)
            {
                return new GlobalMutex(mutex);
            }
        }

        private static Mutex BuildMutex(string name)
        {
            name = name.Replace(":", "_");
            name = name.Replace("/", "_");
            name = name.Replace("\\", "_");
            string mutexId = string.Format("Global\\{0}", name);
            // Benlitz: I suspect the MutexSecurity object to be responible of some issues such as this one: https://github.com/SiliconStudio/xenko/issues/252 so I'm disabling it.
            //var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            //var securitySettings = new MutexSecurity();
            //securitySettings.AddAccessRule(allowEveryoneRule);
            bool createdNew;
            //var mutex = new Mutex(false, mutexId, out createdNew, securitySettings);
            var mutex = new Mutex(false, mutexId, out createdNew);
            return mutex;
        }
    }
}
