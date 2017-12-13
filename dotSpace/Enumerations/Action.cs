namespace dotSpace.Enumerations
{
    /// <summary>
    /// This entity constitutes the supported actions of the distributed tuple space.
    /// </summary>
    public enum ActionType
    {
        NONE,
        /// <summary>
        /// Defines the action as a request for executing a Get operation on the remote space.
        /// </summary>
        GET_REQUEST,
        /// <summary>
        /// Defines the action as a request for executing a GetP operation on the remote space.
        /// </summary>
        GETP_REQUEST,
        /// <summary>
        /// Defines the action as a request for executing a GetAll operation on the remote space.
        /// </summary>
        GETALL_REQUEST,
        /// <summary>
        /// Defines the action as a request for executing a Query operation on the remote space.
        /// </summary>
        QUERY_REQUEST,
        /// <summary>
        /// Defines the action as a request for executing a QueryP operation on the remote space.
        /// </summary>
        QUERYP_REQUEST,
        /// <summary>
        /// Defines the action as a request for executing a QueryAll operation on the remote space.
        /// </summary>
        QUERYALL_REQUEST,
        /// <summary>
        /// Defines the action as a request for executing a Put operation on the remote space.
        /// </summary>
        PUT_REQUEST,

        /// <summary>
        /// Defines the action as a response of executing a Get operation on the remote space.
        /// </summary>
        GET_RESPONSE,
        /// <summary>
        /// Defines the action as a response for executing a GetP operation on the remote space.
        /// </summary>
        GETP_RESPONSE,
        /// <summary>
        /// Defines the action as a response for executing a GetAll operation on the remote space.
        /// </summary>
        GETALL_RESPONSE,
        /// <summary>
        /// Defines the action as a response for executing a Query operation on the remote space.
        /// </summary>
        QUERY_RESPONSE,
        /// <summary>
        /// Defines the action as a response for executing a QueryP operation on the remote space.
        /// </summary>
        QUERYP_RESPONSE,

        /// <summary>
        /// Defines the action as a response for executing a QueryAll operation on the remote space.
        /// </summary>
        QUERYALL_RESPONSE,
        /// <summary>
        /// Defines the action as a response for executing a Put operation on the remote space.
        /// </summary>
        PUT_RESPONSE,
        /// <summary>
        /// Defines the action as a request for executing a Get operation on the remote space.
        /// </summary>
        OBJECT_GET_REQUEST,
        /// <summary>
        /// Defines the action as a request for executing a GetP operation on the remote space.
        /// </summary>
        OBJECT_GETP_REQUEST,
        /// <summary>
        /// Defines the action as a request for executing a GetAll operation on the remote space.
        /// </summary>
        OBJECT_GETALL_REQUEST,
        /// <summary>
        /// Defines the action as a request for executing a Query operation on the remote space.
        /// </summary>
        OBJECT_QUERY_REQUEST,
        /// <summary>
        /// Defines the action as a request for executing a QueryP operation on the remote space.
        /// </summary>
        OBJECT_QUERYP_REQUEST,
        /// <summary>
        /// Defines the action as a request for executing a QueryAll operation on the remote space.
        /// </summary>
        OBJECT_QUERYALL_REQUEST,
        /// <summary>
        /// Defines the action as a request for executing a Put operation on the remote space.
        /// </summary>
        OBJECT_PUT_REQUEST,
        /// <summary>
        /// Defines the action as a response of executing a Get operation on the remote space.
        /// </summary>
        OBJECT_GET_RESPONSE,
        /// <summary>
        /// Defines the action as a response for executing a GetP operation on the remote space.
        /// </summary>
        OBJECT_GETP_RESPONSE,
        /// <summary>
        /// Defines the action as a response for executing a GetAll operation on the remote space.
        /// </summary>
        OBJECT_GETALL_RESPONSE,
        /// <summary>
        /// Defines the action as a response for executing a Query operation on the remote space.
        /// </summary>
        OBJECT_QUERY_RESPONSE,
        /// <summary>
        /// Defines the action as a response for executing a QueryP operation on the remote space.
        /// </summary>
        OBJECT_QUERYP_RESPONSE,

        /// <summary>
        /// Defines the action as a response for executing a QueryAll operation on the remote space.
        /// </summary>
        OBJECT_QUERYALL_RESPONSE,
        /// <summary>
        /// Defines the action as a response for executing a Put operation on the remote space.
        /// </summary>
        OBJECT_PUT_RESPONSE,
    }
}
