using dotSpace.BaseClasses.Network.Messages;
using dotSpace.Interfaces;
using dotSpace.Interfaces.Network;
using System.Collections.Generic;
using System.Threading;

namespace dotSpace.Objects.Utility
{
    /// <summary>
    /// A threadsafe data structure that facilitates message parsing.
    /// It allows threads to wait and signal through querying specific messages similar to that of the 
    /// tuplespace datastructure.
    /// </summary>
    internal sealed class MessageQueue
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Fields

        private readonly Dictionary<string, IMessage> messages;
        private readonly ReaderWriterLockSlim rwLock;

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Constructors

        /// <summary>
        /// Initializes a new instance of the MessageQueue class.
        /// </summary>
        public MessageQueue()
        {
            this.messages = new Dictionary<string, IMessage>();
            this.rwLock = new ReaderWriterLockSlim();
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Public Methods

        /// <summary>
        /// Blocks the calling thread until a message arrives with a matching sessions id.
        /// The message is returned once it arrives.
        /// If the message already exists, the message is dequeued and returned.
        /// </summary>
        public IMessage Get(string key)
        {
            MessageBase t = (MessageBase)this.WaitForSession(key);
            bool successs = true;
            rwLock.EnterWriteLock();
            successs = messages.Remove(key);
            rwLock.ExitWriteLock();
            return successs ? t : null;
        }

        /// <summary>
        /// Inserts a message into the underlying set, thereby awaking any waiting threads.
        /// </summary>
        public void Put(IMessage message)
        {
            Monitor.Enter(messages);
            rwLock.EnterWriteLock();
            messages.Add(message.Session, message);
            rwLock.ExitWriteLock();
            Monitor.PulseAll(messages);
            Monitor.Exit(messages);
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Private Methods

        private IMessage WaitForSession(string key)
        {
            IMessage t;
            Monitor.Enter(messages);
            while (((t = this.Find(key)) == null))
            {
                Monitor.Wait(messages);
            }
            Monitor.Exit(messages);
            return t;
        }
        private IMessage Find(string key)
        {
            rwLock.EnterReadLock();
            MessageBase msg = this.messages.ContainsKey(key) ? (MessageBase)this.messages[key] : null;
            rwLock.ExitReadLock();
            return msg;
        }
        #endregion
    }
}
