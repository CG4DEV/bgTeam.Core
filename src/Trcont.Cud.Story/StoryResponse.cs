namespace Trcont.Cud.Story
{
    using bgTeam;
    using System;

    public class StoryResponse<T> : IResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoryResponse{T}"/> class.
        /// Create successful response
        /// </summary>
        public StoryResponse()
        {
            Success = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryResponse{T}"/> class.
        /// Create successful response with data
        /// </summary>
        public StoryResponse(T data)
            : this()
        {
            Data = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryResponse{T}"/> class.
        /// Create failed response with message
        /// </summary>
        public StoryResponse(string errorMsg)
        {
            Success = false;
            Message = errorMsg;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryResponse{T}"/> class.
        /// Create failed response with exception
        /// </summary>
        public StoryResponse(Exception exception)
        {
            Success = false;
            Message = exception.InnerException?.Message ?? exception.Message;
        }

        public bool Success { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
