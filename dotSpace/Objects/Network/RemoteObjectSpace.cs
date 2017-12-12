using dotSpace.Enumerations;
using dotSpace.Interfaces;
using dotSpace.Interfaces.Network;
using dotSpace.Interfaces.Space;
using dotSpace.Objects.Network.ConnectionModes;
using dotSpace.Objects.Network.Messages.Requests;
using dotSpace.Objects.Network.Messages.Responses;
using dotSpace.Objects.Network.Protocols;
using dotSpace.Objects.Space;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace dotSpace.Objects.Network
{

	public sealed class RemoteObjectSpace : IObjectSpaceSimple
	{

		private IConnectionMode mode;
        private IEncoder encoder;
        private ConnectionString connectionString;

		public RemoteObjectSpace(string uri)
        {
            this.connectionString = new ConnectionString(uri);
            this.encoder = new RequestEncoder();
            if (string.IsNullOrEmpty(this.connectionString.Target))
            {
                throw new Exception("Must specify valid target.");
            }
        }

		public T Get<T>()
		{
			GetRequest request = new GetRequest(this.GetSource(), this.GetSessionId(), this.connectionString.Target, null);
			return (T)this.GetMode()?.PerformRequest<GetResponse>(request).Result[0];
		}

        public T GetP<T>()
		{
			GetPRequest request = new GetPRequest(this.GetSource(), this.GetSessionId(), this.connectionString.Target, null);
            return (T)this.GetMode()?.PerformRequest<GetPResponse>(request).Result[0];
		}			

        public IEnumerable<T> GetAll<T>()
		{
			GetAllRequest request = new GetAllRequest(this.GetSource(), this.GetSessionId(), this.connectionString.Target, null);
			return this.GetMode()?.PerformRequest<GetAllResponse>(request).Result.Select(x => (T)x[0]);
		}

		public T Query<T>()
		{
			QueryRequest request = new QueryRequest(this.GetSource(), this.GetSessionId(), this.connectionString.Target, null);
            return (T)this.GetMode()?.PerformRequest<QueryResponse>(request).Result[0];
		}

		public T QueryP<T>()
		{
			QueryPRequest request = new QueryPRequest(this.GetSource(), this.GetSessionId(), this.connectionString.Target, null);
            return (T)this.GetMode()?.PerformRequest<QueryPResponse>(request).Result[0];
		}

        public IEnumerable<T> QueryAll<T>()
		{
			QueryAllRequest request = new QueryAllRequest(this.GetSource(), this.GetSessionId(), this.connectionString.Target, null);
			return this.GetMode()?.PerformRequest<QueryAllResponse>(request).Result.Select(x => (T)x[0]);
		}

        public void Put<T>(T element)
		{
			object[] eltArray = new object[1] {element};
			PutRequest request = new PutRequest(this.GetSource(), this.GetSessionId(), this.connectionString.Target, eltArray);
            this.GetMode()?.PerformRequest<PutResponse>(request);
		}

		private string GetSessionId()
        {
            return Guid.NewGuid().ToString();
        }
        private string GetSource()
        {
            return string.Empty;
        }
        private IConnectionMode GetMode()
        {
            switch (this.connectionString.Mode)
            {
                case ConnectionMode.KEEP: lock (this.connectionString) { this.mode = this.mode ?? new Keep(this.GetProtocol(), this.encoder); } break;
                case ConnectionMode.CONN: return new Conn(this.GetProtocol(), this.encoder);
                case ConnectionMode.PUSH: return new Push(this.GetProtocol(), this.encoder);
                case ConnectionMode.PULL: return new Pull(this.GetProtocol(), this.encoder);
                default: return null;
            }
            return this.mode;
        }
        private IProtocol GetProtocol()
        {
            switch (this.connectionString.Protocol)
            {
                case Protocol.TCP: return new Tcp(new TcpClient(this.connectionString.Host, this.connectionString.Port));
                case Protocol.UDP: return new Udp(this.connectionString.Host, this.connectionString.Port);
                default: return null;
            }
        }
		
	}
	
}
