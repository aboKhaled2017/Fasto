﻿using System.Threading.Tasks;

namespace Fastdo.Core.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string body);
    }
}