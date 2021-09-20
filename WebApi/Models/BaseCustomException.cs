using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class BaseCustomException : Exception
    {
        private int _code;
        private string _description;
        private string _errorCode;
        private List<Error> _errors { get; }

        public int StatusCode
        {
            get => _code;
        }
        public List<Error> Errors
        {
            get => _errors;
        }
        public string Description
        {
            get => _description;
        }
        public string ErrorCode
        {
            get => _errorCode;
        }

        public BaseCustomException(string message, string description, int code, string errorCode) : base(message)
        {
            _errors = new List<Error>();
            _code = code;
            _errors.Add(new Error() { ErrorCode = Convert.ToString(code), ShortDesc = message, FullDescription = description });
            _description = description;
            _errorCode = errorCode;
        }
    }
    public class Error
    {
        public string ErrorCode { get; set; }
        public string FullDescription { get; set; }
        public string ShortDesc { get; set; }
    }
    public class Status
    {
        public List<Error> Error { get; set; }
        public string StatusCode { get; set; }
    }

}
