using dotSpace.Enumerations;
using dotSpace.Interfaces;
using dotSpace.Interfaces.Network;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Network.Messages.Requests;
using dotSpace.Objects.Network.Messages.Responses;
using dotSpace.Objects.Space;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dotSpace.Objects.Network
{
    /// <summary>
    /// Concrete implementation of the IOperationMap interface.
    /// Provides basic functionality to map requests with operations on a space repository.
    /// </summary>
    internal sealed class ObjectOperationMap : IOperationMap
    {
        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Fields

        public IObjectRepositorySimple repository;

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Constructors

        /// <summary>
        /// Initializes a new instances of the OperationMap class.
        /// </summary>
        public ObjectOperationMap(IObjectRepositorySimple repository)
        {
            this.repository = repository;
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Public Methods

        /// <summary>
        /// Executes an operation defined within the request. Followingly, the response is returned.
        /// </summary>
        public IMessage Execute(IMessage request)
        {
                try
                {
                    return ExecuteHelper((dynamic) request);
                }
                catch (Exception ex)
                {
                    return new BasicResponse(request.Actiontype, request.Source, request.Session, request.Target, StatusCode.METHOD_NOT_ALLOWED, StatusMessage.METHOD_NOT_ALLOWED);
                }
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // public Methods

        private IMessage ExecuteHelper<T>(ObjectGetRequest<T> request)
        {
            IObjectSpaceSimple ts = this.repository.GetSpace(request.Target);
            if (ts != null)
            {
                T result = ts.Get<T>();
                return new ObjectGetResponse<T>(request.Source, request.Session, request.Target, result, StatusCode.OK, StatusMessage.OK);
            }
            return new ObjectGetResponse<T>(request.Source, request.Session, request.Target, default(T), StatusCode.NOT_FOUND, StatusMessage.NOT_FOUND);
        }
        
        private IMessage ExecuteHelper<T>(ObjectGetPRequest<T> request)
        {
            IObjectSpaceSimple ts = this.repository.GetSpace(request.Target);
            if (ts != null)
            {
                T result = ts.GetP<T>();
                return new ObjectGetPResponse<T>(request.Source, request.Session, request.Target, result, StatusCode.OK, StatusMessage.OK);
            }
            return new ObjectGetPResponse<T>(request.Source, request.Session, request.Target, default(T), StatusCode.NOT_FOUND, StatusMessage.NOT_FOUND);
        }

        private IMessage ExecuteHelper<T>(ObjectGetAllRequest<T> request)
        {
            IObjectSpaceSimple ts = this.repository.GetSpace(request.Target);
            if (ts != null)
            {
                IEnumerable<T> results = ts.GetAll<T>();
                return new ObjectGetAllResponse<T>(request.Source, request.Session, request.Target, results, StatusCode.OK, StatusMessage.OK);
            }
            return new ObjectGetAllResponse<T>(request.Source, request.Session, request.Target, new List<T>(), StatusCode.NOT_FOUND, StatusMessage.NOT_FOUND);
        }

        private IMessage ExecuteHelper<T>(ObjectQueryRequest<T> request)
        {
            IObjectSpaceSimple ts = this.repository.GetSpace(request.Target);
            if (ts != null)
            {
                T result = ts.Query<T>();
                return new ObjectQueryResponse<T>(request.Source, request.Session, request.Target, result, StatusCode.OK, StatusMessage.OK);
            }
            return new ObjectQueryResponse<T>(request.Source, request.Session, request.Target, default(T), StatusCode.NOT_FOUND, StatusMessage.NOT_FOUND);
        }

        private IMessage ExecuteHelper<T>(ObjectQueryPRequest<T> request)
        {
            IObjectSpaceSimple ts = this.repository.GetSpace(request.Target);
            if (ts != null)
            {
                T result = ts.QueryP<T>();
                return new ObjectQueryPResponse<T>(request.Source, request.Session, request.Target, result, StatusCode.OK, StatusMessage.OK);
            }
            return new ObjectQueryPResponse<T>(request.Source, request.Session, request.Target, default(T), StatusCode.NOT_FOUND, StatusMessage.NOT_FOUND);
        }

        private IMessage ExecuteHelper<T>(ObjectQueryAllRequest<T> request)
        {
            IObjectSpaceSimple ts = this.repository.GetSpace(request.Target);
            if (ts != null)
            {
                IEnumerable<T> results = ts.QueryAll<T>();
                return new ObjectQueryAllResponse<T>(request.Source, request.Session, request.Target, results, StatusCode.OK, StatusMessage.OK);
            }
            return new ObjectQueryAllResponse<T>(request.Source, request.Session, request.Target, new List<T>(), StatusCode.NOT_FOUND, StatusMessage.NOT_FOUND);
        }

        private IMessage ExecuteHelper<T>(ObjectPutRequest<T> request)
        {
            IObjectSpaceSimple ts = this.repository.GetSpace(request.Target);
            if (ts != null)
            {
                ts.Put(request.Element);
                return new ObjectPutResponse<T>(request.Source, request.Session, request.Target, StatusCode.OK, StatusMessage.OK);
            }
            return new ObjectPutResponse<T>(request.Source, request.Session, request.Target, StatusCode.NOT_FOUND, StatusMessage.NOT_FOUND);
        } 

        #endregion
    }
}
