﻿namespace Project.DTOs
{
    public class EmailRequestAgentDto
    {
        public List<string> To { get; set; }  
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
