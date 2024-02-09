﻿using System.Net;

namespace ToDoList_API.Models
{
    public class ApiResponse
    {
        public HttpStatusCode statusCode { get; set; }

        public bool IsSuccessful { get; set; } = true;

        public List<string> ErrorMessages { get; set; }

        public object Result { get; set; }
    }
}
