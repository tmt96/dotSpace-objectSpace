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
using dotSpace.BaseClasses.Network;

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
            this.encoder = new ObjectEncoderBase();
            if (string.IsNullOrEmpty(this.connectionString.Target))
            {
                throw new Exception("Must specify valid target.");
            }
        }

		public T Get<T>()
		{
			var request = new ObjectGetRequest<T>(this.GetSource(), this.GetSessionId(), this.connectionString.Target);
            var mode = this.GetMode();
			return mode.PerformRequest<ObjectGetResponse<T>>(request).Result;
		}

        public T GetP<T>()
		{
			var request = new ObjectGetPRequest<T>(this.GetSource(), this.GetSessionId(), this.connectionString.Target);
            var mode = this.GetMode();
            return mode.PerformRequest<ObjectGetPResponse<T>>(request).Result;
        }			

        public IEnumerable<T> GetAll<T>()
		{
			var request = new ObjectGetAllRequest<T>(this.GetSource(), this.GetSessionId(), this.connectionString.Target);
            var mode = this.GetMode();
            return mode.PerformRequest<ObjectGetAllResponse<T>>(request).Result;
        }

		public T Query<T>()
		{
			var request = new ObjectQueryRequest<T>(this.GetSource(), this.GetSessionId(), this.connectionString.Target);
            var mode = this.GetMode();
            return mode.PerformRequest<ObjectQueryResponse<T>>(request).Result;
        }

		public T QueryP<T>()
		{
            var request = new ObjectQueryPRequest<T>(this.GetSource(), this.GetSessionId(), this.connectionString.Target);
            var mode = this.GetMode();
            return mode.PerformRequest<ObjectQueryPResponse<T>>(request).Result;
        }

        public IEnumerable<T> QueryAll<T>()
		{
            var request = new ObjectQueryAllRequest<T>(this.GetSource(), this.GetSessionId(), this.connectionString.Target);
            var mode = this.GetMode();
            return mode.PerformRequest<ObjectQueryAllResponse<T>>(request).Result;
        }

        public void Put<T>(T element)
		{
			var request = new ObjectPutRequest<T>(this.GetSource(), this.GetSessionId(), this.connectionString.Target, element);
            this.GetMode()?.PerformRequest<ObjectPutResponse<T>>(request);
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
