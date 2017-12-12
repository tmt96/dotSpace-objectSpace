using dotSpace.Interfaces;
using dotSpace.Interfaces.Network;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Network;
using dotSpace.Objects.Network.Gates;
using System.Collections.Generic;
using System;

namespace dotSpace.BaseClasses.Network
{
    /// <summary>
    /// Provides the basic functionality for supporting multiple distributed spaces. This is an abstract class.
    /// The RepositoryBase class allows direct access to the contained spaces through their respective identifies.
    /// Additionally, RepositoryBase facilitates distributed access to the underlying spaces through Gates. 
    /// </summary>
    public abstract class ObjectRepositoryBase : IObjectRepositorySimple
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Fields

        protected List<IGate> gates;
        protected IEncoder encoder;
        protected Dictionary<string, IObjectSpaceSimple> spaces;
        protected GateFactory gateFactory;

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Constructors
        
        /// <summary>
        /// Initializes a new instance of the RepositoryBase class.
        /// </summary>
        public ObjectRepositoryBase()
        {
            this.spaces = new Dictionary<string, IObjectSpaceSimple>();
            this.gates = new List<IGate>();
            this.encoder = new ResponseEncoder();
            this.gateFactory = new GateFactory();
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Public Methods
        
        /// <summary>
        /// Adds a new Gate to the repository based on the provided connectionstring.
        /// </summary>
        public void AddGate(string connectionstring)
        {
            IGate gate = this.gateFactory.CreateGate(connectionstring, this.encoder);
            if (gate != null)
            {
                this.gates.Add(gate);
                gate.Start(this.OnConnect);
            }
        }
        /// <summary>
        /// Adds a new Space to the repository, identified by the specified parameter.
        /// </summary>
        public void AddSpace(string identifier, IObjectSpaceSimple objectspace)
        {
            if (!this.spaces.ContainsKey(identifier))
            {
                this.spaces.Add(identifier, objectspace);
            }
        }
        /// <summary>
        /// Returns the local instance of the space identified by the parameter.
        /// </summary>
        public IObjectSpaceSimple GetSpace(string identifier)
        {
            if (this.spaces.ContainsKey(identifier))
            {
                return this.spaces[identifier];
            }
            return null;
        }
        /// <summary>
        /// Retrieves and removes the first tuple from the target Space, matching the specified pattern. The operation will block if no elements match.
        /// </summary>
        public T Get<T>(string target)
        {
            return this.GetSpace(target).Get<T>();
        }
        /// <summary>
        /// Retrieves and removes the first tuple from the target Space, matching the specified pattern. The operation is non-blocking. The operation will return null if no elements match.
        /// </summary>
        public T GetP<T>(string target)
        {
            return this.GetSpace(target).GetP<T>();
        }
        /// <summary>
        /// Retrieves and removes all tuples from the target Space matching the specified pattern. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        public IEnumerable<T> GetAll<T>(string target)
        {
            return this.GetSpace(target)?.GetAll<T>();
        }
        /// <summary>
        /// Retrieves the first tuple from the target Space, matching the specified pattern. The operation will block if no elements match.
        /// </summary>
        public T Query<T>(string target)
        {
            return this.GetSpace(target).Query<T>();
        }
        /// <summary>
        /// Retrieves the first tuple from the target Space, matching the specified pattern. The operation is non-blocking. The operation will return null if no elements match.
        /// </summary>
        public T QueryP<T>(string target)
        {
            return this.GetSpace(target).QueryP<T>();
        }
        /// <summary>
        /// Retrieves all tuples from the target Space matching the specified pattern. The operation is non-blocking. The operation will return an empty set if no elements match.
        /// </summary>
        public IEnumerable<T> QueryAll<T>(string target)
        {
            return this.GetSpace(target)?.QueryAll<T>();
        }
        /// <summary>
        /// Inserts the tuple passed as argument into the target Space.
        /// </summary>
        public void Put<T>(string target, T element)
        {
            this.GetSpace(target)?.Put<T>(element);
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Protected Methods
        
        /// <summary>
        /// Template method that is called when the repository receives an incoming connection.
        /// </summary>
        protected abstract void OnConnect(IConnectionMode mode);

        #endregion
    }
}
