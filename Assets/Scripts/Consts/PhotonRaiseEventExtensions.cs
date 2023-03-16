using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Consts
{
    public class PhotonRaiseEvent
    {
        public static RaiseEventOptions ToAll { get => new RaiseEventOptions { Receivers = ReceiverGroup.All }; }
    }
}
