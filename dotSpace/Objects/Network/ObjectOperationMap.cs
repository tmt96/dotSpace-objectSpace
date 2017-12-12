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

        private IObjectRepository repository;
        private Dictionary<Type, Func<IMessage, IMessage>> operationMap;

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Constructors

        /// <summary>
        /// Initializes a new instances of the OperationMap class.
        /// </summary>
        public ObjectOperationMap(IObjectRepository repository)
        {
            this.repository = repository;
            this.operationMap = new Dictionary<Type, Func<IMessage, IMessage>>();
            this.operationMap.Add(typeof(GetRequest), this.PerformGet);
            this.operationMap.Add(typeof(GetPRequest), this.PerformGetP);
            this.operationMap.Add(typeof(GetAllRequest), this.PerformGetAll);
            this.operationMap.Add(typeof(QueryRequest), this.PerformQuery);
            this.operationMap.Add(typeof(QueryPRequest), this.PerformQueryP);
            this.operationMap.Add(typeof(QueryAllRequest), this.PerformQueryAll);
            this.operationMap.Add(typeof(PutRequest), this.PerformPut);
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Public Methods

        /// <summary>
        /// Executes an operation defined within the request. Followingly, the response is returned.
        /// </summary>
        public IMessage Execute(IMessage request)
        {
            Type requestType = request.GetType();
            if (this.operationMap.ContainsKey(requestType))
            {
                return this.operationMap[requestType](request);
            }

            return new BasicResponse(request.Actiontype, request.Source, request.Session, request.Target, StatusCode.METHOD_NOT_ALLOWED, StatusMessage.METHOD_NOT_ALLOWED);
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////
        #region // Private Methods

        private IMessage PerformGet<T>(IMessage request)
        {
            IObjectSpace ts = this.repository.GetSpace(request.Target);
            if (ts != null)
            {
                GetRequest getReq = (GetRequest)request;
                T result = ts.Get<T>((Func<T, bool>) getReq.Template[0]);
                return new GetResponse(request.Source, request.Session, request.Target, new object[1] {result}, StatusCode.OK, StatusMessage.OK);
            }
            return new GetResponse(request.Source, request.Session, request.Target, null, StatusCode.NOT_FOUND, StatusMessage.NOT_FOUND);
        }
        private IMessage PerformGetP<T>(IMessage request)
        {
            IObjectSpace ts = this.repository.GetSpace(request.Target);
            if (ts != null)
            {
                GetPRequest getReq = (GetPRequest)request;
                T result = ts.GetP<T>((Func<T, bool>) getReq.Template[0]);
                return new GetPResponse(request.Source, request.Session, request.Target, new object[1] {result}, StatusCode.OK, StatusMessage.OK);
            }
            return new GetPResponse(request.Source, request.Session, request.Target, null, StatusCode.NOT_FOUND, StatusMessage.NOT_FOUND);
        }
        private IMessage PerformGetAll<T>(IMessage request)
        {
            IObjectSpace ts = this.repository.GetSpace(request.Target);
            if (ts != null)
            {
                GetAllRequest getReq = (GetAllRequest)request;
                IEnumerable<T> results = ts.GetAll<T>((Func<T, bool>) getReq.Template[0]);
                return new GetAllResponse(request.Source, request.Session, request.Target, results.Select(x => new object[1] {x}), StatusCode.OK, StatusMessage.OK);
            }
            return new GetAllResponse(request.Source, request.Session, request.Target, null, StatusCode.NOT_FOUND, StatusMessage.NOT_FOUND);
        }
        private IMessage PerformQuery<T>(IMessage request)
        {
            IObjectSpace ts = this.repository.GetSpace(request.Target);
            if (ts != null)
            {
                QueryRequest getReq = (QueryRequest)request;
                T result = ts.Query<T>((Func<T, bool>) getReq.Template[0]);
                return new QueryResponse(request.Source, request.Session, request.Target, new object[1] {result}, StatusCode.OK, StatusMessage.OK);
            }
            return new QueryResponse(request.Source, request.Session, request.Target, null, StatusCode.NOT_FOUND, StatusMessage.NOT_FOUND);
        }
        private IMessage PerformQueryP<T>(IMessage request)
        {
            IObjectSpace ts = this.repository.GetSpace(request.Target);
            if (ts != null)
            {
                QueryPRequest getReq = (QueryPRequest)request;
                T result = ts.QueryP<T>((Func<T, bool>) getReq.Template[0]);
                return new QueryPResponse(request.Source, request.Session, request.Target, new object[1] {result}, StatusCode.OK, StatusMessage.OK);
            }
            return new QueryPResponse(request.Source, request.Session, request.Target, null, StatusCode.NOT_FOUND, StatusMessage.NOT_FOUND);
        }
        private IMessage PerformQueryAll<T>(IMessage request)
        {
            IObjectSpace ts = this.repository.GetSpace(request.Target);
            if (ts != null)
            {
                QueryAllRequest getReq = (QueryAllRequest)request;
                IEnumerable<T> results = ts.QueryAll<T>((Func<T, bool>) getReq.Template[0]);
                return new QueryAllResponse(request.Source, request.Session, request.Target, results.Select(x => new object[1] {x}), StatusCode.OK, StatusMessage.OK);
            }
            return new QueryAllResponse(request.Source, request.Session, request.Target, null, StatusCode.NOT_FOUND, StatusMessage.NOT_FOUND);
        }
        private IMessage PerformPut<T>(IMessage request)
        {
            IObjectSpace ts = this.repository.GetSpace(request.Target);
            if (ts != null)
            {
                PutRequest putReq = (PutRequest)request;
                ts.Put(putReq.Tuple);
                return new PutResponse(request.Source, request.Session, request.Target, StatusCode.OK, StatusMessage.OK);
            }
            return new PutResponse(request.Source, request.Session, request.Target, StatusCode.NOT_FOUND, StatusMessage.NOT_FOUND);
        } 

        #endregion

		private IMessage PerformGet(IMessage request)
		{
			GetRequest getReq = (GetRequest)request;
			return PerformGetHelper((getReq.Template[0].GetType()).GenericTypeArguments[0], request);
		}

		private IMessage PerformGetHelper<T>(T obj, IMessage request)
		{
			return PerformGet<T>(request);
		}
		
        private IMessage PerformGetP(IMessage request)
		{
			GetPRequest getReq = (GetPRequest)request;
			return PerformGetPHelper((getReq.Template[0].GetType()).GenericTypeArguments[0], request);
		}

		private IMessage PerformGetPHelper<T>(T obj, IMessage request)
		{
			return PerformGetP<T>(request);
		}
		
        private IMessage PerformGetAll(IMessage request)
		{
			GetAllRequest getReq = (GetAllRequest)request;
			return PerformGetAllHelper((getReq.Template[0].GetType()).GenericTypeArguments[0], request);
		}

		private IMessage PerformGetAllHelper<T>(T obj, IMessage request)
		{
			return PerformGetAll<T>(request);
		}

        private IMessage PerformQuery(IMessage request)
		{
			QueryRequest getReq = (QueryRequest)request;
			return PerformQueryHelper((getReq.Template[0].GetType()).GenericTypeArguments[0], request);
		}

		private IMessage PerformQueryHelper<T>(T obj, IMessage request)
		{
			return PerformQuery<T>(request);
		}

        private IMessage PerformQueryP(IMessage request)
		{
			QueryPRequest getReq = (QueryPRequest)request;
			return PerformQueryPHelper((getReq.Template[0].GetType()).GenericTypeArguments[0], request);
		}

		private IMessage PerformQueryPHelper<T>(T obj, IMessage request)
		{
			return PerformQueryP<T>(request);
		}

        private IMessage PerformQueryAll(IMessage request)
		{
			QueryAllRequest getReq = (QueryAllRequest)request;
			return PerformQueryAllHelper((getReq.Template[0].GetType()).GenericTypeArguments[0], request);
		}

		private IMessage PerformQueryAllHelper<T>(T obj, IMessage request)
		{
			return PerformQueryAll<T>(request);
		}

        private IMessage PerformPut(IMessage request)
		{
			PutRequest getReq = (PutRequest)request;
			return PerformPutHelper((getReq.Tuple[0].GetType()).GenericTypeArguments[0], request);
		}
		
		private IMessage PerformPutHelper<T>(T obj, IMessage request)
		{
			return PerformPut<T>(request);
		}

    }
}
