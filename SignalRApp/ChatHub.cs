using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRApp
{
    public class ChatHub : Hub
    {
        //eklencek
       public void HerkeseGonder(string gonderen,string mesaj)
        {
            Clients.All.herkeseGonder(gonderen, mesaj,$"{DateTime.Now:g}");
        }

        public void OzelMesaj(string gonderenId,string AliciId,string mesaj)
        {
            Clients.User(AliciId).mesajGeldi(gonderenId, mesaj);
        }

        public void Login(string kullaniciAdi,string id)
        {
           // eklencek
            Clients.User(id).getId(id);
        }
        public override Task OnConnected()
        {
           // UserList.Add(new )
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            //eklencek
            return base.OnDisconnected(stopCalled);
        }
    }
}