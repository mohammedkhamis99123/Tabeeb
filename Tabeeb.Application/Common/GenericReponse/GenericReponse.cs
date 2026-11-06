using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Application.Common.GenericReponse
{
    public  class GenericReponse
    {
        public bool IsSuccess {  get; set; }
        public string Message {  get; set; }

        public object? Data { get; set; }

        public IDictionary<string, string[]>? Errors { get; set; }
    }
}
