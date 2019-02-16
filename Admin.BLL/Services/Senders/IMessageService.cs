using Admin.Models.Enums;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.BLL.Services.Senders
{
    public interface  IMessageService
    {
        MessageStates MessageState { get; }//Sadece okunur bi nesne

        Task SendAsync(IdentityMessage message, params string[] contacts);
        void Send(IdentityMessage message, params string[] contacts);
        //async ve senkron yazdık bazı yerlerde async kullanamıyoruz.
    }
}
