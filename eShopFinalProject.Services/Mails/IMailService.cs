using eShopFinalProject.Utilities.ViewModel.Mails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Services.Mails
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
